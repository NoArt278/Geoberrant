using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Star : PowerShapes
{
    public void Awake()
    {
        SetPoints(new List<Vector2>
        {
            new Vector2(0, -1),
            new Vector2(0.3f, -0.4f),
            new Vector2(0.9f, -0.3f),
            new Vector2(0.5f, 0.2f),
            new Vector2(0.6f, 0.8f),
            new Vector2(0, 0.5f),
            new Vector2(-0.6f, 0.8f),
            new Vector2(-0.5f, 0.2f),
            new Vector2(-0.9f, -0.3f),
            new Vector2(-0.3f, -0.4f)
        });
        SetMass(40);
        SetRollSpeed(300);
        SetJumpHeight(20);
        SetMoveSpeed(20);
        SetScaleOffset(0.45f);
        SetTangentMode(ShapeTangentMode.Linear);
    }
}
