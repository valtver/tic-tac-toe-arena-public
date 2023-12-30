using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : _baseCompositionModule
{
    public GameData data;
    public SymbolVar ownSym;
    public baseEvent eventGameCameraHit;
    
    public bool activeAfterInit;
    public bool randomAngleInit;
    public GameObject randomInitTarget;

    public override IEnumerator Init() {
        Reset();
        if(randomAngleInit) {
            randomizeTargetAngle();
        }
        yield return null;
    }

    private void randomizeTargetAngle() {
        float rand = Random.Range(0f, 1f);
        float val = 0f;

        if(rand >= 0f && rand < 0.25f)
            val = -90f;
        else if(rand >= 0.25f && rand < 0.5f)
            val = 0f;
        else if(rand >= 0.5f && rand < 0.75f)
            val = 90f;
        else if(rand >= 0.75f && rand <= 1f)
            val = 180f;

        randomInitTarget.transform.eulerAngles = new Vector3( randomInitTarget.transform.rotation.x, val, randomInitTarget.transform.rotation.z);
        Debug.Log("Angles randomized");
    }

    public void eventEmitGameCameraHit() {
        eventGameCameraHit.Raise();
    }

    public void PlayAppear(SymbolVar sym) {
        string stateName = sym.ToString().ToLower() + "-click-up-" + Data.Instance.Game.GridSize.RuntimeValue.ToString();
        Debug.Log(stateName);
        AnimationComponent.Play(stateName, true);
        Invoke("eventEmitGameCameraHit", 0.15f);
    }

    public void PlayDisappear(SymbolVar sym) {
        ownSym = sym;
        Invoke("PlayDisappearDelay", Random.Range(0.2f, 0.5f));
    }

    public void PlayHighlight(SymbolVar sym, bool disappear = true) {
        string stateName = sym.ToString().ToLower() + "-highlight";
        if(disappear) {
            AnimationComponent.Play(stateName, true).OnComplete(()=>{
                PlayDisappear(sym);
            });
        }
        else {
            AnimationComponent.Play(stateName, true);
        }

    }

    private void PlayDisappearDelay() {
        string stateName = ownSym.ToString().ToLower() + "-disappear";
        AnimationComponent.Play(stateName, true);
    }

    public override void Reset() {
        ownSym = new SymbolVar();
        if(!activeAfterInit)
            gameObject.SetActive(false);       
    }
}
