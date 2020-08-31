using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renegade
{
    public Action<int, int, CounterType> OnCounterChangedEvent;
    Counter[,] counters;
    public Renegade(int width, int height, Action<int,int,CounterType> OnCounterChanged) {
        this.counters = new Counter[width, height];

        OnCounterChangedEvent += OnCounterChanged;

        PlaceCounter(3, 3, false);
        PlaceCounter(3, 4, true);
        PlaceCounter(4, 3, true);
        PlaceCounter(4, 4, false);
    }

    public bool PlaceCounter(int x, int y, bool isWhite) {
        if (counters[x, y] == null) {
            counters[x, y] = new Counter(isWhite);
            CounterType type = (isWhite) ? CounterType.whitecounter : CounterType.blackcounter;
            OnCounterChangedEvent?.Invoke(x, y, type);
            return true;
        }

        return false;
    }

    public class Counter {
        bool isWhite;

        public Counter(bool isWhite) {
            this.isWhite = isWhite;
        }
    }
}
