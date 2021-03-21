using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreManager
{
    private List<ScoreZone> scoreZones;
    public int TotalScore { get; private set; }
    public int Combo { get; private set; }

    public ScoreManager()
    {
        scoreZones = new List<ScoreZone>();
        TotalScore = 0;
        Combo = 1;
    }

    public void AddScoreZone(ScoreZone scoreZone)
    {
        scoreZones.Add(scoreZone);
    }
    public int GetScoreUpdate(Vector3 point)
    {
        foreach(ScoreZone zone in scoreZones)
        {
            if (zone.isInFrontOfZone(point))
            {
                Combo = zone.BuildsCombo ? Combo + 1 : 1;
                var score = zone.Score * Combo;
                TotalScore += score;
                return score;
            }
        }
        Combo = 1;
        return 0;
    }
}
