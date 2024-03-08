using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Square : PowerShapes
{
    public void Awake()
    {
        SetPoints(new List<Vector2>
        {
            // Modify the vertices to change the shape of the sprite
            new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1)
        });
        SetMass(100);
        SetRollSpeed(200);
        SetJumpHeight(20);
        SetMoveSpeed(10);
        SetScaleOffset(0.25f);
        SetTangentMode(ShapeTangentMode.Linear);
    }
}
