﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSettingsButton : _baseUiButton
{
    [Header("Behavior")]
    public baseEvent eventGameUiShowSettings;

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        Debug.Log("..." + this.transform.parent.name + ": Init() call..." );
        if(RaycastComponent == null ) {
            RaycastComponent = this.GetComponent<CanvasGroup>();
        }
        State = ButtonState.Selected;
        yield return null;
    }
    
    // Start is called before the first frame update
    public override void Click() {
        AnimationComponent.Play("button-click", true);
        eventGameUiShowSettings.Raise(this.gameObject);
        if(eventUiButtonClick)
            eventUiButtonClick.Raise();
    }
}
