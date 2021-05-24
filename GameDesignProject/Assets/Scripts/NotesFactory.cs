using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NotesFactory
{
    private Transform NotesTransform { get; set; }
    private Dictionary<string, NoteTemplate> prefabs;
    private Vector3 SpawnPoint { get; set; }
    private Vector3 EndPoint { get; set; }
    private int[] Zones = {-1, 0, 1};

    public NotesFactory()
    {
        NotesTransform = GameObject.Find("Notes").transform;
        prefabs = new Dictionary<string, NoteTemplate>()
        {
            {"circle", new NoteTemplate(Resources.Load<GameObject>("Prefabs/DummyCircle"), (float) 1.5)},
            {"check", new NoteTemplate(Resources.Load<GameObject>("Prefabs/DummyCheck"), (float) 1)}
        };
    }

    public Note GetNote(string type, float time)
    {
        int index = Random.Range(0, Zones.Length);
        SpawnPoint = new Vector3(Zones[index] * 285, Screen.height / 2.0f, -5);
        EndPoint = new Vector3(Zones[index] * 285, -Screen.height / 2.0f, -5);
        return new Note(time, prefabs[type].Prefab, NotesTransform, SpawnPoint, EndPoint, type,
            prefabs[type].NoteDuration);
    }

    public Note GetRandomNote(float time)
    {
        var note = prefabs.ElementAt((int) (Random.Range(0, prefabs.Count)));
        int index = Random.Range(0, Zones.Length);
        SpawnPoint = new Vector3(Zones[index] * 285, Screen.height / 2.0f, -5);
        EndPoint = new Vector3(Zones[index] * 285, -Screen.height / 2.0f, -5);
        return new Note(time, note.Value.Prefab, NotesTransform, SpawnPoint, EndPoint, note.Key,
            note.Value.NoteDuration);
    }
}