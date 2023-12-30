using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUiQuit : _baseUiScreen
{
    public override IEnumerator Init() {
        gameObject.SetActive(false);
        yield return null;
    }

    public override void Hide() {
        AnimationComponent.Play("hide-screen", true).OnComplete(CompleteHideTransition);
    }

    public override void CompleteHideTransition() {
        gameObject.SetActive(false);
    }

    public override void Show() {
        gameObject.SetActive(true);
        AnimationComponent.Play("show-screen", true).OnComplete(CompleteShowTransition);
    }

    public override void CompleteShowTransition() {
        UserInput.Instance.ui = true;
    }
}
