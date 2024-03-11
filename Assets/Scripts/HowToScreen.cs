using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToScreen : MonoBehaviour
{
    [SerializeField] GameObject startScreen;

    public void BackToStart()
    {
        startScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
