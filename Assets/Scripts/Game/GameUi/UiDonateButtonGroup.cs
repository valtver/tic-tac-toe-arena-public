using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class UiDonateButtonGroup : _baseUiButtonGroup
{
    public void Show() {
        AnimationComponent.Play("show-screen", true);
    }

    public void Hide() {
        AnimationComponent.Play("hide-screen", true).OnComplete(()=>{
            gameObject.SetActive(false);
        });
    }
}
