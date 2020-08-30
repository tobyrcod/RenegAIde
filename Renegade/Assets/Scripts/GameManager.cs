﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    private int activeSceneIndex = -1;

    public SoundAudioClip[] playerSoundArray;
    public Dictionary<SoundManager.Sound, AudioClip> SoundAudioClipDictionary = new Dictionary<SoundManager.Sound, AudioClip>();

    private void Start() {
        if (instance == null)
            instance = this;

        if (playerSoundArray != null) {
            foreach (SoundAudioClip soundAudioClip in playerSoundArray) {
                SoundAudioClipDictionary.Add(soundAudioClip.sound, soundAudioClip.audioClip);
            }
        }

        AddScene("Menu");
    }

    public void AddScene(string loadSceneName) {
        //Here we are asking the SceneManager to load the desired scene. In this instance we're providing it our variable 'currentScene'
        SceneManager.LoadScene(loadSceneName, LoadSceneMode.Additive);
    }

    public void ChangeScene(string newScene) {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        AddScene(newScene);
    }
}
