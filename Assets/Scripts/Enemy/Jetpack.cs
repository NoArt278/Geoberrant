using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : Enemy
{
    Coroutine shoot;
    [SerializeField] GameObject bazooka, missile;
    // Start is called before the first frame update
    void Start()
    {
        SetMoveSpeed(20f);
        SetPointWorth(20);
        shoot = StartCoroutine(Shoot());
    }

    private void FixedUpdate()
    {
        if (GetIsDefeated() && shoot != null)
        {
            StopCoroutine(shoot);
            shoot = null;
        }
        if (!GetIsDefeated())
        {
            // Face player and move toward player if distance is big
            Vector3 dir;
            float currYRotation = transform.rotation.eulerAngles.y;
            dir = GetPlayer().position - bazooka.transform.position;
            if ((dir.x < 0 && currYRotation == 0) || (dir.x > 0 && currYRotation != 0))
            {
                transform.rotation = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + 180) % 360, 0);
            }
            currYRotation = transform.rotation.eulerAngles.y;
            if (Vector2.Distance(transform.position, GetPlayer().position) > 20)
            {
                GetRB().velocity = new Vector2((currYRotation != 0 ? -1 : 1) * GetMoveSpeed(), 
                    Mathf.Sign(GetPlayer().position.y - transform.position.y) * GetMoveSpeed());
            }
            else
            {
                // Stop when close enough
                GetRB().velocity = Vector2.zero;
            }

            // Aim bazooka
            if (currYRotation != 0)
            {
                dir = bazooka.transform.position - GetPlayer().position;
            }
            dir.Normalize();
            float rotZAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg * (currYRotation != 0 ? -1 : 1);
            Vector3 rotation = bazooka.transform.rotation.eulerAngles;
            rotation.z = rotZAngle;
            bazooka.transform.rotation = Quaternion.Euler(rotation);
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            GameObject newBullet = Instantiate(missile, bazooka.transform.position, bazooka.transform.rotation);
            Rigidbody2D bulletRB = newBullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = newBullet.transform.right * 20f;
            yield return new WaitForSeconds(3f);
        }
    }
}
