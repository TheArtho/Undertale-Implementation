using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioHandler : MonoBehaviour
{
    private enum Index
    {
        Sfx = 0,
        Bgm = 1
    }
    
    private static AudioHandler _main;

    public static AudioHandler Main
    {
        get => _main;
        set
        {
            if (_main == null)
            {
                _main = value;
            }
        }
    }
    
    [Range(0,1)] public float volumeSfx = 1;
    [Range(0,1)] public float volumeBGM = 1;
    
    private AudioSource _sourceSfx;
    private AudioSource _sourceBGM;

    void Awake()
    {
        Main = this;

        if (Main != this)
        {
            Debug.LogWarning("[Audio Handler] Audio instance already existing");
            Destroy(gameObject);
        }
        else
        {
            _sourceSfx = GetComponents<AudioSource>()[(int) Index.Sfx];
            _sourceBGM = GetComponents<AudioSource>()[(int) Index.Bgm];
        }
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1)
    {
        _sourceSfx.PlayOneShot(clip, volumeScale*volumeSfx);
    }

    public void PlayBGM(AudioClip clip, float volumeScale = 1)
    {
        _sourceBGM.volume = volumeBGM*volumeScale;
        _sourceBGM.clip = clip;
        _sourceBGM.loop = true;
        _sourceBGM.Play();
    }

    public void StopBGM()
    {
        _sourceBGM.Stop();
    }
}
