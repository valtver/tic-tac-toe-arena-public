﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPayButton : _baseUiButton
{

    // Start is called before the first frame update

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        Debug.Log("..." + this.transform.parent.name + ": Init() call..." );
        if(RaycastComponent == null ) {
            RaycastComponent = this.GetComponent<CanvasGroup>();
        }
        State = ButtonState.Selected;
        yield return null;
    }

    public override void Click() {
        AnimationComponent.Play("button-click", true);
        if(eventUiButtonClick)
            eventUiButtonClick.Raise();

    }

    public void Show() {
        AnimationComponent.Play("button-show", true);
    }

    public void Hide() {
        AnimationComponent.Play("button-hide", true).OnComplete(()=>{gameObject.SetActive(false);});
    }
}
