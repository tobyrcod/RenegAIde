using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CellUI : MonoBehaviour
{
    private int x, y;

    public CounterType counterType = CounterType.blackcounter;

    private bool isEnabled = false;
    public Image CounterImage;
    public Image CanMoveImage;

    public Action<int, int> OnCellTypeChangedEvent;

    internal void PlaceCell(CounterType type) {
        isEnabled = true;
        CounterImage.enabled = isEnabled;
        counterType = type;
        OnCellTypeChangedEvent?.Invoke(x, y);
    }

    internal void SwapType() {
        if (isEnabled) {
            counterType = (counterType == CounterType.blackcounter) ? CounterType.whitecounter : CounterType.blackcounter;
            OnCellTypeChangedEvent?.Invoke(x, y);
        }
    }

    public void SetColor(Color color) {
        CanMoveImage.color = color;     
    }

    public void CanMoveIcon(bool enabled) {
        CanMoveImage.gameObject.SetActive(enabled);
    }

    internal void SetXY(int x, int y) {
        this.x = x;
        this.y = y;
    }
}
