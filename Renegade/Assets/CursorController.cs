using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] CursorArea currentCursorArea;
    private void Start() {
        currentCursorArea.UpdateCursorLocation();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            currentCursorArea.MoveCursorUp();
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            currentCursorArea.MoveCursorLeft();
        }

        else if (Input.GetKeyDown(KeyCode.S)) {
            currentCursorArea.MoveCursorDown();
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            currentCursorArea.MoveCursorRight();
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            currentCursorArea.ActivateSelection();
        }
    }
}

[Serializable]
public abstract class CursorArea : MonoBehaviour {
    public abstract void MoveCursorUp();
    public abstract void MoveCursorDown();
    public abstract void MoveCursorLeft();
    public abstract void MoveCursorRight();
    public abstract void UpdateCursorLocation();
    public abstract void ActivateSelection();
}
