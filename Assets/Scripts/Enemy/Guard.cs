using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Enemy
{
    Coroutine shoot;
    [SerializeField] GameObject rifle, bullet;
    private void Start()
    {
        SetMoveSpeed(10f);
        SetPointWorth(10);
        shoot = StartCoroutine(Shoot());
    }

    private void FixedUpdate()
    {
        if (GetIsDefeated() && shoot!= null)
        {
            StopCoroutine(shoot);
            shoot = null;
        }
        if (!GetIsDefeated())
        {
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
}
