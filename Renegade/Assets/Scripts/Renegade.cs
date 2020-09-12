using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renegade {
    public Action<int, int, CounterType> OnCounterChangedEvent;

    int[,] PieceSquareTable = { 
        { 30, 25, 20, 15, 15, 20, 25, 30 },
        { 25, 8, 7, 6, 6, 7, 8, 25 },
        { 20, 7, 6, 5, 5, 6, 7, 20 },
        { 15, 6, 5, 4, 4, 5, 6, 15 },
        { 15, 6, 5, 4, 4, 5, 6, 15 },
        { 20, 7, 6, 5, 5, 6, 7, 20 },
        { 25, 8, 7, 6, 6, 7, 8, 25 },
        { 30, 25, 20, 15, 15, 20, 25, 30 }
    };


    Vector2Int[] directions = { new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1),
                                       new Vector2Int(0,-1), new Vector2Int(0, 1),
                                       new Vector2Int(1, -1), new Vector2Int(1, 0),  new Vector2Int(1, 1),
    };

    int whiteScoreTotal;
    int blackScoreTotal;

    int width, height;

    Counter[,] counters;

    public bool isWhitesTurn { get; private set; }
    public int placedCounters { get; private set; }

    public HashSet<Vector2Int> PossibleMoves { get; private set; }
    private HashSet<Vector2Int> potentialMoves = new HashSet<Vector2Int>();
    private Dictionary<Vector2Int, List<Vector2Int>> PossibleMovesSwapIndexCache = new Dictionary<Vector2Int, List<Vector2Int>>();

    public Renegade(int width, int height, bool isWhitesTurn, Action<int, int, CounterType> OnCounterChanged) {
        this.width = width;
        this.height = height;

        this.counters = new Counter[width, height];
        this.isWhitesTurn = isWhitesTurn;

        this.whiteScoreTotal = 0;
        this.blackScoreTotal = 0;

        OnCounterChangedEvent += OnCounterChanged;

        PlaceCounterOfColour(new Vector2Int(3, 3), false);
        PlaceCounterOfColour(new Vector2Int(3, 4), true);
        PlaceCounterOfColour(new Vector2Int(4, 3), true);
        PlaceCounterOfColour(new Vector2Int(4, 4), false);

        UpdatePossibleMoves();
    }

    public Renegade(int width, int height, bool isWhitesTurn, Counter[,] counters, Dictionary<Vector2Int, List<Vector2Int>> PossibleMovesSwapIndexCache, HashSet<Vector2Int> potentialMoves, int placedCounters, int whiteScoreTotal, int blackScoreTotal) {
        this.counters = new Counter[width, height]; 
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (counters[x, y] != null) {
                    this.counters[x, y] = new Counter(counters[x, y].isWhite);
                }
            }
        }

        this.PossibleMovesSwapIndexCache = PossibleMovesSwapIndexCache;
        foreach (Vector2Int move in potentialMoves) {
            this.potentialMoves.Add(move);
        }

        this.width = width;
        this.height = height;

        this.whiteScoreTotal = whiteScoreTotal;
        this.blackScoreTotal = blackScoreTotal;

        this.isWhitesTurn = isWhitesTurn;
        this.placedCounters = placedCounters;
    }

    public bool GameOver;

    private bool IsGameOver() {
        return (placedCounters >= width * height) || (whiteScoreTotal == 0) || (blackScoreTotal == 0);
    }

    public int StaticEvalutation() {
        //White is Mini Player
        //Black is Max Player

        return blackScoreTotal - whiteScoreTotal;
    }

    public void GetCounterCount(out int whiteCount, out int blackCount) {
        int _blackCount = 0;
        int _whiteCount = 0;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (counters[x, y] != null) {
                    if (counters[x, y].isWhite) {
                        _whiteCount++;
                    }
                    else {
                        _blackCount++;
                    }
                }
            }
        }
        whiteCount = _whiteCount;
        blackCount = _blackCount;
    }

    public void PlaceCounterOfColour(Vector2Int move, bool isWhite) {

        int x = move.x;
        int y = move.y;

        counters[x, y] = new Counter(isWhite);
        if (isWhite) {
            whiteScoreTotal += PieceSquareTable[x, y];
        }
        else {
            blackScoreTotal += PieceSquareTable[x, y];
        }

        CounterType type = (isWhite) ? CounterType.whitecounter : CounterType.blackcounter;
        OnCounterChangedEvent?.Invoke(x, y, type);

        placedCounters++;

        for (int i = move.y + 1; i >= move.y - 1; i--) {
            for (int j = move.x - 1; j <= move.x + 1; j++) {
                Vector2Int potentialMove = new Vector2Int(j, i);
                if (IsIndexValid(potentialMove)) {
                    if (!potentialMoves.Contains(potentialMove)) {
                        if (counters[potentialMove.x, potentialMove.y] == null) {
                            potentialMoves.Add(potentialMove);
                        }
                    }
                }
            }
        }

        if (potentialMoves.Contains(move)) {
            potentialMoves.Remove(move);
        }
    }

    private HashSet<Vector2Int> CalculatePossibleMoves(out Dictionary<Vector2Int, List<Vector2Int>> possibleMovesSwapIndex) {
        HashSet<Vector2Int> possibleMoves = new HashSet<Vector2Int>();
        possibleMovesSwapIndex = new Dictionary<Vector2Int, List<Vector2Int>>();
        foreach (Vector2Int potentialMove in potentialMoves) {
            if (CanSwap(isWhitesTurn, potentialMove, out List<Vector2Int> swapIndexes)) {
                possibleMoves.Add(potentialMove);
                possibleMovesSwapIndex.Add(potentialMove, swapIndexes);
            }       
        } 
           
        return possibleMoves;
    }

    public bool TryToApplyMove(Vector2Int move) {
        if (PossibleMoves.Contains(move)) {
            ApplyMove(move);
            return true;
        }

        return false;
    }

    public void ApplyMove(Vector2Int move) {

        List<Vector2Int> swapIndexes = PossibleMovesSwapIndexCache[move];
        swapIndexes.ForEach(c => SwapCounterColour(c.x, c.y));

        PlaceCounterOfColour(move, isWhitesTurn);

        GameOver = IsGameOver();
        if (!GameOver) {
            isWhitesTurn = !isWhitesTurn;
            UpdatePossibleMoves();

            if (PossibleMoves.Count == 0 && OnCounterChangedEvent != null) {
                isWhitesTurn = !isWhitesTurn;
                UpdatePossibleMoves();
            }
        }
    }

    private void UpdatePossibleMoves() {
        PossibleMoves = CalculatePossibleMoves(out Dictionary<Vector2Int, List<Vector2Int>> possibleSwapIndexDictionary);
        PossibleMovesSwapIndexCache = possibleSwapIndexDictionary;
    }

    public bool CanSwap(bool isPlacedWhite, Vector2Int placedPos, out List<Vector2Int> swapIndexes) {

        List<Vector2Int> outSwapIndexes = new List<Vector2Int>();
        bool swapped = false;

        foreach (Vector2Int direction in directions) {

            List<Vector2Int> localSwapIndexes = new List<Vector2Int>();
            bool shouldCheck = true;
            int i = 0;
            do {
                i++;
                Vector2Int checkPos = placedPos + i * direction;

                if (IsIndexValid(checkPos)) {
                    //Posiion is Valid

                    Counter counter = counters[checkPos.x, checkPos.y];
                    if (counter != null) {
                        //Counter Found

                        if (counter.isWhite == isPlacedWhite) {
                            //Counter is Same Color

                            if (localSwapIndexes.Count > 0) {
                                localSwapIndexes.ForEach(s => outSwapIndexes.Add(s));
                                swapped = true;
                            }

                            shouldCheck = false;
                        }
                        else {
                            //Counter is other Color

                            localSwapIndexes.Add(checkPos);
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

        swapIndexes = outSwapIndexes;
        return swapped;
    }

    private bool IsIndexValid(Vector2Int index) {
        return !(index.x > 7 || index.x < 0 || index.y > 7 || index.y < 0);
    }

    public void SwapCounterColour(int x, int y) {
        Counter counter = counters[x, y];

        if (counter.isWhite) {
            whiteScoreTotal -= PieceSquareTable[x, y];
            blackScoreTotal += PieceSquareTable[x, y];
        }
        else {
            whiteScoreTotal += PieceSquareTable[x, y];
            blackScoreTotal -= PieceSquareTable[x, y];
        }

        counter.isWhite = !counter.isWhite;
        CounterType type = (counter.isWhite) ? CounterType.whitecounter : CounterType.blackcounter;
        OnCounterChangedEvent?.Invoke(x, y, type);
    }

    public Renegade GetStateAfterMove(Vector2Int possibleMove) {
        Renegade currentState = this.DeepCopy();
        currentState.ApplyMove(possibleMove);
        return currentState;
    }

    private Renegade DeepCopy() {
        return new Renegade(width, height, isWhitesTurn, counters, PossibleMovesSwapIndexCache, potentialMoves, placedCounters, whiteScoreTotal, blackScoreTotal);
    }

    public class Counter {
        public bool isWhite;

        public Counter(bool isWhite) {
            this.isWhite = isWhite;
        }
    }
}