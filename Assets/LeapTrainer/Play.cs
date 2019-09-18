using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine.UI;

public class Play : MonoBehaviour
{

    // public MenuReferences menu;

    //   public AudioSource yay;
    //  public AudioSource clear;
    // private Animator animator;
    private CanvasGroup group;
    public Level level;
   // public Exercise exercise;
    public Text text;
    //  public Level level;
    public GameObject floatingTitle;
    public AudioSource audioSource;
    public GameObject wordsPositions;
    public Dictionary<Exercise, GameObject> floatingExercises = new Dictionary<Exercise, GameObject>();
    public Transform center;
    // public ResultsFiller exam;
    public Tutorial tutorial;
    public LeapTrainer trainer;

    public GameObject exercisePrefab;
    public float adjestheight = 50.0f;
    //   public GameObject[] respown5;

    private bool training = false;
    public float maxScore = 0;
    public float totalScore = 0;
    public string getname;

    private int failCount = 0;
    public bool isGesture=true;
    private Animator tutoanimator;
    public Hiragana hira;

    Exercise recognizedExercise;
   // public bool isPose=false;

    // Use this for initialization
    void Start()
    {
        //this.text = GetComponent<Text>();
        //    menu = GetComponentInParent<MenuReferences>();
        //   animator = GetComponent<Animator>();
        //  animator.SetBool("Show", true);
        //tutoanimator = tutorial.GetComponent<Animator> ();
        group = GetComponent<CanvasGroup>();
        //認識されたら（recognized=true)、現時点でのスコアをmaxscoreとする。
        trainer.OnGestureRecognized += (name, value, allHits) =>
        {
            maxScore = value;
            

        };

        trainer.OnGestureUnknown += (allHits) =>
        {
            failCount++;
            if (failCount == 30)
            {

                //	tutorial.playing = true;
                tutoanimator.SetBool("Show", true);
            }
        };

    }

    // Update is called once per frame
    void Update()
    {

        //playシーンがフェードアウトするときに、タイトルを削除する。
        //if (group.alpha == 0)
        //{
        //    animator.SetBool("Show", true);
        //    //	tutorial.playing = false;
        //    //  Debug.Log("GroupCanvasのα値が0になった");
        //    if (floatingTitle)
        //        DestroyImmediate(floatingTitle);
        //    //認識中の現行のDictionaryのうち、文字の値を消去。
        //    foreach (var v in floatingExercises)
        //    {
        //        DestroyImmediate(v.Value);
        //    }
        //    //黒板の文字を張り付けるオブジェクトを削除
        //    for (int i = wordsPositions.transform.childCount - 1; i >= 0; i--)
        //    {
        //        GameObject.DestroyImmediate(wordsPositions.transform.GetChild(i).gameObject);
        //    }
        //    floatingExercises.Clear();
        //}
    }

    public void StartTraining()
    {
        aborted = false;
        training = true;
        StartCoroutine(StartPlay());

    }
    //認識開始のAボタンが押されたら始まる
    public void StartExam()
    {
        aborted = false;
        training = false;
        //   一度初期化
        totalScore = 0;
        StartCoroutine(StartPlay());
    }

    private bool aborted = false;
    //認識を中断する。
    public void Abort()
    {
        //  animator.SetBool("Show", false);
        trainer.Clean();
        trainer.paused = true;
        StopAllCoroutines();

        //	tutorial.GetComponent<Animator> ().SetBool ("Show", false);
        // exam.GetComponent<Animator>().SetBool("Show", false);
    }

    // //認識開始のAボタンが押されたら始まる
    private IEnumerator StartPlay()
    {

        while (true)
        {
            //ひらがなの一文字を比較対象とする。
            foreach (var e in hira.exercises)
            {
                //   this.text.GetComponent<Text>().text= e.exerciseName;



                try
                {

                    if (aborted)
                    { }//return false;
                       //認識するための枠組みである文字が入力される
                    tutorial.gesture = e;


                    //上で渡された文字のバイナリファイルの読み込み。frames_に格納
                    tutorial.UpdateGesture();

                }
                catch (System.Exception)
                { Debug.Log("例外発生"); }//{return false;}

                yield return new WaitForSeconds(0.1f);


                //前回の文字でのgesturesの情報を削除して、今回の文字における名前とフレームを取得。
                //GetFrames()で、シリアル化して保存されたframes_をデシリアライズして、framesに格納。loadFromeFrameでtrainingGesturesに位置情報が格納。
                trainer.Clean();
                trainer.loadFromFrames(e.exerciseName, tutorial.GetFrames(), isGesture);

                trainer.paused = false;

                
                //以下、枠組みなしの場合、eの値を保存して、最大のeを渡す必要がある。
                //ここでは、順次的な処理のため、exerciseをLeapTrainerから取り出す必要がありそう。
                if (trainer.isRecognized ==true)
                {
                    recognizedExercise = e;
                }

            }
                //スコアが0.3超えるまで中断できる。recognizeがtrueされたらmaxscoreに渡される。
                yield return new WaitUntil(() => trainer.isRecognized == true);

                



                //hand5というタグがついた、自分の手の位置を取得し、e.FontObject（３D文字）をインスタンス化して、手の上に表示。
                GameObject respown5 = GameObject.FindGameObjectWithTag("hand5");
                Vector3 pos = respown5.transform.position;
                //Transform palmTransform = new GameObject("Palm Transform").transform;
                //// palmTransform.position.y += adjestheight;

                pos.y += adjestheight;

                //Vector3 position = GameObject.Find("Contact Palm Bone").transform.position;
                //position.y += adjestheight;

                var fontObj = Instantiate(recognizedExercise.FontObject, pos, Quaternion.identity);
                fontObj.SetActive(true);
                //fontObj.transform.parent = GameObject.Find("Contact Palm Bone").transform;
                fontObj.transform.parent = respown5.transform;

                //それぞれの文字の音を出す。
                // var eAudio = Instantiate(exercise.audio);
                //  e.audio.GetComponent<AudioSource>().Play();
                //   this.GetComponent<AudioSource>().clip = e.audioClip;
                this.audioSource = this.GetComponent<AudioSource>();
                this.audioSource.clip = recognizedExercise.audioClip;
                this.audioSource.Play();


                if (maxScore > 0.3f) trainer.paused = true;

                yield return new WaitForSeconds(2f);

                Destroy(fontObj);


                // if (aborted)
                // { return false;}
                try
                {

                    //認識モードを終了して、今までのスコアより高いスコアをだしたら、更新。
                    trainer.paused = true;

                    if (!training && maxScore > recognizedExercise.score)
                    {
                    recognizedExercise.score = maxScore;
                        //  exercise.onname = getname;

                    }

                    // Save score
                    // Give feedback

                    totalScore += maxScore;
                    maxScore = 0;
                    //認識し終わったあとの文字を緑色にかえて、戻す。
                    //  ft.destination = exercisePos;
                    // ft.destinationColor = Color.green;

                    //			tutorial.GetComponent<Animator> ().SetBool ("Show", false);


                    float score = 0;
                    //foreach (var e in level.exercises)
                    //{
                    score += recognizedExercise.score;
                    //  }

                }
                catch (System.Exception)
                {

                    Debug.Log("例外発生");
                    // break;
                }
                // }

                //yield return new WaitForSeconds(2f);
                ////	tutorial.playing = false;
                //行ごとにやらないので、コメントアウト
                //try
                //{
                //    //トレーニングモードでないとき、今やったスコアの平均を出して、最高点だったら、更新。その後、戻る。
                //    if (!training)
                //    {
                //        //    yay.PlayDelayed(.5f);
                //        float score = 0;
                //        //foreach (var e in level.exercises)
                //        //{
                //        score += e.score;
                //        //  }

                //        //score /= (level.exercises.Length * 1f);
                //        //Debug.Log("平均スコア：" + score);
                //        //if (score > level.score)
                //        //    level.score = score;

                //        //exam.level = level;
                //        //exam.GetComponent<Animator>().SetBool("Show", true);
                //    }
                //    else
                //    {
                //        //  menu.Menu = 3;
                //        Abort();
                //    }
                //}
                //catch (UnityException) { Debug.Log("例外発生"); }


            }

        }
    
}
