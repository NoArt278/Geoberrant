using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] PlayerMovement pm;
    const float minYPos = 0;
    void Update()
    {
        float camY = player.transform.position.y + 2;
        camY = Mathf.Clamp(camY, minYPos, Mathf.Infinity);
        transform.position = new Vector3(player.transform.position.x + 3, camY, transform.position.z);
    }
}
