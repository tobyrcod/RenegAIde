using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.U2D;
using UnityEngine.UI;

public class RenegadeUI : CursorArea
{
    Renegade renegade;

    HashSet<Vector2Int> possibleMoves = new HashSet<Vector2Int>();

    [SerializeField] MovesLeftUI movesLeftUI;
    [SerializeField] SpriteAtlas digitAtlas;

    [Space]

    [SerializeField] CursorArea downArea;
    [SerializeField] CursorArea rightArea;

    [Space]

    [SerializeField] Color cursorColorWhite;
    [SerializeField] Color cursorColorBlack;
    Vector2 cursorLocation = new Vector2Int(6, 2);
    Vector2 originPosition = new Vector2Int(-28, -28);
    [SerializeField] CellsController cellsController;

    [Space]

    [SerializeField] bool doesWhiteStart;
    [SerializeField] bool isPlayerWhite;
    int cellSize = 8;
    int width = 8;
    int height = 8;

    bool AIMoving = false;

    private void Awake() {
        cellsController.InitializeCellUIs();

        doesWhiteStart = GameManager.instance.doesPlayerStart;
        isPlayerWhite = GameManager.instance.isPlayerWhite;

        renegade = new Renegade(cellSize, cellSize, doesWhiteStart, UpdateCellsUIAtIndex);
        UpdateUI();

        if (isPlayerWhite) {
            cursorUI.SetColor(cursorColorWhite);
        }else {
            cursorUI.SetColor(cursorColorBlack);
        }

        if (isPlayerWhite != doesWhiteStart) {
            StartCoroutine(MakeAIMove(2f));
        }
    }

    private void UpdateCellsUIAtIndex(int x, int y, CounterType type) {
        cellsController.UpdateCell(x, y, type);
    }

    public void TryToApplyMove(Vector2Int move) {
        if (renegade.TryToApplyMove(move)) {
            UpdateUI();
            PlayCounterPlacedSFX();
            if (renegade.GameOver) {
                Debug.Log("Gameover");
            }
            else {
                if (renegade.isWhitesTurn != isPlayerWhite) {
                    StartCoroutine(MakeAIMove(0.1f));
                }
            }
        }
    }

    private void PlayCounterPlacedSFX() {
        SoundManager.Sound sound = (isPlayerWhite != renegade.isWhitesTurn) ? SoundManager.Sound.playerPlaceCounter : SoundManager.Sound.enemyPlaceCounter;
        SoundManager.PlaySound(sound);
    }

    public override void ActivateSelection() {
        if (!AIMoving) {
            if (renegade.isWhitesTurn == isPlayerWhite) {
                TryToApplyMove(new Vector2Int((int)cursorLocation.x, (int)cursorLocation.y));
            }
        }
    }

    private IEnumerator MakeAIMove(float waitTime) {
        AIMoving = true;
        yield return new WaitForSeconds(waitTime);
        //White is the Minimizing Player
        //Black is the Maximizing Player;

        //Logic
        //bool maximisingPlayer;
        //if (renegade.isWhitesTurn) {
        //    if (!isPlayerWhite) {
        //        //AI is white
        //        maximisingPlayer = false;
        //    }
        //}
        //else {
        //    //black turn
        //    if (isPlayerWhite) {
        //        //AI is black
        //        maximisingPlayer = true;
        //    }
        //}

        //therefore, maximisingPlayer = isPlayerWhite
        RenegadeAI.Minimax(renegade, 3, int.MinValue, int.MaxValue, isPlayerWhite);
        TryToApplyMove(RenegadeAI.BestMove);
        AIMoving = false;
    }

    private void UpdateUI() {
        UpdatePossibleMovesUI();
        UpdateMovesLeftUI();
    }

    private void UpdateMovesLeftUI() {
        int movesLeft = width * height - renegade.placedCounters;
        int digit1 = movesLeft / 10;
        int digit2 = movesLeft % 10;

        movesLeftUI.Digit1.sprite = digitAtlas.GetSprite(digit1.ToString());
        movesLeftUI.Digit2.sprite = digitAtlas.GetSprite(digit2.ToString());

        movesLeftUI.Digit1.SetNativeSize();
        movesLeftUI.Digit2.SetNativeSize();
    }

    private void UpdatePossibleMovesUI() {
        foreach (Vector2Int move in possibleMoves) {
            cellsController.CellUIs[move.x, move.y].CanMoveIcon(false);
        }
        possibleMoves = renegade.PossibleMoves;
        if (renegade.isWhitesTurn == isPlayerWhite) {
            foreach (Vector2Int move in possibleMoves) {
                cellsController.CellUIs[move.x, move.y].CanMoveIcon(true);
            }
        }
    }

    public override void MoveCursorDown() {
        if (cursorLocation.y == 0) {
            cursorController.ChangeCursorArea(downArea);
        }
        else if (cursorLocation.y >= 1) {
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
        if (cursorLocation.x == 7) {
            cursorController.ChangeCursorArea(rightArea);
        }
        else if (cursorLocation.x <= 6) {
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
        cursorUI.transform.localPosition = originPosition + cursorLocation * cellSize;
    }

    public override void Exit() {
        cursorUI.gameObject.SetActive(false);
    }

    public override void Enter() {
        cursorUI.gameObject.SetActive(true);
    }

    [Serializable]
    private struct MovesLeftUI {
        public Image Digit1;
        public Image Digit2;
    }
}
