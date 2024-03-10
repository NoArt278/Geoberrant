using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] PlayerMovement pm;
    const float minYPos = 0;
    void Update()
    {
        float camY = Mathf.Lerp(transform.position.y, player.transform.position.y + 2, 8*Time.deltaTime);
        if (player.transform.position.y < 10)
        {
            camY = Mathf.Lerp(transform.position.y, 1.5f, 8*Time.deltaTime);
        }
        camY = Mathf.Clamp(camY, minYPos, Mathf.Infinity);
        transform.position = new Vector3(player.transform.position.x + 3, camY, transform.position.z);
    }
}
