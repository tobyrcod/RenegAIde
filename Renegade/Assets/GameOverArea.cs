using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameOverArea : CursorArea
{
    int selectedIndex = 0;

    [Space]
    [SerializeField] Image winnerImage;
    [SerializeField] Sprite player1WinnerSprite;
    [SerializeField] Sprite player2WinnerSprite;
    [SerializeField] Sprite playerAIWinnerSprite;
    [Space]
    [SerializeField] Image player2IconImage;
    [SerializeField] Sprite player2WinnerIconSprite;
    [SerializeField] Sprite playerAIWinnerIconSprite;
    [Space]
    [SerializeField] SpriteAtlas digitAtlas;
    [SerializeField] DigitsUI player1DigitsUI;
    [SerializeField] DigitsUI player2DigitsUI;
    [Space]
    [SerializeField] Transform retry;
    [SerializeField] Transform home;

    public void SetUp(int player1Score, int player2Score, bool isAIActive) {
        UpdateDigitUI(player1DigitsUI, player1Score);
        UpdateDigitUI(player2DigitsUI, player2Score);

        Sprite winnerSprite;
        if (player1Score > player2Score) {
            winnerSprite = player1WinnerSprite;
        }
        else {
            if (isAIActive) {
                winnerSprite = playerAIWinnerSprite;
            }
            else {
                winnerSprite = player2WinnerSprite;
            }
        }
        winnerImage.sprite = winnerSprite;
        winnerImage.SetNativeSize();

        Sprite player2IconSprite;
        if (isAIActive) {
            player2IconSprite = playerAIWinnerIconSprite;
        }
        else {
            player2IconSprite = player2WinnerIconSprite;
        }
        player2IconImage.sprite = player2IconSprite;
        player2IconImage.SetNativeSize();
    }

    public override void MoveCursorUp() {
        //
    }

    public override void MoveCursorDown() {
        //
    }

    public override void MoveCursorLeft() {
        if (selectedIndex == 0) {
            selectedIndex = 1;
        }
        else {
            selectedIndex = 0;
        }

        UpdateCursorLocation();
    }

    public override void MoveCursorRight() {
        if (selectedIndex == 0) {
            selectedIndex = 1;
        }
        else {
            selectedIndex = 0;
        }

        UpdateCursorLocation();
    }

    public override void UpdateCursorLocation() {
        Vector3 pos = (selectedIndex == 0) ? retry.position : home.position;
        cursorUI.transform.position = pos;
    }

    public override void ActivateSelection() {
        if (selectedIndex == 0) {
            GameManager.instance.ChangeScene("Game");
        }
        else {
            GameManager.instance.ChangeScene("Menu");
        }
    }

    public override void Exit() {
       //
    }

    public override void Enter() {
        this.gameObject.SetActive(true);
        UpdateCursorLocation();
        cursorUI.gameObject.SetActive(true);
    }

    private void UpdateDigitUI(DigitsUI digitsUI, int score) {
        int digit1 = score / 10;
        int digit2 = score % 10;

        digitsUI.Digit1.sprite = digitAtlas.GetSprite(digit1.ToString());
        digitsUI.Digit2.sprite = digitAtlas.GetSprite(digit2.ToString());

        digitsUI.Digit1.SetNativeSize();
        digitsUI.Digit2.SetNativeSize();
    }
}
