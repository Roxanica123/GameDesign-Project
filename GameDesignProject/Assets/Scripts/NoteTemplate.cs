using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class NoteTemplate
{
    public GameObject Prefab { get; private set; }
    public float NoteDuration { get; private set; }

    public NoteTemplate(GameObject prefab, float noteDuration)
    {
        Prefab = prefab;
        NoteDuration = noteDuration;
    }
}