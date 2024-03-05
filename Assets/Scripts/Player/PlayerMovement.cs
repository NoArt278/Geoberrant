using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    PControls pInput;
    InputAction move, jump, power;
    private SpriteShapeController spriteShapeControl;
    Rigidbody2D rb;
    public bool isGrounded;
    bool justExit;
    Coroutine coyote;
    float rollSpeed = 300, jumpHeight = 10, moveSpeed = 20;
    [SerializeField] List<PowerShapes> powerShapes = new();

    void Awake()
    {
        spriteShapeControl = GetComponent<SpriteShapeController>();
        rb = GetComponent<Rigidbody2D>();
        spriteShapeControl.splineDetail = 8;
        spriteShapeControl.spline.Clear();
        for (int i = 0; i < 8; i++)
        {
            spriteShapeControl.spline.InsertPointAt(i, powerShapes[0].points[i]);
            spriteShapeControl.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }
        pInput = new PControls();
        isGrounded = true;
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
        if (inputVal > powerShapes.Count)
        {
            return;
        }
        PowerShapes chosenShape = powerShapes[inputVal-1];

        // Set movement variables value
        rollSpeed = chosenShape.rollSpeed;
        rb.mass = chosenShape.mass;
        moveSpeed = chosenShape.moveSpeed;
        jumpHeight = chosenShape.jumpHeight;
        
        // Move vertices
        for (int i = 0; i < chosenShape.points.Count; i++)
        {
            if (i >= spriteShapeControl.splineDetail)
            {
                spriteShapeControl.spline.InsertPointAt(i, chosenShape.points[i]);
            } else
            {
                spriteShapeControl.spline.SetPosition(i, chosenShape.points[i]);
            }
            spriteShapeControl.spline.SetTangentMode(i, chosenShape.shapeTangentMode);
        }
        if (chosenShape.points.Count < 8)
        {
            for (int i = chosenShape.points.Count; i < 8; i++)
            {
                spriteShapeControl.spline.RemovePointAt(chosenShape.points.Count);
            }
        }
        spriteShapeControl.splineDetail = chosenShape.points.Count;
    }

    void FixedUpdate()
    {
        Vector2 readMoveValue = move.ReadValue<Vector2>();
        rb.velocity = new Vector2(readMoveValue.x * moveSpeed, rb.velocity.y);
        rb.angularVelocity = readMoveValue.x * rollSpeed * -1;
    }

    IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(0.5f);
        if (justExit)
        {
            isGrounded = false;
        }
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            rb.velocity += new Vector2(0, jumpHeight);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        justExit = false;
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        justExit = true;
        if (coyote == null)
        {
            coyote = StartCoroutine(CoyoteTime());
        }
    }
}
