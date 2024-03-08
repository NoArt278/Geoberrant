using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class PlayerMovement : MonoBehaviour
{
    PControls pInput;
    InputAction move, jump, power;
    private SpriteShapeController spriteShapeControl;
    Rigidbody2D rb;
    public bool isGrounded;
    Coroutine coyote, transformShape;
    float rollSpeed, jumpHeight, moveSpeed;
    const float transformSpeed = 0.05f, playerScale = 2;
    bool justExit;
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
        isGrounded = true;
        justExit = false;
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
        jump.performed += Jump;
        power.performed += ChangeShape;
        move.Enable();
        jump.Enable();
        power.Enable();
    }

    private void OnDisable()
    {
        jump.performed -= Jump;
        power.performed -= ChangeShape;
        move.Disable();
        jump.Disable();
        power.Disable();
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
        rb.velocity = new Vector2(readMoveValue.x * moveSpeed, rb.velocity.y);
        rb.angularVelocity = readMoveValue.x * rollSpeed * -1;
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

    IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(0.2f);
        if (isGrounded)
        {
            isGrounded = !justExit;
        }
        coyote = null;
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            rb.velocity += new Vector2(0, jumpHeight);
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.rotation.eulerAngles.z < 90)
        {
            justExit = false;
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (coyote == null)
        {
            justExit = true;
            coyote = StartCoroutine(CoyoteTime());
        }
    }
}
