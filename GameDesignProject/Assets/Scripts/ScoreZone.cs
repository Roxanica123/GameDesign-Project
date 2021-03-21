using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone
{
    private BoxCollider2D collider;
    public int Score { get; private set; }
    public bool BuildsCombo { get; private set; }

    public ScoreZone(BoxCollider2D collider, int score, bool buildsCombo)
    {
        this.collider = collider;
        Score = score;
        BuildsCombo = buildsCombo;
    }

    public bool isInFrontOfZone(Vector3 point)
    {
        return collider.OverlapPoint(point);
    }
}