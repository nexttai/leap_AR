using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[CreateAssetMenu]
public class Exercise : ScriptableObject
{

    public string exerciseName;
    public Sprite[] teacherFrames;
    public TextAsset recording;
    public float score;
    public GameObject FontObject;
    //public AudioSource audio;
    public AudioClip audioClip;
}
