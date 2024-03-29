using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Octagon : PowerShapes
{
    public void Awake() 
    {
        SetPoints(new List<Vector2>
        {
            new Vector2(0, 1), new Vector2(Mathf.Sqrt(0.5f), Mathf.Sqrt(0.5f)),
            new Vector2(1, 0), new Vector2(Mathf.Sqrt(0.5f), -Mathf.Sqrt(0.5f)),
            new Vector2(0, -1), new Vector2(-Mathf.Sqrt(0.5f), -Mathf.Sqrt(0.5f)),
            new Vector2(-1, 0), new Vector2(-Mathf.Sqrt(0.5f), Mathf.Sqrt(0.5f)),
        });
        SetMass(40);
        SetRollSpeed(300);
        SetJumpHeight(20);
        SetMoveSpeed(25);
        SetScaleOffset(0.25f);
        SetBounciness(0);
        SetDef(3);
        SetTangentMode(ShapeTangentMode.Continuous);
        SetGravScale(3);
        SetColor(Color.white);
    }
}
