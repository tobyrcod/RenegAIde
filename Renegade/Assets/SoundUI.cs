using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : CursorArea {

    [SerializeField] CursorArea downArea;
    [SerializeField] CursorArea leftArea;

    [SerializeField] Image soundOnImage;
    [SerializeField] Image soundOffImage;

    private void Start() {
        UpdateIcon();
    }

    private void UpdateIcon() {
        if (GameManager.instance.isSoundOn) {
            soundOnImage.gameObject.SetActive(true);
            soundOffImage.gameObject.SetActive(false);
        }
        else {
            soundOnImage.gameObject.SetActive(false);
            soundOffImage.gameObject.SetActive(true);
        }
    }

    public override void ActivateSelection() {
        GameManager.instance.ToggleSound();
        UpdateIcon();
    }

    public override void Enter() {
        cursorUI.gameObject.SetActive(true);
    }

    public override void Exit() {
        cursorUI.gameObject.SetActive(false);
    }

    public override void MoveCursorDown() {
        cursorController.ChangeCursorArea(downArea);
    }

    public override void MoveCursorLeft() {
        cursorController.ChangeCursorArea(leftArea);
    }

    public override void MoveCursorRight() {
        //
    }

    public override void MoveCursorUp() {
        //
    }

    public override void UpdateCursorLocation() {
        //
    }
}
