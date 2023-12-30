using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Spine.Unity;

// [RequireComponent(typeof(Animator))]
public class AnimationComponent : _baseCompositionModuleComponent
{
    public delegate void _callback();

    public bool unscaledTime;
    public SpineUi SpineUiComponent = new SpineUi();

    // [SerializeField]
    // private AnimatorOverrideController animatorController;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private List<animationCall> activeCalls = new List<animationCall>();
    [SerializeField]
    private animationTransition activeTransition;
    [SerializeField]
    private int classLayerIndex = 0;

    [Header("Event Player")]
    public eventPlayerEntry playerEntry1;
    public eventPlayerEntry playerEntry2;
    public eventPlayerEntry playerEntry3;

    [Serializable]
    public class eventPlayerEntry {
        public int layerIndex;
        public string stateName;
    }

    [Header("Randomizer")]
    public bool isRandomizerEnabled = false;
    private float randomProgressTime = 0f;
    private float nextRandomTime = 10f;

    [Range(0.1f, 19.0f)]
    public float minTimerInSeconds = 0.1f;
    [Range(0.2f, 20.0f)]
    public float maxTimerInSeconds = 10f;

    [SerializeField]
    private List<animRandomizer> randomizerEntry;
    [Serializable]
    public class animRandomizer {
        public string layerName;
        public string animationName;
    }

    [Serializable]
    public class SpineUi {
        public _callback startCall = null;
        public _callback completeCall = null;
        public SkeletonGraphic skeletonGraphic;
        
        public SpineUi Play(string spineAnimName) {
            completeCall = null;
            // skeletonGraphic.AnimationState.ClearTracks();
            skeletonGraphic.AnimationState.SetAnimation(0, spineAnimName, false);
            return this;
        }

        public SpineUi Loop(string spineAnimName) {
            skeletonGraphic.AnimationState.SetAnimation(0, spineAnimName, true);
            return this;
        }

        public SpineUi OnComplete(_callback completeCb) {
            completeCall = completeCb;
            skeletonGraphic.AnimationState.Complete += OnSpineAnimationComplete;
            return this;
        }

        public SpineUi OnStart(_callback startCb) {
            startCall = startCb;
            skeletonGraphic.AnimationState.Start += OnSpineAnimationStart;
            return this;
        }

        public void OnSpineAnimationComplete(Spine.TrackEntry trackEntry) {
            if(completeCall != null) {
                _callback cbCall = completeCall;
                completeCall = null;
                cbCall();
            }
        }

        public void OnSpineAnimationStart(Spine.TrackEntry trackEntry) {
            if(startCall != null) {
                _callback cbCall = startCall;
                startCall = null;
                cbCall();
            }
        }
    }

    public class animationCall {
        public string state;
        public bool started = false;
        public _callback completeCall; //complete ONCE call for non-looped animations

        public animationCall(string animationName) {
            state = animationName;
            completeCall = null;
            started = false;
        }

        public void OnComplete(_callback completeCb) {
            completeCall = completeCb;
        }
    }

    public class animationTransition {
        public string name;
        public bool started = false;
        public _callback completeCall;

        public animationTransition(string triggerName) {
            name = triggerName;
            completeCall = null;
            started = false;
        }

        public void OnComplete(_callback completeCb) {
            completeCall = completeCb;
        }
    }

    public void randomizerStop() {

    }

    public void StopAll(int index) {
        animator.Play("Exit", classLayerIndex, 0f);
    }

    public override void Init() {
        if(animator == null && GetComponent<Animator>() != null) {
            animator = GetComponent<Animator>();
        }
        if(unscaledTime) {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        else if(!unscaledTime) {
            animator.updateMode = AnimatorUpdateMode.Normal;
        }
    }

    public animationTransition setTrigger(string animationName) {
        if(animator.HasState(classLayerIndex, Animator.StringToHash(animationName))) {
            // Debug.Log(animationName + " Got such state!");
            activeTransition = new animationTransition(animationName);
            animator.SetTrigger(animationName);
            return activeTransition;
        }
        else {
            // Debug.Log("No such state!");
            return null;
        }
    }

    public void setFloat(string nm, float val) {
        animator.SetFloat(nm, val);
    }

    public void resetTrigger(string triggerName) {
        animator.ResetTrigger(triggerName);
    }

    public void PlayEntry1() {
        if(animator.HasState(playerEntry1.layerIndex, Animator.StringToHash(playerEntry1.stateName))) {
            activeCalls.Clear();

            animator.Play(playerEntry1.stateName, playerEntry1.layerIndex, 0f);
                
            // Debug.Log("Playing " + animationName);
            animator.Update(0f);
            // Debug.Log("Is it playing? " + animator.GetCurrentAnimatorStateInfo(classLayerIndex).IsName(animationName));
        }
    }

    public void PlayEntry2() {
        if(animator.HasState(playerEntry2.layerIndex, Animator.StringToHash(playerEntry2.stateName))) {
            activeCalls.Clear();

            animator.Play(playerEntry2.stateName, playerEntry2.layerIndex, 0f);
                
            // Debug.Log("Playing " + animationName);
            animator.Update(0f);
            // Debug.Log("Is it playing? " + animator.GetCurrentAnimatorStateInfo(classLayerIndex).IsName(animationName));
        }
    }

    public void PlayEntry3() {
        if(animator.HasState(playerEntry3.layerIndex, Animator.StringToHash(playerEntry3.stateName))) {
            activeCalls.Clear();

            animator.Play(playerEntry3.stateName, playerEntry3.layerIndex, 0f);
                
            // Debug.Log("Playing " + animationName);
            animator.Update(0f);
            // Debug.Log("Is it playing? " + animator.GetCurrentAnimatorStateInfo(classLayerIndex).IsName(animationName));
        }
    }

    public animationCall Play(string animationName, int layerIndex, bool forcer = false) {
        // Debug.Log("Trying state: " + animationName);
        if(animator.HasState(layerIndex, Animator.StringToHash(animationName))) {
            animationCall animationCall = new animationCall(animationName);
            if(forcer)
                activeCalls.Clear();

            activeCalls.Add(animationCall);

            if(forcer)
                animator.Play(animationName, layerIndex, 0f);
            else
                animator.Play(animationName, layerIndex);
                
            // Debug.Log("Playing " + animationName);
            animator.Update(0f);
            // Debug.Log("Is it playing? " + animator.GetCurrentAnimatorStateInfo(classLayerIndex).IsName(animationName));
            return animationCall;
        }
        else {
            // Debug.Log("No such state!");
            return null;
        }
    }

    public animationCall Play(string animationName, bool forcer = false) {
        // Debug.Log("Trying state: " + animationName);
        if(animator.HasState(classLayerIndex, Animator.StringToHash(animationName))) {
            animationCall animationCall = new animationCall(animationName);
            if(forcer)
                activeCalls.Clear();

            activeCalls.Add(animationCall);

            if(forcer)
                animator.Play(animationName, classLayerIndex, 0f);
            else
                animator.Play(animationName, classLayerIndex);
                
            // Debug.Log("Playing " + animationName);
            animator.Update(0f);
            // Debug.Log("Is it playing? " + animator.GetCurrentAnimatorStateInfo(classLayerIndex).IsName(animationName));
            return animationCall;
        }
        else {
            // Debug.Log("No such state!");
            return null;
        }
    }

    public animationCall For(string animationName) {

        for(int i = 0; i < activeCalls.Count; i++) {
            Debug.Log("activeCall: " + activeCalls[i].state);
            if(activeCalls[i].state == animationName) {
                Debug.Log("activeCall: " + activeCalls[i].state);
                return activeCalls[i];
            }
        }

        // Debug.Log("No such state like " + animationName);
        return null;
    }

    void Update() {
        // Debug.Log(animator.GetCurrentAnimatorStateInfo(classLayerIndex).normalizedTime);
        if(activeCalls.Count > 0) {
            for(int i = 0; i < activeCalls.Count; i++) {
                AnimatorStateInfo playState = animator.GetCurrentAnimatorStateInfo(classLayerIndex);
                if(!playState.IsName(activeCalls[i].state) || (playState.normalizedTime > 1 && playState.IsName(activeCalls[i].state))) {
                    // Debug.Log(activeCalls[i].state + " AnimationComplete!");
                    if(activeCalls[i].completeCall != null) {
                        _callback callbackExec = activeCalls[i].completeCall;
                        activeCalls[i].completeCall = null;
                        callbackExec();
                    }
                    else {
                        activeCalls.RemoveAt(i);
                    }
                    
                }                
            }

        }
        if(isRandomizerEnabled && (randomizerEntry.Count > 0)) {
            if(randomProgressTime >= nextRandomTime) {
                animRandomizer randomEntry = randomizerEntry[UnityEngine.Random.Range(0, randomizerEntry.Count)];
                animator.Play(randomEntry.animationName, animator.GetLayerIndex(randomEntry.layerName));
                // Debug.Log(randomEntry.animationName + ": random animation triggered at " + this.name);
                randomProgressTime = 0f;
                nextRandomTime = UnityEngine.Random.Range(minTimerInSeconds, maxTimerInSeconds);
            }
                randomProgressTime += Time.deltaTime;
        }
    }
}
