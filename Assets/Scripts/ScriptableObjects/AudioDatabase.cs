using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatabase",menuName = "Audio/Database")]
public class AudioDatabase : ScriptableObject
{
    [Serializable]
    public class AudioAsset
    {
        public string key;
        public AudioClip value;
    }

    public List<AudioAsset> database;

    public AudioClip Get(string key)
    {
        return database.Find(x => x.key == key).value;
    }
}
