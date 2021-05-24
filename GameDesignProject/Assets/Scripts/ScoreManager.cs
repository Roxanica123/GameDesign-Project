using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    private List<ScoreZone> scoreZones;
    public int TotalScore { get; private set; }
    public int Combo { get; private set; }
    public int MaxCombo { get; private set; }
    public int CombosLost { get; private set; }


    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    private void Awake()
    {
        scoreZones = new List<ScoreZone>();
        TotalScore = 0;
        Combo = 1;
        MaxCombo = 5;
        CombosLost = 0;
    }

    public void AddScoreZone(ScoreZone scoreZone)
    {
        scoreZones.Add(scoreZone);
    }

    public void MissedNote()
    {
        Combo = 1;
        CombosLost += 1;
        comboText.SetText(Combo.ToString());
    }

    public bool UpdateScore(Vector3 point)
    {
        foreach (ScoreZone zone in scoreZones)
        {
            if (zone.IsIn(point))
            {
                var score = zone.Score * Combo;
                TotalScore += score;
                scoreText.SetText(TotalScore.ToString());
                Combo = Math.Min(zone.BuildsCombo ? Combo + 1 : 1, MaxCombo);
                if (!zone.BuildsCombo) CombosLost++;
                comboText.SetText(Combo.ToString());
                return true;
            }
        }

        return false;
    }

    public bool IsInAnyScoreZone(Vector3 point)
    {
        foreach (ScoreZone zone in scoreZones)
            if (zone.IsIn(point))
                return true;
        return false;
    }

    public int GetStars(int difficulty)
    {
        return Math.Min(Math.Max(5 - ((CombosLost + difficulty - 1) / 2) + 1, 0), 5);
    }
}