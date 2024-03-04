using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class TransformingScript : MonoBehaviour
{
    private SpriteShapeController spriteShapeControl;

    void Start()
    {
        spriteShapeControl = GetComponent<SpriteShapeController>();
        spriteShapeControl.splineDetail = 8;    
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeToCircle();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ChangeToSquare();
        }
    }

    void ChangeToCircle()
    {
        List<Vector2> points = new List<Vector2>
        {
            new Vector2(0, 1), new Vector2(Mathf.Sqrt(0.5f), Mathf.Sqrt(0.5f)), 
            new Vector2(1, 0), new Vector2(Mathf.Sqrt(0.5f), -Mathf.Sqrt(0.5f)), 
            new Vector2(0, -1), new Vector2(-Mathf.Sqrt(0.5f), -Mathf.Sqrt(0.5f)), 
            new Vector2(-1, 0), new Vector2(-Mathf.Sqrt(0.5f), Mathf.Sqrt(0.5f)),
        };
        spriteShapeControl.spline.Clear();
        for (int i = 0; i < points.Count; i++)
        {
            spriteShapeControl.spline.InsertPointAt(i, points[i]);
            spriteShapeControl.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }
    }

    void ChangeToSquare()
    {
        List<Vector2> points = new List<Vector2>
        {
            // Modify the vertices to change the shape of the sprite
            new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1)
        };
        spriteShapeControl.spline.Clear();
        // Update the Sprite Shape Renderer with the new vertices
        for (int i = 0; i < 4; i++)
        {
            spriteShapeControl.spline.InsertPointAt(i, points[i]);
            spriteShapeControl.spline.SetTangentMode(i, ShapeTangentMode.Linear);
        }
    }
}
