using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmEffectsManager : MonoBehaviour
{
    private FiguresManager _figuresManager;

    [SerializeField] private float sidewaysForce = 5000f;
    private Vector3 startDirection;
    private Queue<float> tempo;
    private AudioSource _audioSource;

    public Vector3 StartDirection => startDirection;

    // Start is called before the first frame update
    void Start()
    {
        this._figuresManager = transform.GetComponent<FiguresManager>();
        this.startDirection = new Vector3(sidewaysForce, 0, 0);
        this.tempo = new Queue<float>(_figuresManager.Tempo);
        this._audioSource = _figuresManager.audioSource;
        StartCoroutine(nameof(KeepTempo));
    }
    

    public void MoveNotesSideways()
    {
        startDirection = new Vector3(-startDirection.x, 0, 0);
        foreach (Note note in this._figuresManager.currentNotes)
            if (note != null)
                note.rotate();
    }

    private void OnTempo()
    {
        MoveNotesSideways();
    }

    private IEnumerator KeepTempo()
    {
        while (tempo.Count > 0)
        {
            float nextTempo = tempo.Dequeue();
            yield return new WaitUntil(() => _audioSource.time >= nextTempo);
            OnTempo();
        }
    }
}