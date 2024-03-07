using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Triangle : PowerShapes
{
    public Triangle()
    {
        points = new List<Vector2>
        {
            new Vector2(0, 1),
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(0,0),
        };
        mass = 20;
        rollSpeed = 300;
        jumpHeight = 10;
        moveSpeed = 20;
        shapeTangentMode = ShapeTangentMode.Continuous;
    }
}
