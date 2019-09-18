using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognizeButton : MonoBehaviour
{
    public GameObject play;
    private bool training = false;
    public LeapTrainer trainer;
    public Tutorial tutorial;
    // Start is called before the first frame update
    void Start()
    {
        Play plays = play.GetComponent<Play>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Play plays = play.GetComponent<Play>();
            plays.StartExam();
        }
    }
}
