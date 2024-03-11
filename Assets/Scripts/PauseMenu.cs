using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider master, bgm, sfx;

    private void Awake()
    {
        audioMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol", Mathf.Log10(0.5f) * 20));
        audioMixer.SetFloat("BGMVol", PlayerPrefs.GetFloat("BGMVol", Mathf.Log10(1f) * 20));
        audioMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol", Mathf.Log10(1f) * 20));
    }
    public void UpdateVolume()
    {
        audioMixer.GetFloat("MasterVol", out float masterVol);
        audioMixer.GetFloat("BGMVol", out float bgmVol);
        audioMixer.GetFloat("SFXVol", out float sfxVol);
        master.value = Mathf.Pow(10, masterVol / 20);
        bgm.value = Mathf.Pow(10, bgmVol / 20);
        sfx.value = Mathf.Pow(10, sfxVol / 20);
    }
    public void SetMasterVol(float vol)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("MasterVol", Mathf.Log10(vol) * 20);
    }
    public void SetBGMVol(float vol)
    {
        audioMixer.SetFloat("BGMVol", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("BGMVol", Mathf.Log10(vol) * 20);
    }

    public void SetSFXVol(float vol)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("SFXVol", Mathf.Log10(vol) * 20);
    }
}
