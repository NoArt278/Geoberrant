using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] GameObject howToScreen;
    private void Awake()
    {
        audioMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol", Mathf.Log10(0.5f) * 20));
        audioMixer.SetFloat("BGMVol", PlayerPrefs.GetFloat("BGMVol", Mathf.Log10(1f) * 20));
        audioMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol", Mathf.Log10(1f) * 20));
        audioMixer.SetFloat("CoinVol", PlayerPrefs.GetFloat("CoinVol", Mathf.Log10(1f) * 20));
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HowTo()
    {
        howToScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
