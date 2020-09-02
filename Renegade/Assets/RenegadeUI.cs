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

    [SerializeField] DigitsUI movesLeftUI;
    [SerializeField] SpriteAtlas digitAtlas;

    [Space]

    [SerializeField] CursorArea downArea;
    [SerializeField] CursorArea rightArea;
    [SerializeField] GameOverArea gameOverArea;

    [Space]

    [SerializeField] Color cursorColorWhite;
    [SerializeField] Color cursorColorBlack;
    Vector2 cursorLocation = new Vector2Int(6, 2);
    Vector2 originPosition = new Vector2Int(-28, -28);
    [SerializeField] CellsController cellsController;

    [Space]

    bool doesPlayerStart;
    bool isPlayerWhite;
    bool isAIActive;

    int cellSize = 8;
    int width = 8;
    int height = 8;

    bool AIMoving = false;

    private void Awake() {
        cellsController.InitializeCellUIs();

        doesPlayerStart = GameManager.instance.doesPlayerStart;
        isPlayerWhite = GameManager.instance.isPlayerWhite;
        isAIActive = GameManager.instance.isAIActive;

        renegade = new Renegade(cellSize, cellSize, (doesPlayerStart == isPlayerWhite), UpdateCellsUIAtIndex);
        UpdateUI();

        if (isPlayerWhite) {
            cursorUI.SetColor(cursorColorWhite);
        }else {
            cursorUI.SetColor(cursorColorBlack);
        }

        if (!doesPlayerStart) {
            StartCoroutine(MakeAIMove(2f, isPlayerWhite));
        }
    }

    private void UpdateCellsUIAtIndex(int x, int y, CounterType type) {
        cellsController.UpdateCell(x, y, type);
    }

    public void TryToApplyMove(Vector2Int move) {
        if (!renegade.GameOver) {
            if (renegade.TryToApplyMove(move)) {
                UpdateUI();
                PlayCounterPlacedSFX();
                if (renegade.GameOver) {
                    GameOver();
                }
                else {
                    if (isAIActive) {
                        if (renegade.isWhitesTurn != isPlayerWhite) {
                            StartCoroutine(MakeAIMove(0f, isPlayerWhite));
                        }
                    }
                }
            }
        }
    }

    private void GameOver() {
        renegade.GetCounterCount(out int whiteCount, out int blackCount);
        if (doesPlayerStart == isPlayerWhite) {
            gameOverArea.SetUp(whiteCount, blackCount, isAIActive);
        }
        else {
            gameOverArea.SetUp(blackCount, whiteCount, isAIActive);
        }
    }


    private void PlayCounterPlacedSFX() {
        SoundManager.Sound sound = (isPlayerWhite != renegade.isWhitesTurn) ? SoundManager.Sound.playerPlaceCounter : SoundManager.Sound.enemyPlaceCounter;
        SoundManager.PlaySound(sound);
    }

    public override void ActivateSelection() {
        if (!renegade.GameOver) {
            if (!AIMoving) {
                if (renegade.isWhitesTurn == isPlayerWhite || !isAIActive) {
                    TryToApplyMove(new Vector2Int((int)cursorLocation.x, (int)cursorLocation.y));
                }
            }
        }
    }

    private IEnumerator MakeAIMove(float waitTime, bool maximizingPlayer) {
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
        RenegadeAI.Minimax(renegade, 4, int.MinValue, int.MaxValue, maximizingPlayer);
        Debug.Log(RenegadeAI.BestMove);
        TryToApplyMove(RenegadeAI.BestMove);
        AIMoving = false;
    }

    private void UpdateUI() {
        UpdatePossibleMovesUI();
        UpdateMovesLeftUI();
        if (!isAIActive) {
            Color color = (renegade.isWhitesTurn) ? cursorColorWhite : cursorColorBlack;
            cursorUI.SetColor(color);
        }
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
        if (!renegade.GameOver) {
            foreach (Vector2Int move in possibleMoves) {
                cellsController.CellUIs[move.x, move.y].CanMoveIcon(false);
            }
            possibleMoves = renegade.PossibleMoves;
            if (renegade.isWhitesTurn == isPlayerWhite || !isAIActive) {
                Color color = (renegade.isWhitesTurn) ? cursorColorWhite : cursorColorBlack;
                foreach (Vector2Int move in possibleMoves) {
                    CellUI cellUI = cellsController.CellUIs[move.x, move.y];
                    cellUI.SetColor(color);
                    cellUI.CanMoveIcon(true);
                }
            }
        }
        else {
            foreach (Vector2Int move in possibleMoves) {
                cellsController.CellUIs[move.x, move.y].CanMoveIcon(false);
            }
            cursorController.ChangeCursorArea(gameOverArea);
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
}

[Serializable]
public struct DigitsUI {
    public Image Digit1;
    public Image Digit2;
}
