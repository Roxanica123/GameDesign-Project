using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmEffectsManager : MonoBehaviour
{
    private FiguresManager _figuresManager;

    [SerializeField] private float sidewaysForce = 5000f;
    private Vector3 startDirection;

    public Vector3 StartDirection => startDirection;

    // Start is called before the first frame update
    void Start()
    {
        this._figuresManager = transform.GetComponent<FiguresManager>();
        this.startDirection = new Vector3(sidewaysForce, 0, 0);
    }

    public void MoveNotesSideways()
    {
        startDirection = new Vector3(-startDirection.x, 0, 0);
        foreach (Note note in this._figuresManager.currentNotes)
            if (note != null)
                note.rotate();
    }
}