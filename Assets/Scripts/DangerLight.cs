using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLight : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, 50 * Time.deltaTime);
    }
}
