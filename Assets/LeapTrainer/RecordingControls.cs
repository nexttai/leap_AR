using UnityEngine;
using System.Collections;
using Leap.Unity;
using UnityEngine.UI;

public class RecordingControls : MonoBehaviour
{
    [Multiline]
    public string header;
    public Text controlsGui;
    public Text recordingGui;
    public HandModelManager handModel;
    public KeyCode beginRecordingKey = KeyCode.R;
    public KeyCode endRecordingKey = KeyCode.R;
    public KeyCode beginPlaybackKey = KeyCode.P;
    public KeyCode pausePlaybackKey = KeyCode.P;
    public KeyCode stopPlaybackKey = KeyCode.S;

    void Update()
    {
        if (controlsGui != null) controlsGui.text = header + "\n";

        switch (HandModelManager.Main.GetLeapRecorder().state)
        {
            case RecorderState.Recording:
                allowEndRecording();
                break;
            case RecorderState.Playing:
                allowPausePlayback();
                allowStopPlayback();
                break;
            case RecorderState.Paused:
                allowBeginPlayback();
                allowStopPlayback();
                break;
            case RecorderState.Stopped:
                allowBeginRecording();
                allowBeginPlayback();
                break;
        }
    }

    private IEnumerator BeginRecording()
    {

        yield return new WaitForSeconds(2f);

        HandModelManager.Main.ResetRecording();
        HandModelManager.Main.Record();
        Debug.Log("認識開始");
       // recordingGui.text = "started";
    }

    private void allowBeginRecording()
    {



        if (controlsGui != null) controlsGui.text += beginRecordingKey + " - Begin Recording\n";
        if (Input.GetKeyDown(beginRecordingKey))
        {
            StartCoroutine(BeginRecording());
        }
    }

    private void allowBeginPlayback()
    {
        if (controlsGui != null) controlsGui.text += beginPlaybackKey + " - Begin Playback\n";
        if (Input.GetKeyDown(beginPlaybackKey))
        {
            HandModelManager.Main.PlayRecording();
        }
    }

    private void allowEndRecording()
    {
        if (controlsGui != null) controlsGui.text += endRecordingKey + " - End Recording\n";
        if (Input.GetKeyDown(endRecordingKey))
        {
            string savedPath = HandModelManager.Main.FinishAndSaveRecording();
       //     recordingGui.text = "Recording saved to:\n" + savedPath;
        }
    }

    private void allowPausePlayback()
    {
        if (controlsGui != null) controlsGui.text += pausePlaybackKey + " - Pause Playback\n";
        if (Input.GetKeyDown(pausePlaybackKey))
        {
            HandModelManager.Main.PauseRecording();
        }
    }

    private void allowStopPlayback()
    {
        if (controlsGui != null) controlsGui.text += stopPlaybackKey + " - Stop Playback\n";
        if (Input.GetKeyDown(stopPlaybackKey))
        {
            Debug.Log("認識開始準備ok");
            HandModelManager.Main.StopRecording();
        }
    }
}
