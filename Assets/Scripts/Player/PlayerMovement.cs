using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class PlayerMovement : MonoBehaviour
{
    PControls pInput;
    InputAction move, jump, power, trigger;
    private SpriteShapeController spriteShapeControl;
    Rigidbody2D rb;
    Coroutine transformShape;
    float rollSpeed, jumpHeight, moveSpeed;
    const float transformSpeed = 0.05f, playerScale = 2;
    bool jumpAvailable;
    [SerializeField] List<PowerShapes> powerShapes;
    int currChosenShape;
    PolygonCollider2D polCollider;

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
        rb.mass = startShape.GetMass();

        // Set collider points
        Vector2[] points = new Vector2[startShape.GetPoints().Count];
        for (int i = 0; i < startShape.GetPoints().Count; i++)
        {
            points[i] = startShape.GetPoints()[i] * (playerScale + startShape.GetScaleOffset());
        }
        polCollider.points = points;
        polCollider.SetPath(0, points);
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
        powerShapes[currChosenShape-1].ActivatePower();
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

        transformShape = null;

        // Set collider points
        Vector2[] points = new Vector2[chosenShape.GetPoints().Count];
        for (int i = 0; i < chosenShape.GetPoints().Count; i++)
        {
            points[i] = chosenShape.GetPoints()[i] * (playerScale + chosenShape.GetScaleOffset());
        }
        polCollider.points = points;
        polCollider.SetPath(0, points);
    }

    void FixedUpdate()
    {
        Vector2 readMoveValue = move.ReadValue<Vector2>();
        float xSpeed = readMoveValue.x * moveSpeed;
        if (Mathf.Abs(xSpeed) < rb.velocity.x)
        {
            xSpeed = rb.velocity.x;
        }
        float nextRollSpeed = readMoveValue.x * rollSpeed * -1;
        if (Mathf.Abs(nextRollSpeed) < rb.angularVelocity)
        {
            nextRollSpeed = rb.angularVelocity;
        }
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        rb.angularVelocity = nextRollSpeed;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.rotation.eulerAngles.z < 90)
        {
            jumpAvailable = true;
        }
    }
}
