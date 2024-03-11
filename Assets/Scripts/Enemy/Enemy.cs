using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    new BoxCollider2D collider;
    bool isDefeated, seePlayer;
    SpriteRenderer spriteRenderer;
    float moveSpeed, pointWorth;
    Transform player;
    RaycastHit2D[] hitList;
    GameManager gm;
    AudioSource audioSource;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isDefeated = false;
        seePlayer = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Getters and setters
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
    public void SetPointWorth(float pointWorth)
    {
        this.pointWorth = pointWorth;
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
    public float GetPointWorth()
    {
        return pointWorth;
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
            int collideDir = (collision.transform.position.x < transform.position.x) ? 1 : -1;
            rb.velocity = new Vector2(Random.Range(10,30) * collideDir, Random.Range(10,30));
            player.GetComponent<PlayerMovement>().Heal(GetPointWorth() / 2); // Heal player when killed
            gm.AddScore(GetPointWorth()); // Add to score when killed
            if (gm.GetHitSoundCount() < 3)
            {
                audioSource.Play();
                gm.AddHitSoundCount();
                StartCoroutine(HitSoundStopped());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            isDefeated = true;
            spriteRenderer.color = Color.black;
            collider.enabled = false;
            rb.velocity = (new Vector2(Random.Range(10, 30) * Mathf.Sign(Random.Range(-1,1)), Random.Range(10, 30)));
            player.GetComponent<PlayerMovement>().Heal(GetPointWorth() / 2);
            gm.AddScore(GetPointWorth()); // Add to score when killed
            if (gm.GetHitSoundCount() < 3)
            {
                audioSource.Play();
                gm.AddHitSoundCount();
                StartCoroutine(HitSoundStopped());
            }
        }
    }

    IEnumerator HitSoundStopped()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        gm.ReduceHitSoundCount();
    }

    private void OnBecameInvisible()
    {
        if (isDefeated)
        {
            if (audioSource.isPlaying)
            {
                gm.ReduceHitSoundCount();
            }
            Destroy(gameObject);
        }
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
                    break;
                }
            }
        }
    }
}
