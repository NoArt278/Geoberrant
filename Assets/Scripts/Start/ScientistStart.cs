using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistStart : MonoBehaviour
{
    Rigidbody2D rb;
    float moveSpeed = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Patrol(Mathf.Sign(Random.Range(-1, 1))));
    }

    IEnumerator Patrol(float startDir)
    {
        float dir = startDir;
        if (dir < 0)
        {
            transform.rotation = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + 180) % 360, 0);
        }
        float prevChangeDirTime = Time.time;
        float changeDirInterval = Random.Range(2f, 5f);
        while (true)
        {
            if (Time.time - prevChangeDirTime >= changeDirInterval) // Flip direction every random seconds
            {
                transform.rotation = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + 180) % 360, 0);
                dir *= -1;
                prevChangeDirTime = Time.time;
                changeDirInterval = Random.Range(2f, 5f);
            }
            rb.velocity = new Vector2(dir * moveSpeed, 0);
            yield return null;
        }
    }
}
