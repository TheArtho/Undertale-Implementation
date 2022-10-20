using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
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

    private AudioSource source;
    
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
            source = GetComponent<AudioSource>();
        }
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1)
    {
        source.PlayOneShot(clip, volumeScale);
    }
}
