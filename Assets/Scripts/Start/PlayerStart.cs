using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerStart : MonoBehaviour
{
    private SpriteShapeController spriteShapeControl;
    RectTransform rt;
    [SerializeField] PowerShapes octagon;

    private void Awake()
    {
        spriteShapeControl = GetComponent<SpriteShapeController>();
        rt = GetComponent<RectTransform>();
    }

    private void Start()
    {
        spriteShapeControl.spline.Clear();
        for (int i = 0; i < 8; i++)
        {
            spriteShapeControl.spline.InsertPointAt(i, octagon.GetPoints()[i] * 2);
            spriteShapeControl.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }

        StartCoroutine(UpDown());
    }

    IEnumerator UpDown()
    {
        while (true)
        {
            while (rt.position.y < -1)
            {
                rt.position += new Vector3(0, Time.deltaTime);
                yield return null;
            }
            while (rt.position.y > -4.5)
            {
                rt.position -= new Vector3(0, Time.deltaTime);
                yield return null;
            }
        }
    }
}
