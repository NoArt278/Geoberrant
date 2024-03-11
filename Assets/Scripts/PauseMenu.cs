using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider master, bgm, sfx;

    private void OnEnable()
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
