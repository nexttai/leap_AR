using UnityEngine;
using System.Collections;
using Leap.Unity.Interaction;
using UnityEngine.Events;
public class BeamShot : MonoBehaviour
{

    float timer = 0.0f;
    float effectDisplayTime = 3f;
    float range = 100.0f;
    Ray shotRay;
    RaycastHit shotHit;
    ParticleSystem beamParticle;
    LineRenderer lineRenderer;
    InteractionButton isBool;
    // Use this for initialization
    void Awake()
    {
        beamParticle = GetComponent<ParticleSystem>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < effectDisplayTime)
        {
        //    Debug.Log(isBool.isPressed);
            Shot();
        }
        if (timer >= effectDisplayTime)
        {
            disableEffect();
        }
    }

    public void Shot()
    {
        timer = 0f;
        beamParticle.Stop();
        beamParticle.Play();
        lineRenderer.enabled = true;

        Transform trans = GameObject.Find("Palm UI Pivot Animation").transform;
        lineRenderer.SetPosition(0, trans.position);
        shotRay.origin = transform.position;
        shotRay.direction = transform.forward;

        int layerMask = 0;
        if (Physics.Raycast(shotRay, out shotHit, range, layerMask))
        {
            // hit 
        }
        lineRenderer.SetPosition(1, shotRay.origin + shotRay.direction * range);
        

    }

    private void disableEffect()
    {
        beamParticle.Stop();
        lineRenderer.enabled = false;
    }
}
