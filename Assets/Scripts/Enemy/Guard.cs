using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Enemy
{
    Coroutine patrol, shoot;
    [SerializeField] GameObject rifle, bullet;
    private void Start()
    {
        SetMoveSpeed(10f);
        patrol = StartCoroutine(Patrol(Mathf.Sign(Random.Range(-1,1))));
    }

    private void FixedUpdate()
    {
        if (GetIsDefeated())
        {
            if (patrol != null)
            {
                StopCoroutine(patrol);
                patrol = null;
            }
            if (shoot != null)
            {
                StopCoroutine(shoot);
                shoot = null;
            }
        }
        if (GetSeePlayer() && !GetIsDefeated())
        {
            if (patrol != null)
            {
                StopCoroutine(patrol);
                patrol = null;
                shoot = StartCoroutine(Shoot());
            }

            // Face player and move toward player if distance is big
            Vector3 dir;
            float currYRotation = transform.rotation.eulerAngles.y;
            dir = GetPlayer().position - rifle.transform.position;
            if ((dir.x < 0 && currYRotation == 0) || (dir.x > 0 && currYRotation != 0))
            {
                transform.rotation = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + 180) % 360, 0);
            }
            currYRotation = transform.rotation.eulerAngles.y;
            if (Vector2.Distance(transform.position, GetPlayer().position) > 20)
            {
                GetRB().velocity = new Vector2((currYRotation != 0 ? -1 : 1) * GetMoveSpeed(), 0);
            } else
            {
                // Stop when close enough
                GetRB().velocity = Vector2.zero;
            }

            // Aim rifle
            if (currYRotation != 0)
            {
                dir = rifle.transform.position - GetPlayer().position;
            }
            dir.Normalize();
            float rotZAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg * (currYRotation != 0 ? -1 : 1);
            Vector3 rotation = rifle.transform.rotation.eulerAngles;
            rotation.z = rotZAngle;
            rifle.transform.rotation = Quaternion.Euler(rotation);
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            GameObject newBullet = Instantiate(bullet, rifle.transform.position, rifle.transform.rotation);
            Rigidbody2D bulletRB = newBullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = newBullet.transform.right * 30f;
            yield return new WaitForSeconds(3f);
            if (newBullet != null)
            {
                Destroy(newBullet);
            }
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
