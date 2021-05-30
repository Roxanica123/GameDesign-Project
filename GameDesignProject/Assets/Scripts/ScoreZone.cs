using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreZone : MonoBehaviour
{
    private BoxCollider2D _collider;
    [SerializeField] private int score;
    [SerializeField] private bool buildsCombo;
    public int Score => score;
    public bool BuildsCombo => buildsCombo;
    private Color _initialColor;

    void Start()
    {
        this._collider = transform.GetComponent<BoxCollider2D>();
        this._initialColor = _collider.gameObject.GetComponent<SpriteRenderer>().color;
    }

    public bool IsIn(Vector3 point)
    {
        return _collider.OverlapPoint(point);
    }

    public void Glow()
    {
        _collider.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    public void ResetColor()
    {
        _collider.gameObject.GetComponent<SpriteRenderer>().color = _initialColor;
    }
}