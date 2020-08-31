using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CellsController : MonoBehaviour
{
    public CellUI[,] CellUIs;

    [Space]

    [SerializeField] SpriteAtlas counterAtlas;

    public void InitializeCellUIs() {
        if (CellUIs == null) {
            CellUIs = new CellUI[8, 8];
            CellUI[] cells = GetComponentsInChildren<CellUI>();
            for (int i = 0; i < cells.Length; i++) {
                int x = i / 8;
                int y = i % 8;
                CellUIs[x, y] = cells[i];
                CellUIs[x, y].SetXY(x, y);
                CellUIs[x, y].counterType = CounterType.blackcounter;
                CellUIs[x, y].OnCellTypeChangedEvent += UpdateCellSprite;

                //eg 9 / 8 = 1
                //   9 % 8 = 1
                //therefore 9 => (1, 1)
            }
        }
    }

    internal void UpdateCell(int x, int y, CounterType type) {
        CellUIs[x, y].PlaceCell(type);
    }

    private void UpdateCellSprite(int x, int y) {
        CellUI cell = CellUIs[x, y];
        cell.ImageRenderer.sprite = counterAtlas.GetSprite(cell.counterType.ToString()); 
    }
}

public enum CounterType { whitecounter, blackcounter }
