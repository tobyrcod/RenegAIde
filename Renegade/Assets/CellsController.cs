using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CellsController : MonoBehaviour
{
    public Cell[,] Cells;

    [Space]

    [SerializeField] SpriteAtlas counterAtlas;

    private void InitializeCells() {
        if (Cells == null) {
            Cells = new Cell[8, 8];
            Cell[] cells = GetComponentsInChildren<Cell>();
            for (int i = 0; i < cells.Length; i++) {
                int x = i / 8;
                int y = i % 8;
                Cells[x, y] = cells[i];
                Cells[x, y].SetXY(x, y);
                Cells[x, y].counterType = CounterType.blackcounter;
                Cells[x, y].OnCellTypeChangedEvent += UpdateCellSprite;

                //eg 9 / 8 = 1
                //   9 % 8 = 1
                //therefore 9 => (1, 1)
            }
        }
    }

    internal void PlaceCell(int x, int y, CounterType type) {
        Cells[x, y].PlaceCell(type);
    }

    private void Awake() {
        InitializeCells();
    }

    private void Start() {
        Cells[2, 2].PlaceCell(CounterType.whitecounter);
    }

    private void UpdateCellSprite(int x, int y) {
        Debug.Log("Changing");
        Cell cell = Cells[x, y];
        cell.ImageRenderer.sprite = counterAtlas.GetSprite(cell.counterType.ToString()); 
    }
}

public enum CounterType { whitecounter, blackcounter }
