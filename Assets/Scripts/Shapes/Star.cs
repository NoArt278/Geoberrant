using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class Star : PowerShapes
{
    Rigidbody2D rb;
    float dashSpeed;
    public void Awake()
    {
        SetPoints(new List<Vector2>
        {
            new Vector2(0, -1),
            new Vector2(0.29389262614623657f, -0.4045084971874737f),
            new Vector2(0.9510565162951535f, -0.3090169943749474f),
            new Vector2(0.47552825814757677f, 0.1545084971874737f),
            new Vector2(0.5877852522924731f, 0.8090169943749475f),
            new Vector2(0, 0.5f),
            new Vector2(-0.587785252292473f, 0.8090169943749475f),
            new Vector2(-0.47552825814757677f, 0.15450849718747375f),
            new Vector2(-0.9510565162951536f, -0.3090169943749473f),
            new Vector2(-0.2938926261462366f, -0.40450849718747367f)
        });
        SetMass(40);
        SetRollSpeed(300);
        SetJumpHeight(20);
        SetMoveSpeed(30);
        SetScaleOffset(0.5f);
        SetTangentMode(ShapeTangentMode.Linear);
        rb = GetComponent<Rigidbody2D>();
        SetBounciness(0.6f);
        dashSpeed = 10;
    }

    public override void ActivatePower()
    {
        Vector2 dir = Camera.main.ScreenToWorldPoint(new Vector2(Mouse.current.position.x.value, Mouse.current.position.y.value)) - transform.position;
        rb.velocity = dir * dashSpeed;
    }
}
