using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsStartArea : CursorArea {

    [SerializeField] CursorArea upArea;

    bool pressed;

    private void Start() {
        pressed = false;
    }

    public override void ActivateSelection() {
        if (!pressed) {
            pressed = true;
            SoundManager.PlaySound(SoundManager.Sound.pressStart);
            StartCoroutine(StartGame(1f));
        }
    }

    private IEnumerator StartGame(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        GameManager.instance.ChangeScene("Game");
    }

    public override void Enter() {
        cursorUI.gameObject.SetActive(true);
    }

    public override void Exit() {
        cursorUI.gameObject.SetActive(false);
    }

    public override void MoveCursorDown() {
        //
    }

    public override void MoveCursorLeft() {
        //
    }

    public override void MoveCursorRight() {
        //
    }

    public override void MoveCursorUp() {
        cursorController.ChangeCursorArea(upArea);
    }

    public override void UpdateCursorLocation() {
        //
    }
}
