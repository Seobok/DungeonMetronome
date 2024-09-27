using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;

    private void Start()
    {
        BGMSlider.value = SoundManager.instance.BGMVolume;
        SFXSlider.value = SoundManager.instance.SFXVolume;

        BGMSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        SFXSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    public void OnBGMVolumeChanged(float value)
    {
        SoundManager.instance.SetBGMVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        SoundManager.instance.SetSFXVolume(value);
    }
}
