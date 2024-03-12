using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

public class PlayerMovement : MonoBehaviour
{
    PControls pInput;
    InputAction move, jump, power, trigger;
    private SpriteShapeController spriteShapeControl;
    Rigidbody2D rb;
    Coroutine transformShape, triggerPower;
    float rollSpeed, jumpHeight, moveSpeed, def, hp, reduceHPInterval;
    const float transformSpeed = 0.05f, playerScale = 2, powerCooldown=1.5f;
    bool jumpAvailable, isHurt;
    [SerializeField] List<PowerShapes> powerShapes;
    int currChosenShape;
    PolygonCollider2D polCollider;
    Gradient colorGradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;
    [SerializeField] GameManager gm;

    void Awake()
    {
        spriteShapeControl = GetComponent<SpriteShapeController>();
        polCollider = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteShapeControl.splineDetail = 8;
        spriteShapeControl.spline.Clear();
        pInput = new PControls();
        jumpAvailable = true;
        currChosenShape = 1;
        colorGradient = new Gradient();
        colorKey = new GradientColorKey[2];
        alphaKey = new GradientAlphaKey[2];
        hp = 100;
        isHurt = false;
        reduceHPInterval = 0.5f;
    }
    private void Start()
    {
        PowerShapes startShape = powerShapes[0];
        for (int i = 0; i < 8; i++)
        {
            spriteShapeControl.spline.InsertPointAt(i, startShape.GetPoints()[i] * playerScale);
            spriteShapeControl.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }

        rollSpeed = startShape.GetRollSpeed();
        jumpHeight = startShape.GetJumpHeight();
        moveSpeed = startShape.GetMoveSpeed();
        def = startShape.GetDef();
        rb.mass = startShape.GetMass();
        rb.sharedMaterial.bounciness = startShape.GetBounciness();

        // Set collider points
        Vector2[] points = new Vector2[startShape.GetPoints().Count];
        for (int i = 0; i < startShape.GetPoints().Count; i++)
        {
            points[i] = startShape.GetPoints()[i] * (playerScale + startShape.GetScaleOffset());
        }
        polCollider.points = points;
        polCollider.SetPath(0, points);

        colorKey[0] = new GradientColorKey(Color.white, 0.0f);
        colorKey[1] = new GradientColorKey(startShape.GetColor(), 1.0f);

        alphaKey[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphaKey[1] = new GradientAlphaKey(1.0f, 1.0f);

        colorGradient.SetKeys(colorKey, alphaKey);
        spriteShapeControl.spriteShapeRenderer.color = colorGradient.Evaluate(1f);

        StartCoroutine(ReduceHP());
        InvokeRepeating("FastenDepeleteHP", 60, 60);
    }

    private void OnEnable()
    {
        move = pInput.Player.Move;
        jump = pInput.Player.Jump;
        power = pInput.Player.Powers;
        trigger = pInput.Player.Fire;
        jump.performed += Jump;
        power.performed += ChangeShape;
        trigger.performed += TriggerPower;
        move.Enable();
        jump.Enable();
        power.Enable();
        trigger.Enable();
    }

    private void OnDisable()
    {
        jump.performed -= Jump;
        power.performed -= ChangeShape;
        trigger.performed -= TriggerPower;
        move.Disable();
        jump.Disable();
        power.Disable();
        trigger.Disable();
    }

    private void TriggerPower(InputAction.CallbackContext ctx)
    {
        if (triggerPower == null && transformShape == null && currChosenShape > 1)
        {
            triggerPower = StartCoroutine(StartPower());
        }
    }

    IEnumerator StartGradient()
    {
        float startTime = Time.time;
        float currInterval = Time.time - startTime;
        while (currInterval < powerCooldown)
        {
            if (!isHurt)
            {
                spriteShapeControl.spriteShapeRenderer.color = colorGradient.Evaluate(currInterval / powerCooldown);
            }
            currInterval = Time.time - startTime;
            yield return null;
        }
        colorGradient.Evaluate(1f);
    }

    IEnumerator Hurt()
    {
        Color prevColor = spriteShapeControl.spriteShapeRenderer.color;
        spriteShapeControl.spriteShapeRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteShapeControl.spriteShapeRenderer.color = prevColor;
        isHurt = false;
    }

    IEnumerator StartPower()
    {
        powerShapes[currChosenShape - 1].ActivatePower();
        StartCoroutine(StartGradient());
        yield return new WaitForSeconds(powerCooldown);
        triggerPower = null;
    }

    private void ChangeShape(InputAction.CallbackContext ctx)
    {
        int inputVal = Mathf.RoundToInt(ctx.ReadValue<float>());
        if (inputVal > powerShapes.Count || currChosenShape == inputVal || transformShape != null)
        {
            return;
        }
        currChosenShape = inputVal;
        PowerShapes chosenShape = powerShapes[inputVal-1];
        colorKey[0] = new GradientColorKey(colorKey[1].color, 0.0f); ;
        colorKey[1] = new GradientColorKey(chosenShape.GetColor(), 1.0f);
        colorGradient.SetKeys(colorKey, alphaKey);
        StartCoroutine(StartGradient());
        transformShape = StartCoroutine(TransformShape(chosenShape));
    }

    IEnumerator TransformShape(PowerShapes chosenShape)
    {
        List<Vector2> shapePoints = chosenShape.GetPoints();
        // Move vertices
        for (int i = 0; i < shapePoints.Count; i++)
        {
            if (i >= spriteShapeControl.splineDetail)
            {
                spriteShapeControl.spline.InsertPointAt(i, Vector2.zero);
            }
            StartCoroutine(MoveVertex(i, shapePoints[i] * playerScale));
            spriteShapeControl.spline.SetTangentMode(i, chosenShape.GetTangentMode());
            yield return new WaitForSeconds(0.1f);
        }
        if (shapePoints.Count < spriteShapeControl.splineDetail)
        {
            for (int i = shapePoints.Count; i < spriteShapeControl.splineDetail; i++)
            {
                StartCoroutine(DeleteVertex(shapePoints.Count));
                yield return new WaitForSeconds(0.1f);
            }
        }
        spriteShapeControl.splineDetail = shapePoints.Count;

        // Set movement variables value
        rollSpeed = chosenShape.GetRollSpeed();
        rb.mass = chosenShape.GetMass();
        moveSpeed = chosenShape.GetMoveSpeed();
        jumpHeight = chosenShape.GetJumpHeight();
        def = chosenShape.GetDef();
        rb.gravityScale = chosenShape.GetGravScale();
        rb.sharedMaterial.bounciness = chosenShape.GetBounciness();

        transformShape = null;

        // Set collider points
        Vector2[] points = new Vector2[chosenShape.GetPoints().Count];
        for (int i = 0; i < chosenShape.GetPoints().Count; i++)
        {
            points[i] = chosenShape.GetPoints()[i] * (playerScale + chosenShape.GetScaleOffset());
        }
        polCollider.points = points;
        polCollider.SetPath(0, points);

        colorKey[0] = new GradientColorKey(Color.white, 0.0f); ;
        colorKey[1] = new GradientColorKey(chosenShape.GetColor(), 1.0f);
        colorGradient.SetKeys(colorKey, alphaKey);

        if(chosenShape.GetType().ToString() == "Triangle")
        {
            Triangle triangle = (Triangle) chosenShape;
            triangle.ActivateLaser();
        } else
        {
            Triangle triangle = (Triangle)powerShapes[2];
            triangle.DeactivateLaser();
        }
    }
    private void Update()
    {
        if (hp <= 0)
        {
            jump.performed -= Jump;
            power.performed -= ChangeShape;
            trigger.performed -= TriggerPower;
            move.Disable();
            jump.Disable();
            power.Disable();
            trigger.Disable();
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        while (transform.localScale.y > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.01f * Time.deltaTime, transform.localScale.z);
            spriteShapeControl.spriteShapeRenderer.color = Color.red;
            yield return null;
        }
        gm.GameOver();
    }

    void FixedUpdate()
    {
        Vector2 readMoveValue = move.ReadValue<Vector2>();
        if (readMoveValue.x != 0 && (triggerPower == null || rb.velocity.x == 0)) // Can't move normally if power is active except when stopped moving
        {
            rb.velocity = new Vector2(readMoveValue.x * moveSpeed, rb.velocity.y);
            rb.angularVelocity = readMoveValue.x * rollSpeed * -1;
        }
    }
    
    IEnumerator MoveVertex(int idx, Vector2 position)
    {
        Vector2 vertexPos = spriteShapeControl.spline.GetPosition(idx);
        while (!Equals(vertexPos, position))
        {
            spriteShapeControl.spline.SetPosition(idx, Vector2.MoveTowards(vertexPos, position, transformSpeed));
            vertexPos = spriteShapeControl.spline.GetPosition(idx);
            yield return null;
        }
    }

    IEnumerator DeleteVertex(int idx)
    {
        Vector2 vertexPos = spriteShapeControl.spline.GetPosition(idx);
        while (!Equals(vertexPos, Vector2.zero))
        {
            spriteShapeControl.spline.SetPosition(idx, Vector2.MoveTowards(vertexPos, Vector2.zero, transformSpeed));
            vertexPos = spriteShapeControl.spline.GetPosition(idx);
            yield return null;
        }
        spriteShapeControl.spline.RemovePointAt(idx);
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (jumpAvailable)
        {
            rb.velocity += new Vector2(0, jumpHeight);
            jumpAvailable = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            if (!isHurt)
            {
                float damage = collision.gameObject.GetComponent<Damage>().damage;
                if (damage > def)
                {
                    isHurt = true;
                    hp -= damage - def;
                    StartCoroutine(Hurt());
                }
            }
            Destroy(collision.gameObject);
        }
    }

    public float GetHP()
    {
        return hp;
    }

    private void FastenDepeleteHP()
    {
        if (reduceHPInterval > 0.2f)
        {
            reduceHPInterval -= 0.1f;
        }
    }

    IEnumerator ReduceHP()
    {
        while (true)
        {
            hp -= 3;
            yield return new WaitForSeconds(reduceHPInterval);
        }
    }

    public void Heal(float amount)
    {
        if (hp < 100)
        {
            hp += amount;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.rotation.eulerAngles.z < 90)
        {
            jumpAvailable = true;
        }
    }
}
