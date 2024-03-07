using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Octagon : PowerShapes
{
    public Octagon() 
    {
        points = new List<Vector2>
        {
            new Vector2(0, 1), new Vector2(Mathf.Sqrt(0.5f), Mathf.Sqrt(0.5f)),
            new Vector2(1, 0), new Vector2(Mathf.Sqrt(0.5f), -Mathf.Sqrt(0.5f)),
            new Vector2(0, -1), new Vector2(-Mathf.Sqrt(0.5f), -Mathf.Sqrt(0.5f)),
            new Vector2(-1, 0), new Vector2(-Mathf.Sqrt(0.5f), Mathf.Sqrt(0.5f)),
        };
        mass = 20;
        rollSpeed = 300;
        jumpHeight = 10;
        moveSpeed = 20;
        shapeTangentMode = ShapeTangentMode.Continuous;
    }
}
