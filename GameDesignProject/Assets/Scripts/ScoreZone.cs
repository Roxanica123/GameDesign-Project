using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreZone : MonoBehaviour
{
    private MeshRenderer _renderer;
    private BoxCollider2D _collider;
    [SerializeField] private int score;
    [SerializeField] private bool buildsCombo;
    public int Score => score;
    public bool BuildsCombo => buildsCombo;
    private Color _initialColor;

    void Start()
    {
        this._collider = transform.GetComponent<BoxCollider2D>();
        this._renderer = transform.GetComponent<MeshRenderer>();
        this._initialColor = _renderer.materials[0].color;
    }

    public bool IsIn(Vector3 point)
    {
        return _collider.OverlapPoint(point);
    }

    public void Glow()
    {
        // _collider.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        var newColor = new Color(_initialColor.r, _initialColor.g, _initialColor.b, 1.0f);
        _renderer.material.color = newColor;
    }

    public void ResetColor()
    {
        // _collider.gameObject.GetComponent<SpriteRenderer>().color = _initialColor;
        _renderer.material.color = _initialColor;
    }
}