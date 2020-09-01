using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renegade
{
    public Action<int, int, CounterType> OnCounterChangedEvent;

    int width, height;
    Counter[,] counters;
    private bool isWhitesTurn;
    private int placedCounters;

    public List<Vector2Int> PossibleMoves { get; private set; }
    public Renegade(int width, int height, bool isWhitesTurn, Action<int, int, CounterType> OnCounterChanged) {
        this.width = width;
        this.height = height;

        this.counters = new Counter[width, height];
        this.isWhitesTurn = isWhitesTurn;

        OnCounterChangedEvent += OnCounterChanged;

        PlaceCounterOfColour(3, 3, false);
        PlaceCounterOfColour(3, 4, true);
        PlaceCounterOfColour(4, 3, true);
        PlaceCounterOfColour(4, 4, false);

        UpdatePossibleMoves();
    }

    public Renegade(int width, int height, bool isWhitesTurn, Counter[,] counters, int placedCounters) {
        this.counters = new Counter[width, height]; 
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (counters[x, y] != null) {
                    this.counters[x, y] = new Counter(counters[x, y].isWhite);
                }
            }
        }

        this.isWhitesTurn = isWhitesTurn;
        this.placedCounters = placedCounters;
    }

    public bool GameOver;

    private bool IsGameOver() {
        return placedCounters >= 64;
    }

    public int StaticEvalutation() {
        //White is Mini Player
        //Black is Max Player
        int whiteCounters = GetCountersOfColour(true);

        return placedCounters - whiteCounters;
    }

    private int GetCountersOfColour(bool isWhite) {
        int count = 0;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (counters[x, y] != null) {
                    if (counters[x, y].isWhite)
                        count++;
                }
            }
        }

        return count;
    }

    public bool PlaceCounterOfColour(int x, int y, bool isWhite) {
        if (counters[x, y] == null) {
            counters[x, y] = new Counter(isWhite);
            CounterType type = (isWhite) ? CounterType.whitecounter : CounterType.blackcounter;
            OnCounterChangedEvent?.Invoke(x, y, type);

            placedCounters++;

            return true;
        }      

        return false;
    }

    private List<Vector2Int> CalculatePossibleMoves() {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (counters[x, y] == null) {
                    if (CanSwap(isWhitesTurn, x, y)) {
                        possibleMoves.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        return possibleMoves;
    }

    public bool TryToApplyMove(int x, int y) {
        if (counters[x, y] == null) {
            if (CanSwap(isWhitesTurn, x, y)) {
                ApplyMove(x, y);
                return true;
            }
        }
        return false;
    }

    public void ApplyMove(int x, int y) {

        List<Vector2Int> swapIndexes = GetSwapIndexes(isWhitesTurn, x, y);
        swapIndexes.ForEach(c => SwapCounterColour(c.x, c.y));

        counters[x, y] = new Counter(isWhitesTurn);
        CounterType type = (isWhitesTurn) ? CounterType.whitecounter : CounterType.blackcounter;

        isWhitesTurn = !isWhitesTurn;
        OnCounterChangedEvent?.Invoke(x, y, type);

        placedCounters++;

        UpdatePossibleMoves();
    }

    private void UpdatePossibleMoves() {
        PossibleMoves = CalculatePossibleMoves();
    }

    public List<Vector2Int> GetSwapIndexes(bool isPlacedWhite, int placedX, int placedY) {

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

        return returnSwapIndexes;
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

    public Renegade GetStateAfterMove(Vector2Int possibleMove) {
        Renegade currentState = this.DeepCopy();
        currentState.ApplyMove(possibleMove.x, possibleMove.y);
        return currentState;
    }

    private Renegade DeepCopy() {
        return new Renegade(width, height, isWhitesTurn, counters, placedCounters);
    }

    public class Counter {
        public bool isWhite;

        public Counter(bool isWhite) {
            this.isWhite = isWhite;
        }
    }
}
