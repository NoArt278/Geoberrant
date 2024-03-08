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
            new Vector2(0, 0.5f),
            new Vector2(-0.5f, -0.5f),
            new Vector2(0.5f, -0.5f),
        });
        SetMass(20);
        SetRollSpeed(300);
        SetJumpHeight(10);
        SetMoveSpeed(20);
        SetTangentMode(ShapeTangentMode.Linear);
    }
}
