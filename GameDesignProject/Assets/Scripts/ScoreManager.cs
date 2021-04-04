using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager
{
    private List<ScoreZone> scoreZones;
    public int TotalScore { get; private set; }
    public int Combo { get; private set; }
    private string scoreText = "Score: ";
    private string comboText = "Combo: ";
    private Text scoreElement;
    private Text comboElement;

    public ScoreManager()
    {
        scoreZones = new List<ScoreZone>();
        TotalScore = 0;
        Combo = 1;
        scoreElement = GameObject.Find("ScoreText").GetComponent<Text>();
        comboElement = GameObject.Find("ComboText").GetComponent<Text>();
        scoreElement.text = scoreText + TotalScore;
        comboElement.text = comboText + Combo;
    }

    public void Update()
    {
        scoreElement.text = scoreText + TotalScore;
        comboElement.text = comboText + Combo;
    }

    public void AddScoreZone(ScoreZone scoreZone)
    {
        scoreZones.Add(scoreZone);
    }

    public int GetScoreUpdate(Vector3 point)
    {
        foreach (ScoreZone zone in scoreZones)
        {
            if (zone.isInFrontOfZone(point))
            {
                var score = zone.Score * Combo;
                TotalScore += score;
                Combo = zone.BuildsCombo ? Combo + 1 : 1;
                return score;
            }
        }

        Combo = 1;
        return 0;
    }
}