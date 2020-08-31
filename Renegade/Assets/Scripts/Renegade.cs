using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renegade
{
    public Action<int, int, CounterType> OnCounterChangedEvent;
    Counter[,] counters;

    private bool isWhitesTurn;
    public Renegade(int width, int height, bool isWhitesTurn, Action<int,int,CounterType> OnCounterChanged) {
        this.counters = new Counter[width, height];
        this.isWhitesTurn = isWhitesTurn;
        OnCounterChangedEvent += OnCounterChanged;

        PlaceCounterOfColour(3, 3, false);
        PlaceCounterOfColour(3, 4, true);
        PlaceCounterOfColour(4, 3, true);
        PlaceCounterOfColour(4, 4, false);
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

    public bool PlaceCounter(int x, int y) {

        if (counters[x, y] == null) {
            counters[x, y] = new Counter(isWhitesTurn);
            CounterType type = (isWhitesTurn) ? CounterType.whitecounter : CounterType.blackcounter;
            CheckForSwaps(isWhitesTurn, x, y);

            isWhitesTurn = !isWhitesTurn;       
            OnCounterChangedEvent?.Invoke(x, y, type);

            return true;
        }     

        return false;
    }

    public void CheckForSwaps(bool isPlacedWhite, int placedX, int placedY) {

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

                Debug.Log(checkPos);

                if (IsIndexValid(checkPos)) {
                    Debug.Log("Valid");

                    Counter counter = counters[checkPos.x, checkPos.y];
                    if (counter != null) {
                        Debug.Log("Counter Found");

                        if (counter.isWhite == isPlacedWhite) {
                            Debug.Log("Counter is Same Color");

                            swapIndexes.ForEach(c => SwapCounterColour(c.x, c.y));
                            shouldCheck = false;
                        }
                        else {
                            Debug.Log("Counter is other Color");

                            swapIndexes.Add(checkPos);
                        }
                    }
                    else {
                        Debug.Log("No Counter Found");

                        shouldCheck = false;
                    }
                }
                else {
                    Debug.Log("Invalid");

                    shouldCheck = false;
                }

            } while (shouldCheck);
        }
    }

    private bool IsIndexValid(Vector2Int index) {
        return !(index.x > 8 || index.x < 0 || index.y > 8 || index.y < 0);
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
