using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : CursorArea
{
    [SerializeField] Transform gridCursor;
    Vector2 cursorLocation = new Vector2Int(6, 2);
    Vector2 originPosition = new Vector2Int(-28, -28);
    int cellSize = 8;
    [SerializeField] CellsController cellsController;

    public override void ActivateSelection() {
        cellsController.PlaceCell((int)cursorLocation.x, (int)cursorLocation.y, CounterType.whitecounter);
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
