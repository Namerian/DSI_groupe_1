using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAnchorChallenge : Challenge
{
    public int X { get; private set; }
    public int Current { get; private set; }
    public int Score { get; private set; }

    public string Name { get { return "StageAltitudeChallenge"; } }
    public bool Completed { get { return Current >= X; } }
    public string Description { get { return ""; } }

    private bool _active;

    public StageAnchorChallenge(int x, int score)
    {
        X = x;
        Current = 0;
        Score = score;

        EventManager.Instance.OnStageStartedEvent += OnStageStartedEvent;
        EventManager.Instance.OnAnchorGrabbedEvent += OnAnchorGrabbedEvent;
    }

    private void OnAnchorGrabbedEvent()
    {
        Current++;
    }

    private void OnStageStartedEvent()
    {
        Current = 0;
    }
}
