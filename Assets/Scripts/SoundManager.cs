using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource BGMAudioSource;
    [SerializeField] private AudioSource SFXAudioSource;

    private Dictionary<string, AudioClip> BGMClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> SFXClips = new Dictionary<string, AudioClip>();

    [System.Serializable]
    public struct NamedAudioClip
    {
        public string name;
        public AudioClip clip;
    }

    public NamedAudioClip[] BGMClipList;
    public NamedAudioClip[] SFXClipList;

    [HideInInspector] public float BGMVolume = 1f;
    [HideInInspector] public float SFXVolume = 1f;

    private Coroutine currentBGMCoroutine;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeAudioClips();
    }

    private void Start()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        if(SceneName == "Mainmenu")
        {
            PlayBGM("Mainmenu", 1f);
        }
    }

    private void InitializeAudioClips()
    {
        foreach(var bgm in BGMClipList)
        {
            if(!BGMClips.ContainsKey(bgm.name))
            {
                BGMClips.Add(bgm.name, bgm.clip);
            }
        }

        foreach(var sfx in  SFXClipList)
        {
            if(!SFXClips.ContainsKey(sfx.name))
            {
                SFXClips.Add(sfx.name, sfx.clip);
            }
        }
    }

    public void PlayBGM(string clip, float fadeDuration = 1f)
    {
        if(BGMClips.ContainsKey(clip))
        {
            if(currentBGMCoroutine != null)
            {
                StopCoroutine(currentBGMCoroutine);
            }

            currentBGMCoroutine = StartCoroutine(FadeOutBGM(fadeDuration, () =>
            {
                BGMAudioSource.clip = BGMClips[clip];
                BGMAudioSource.Play();
                currentBGMCoroutine = StartCoroutine(FadeInBGM(fadeDuration));
            }));
            
        }
        else
        {
            Debug.LogError($"해당 이름의 배경음이 존재하지 않습니다. : {clip}");
        }
    }

    public void PlaySFX(string clip)
    {
        if(SFXClips.ContainsKey(clip))
        {
            SFXAudioSource.PlayOneShot(SFXClips[clip]);
        }
        else
        {
            Debug.LogError($"해당 이름의 효과음이 존재하지 않습니다. : {clip}");
        }
    }

    public void StopBGM()
    {
        BGMAudioSource.Stop();
    }

    public void StopSFX()
    {
        SFXAudioSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        BGMVolume = Mathf.Clamp(volume, 0f, 1f);
        BGMAudioSource.volume = BGMVolume;
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = Mathf.Clamp(volume, 0f, 1f);
        SFXAudioSource.volume = SFXVolume;
    }

    private IEnumerator FadeOutBGM(float duration, Action onFadeComplete)
    {
        float startVolume = BGMVolume;
        for(float t = 0; t<duration; t+= Time.deltaTime)
        {
            BGMAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);

            yield return null;
        }

        BGMAudioSource.volume = 0f;
        onFadeComplete?.Invoke();
    }

    private IEnumerator FadeInBGM(float duration)
    {
        float startVolume = 0f;
        float endVolume = BGMVolume;
        
        for(float t = 0; t< duration; t+= Time.deltaTime)
        {
            BGMAudioSource.volume = Mathf.Lerp(startVolume, endVolume, t / duration);
            yield return null;
        }

        BGMAudioSource.volume = BGMVolume;
    }
}
