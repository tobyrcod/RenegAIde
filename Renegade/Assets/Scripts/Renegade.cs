using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renegade
{
    public Action<int, int, CounterType> OnCounterChangedEvent;
    Counter[,] counters;

    private bool isWhitesTurn;

    public List<Vector2Int> PossibleMoves { get; private set; }
    public Renegade(int width, int height, bool isWhitesTurn, Action<int,int,CounterType> OnCounterChanged) {
        this.counters = new Counter[width, height];
        this.isWhitesTurn = isWhitesTurn;
        OnCounterChangedEvent += OnCounterChanged;

        PlaceCounterOfColour(3, 3, false);
        PlaceCounterOfColour(3, 4, true);
        PlaceCounterOfColour(4, 3, true);
        PlaceCounterOfColour(4, 4, false);

        UpdatePossibleMoves();
    }

    public bool PlaceCounterOfColour(int x, int y, bool isWhite) {
        if (counters[x, y] == null) {
            counters[x, y] = new Counter(isWhite);
            CounterType type = (isWhite) ? CounterType.whitecounter : CounterType.blackcounter;
            OnCounterChangedEvent?.Invoke(x, y, type);
            return true;
        }      

        return false;
    }

    private List<Vector2Int> CalculatePossibleMoves() {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                if (counters[x, y] == null) {
                    if (CanSwap(isWhitesTurn, x, y)) {
                        possibleMoves.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        return possibleMoves;
    }

    public bool PlaceCounter(int x, int y) {

        Debug.Log(new Vector2Int(x, y));

        if (counters[x, y] == null) {

            if (CanSwap(isWhitesTurn, x, y, out List<Vector2Int> swapIndexes)) {

                swapIndexes.ForEach(c => SwapCounterColour(c.x, c.y));

                counters[x, y] = new Counter(isWhitesTurn);
                CounterType type = (isWhitesTurn) ? CounterType.whitecounter : CounterType.blackcounter;

                isWhitesTurn = !isWhitesTurn;
                OnCounterChangedEvent?.Invoke(x, y, type);

                UpdatePossibleMoves();

                return true;
            }
        }     

        return false;
    }

    private void UpdatePossibleMoves() {
        PossibleMoves = CalculatePossibleMoves();
    }

    public bool CanSwap(bool isPlacedWhite, int placedX, int placedY, out List<Vector2Int> totalSwapIndexes) {

        bool swapped = false;

        List<Vector2Int> returnSwapIndexes = new List<Vector2Int>();
        Vector2Int placedPos = new Vector2Int(placedX, placedY);
        Vector2Int[] displacements = { new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1),
                                       new Vector2Int(0,-1), new Vector2Int(0, 1),
                                       new Vector2Int(1, -1),new Vector2Int(1, 0),  new Vector2Int(1, 1),
        };
        foreach (Vector2Int displacement in displacements) {

            List<Vector2Int> swapIndexes = new List<Vector2Int>();
            bool shouldCheck = true;
            int i = 0;
            do {
                i++;
                Vector2Int checkPos = placedPos + i * displacement;

                if (IsIndexValid(checkPos)) {
                    //Posiion is Valid

                    Counter counter = counters[checkPos.x, checkPos.y];
                    if (counter != null) {
                        //Counter Found

                        if (counter.isWhite == isPlacedWhite) {
                            //Counter is Same Color

                            if (swapIndexes.Count > 0) {
                                swapIndexes.ForEach(c => returnSwapIndexes.Add(c));
                                swapped = true;
                            }

                            shouldCheck = false;
                        }
                        else {
                            //Counter is other Color

                            swapIndexes.Add(checkPos);
                        }
                    }
                    else {
                        //No Counter Found

                        shouldCheck = false;
                    }
                }
                else {
                    //Invalid

                    shouldCheck = false;
                }

            } while (shouldCheck);
        }

        totalSwapIndexes = returnSwapIndexes;
        return swapped;
    }

    public bool CanSwap(bool isPlacedWhite, int placedX, int placedY) {

        Vector2Int placedPos = new Vector2Int(placedX, placedY);
        Vector2Int[] displacements = { new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1),
                                       new Vector2Int(0,-1), new Vector2Int(0, 1),
                                       new Vector2Int(1, -1),new Vector2Int(1, 0),  new Vector2Int(1, 1),
        };
        foreach (Vector2Int displacement in displacements) {

            List<Vector2Int> swapIndexes = new List<Vector2Int>();
            bool shouldCheck = true;
            int i = 0;
            do {
                i++;
                Vector2Int checkPos = placedPos + i * displacement;

                if (IsIndexValid(checkPos)) {
                    //Posiion is Valid

                    Counter counter = counters[checkPos.x, checkPos.y];
                    if (counter != null) {
                        //Counter Found

                        if (counter.isWhite == isPlacedWhite) {
                            //Counter is Same Color

                            if (swapIndexes.Count > 0) {
                                return true;
                            }

                            shouldCheck = false;
                        }
                        else {
                            //Counter is other Color

                            swapIndexes.Add(checkPos);
                        }
                    }
                    else {
                        //No Counter Found

                        shouldCheck = false;
                    }
                }
                else {
                    //Invalid

                    shouldCheck = false;
                }

            } while (shouldCheck);
        }

        return false;
    }

    private bool IsIndexValid(Vector2Int index) {
        return !(index.x > 7 || index.x < 0 || index.y > 7 || index.y < 0);
    }

    public void SwapCounterColour(int x, int y) {
        Counter counter = counters[x, y];
        counter.isWhite = !counter.isWhite;
        CounterType type = (counter.isWhite) ? CounterType.whitecounter : CounterType.blackcounter;
        OnCounterChangedEvent?.Invoke(x, y, type);
    }

    public class Counter {
        public bool isWhite;

        public Counter(bool isWhite) {
            this.isWhite = isWhite;
        }
    }
}
