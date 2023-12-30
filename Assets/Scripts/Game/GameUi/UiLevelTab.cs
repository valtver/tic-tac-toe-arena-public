using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLevelTab : _baseCompositionModule
{
    [Header("Behavior")]

    [Header("Behavior")]
    public LevelVar Level;
    public baseEvent eventDataLevelChange;

    public GameObject Content;
    public UiLevel[] units;

    public GameObject LeftArrow;
    public GameObject RightArrow;

    VectorTween transTween = new VectorTween();

    float unitWidth;
    float unitHeight;

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        units = Content.transform.GetComponentsInChildren<UiLevel>();
        unitWidth = units[0].gameObject.GetComponent<RectTransform>().sizeDelta.x;
        unitHeight = units[0].gameObject.GetComponent<RectTransform>().sizeDelta.y;

        Debug.Log("..." + this.transform.parent.name + ": Init() call..." );
        yield return null;
    }

    public override void Setup() {
        base.Setup();
        Level = Data.Instance.Game.Level.RuntimeValue;
        GoToPosition((int)Level);
    }

    public void GoToPosition(int levelIndex) {
        RectTransform transform = Content.GetComponent<RectTransform>();
        Vector3 nextPos = new Vector3();
        float finishPositionX;
        finishPositionX = -(levelIndex * unitWidth);
        nextPos.x = finishPositionX;

        if(nextPos.x >= 0f) {
            nextPos.x = 0f;
        }

        if(nextPos.x <= -((units.Length-1) * unitWidth)) {
            nextPos.x = -((units.Length-1) * unitWidth);
        }
        
        UpdateButtonVisual(nextPos);

        transTween
        .DoUpdate((Vector3 value) =>{transform.localPosition = value; UpdateVisuals();})
        .fromValue(transform.localPosition)
        .toValue(nextPos)
        .inTime(0.5f)
        .easeLike(EasingFunction.Ease.EaseOutElastic)
        .OnComplete(() => {
            UpdateButtonVisual(nextPos);
            UpdateVisuals();
            })
        .play();
    }

    public void SetPosition(int levelIndex) {
        transTween.stop();
        RectTransform transform = Content.GetComponent<RectTransform>();
        Vector3 nextPos = new Vector3();

        nextPos.x = -(levelIndex * unitWidth);
        transform.localPosition = nextPos;
        UpdateButtonVisual(nextPos);
    }

    public void IterateNextLevel() {
        transTween.complete();
        int newIndex = (int)Level + 1;
        GoToPosition(newIndex);
        Level = (LevelVar)newIndex;
        eventDataLevelChange.Raise(this.gameObject);
    }

    public void IteratePrevLevel() {
        transTween.complete();
        int newIndex = (int)Level - 1;
        GoToPosition(newIndex);
        Level = (LevelVar)newIndex;
        eventDataLevelChange.Raise(this.gameObject);
    }

    public void UpdateVisuals() {
        RectTransform transform = Content.GetComponent<RectTransform>();
        int target = -Mathf.RoundToInt(transform.localPosition.x / unitWidth);
        for(int i = 0; i < units.Length; i++) {
            units[i].gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            UiLevelButton btn = units[i].gameObject.GetComponent<UiLevelButton>();
            if(target == i) {
                units[i].gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            }
            else {
                units[i].gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            }
        }
    }

    public void UpdateButtonVisual(Vector3 nextPosition) {
        RectTransform transform = Content.GetComponent<RectTransform>();
        if(nextPosition.x >= 0f) {
            LeftArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.alpha = 0f;
            LeftArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.blocksRaycasts = false;
            RightArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.alpha = 1f;
            RightArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.blocksRaycasts = true;
        }
        else if(nextPosition.x <= -((units.Length-1) * unitWidth)){
            LeftArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.alpha = 1f;
            LeftArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.blocksRaycasts = true;
            RightArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.alpha = 0f;
            RightArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.blocksRaycasts = false;
        }
        else {
            RightArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.alpha = 1f;
            RightArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.blocksRaycasts = true;
            LeftArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.alpha = 1f;
            LeftArrow.GetComponent<UiLevelArrowButton>().RaycastComponent.blocksRaycasts = true;
        }
    }

    void Update() {

    }
}
