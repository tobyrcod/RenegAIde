using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class SettingsMainArea : CursorArea {

    [SerializeField] TogglesController togglesController;
    [SerializeField] Vector2Int cursorIndex;
    [SerializeField] CursorArea downArea;

    bool isPlayerWhite = true;
    bool isPlayerFirst = true;
    bool isAIActive = true;

    private List<Image> toggles = new List<Image>();
    private SoundManager.Sound[] sounds = { SoundManager.Sound.settings1, SoundManager.Sound.settings2, SoundManager.Sound.settings3 };
    private Random rnd = new Random();

    private void Awake() {
        toggles.Add(togglesController.WhiteToggle);
        toggles.Add(togglesController.BlackToggle);

        toggles.Add(togglesController.FirstToggle);
        toggles.Add(togglesController.SecondToggle);

        toggles.Add(togglesController.AIToggle);
    }

    private Vector3 GetCursorPosition() {
        int toggleIndex = GetToggleIndex();
        return toggles[toggleIndex].transform.position;
    }

    private int GetToggleIndex() {
        if (cursorIndex.y == -1) {
            return 4;
        }
        else {
            return cursorIndex.x;
        }
    }

    public override void ActivateSelection() {
        int toggleIndex = GetToggleIndex();
        if (toggleIndex == 0) {
            if (!isPlayerWhite) {
                isPlayerWhite = true;
                togglesController.ToggleUI(toggles[0], true);
                togglesController.ToggleUI(toggles[1], false);
            }
        }
        else if (toggleIndex == 1) {
            if (isPlayerWhite) {
                isPlayerWhite = false;
                togglesController.ToggleUI(toggles[0], false);
                togglesController.ToggleUI(toggles[1], true);
            }
        }
        else if (toggleIndex == 2) {
            if (!isPlayerFirst) {
                isPlayerFirst = true;
                togglesController.ToggleUI(toggles[2], true);
                togglesController.ToggleUI(toggles[3], false);
            }
        }
        else if (toggleIndex == 3) {
            if (isPlayerFirst) {
                isPlayerFirst = false;
                togglesController.ToggleUI(toggles[2], false);
                togglesController.ToggleUI(toggles[3], true);
            }
        }
        else if (toggleIndex == 4) {
            isAIActive = !isAIActive;
            togglesController.ToggleUI(toggles[4], isAIActive);
        }

        PlayRandomSound();
    }

    private void PlayRandomSound() {
        int soundIndex = rnd.Next(0, 3);
        SoundManager.PlaySound(sounds[soundIndex]);
    }

    public override void Enter() {
        cursorUI.gameObject.SetActive(true);
    }

    public override void Exit() {
        cursorUI.gameObject.SetActive(false);

        GameManager.instance.isPlayerWhite = isPlayerWhite;
        GameManager.instance.doesPlayerStart = isPlayerFirst;
        GameManager.instance.isAIActive = isAIActive;
    }

    public override void MoveCursorDown() {
        if (cursorIndex.y == -1) {
            cursorController.ChangeCursorArea(downArea);
        }
        else {
            cursorIndex.y = -1;
        }

        UpdateCursorLocation();
    }

    public override void MoveCursorLeft() {
        cursorIndex.x -= 1;
        if (cursorIndex.x < 0)
            cursorIndex.x = 3;

        UpdateCursorLocation();
    }

    public override void MoveCursorRight() {
        cursorIndex.x += 1;
        if (cursorIndex.x > 3)
            cursorIndex.x = 0;

        UpdateCursorLocation();
    }

    public override void MoveCursorUp() {
        cursorIndex.y = 0;

        UpdateCursorLocation();
    }

    public override void UpdateCursorLocation() {
        cursorUI.transform.position = GetCursorPosition();
    }
}
