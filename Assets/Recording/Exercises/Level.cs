using UnityEngine;
using System.Collections;
[CreateAssetMenu]
public class Level : ScriptableObject
{

    public string levelName;
    public Sprite image;
    public Exercise[] exercises;
    [Range(0f, 1f)]
    public float score;
    public Level[] unlocks;


}
