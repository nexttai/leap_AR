using UnityEngine;
using System.Collections;

public class MyButton : MonoBehaviour
{
    public ParticleSystem particle;

    
    //ボタンをプレスしたときの処理
    public void OnClick()
    {
        particle.Play();
        Debug.Log("Button click!");
    }
    public void UnClick()
    {
       // particle.Stop();
    }
}

