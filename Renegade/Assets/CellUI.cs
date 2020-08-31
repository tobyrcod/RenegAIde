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
    public Image ImageRenderer; 

    public Action<int, int> OnCellTypeChangedEvent;


    private void OnValidate() {
        if (ImageRenderer == null) {
            ImageRenderer = GetComponentInChildren<Image>();
        }

        ImageRenderer.enabled = isEnabled;
    }

    internal void PlaceCell(CounterType type) {
        isEnabled = true;
        ImageRenderer.enabled = isEnabled;
        counterType = type;
        OnCellTypeChangedEvent?.Invoke(x, y);
    }

    internal void SwapType() {
        if (isEnabled) {
            counterType = (counterType == CounterType.blackcounter) ? CounterType.whitecounter : CounterType.blackcounter;
            OnCellTypeChangedEvent?.Invoke(x, y);
        }
    }

    internal void SetXY(int x, int y) {
        this.x = x;
        this.y = y;
    }
}
