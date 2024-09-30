using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;

    private void Start()
    {
        BGMSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        SFXSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnEnable()
    {
        BGMSlider.value = SoundManager.instance.BGMVolume;
        SFXSlider.value = SoundManager.instance.SFXVolume;
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
