using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    new BoxCollider2D collider;
    bool isDefeated, seePlayer;
    float moveSpeed;
    Transform player;
    RaycastHit2D[] hitList;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        isDefeated = false;
        seePlayer = false;
    }

    // Getters and setters
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public bool GetIsDefeated()
    {
        return isDefeated;
    }
    public bool GetSeePlayer()
    {
        return seePlayer;
    }
    public Rigidbody2D GetRB()
    {
        return rb;
    }
    public Transform GetPlayer()
    {
        return player;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isDefeated = true;
            collider.enabled = false;
            rb.velocity = (collision.relativeVelocity + new Vector2(0, Random.Range(5,20))) * 15;
            StartCoroutine(DestroySelf());
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!seePlayer)
        {
            hitList = Physics2D.RaycastAll(transform.position, transform.right, 10f);
            foreach (RaycastHit2D hit in hitList)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    seePlayer = true;
                    player = hit.transform;
                    break;
                }
            }
        }
    }
}
