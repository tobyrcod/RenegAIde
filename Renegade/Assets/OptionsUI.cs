using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : CursorArea {

    [SerializeField] CursorArea upArea;

    public override void ActivateSelection() {
        GameManager.instance.ChangeScene("Menu");
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
