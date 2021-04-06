using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreZone : MonoBehaviour
{
    private BoxCollider2D _collider;
    [SerializeField] private int score;
    [SerializeField]private bool buildsCombo;
    public int Score => score;
    public bool BuildsCombo => buildsCombo;

    void Start()
    {
        this._collider = transform.GetComponent<BoxCollider2D>();
    }

    public bool IsIn(Vector3 point)
    {
        return _collider.OverlapPoint(point);
    }
}