using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    private List<ScoreZone> scoreZones;
    public int TotalScore { get; private set; }
    public int Combo { get; private set; }
    private string scoreText = "Score: ";
    private string comboText = "Combo: ";
    private Text scoreElement;
    private Text comboElement;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    private void Awake()
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

    public void UpdateScore(Vector3 point)
    {
        foreach (ScoreZone zone in scoreZones)
        {
            if (zone.IsIn(point))
            {
                var score = zone.Score * Combo;
                TotalScore += score;
                scoreText.SetText(TotalScore.ToString());
                Combo = zone.BuildsCombo ? Combo + 1 : 1;
                comboText.SetText(Combo.ToString());
                return;
            }
        }

        Combo = 1;
        comboText.SetText(Combo.ToString());
    }
}