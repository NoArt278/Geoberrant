using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Square : PowerShapes
{
    public Square()
    {
        points = new List<Vector2>
        {
            // Modify the vertices to change the shape of the sprite
            new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1)
        };
        mass = 50;
        rollSpeed = 300;
        jumpHeight = 10;
        moveSpeed = 10;
        shapeTangentMode = ShapeTangentMode.Linear;
    }
}
