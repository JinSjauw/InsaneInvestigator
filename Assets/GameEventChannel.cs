using System;
using UnityEngine;

[CreateAssetMenu (fileName = "Game Event Channel")]
public class GameEventChannel : ScriptableObject
{
    public event EventHandler OnEvidenceShot;
    public event EventHandler OnEvidenceCaptured;
    public event EventHandler OnGameEnd;
    
    public void InvokeEvidenceCaptureEvent()
    {
        OnEvidenceCaptured?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeGameEndEvent()
    {
        OnGameEnd?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeEvidenceShot()
    {
        OnEvidenceShot?.Invoke(this, EventArgs.Empty);
    }
}
