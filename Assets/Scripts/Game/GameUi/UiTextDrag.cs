using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiTextDrag : _baseCompositionModule
{
    [Header("Behavior")]
    public GameObject Content;
    public Vector3 dragDelta = new Vector3();

    Vector3 nextStepPos = new Vector3();
    Vector3 finalPos = new Vector3();
    VectorTween transTween = new VectorTween();
    VectorTween releaseDeltaTween = new VectorTween();

    public float unitWidth;
    public float unitHeight;
    public float topEdge;

    public bool isHold = true;
    public int activeIndex = 0;
    bool active = false;

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        unitWidth = Content.GetComponent<RectTransform>().sizeDelta.x;
        unitHeight = Content.GetComponent<RectTransform>().sizeDelta.y;
        topEdge = Content.GetComponent<RectTransform>().localPosition.y;
        active = true;
        yield return null;
    }

    public override void Setup() {
        base.Setup();

    }

    public void Drag() {
        dragDelta = UserInput.Instance.pointerData.pointerDelta;
        RectTransform contentTransform = Content.GetComponent<RectTransform>();       

        Translate();

    }

    public void ClickDown() {
        isHold = true;
        releaseDeltaTween.stop();
        transTween.stop();
        Translate();
    }

    public void DragOff() {
        isHold = false;
        FinishTranslateDrag();
    }

    public void Translate() {
        dragDelta = UserInput.Instance.pointerData.pointerDelta;
            RectTransform contentTransform = Content.GetComponent<RectTransform>();
            Vector3 nextPos = new Vector3();

            nextPos.y = dragDelta.y;

            nextStepPos = contentTransform.localPosition + nextPos * UserInput.Instance.pointerData.pointerScale.x;

            if(nextStepPos.y < topEdge) {
                nextStepPos.y = topEdge;
                dragDelta = Vector3.zero;
            }

            if(nextStepPos.y > unitHeight ) {
                nextStepPos.y = unitHeight;
                dragDelta = Vector3.zero;
            }
            
            contentTransform.localPosition = nextStepPos;


    }

    public void FinishTranslateDrag() {
        releaseDeltaTween.stop();
        RectTransform transform = Content.GetComponent<RectTransform>();
        Vector3 nextPos = new Vector3();
        float finishPositionY;

                // activeIndex = Mathf.Abs(Mathf.RoundToInt((transform.localPosition.y + dragDelta.y) / unitWidth));
                finishPositionY = (transform.localPosition.y + dragDelta.y*Mathf.Abs(dragDelta.y)/2f);
                if(finishPositionY < topEdge) {
                    finishPositionY = topEdge;
                }
                if(finishPositionY > unitHeight) {
                    finishPositionY = unitHeight;
                }
                nextPos.y = finishPositionY;
                dragDelta = Vector3.zero;


            // Debug.Log("FINAL POSITION: " + nextPos);

        transTween
        .DoUpdate((Vector3 value) =>{transform.localPosition = value;})
        .fromValue(transform.localPosition)
        .toValue(nextPos)
        .easeLike(EasingFunction.Ease.EaseOutExpo)
        .inTime(1f)
        .play();
        
    }

    void Update() {

        }
    
}
