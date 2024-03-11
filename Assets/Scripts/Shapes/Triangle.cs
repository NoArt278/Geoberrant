using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class Triangle : PowerShapes
{
    Rigidbody2D rb;
    const float initialLaserWidth = 0.1f, firedLaserWidth = 1f, laserFireTime = 0.8f, laserDist = 100f, recoil = 50f;
    Gradient laserGradient;
    [SerializeField] GameObject laser;
    List<GameObject> lasers;
    List<SpriteRenderer> laserRenderers;
    List<LineRenderer> laserLines;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;
    Vector2 mousePos;
    public void Awake()
    {
        SetPoints(new List<Vector2>
        {
            new Vector2(-1f,0),
            new Vector2(0, Mathf.Sqrt(3)),
            new Vector2(1f, 0),
        });
        SetMass(20);
        SetRollSpeed(200);
        SetJumpHeight(30);
        SetMoveSpeed(20);
        SetScaleOffset(0.4f);
        SetBounciness(0);
        SetGravScale(0.5f);
        SetDef(1);
        SetTangentMode(ShapeTangentMode.Linear);
        SetColor(Color.green);

        laserGradient = new Gradient();
        colorKey = new GradientColorKey[2];
        alphaKey = new GradientAlphaKey[2];

        colorKey[0] = new GradientColorKey(Color.red, 0.0f);
        colorKey[1] = new GradientColorKey(Color.red, 1.0f);

        alphaKey[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphaKey[1] = new GradientAlphaKey(0.0f, 1.0f);

        laserGradient.SetKeys(colorKey, alphaKey);

        rb = GetComponent<Rigidbody2D>();

        // Set up lasers
        lasers = new List<GameObject>();
        laserRenderers = new List<SpriteRenderer>();
        laserLines = new List<LineRenderer>();
        for (int i=0; i<GetPoints().Count; i++)
        {
            GameObject newLaser = Instantiate(laser, transform);
            LineRenderer newLaserLine = newLaser.GetComponent<LineRenderer>();
            SpriteRenderer laserRender = newLaser.GetComponent<SpriteRenderer>();

            newLaser.transform.localPosition = GetPoints()[i] * 2;
            laserRender.color = laserGradient.Evaluate(1f);
            newLaserLine.startWidth = initialLaserWidth;
            newLaserLine.endWidth = initialLaserWidth;
            newLaserLine.startColor = laserGradient.Evaluate(0.5f);
            newLaserLine.endColor = laserGradient.Evaluate(0.5f);

            lasers.Add(newLaser);
            laserRenderers.Add(laserRender);
            laserLines.Add(newLaserLine);

            newLaser.SetActive(false);
        }
    }

    public void ActivateLaser()
    {
        foreach(var laser in lasers)
        {
            laser.SetActive(true);
        }
    }

    public void DeactivateLaser()
    {
        foreach (var laser in lasers)
        {
            laser.SetActive(false);
        }
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        for (int i=0; i < laserLines.Count; i++)
        {
            laserLines[i].SetPosition(0, lasers[i].transform.position);
            laserLines[i].SetPosition(1, mousePos);
        }
    }

    public override void ActivatePower()
    {
        StartCoroutine(FireLaser());
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        rb.velocity = (new Vector2(transform.position.x, transform.position.y) - mousePos).normalized * recoil;
    }

    IEnumerator FireLaser()
    {
        float fireTime = Time.time;
        for (int i = 0; i < laserLines.Count; i++)
        {
            laserLines[i].startWidth = firedLaserWidth/2;
            laserLines[i].endWidth = firedLaserWidth/2;
        }
        float currDuration = Time.time - fireTime;
        GameObject giantLaser = Instantiate(laser, mousePos, Quaternion.identity);
        giantLaser.tag = "Laser";
        LineRenderer giantLaserLine = giantLaser.GetComponent<LineRenderer>();
        giantLaserLine.startWidth = firedLaserWidth;
        giantLaserLine.endWidth = firedLaserWidth;
        giantLaserLine.SetPosition(0, mousePos);
        giantLaserLine.SetPosition(1, mousePos + (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized * laserDist);
        EdgeCollider2D laserCollider = giantLaser.GetComponent<EdgeCollider2D>();
        laserCollider.enabled = true;
        List<Vector2> linePoints = new()
        {
            Vector2.zero, giantLaserLine.GetPosition(1) - new Vector3(mousePos.x, mousePos.y),
        };
        laserCollider.SetPoints(linePoints);
        laserCollider.edgeRadius = 1;

        while (currDuration < laserFireTime)
        {
            for (int i = 0; i < laserLines.Count; i++)
            {
                laserRenderers[i].color = laserGradient.Evaluate(currDuration / laserFireTime);
                laserLines[i].startColor = laserGradient.Evaluate((currDuration / 2) / laserFireTime);
                laserLines[i].endColor = laserGradient.Evaluate((currDuration / 2) / laserFireTime);
            }
            giantLaserLine.startColor = laserGradient.Evaluate(currDuration / laserFireTime);
            giantLaserLine.endColor = laserGradient.Evaluate(currDuration / laserFireTime);
            currDuration = Time.time - fireTime;
            yield return null;
        }
        for (int i = 0; i < laserLines.Count; i++)
        {
            laserLines[i].startWidth = initialLaserWidth;
            laserLines[i].endWidth = initialLaserWidth;
            laserLines[i].startColor = laserGradient.Evaluate(0.5f);
            laserLines[i].endColor = laserGradient.Evaluate(0.5f);
        }
        laserCollider.enabled = false;
        Destroy(giantLaser);
    }
}
