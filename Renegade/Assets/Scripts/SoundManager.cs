using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class SoundManager 
{
    public static float PlaySound(Sound sound) {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        AudioClip audioClip = GameManager.instance.SoundAudioClipDictionary[sound];
        audioSource.PlayOneShot(audioClip);
        Object.Destroy(soundGameObject, audioClip.length * 2);
        return audioClip.length;
    }

    public static GameObject PlayBackgroundMusic(Sound sound) {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        AudioClip audioClip = GameManager.instance.SoundAudioClipDictionary[sound];
        audioSource.clip = audioClip;

        audioSource.Play();
        return soundGameObject;
    }

    public enum Sound {
        pressStart,
        gameBackground,
    }
}

[Serializable]
public class SoundAudioClip {
    public SoundManager.Sound sound;
    public AudioClip audioClip;
}

