using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUiHUD : _baseUiScreen
{
    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        gameObject.SetActive(false);
        yield return null;
    }

    public override void Hide() {
        transform.Find("UiMeterBase").transform.Find("UiMeterCross").gameObject.SetActive(false);
        transform.Find("UiMeterBase").transform.Find("UiMeterCircle").gameObject.SetActive(false);
        AnimationComponent.Play("hide-screen", true).OnComplete(CompleteHideTransition);
        AudioComponent.PlaySound("hud-ui-hide");
    }

    public override void CompleteHideTransition() {
        gameObject.SetActive(false);
    }

    public override void Show() {
        gameObject.SetActive(true);
        // transform.Find("UiMeterBase").transform.Find("UiMeterCross").gameObject.SetActive(true);
        // transform.Find("UiMeterBase").transform.Find("UiMeterCircle").gameObject.SetActive(true);
        AnimationComponent.Play("show-screen", true).OnComplete(CompleteShowTransition);
        AudioComponent.PlaySound("hud-ui-show");
    }

    public override void CompleteShowTransition() {
        UserInput.Instance.ui = true;
        UserInput.Instance.game = true;
    }

}
