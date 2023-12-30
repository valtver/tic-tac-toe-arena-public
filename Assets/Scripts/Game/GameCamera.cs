using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : _baseCompositionModule
{   
    // [SerializeField]
    // private Camera cam;

    float fov;
    float refRatioVal;

    float lastRatioVal;

    public baseEvent eventGameLevelIntroComplete;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Init());
    }

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        // cam = Camera.main;
        fov = 60f;
        refRatioVal = 9f/18f;
        setFOV();
        yield return null;
    }

    private void setFOV() {
            lastRatioVal = (float)Screen.width/(float)Screen.height;
            GetComponent<Camera>().fieldOfView = (refRatioVal/lastRatioVal)*fov;
    }

    public void PlayHit() {
        string stateName = "camera-hit-" + Data.Instance.Game.GridSize.RuntimeValue.ToString();
        AnimationComponent.Play(stateName, true);
    }

    public void PlayIntro() {
        AnimationComponent.Play("camera-intro", true).OnComplete(()=>{
            eventGameLevelIntroComplete.Raise();
        });
    }

}