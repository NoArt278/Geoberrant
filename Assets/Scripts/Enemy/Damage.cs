using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage;

    private void Awake()
    {
        Invoke("DestroySelf", 3f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
