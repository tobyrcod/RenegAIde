using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : CursorArea
{
    Renegade renegade;


    [SerializeField] Transform gridCursor;
    Vector2 cursorLocation = new Vector2Int(6, 2);
    Vector2 originPosition = new Vector2Int(-28, -28);
    int cellSize = 8;
    [SerializeField] CellsController cellsController;

    [Space]

    [SerializeField] bool doesWhiteStart;

    private void Awake() {
        cellsController.InitializeCellUIs();

        renegade = new Renegade(8, 8, doesWhiteStart, UpdateCellsUIAtIndex);
    }

    private void UpdateCellsUIAtIndex(int x, int y, CounterType type) {
        cellsController.UpdateCell(x, y, type);
    }

    public override void ActivateSelection() {
        renegade.PlaceCounter((int)cursorLocation.x, (int)cursorLocation.y);     
    }

    public override void MoveCursorDown() {
        if (cursorLocation.y >= 1) {
            cursorLocation.y--;
            UpdateCursorLocation();
        }
    }

    public override void MoveCursorLeft() {
        if (cursorLocation.x >= 1) {
            cursorLocation.x--;
            UpdateCursorLocation();
        }
    }

    public override void MoveCursorRight() {
        if (cursorLocation.x <= 6) {
            cursorLocation.x++;
            UpdateCursorLocation();
        }
    }

    public override void MoveCursorUp() {
        if (cursorLocation.y <= 6) {
            cursorLocation.y++;
            UpdateCursorLocation();
        }
    }

    public override void UpdateCursorLocation() {
        gridCursor.localPosition = originPosition + cursorLocation * cellSize;
    }
}
