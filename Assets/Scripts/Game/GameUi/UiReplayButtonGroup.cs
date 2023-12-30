using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class UiReplayButtonGroup : _baseUiButtonGroup
{
    public bool hidden = false;

    public void Show() {
        AnimationComponent.Play("show-screen", true);
        hidden = false;
    }

    public void Hide() {
        AnimationComponent.Play("hide-screen", true).OnComplete(()=>{
            hidden = true;
            gameObject.SetActive(false);
        });
    }
}
