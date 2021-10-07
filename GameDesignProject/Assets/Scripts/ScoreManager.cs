using System;
using System.Collections;
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
    [SerializeField] private float glowTime = 0.1f;

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
                StartCoroutine(nameof(GlowZone), zone);
                return true;
            }
        }

        return false;
    }

    public ScoreZone IsInAnyScoreZone(Vector3 point)
    {
        foreach (ScoreZone zone in scoreZones)
            if (zone.IsIn(point))
                return zone;
        return null;
    }

    public int GetStars(int difficulty)
    {
        return Math.Min(Math.Max(5 - ((CombosLost + difficulty - 1) / 2) + 1, 0), 5);
    }

    // public void Glow(List<Note> notes)
    // {
        // foreach (Note note in notes)
        // {
        //     ScoreZone zone = IsInAnyScoreZone(note.GetPosition());
        //     if (zone != null && note.Hit) //&& zone.BuildsCombo
        //     {
        //         zone.Glow();
        //         return;
        //     }
        // }
        // foreach (var zone in scoreZones)
        // {
        //     zone.ResetColor();
        // }
    // }

    private IEnumerator GlowZone(ScoreZone zone)
    {
        Debug.Log("Glowing zone");
        zone.Glow();
        yield return new WaitForSeconds(glowTime);
        zone.ResetColor();
    }
}