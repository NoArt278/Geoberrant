using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Square : PowerShapes
{
    Rigidbody2D rb;
    bool isGrounded;
    Coroutine power;
    const float powerJumpHeight = 60f, powerJumpRange = 20f;
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetPoints(new List<Vector2>
        {
            // Modify the vertices to change the shape of the sprite
            new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1)
        });
        SetMass(100);
        SetRollSpeed(150);
        SetJumpHeight(30);
        SetMoveSpeed(10);
        SetScaleOffset(0.25f);
        SetTangentMode(ShapeTangentMode.Linear);
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
        rb.velocity += new Vector2(powerJumpRange * Mathf.Sign(rb.velocity.x), powerJumpHeight);
        yield return new WaitForSeconds(1f);
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
        if (collision.transform.rotation.z < 90)
        {
            rb.freezeRotation = false;
        }
    }
}
