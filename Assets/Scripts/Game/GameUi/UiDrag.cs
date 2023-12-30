using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiDrag : _baseCompositionModule
{
    [Header("Behavior")]
    public GameObject Content;
    public _baseUiButton[] units;

    public bool X;
    public bool Y;

    public Vector3 dragDelta = new Vector3();
    Vector3 nextStepPos = new Vector3();
    Vector3 finalPos = new Vector3();

    VectorTween transTween = new VectorTween();
    VectorTween releaseDeltaTween = new VectorTween();

    float unitWidth;
    float unitHeight;

    public int activeIndex = 0;
    bool active = false;

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        units = Content.transform.GetComponentsInChildren<_baseUiButton>();
        unitWidth = units[0].gameObject.GetComponent<RectTransform>().sizeDelta.x;
        unitHeight = units[0].gameObject.GetComponent<RectTransform>().sizeDelta.y;

        Debug.Log("..." + this.transform.parent.name + ": Init() call..." );
        active = true;
        yield return null;
    }

    public override void Setup() {
        base.Setup();
        FinishTranslateIndex((int)Data.Instance.Game.Level.RuntimeValue);
    }

    public void Drag() {
        dragDelta = UserInput.Instance.pointerData.pointerDelta;

        Translate();
        ControlVisual();

        // releaseDeltaTween
        // .DoUpdate((Vector3 value)=>{ dragDelta = value;})
        // .fromValue(dragDelta)
        // .toValue(Vector3.zero)
        // .inTime(1f)
        // .play();
    }

    public void ClickDown() {
        // releaseDeltaTween.stop();
        transTween.stop();
        dragDelta = UserInput.Instance.pointerData.pointerDelta;
        Translate();
    }

    public void ClickUp()  {
        ControlVisual();
        FinishTranslateDrag();
    }

    public void DragOff() {
        ControlRaycast();
        FinishTranslateDrag();
    }

    public void Translate() {
            RectTransform transform = Content.GetComponent<RectTransform>();
            Vector3 nextPos = new Vector3();

            if(X)
                nextPos.x = dragDelta.x;
            if (Y)
                nextPos.y = dragDelta.y;

            nextStepPos = transform.localPosition + nextPos * UserInput.Instance.pointerData.pointerScale.x;

            if(nextStepPos.x > 0f && X) {
                nextStepPos.x = 0f;
                dragDelta = Vector3.zero;
            }

            if(nextStepPos.x < -((units.Length-1) * unitWidth) && X) {
                nextStepPos.x = -((units.Length-1) * unitWidth);
                dragDelta = Vector3.zero;
            }
            
            transform.localPosition = nextStepPos;

    }

    public void FinishTranslateDrag() {
        releaseDeltaTween.stop();
        RectTransform transform = Content.GetComponent<RectTransform>();
        Vector3 nextPos = new Vector3();
        float finishPositionX;
        float finishPositionY;

            if(X) {
                // activeIndex = Mathf.Abs(Mathf.RoundToInt((transform.localPosition.x + dragDelta.x) / unitWidth));
                finishPositionX = Mathf.RoundToInt((transform.localPosition.x + dragDelta.x*Mathf.Abs(dragDelta.x)/2f) / unitWidth) * unitWidth;
                nextPos.x = finishPositionX;
            }
            if(Y) {
                // activeIndex = Mathf.Abs(Mathf.RoundToInt((transform.localPosition.y + dragDelta.y) / unitWidth));
                finishPositionY = Mathf.RoundToInt((transform.localPosition.y + dragDelta.y*Mathf.Abs(dragDelta.y)/2f) / unitHeight) * unitHeight;
                nextPos.y = finishPositionY;
            }

            if(nextPos.x > 0f && X) {
                nextPos.x = 0f;
            }

            if(nextPos.x < -((units.Length-1) * unitWidth) && X) {
                nextPos.x = -((units.Length-1) * unitWidth);
            }

            // Debug.Log("FINAL POSITION: " + nextPos);

        transTween
        .DoUpdate((Vector3 value) =>{transform.localPosition = value; ControlVisual(); ControlRaycast();})
        .fromValue(transform.localPosition)
        .toValue(nextPos)
        .inTime(0.5f)
        .play();
        
    }

    public void FinishTranslateIndex(int finishPos = -1) {
        releaseDeltaTween.stop();
        RectTransform transform = Content.GetComponent<RectTransform>();
        Vector3 nextPos = new Vector3();
        float finishPositionX;
        float finishPositionY;
        if(finishPos != -1) {
            if(X) {
                finishPositionX = -(finishPos * unitWidth);
                nextPos.x = finishPositionX;
                activeIndex = finishPos;
            }
            if(Y) {
                finishPositionY = -(finishPos * unitHeight);
                nextPos.y = finishPositionY;
                activeIndex = finishPos;
            }
        }
        else {
            if(X) {
                activeIndex = Mathf.Abs(Mathf.RoundToInt(transform.localPosition.x / unitWidth));
                finishPositionX = Mathf.RoundToInt(transform.localPosition.x / unitWidth) * unitWidth;
                nextPos.x = finishPositionX;
            }
            if(Y) {
                activeIndex = Mathf.Abs(Mathf.RoundToInt(transform.localPosition.y / unitWidth));
                finishPositionY = Mathf.RoundToInt(transform.localPosition.y / unitHeight) * unitHeight;
                nextPos.y = finishPositionY;
            }
        }

            transTween
            .DoUpdate((Vector3 value) =>{transform.localPosition = value; ControlVisual(); ControlRaycast();})
            .fromValue(transform.localPosition)
            .toValue(nextPos)
            .inTime(0.5f)
            .play();

            ControlVisual();
            ControlRaycast();
        
    }

    public void ControlVisual() {
        RectTransform transform = Content.GetComponent<RectTransform>();
        int target = -Mathf.RoundToInt(transform.localPosition.x / unitWidth);
        for(int i = 0; i < units.Length; i++) {
            units[i].gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            UiLevelButton btn = units[i].gameObject.GetComponent<UiLevelButton>();
            if(target == i) {
                units[i].gameObject.GetComponent<CanvasGroup>().alpha = 0.5f;
            }
            else {
                units[i].gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            }
        }
    }

    public void ControlRaycast() {
        RectTransform transform = Content.GetComponent<RectTransform>();
        int target = -Mathf.RoundToInt(transform.localPosition.x / unitWidth);
        for(int i = 0; i < units.Length; i++) {
            UiLevelButton btn = units[i].gameObject.GetComponent<UiLevelButton>();
            if(target == i) {
                units[i].gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else {
                units[i].gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }

    void Update() {

    }
}
