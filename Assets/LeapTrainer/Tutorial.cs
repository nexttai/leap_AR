using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Leap;
using Leap.Unity;

public class Tutorial : MonoBehaviour
{

    public bool playing;
    private bool previousPlaying;
    public Exercise gesture;

    public LeapRecorder player;
    //お手本のCG
    public HandModelManager controller;
    //	public UnityEngine.UI.Image teacherImage;

    public LeapTrainer trainer;
    private bool trained = false;

    // Use this for initialization
    void Start()
    {
        player = controller.GetLeapRecorder();
        trainer = GetComponent<LeapTrainer>();

        if (trainer != null)
        {
            Debug.Log(trainer.gameObject.transform.position.x);
            trainer.OnEndedRecording += () => Debug.Log("OnEndedRecording");
            trainer.OnGestureCreated += (name, trainingSkipped) => Debug.Log("OnGestureCreated");
            trainer.OnGestureDetected += (points, frameCount) => Debug.Log("OnGestureDetected");
            trainer.OnGestureRecognized += (name, value, allHits) => {

                Debug.Log("OnGestureRecognized");
                foreach (var v in allHits)
                    Debug.Log(" ==> " + v.Key + " : " + v.Value);

            };
            trainer.OnGestureUnknown += (allHits) => {

                Debug.Log("OnGestureUnknown");
                foreach (var v in allHits)
                    Debug.Log(" ==> " + v.Key + " : " + v.Value);

            };
            trainer.OnStartedRecording += () => Debug.Log("OnStartedRecording");
            trainer.OnTrainingComplete += (name, gestures, isPose) => Debug.Log("OnTrainingComplete");
            trainer.OnTrainingCountdown += (countdown) => Debug.Log("OnTrainingCountdown");
            trainer.OnTrainingGestureSaved += (name, gestures) => Debug.Log("OnTrainingGestureSaved");
            trainer.OnTrainingStarted += (name) => Debug.Log("OnTrainingStarted");

        }
    }

    // Update is called once per frame
    void Update()
    {

        if (playing)
        {
            // controller.gameObject.SetActive(true);
            if (!previousPlaying)
            {
                //  controller.gameObject.SetActive(true);

                Debug.Log("Loaded FrameCount: " + player.GetFramesCount());
                player.Play();
                player.loop = true;
            }

            //			teacherImage.gameObject.SetActive (false);
            //		teacherImage.sprite = gesture.teacherFrames [Mathf.FloorToInt (Time.time) % 2];

        }
        else
        {
            //	teacherImage.gameObject.SetActive (false);
            // controller.gameObject.SetActive(false);
        }

        previousPlaying = playing;
    }

    public void UpdateGesture()
    {

        //Aが押されたら、playからのrecordingデータを読み込む。
        //選択した文字の認識情報データをLeapMotionのFrameの再生と追跡をするメソッドLeapRecorderでローディング.frames_に格納される。
        player.Load(gesture.recording);

        
        //そのファイルのフレームの数を表示。
        Debug.Log("読み込んでいるdataのFrameCount:" + player.GetFramesCount());
        //そのファイルをFrame型にデシリアライズされたリスト型が入る。frames

        var newFrames = player.GetFrames();

        //読み込んだデータの
        
        //newFrames.RemoveRange(player.GetFramesCount() - 200, 199);
        //plyaerに保存されていたフレームは削除。

        player.Resets();
        //Recordingデータを再び、frames_に戻す。
        foreach (var f in newFrames)
        {
            player.AddFrame(f);
        }


    }

    public List<Frame> GetFrames()
    {
        return player.GetFrames();
    }
}
