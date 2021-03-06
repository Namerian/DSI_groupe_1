﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMoonstoneChallenge : Challenge
{
    public int X { get; private set; }
    public int Current { get; private set; }
    public int Score { get; private set; }

    public string Name { get { return "StageAnchorChallenge"; } }
    public bool Completed { get { return Current >= X; } }
    public string Description { get { return ""; } }

    public StageMoonstoneChallenge(int x, int score, int current = 0)
    {
        X = x;
        Current = current;
        Score = score;

        EventManager.Instance.OnStageStartedEvent += OnStageStartedEvent;
        EventManager.Instance.OnMoonstoneCollectedEvent += OnMoonstoneCollectedEvent;
    }

    private void OnMoonstoneCollectedEvent()
    {
        if (!Completed)
        {
            Current++;
        }
    }

    private void OnStageStartedEvent()
    {
        if (!Completed)
        {
            Current = 0;
        }
    }
}
