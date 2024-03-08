using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Triangle : PowerShapes
{
    public void Awake()
    {
        SetPoints(new List<Vector2>
        {
            new Vector2(0, Mathf.Sqrt(3)),
            new Vector2(-1f,0),
            new Vector2(1f, 0),
        });
        SetMass(20);
        SetRollSpeed(200);
        SetJumpHeight(30);
        SetMoveSpeed(15);
        SetScaleOffset(0.4f);
        SetBounciness(0);
        SetTangentMode(ShapeTangentMode.Linear);
    }
}
