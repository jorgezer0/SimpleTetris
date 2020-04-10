using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    public static event EventHandler OnCallNewBlock;

    public static void RaiseCallNewBlock(object sender)
    {
        OnCallNewBlock?.Invoke(sender, EventArgs.Empty);
    }
    
    public class ScoreArgs : EventArgs
    {
        public readonly int combo;

        public ScoreArgs(int comboValue)
        {
            combo = comboValue;
        }
    }
    
    public static event EventHandler OnAddPoints;

    public static void RaiseAddPoints(object sender, int comboValue)
    {
        OnAddPoints?.Invoke(sender, new ScoreArgs(comboValue));
    }
    
    public static event EventHandler OnBlockDrop;

    public static void RaiseBlockDrop(object sender)
    {
        OnBlockDrop?.Invoke(sender, EventArgs.Empty);
    }
    
    public static event EventHandler OnLevelUp;

    public static void RaiseLevelUp(object sender)
    {
        OnLevelUp?.Invoke(sender, EventArgs.Empty);
    }
    
    public static event EventHandler OnGameOver;

    public static void RaiseGameOver(object sender)
    {
        OnGameOver?.Invoke(sender, EventArgs.Empty);
    }
    
    public static event EventHandler OnShowGameOverUi;

    public static void RaiseShowGameOverUi(object sender, int score)
    {
        OnShowGameOverUi?.Invoke(sender, new ScoreArgs(score));
    }
}