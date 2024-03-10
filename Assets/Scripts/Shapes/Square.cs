using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class Square : PowerShapes
{
    Rigidbody2D rb;
    Coroutine power;
    const float powerJumpHeight = 60f, powerJumpRange = 30f;
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetPoints(new List<Vector2>
        {
            // Modify the vertices to change the shape of the sprite
            new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1)
        });
        SetMass(100);
        SetRollSpeed(200);
        SetJumpHeight(30);
        SetMoveSpeed(15);
        SetScaleOffset(0.25f);
        SetBounciness(0);
        SetGravScale(3);
        SetTangentMode(ShapeTangentMode.Linear);
        SetColor(Color.gray);
    }

    public override void ActivatePower()
    {
        if (power == null)
        {
            power = StartCoroutine(Slam());
        }
    }

    IEnumerator Slam()
    {
        StartCoroutine(StraightenRotation());
        Vector2 dir = Camera.main.ScreenToWorldPoint(new Vector2(Mouse.current.position.x.value, Mouse.current.position.y.value)) - transform.position;
        rb.velocity += new Vector2(powerJumpRange * Mathf.Sign(dir.x), powerJumpHeight);
        yield return new WaitForSeconds(1.3f);
        rb.velocity += new Vector2(0, -powerJumpHeight * 2);
        power = null;
    }

    IEnumerator StraightenRotation()
    {
        while (rb.rotation != 0)
        {
            rb.rotation = Mathf.Lerp(rb.rotation, 0, 1);
            yield return null;
        }
        rb.freezeRotation = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            rb.freezeRotation = false;
        }
    }
}
