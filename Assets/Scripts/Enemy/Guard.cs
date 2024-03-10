using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Enemy
{
    Coroutine patrol;
    [SerializeField] GameObject rifle, bullet;
    private void Start()
    {
        SetMoveSpeed(10f);
        patrol = StartCoroutine(Patrol(Mathf.Sign(Random.Range(-1,1))));
    }

    private void FixedUpdate()
    {
        if (GetIsDefeated() && patrol != null)
        {
            StopCoroutine(patrol);
            patrol = null;
        }
        if (GetSeePlayer() && !GetIsDefeated())
        {
            if (patrol != null)
            {
                StopCoroutine(patrol);
                patrol = null;
            }
            GetRB().velocity = new Vector2((transform.rotation.eulerAngles.y != 0 ? -1 : 1) * GetMoveSpeed() * 2, 0);
        }
    }

    IEnumerator Patrol(float startDir)
    {
        float dir = startDir;
        if (dir < 0)
        {
            transform.rotation = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + 180) % 360, 0);
        }
        float prevChangeDirTime = Time.time;
        while (true)
        {
            if (Time.time - prevChangeDirTime >= 3) // Flip direction every 3 seconds
            {
                transform.rotation = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + 180) % 360, 0);
                dir *= -1;
                prevChangeDirTime = Time.time;
            }
            GetRB().velocity = new Vector2(dir * GetMoveSpeed(), 0);
            yield return null;
        }
    }
}
