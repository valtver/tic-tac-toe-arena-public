using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUiDecoration : _baseUiScreen
{

    public override IEnumerator Init() {
        StartCoroutine(base.Init());
        gameObject.SetActive(false);
        yield return null;
    }

    public override void Hide() {
        AnimationComponent.Play("hide-decoration", true).OnComplete(CompleteHideTransition);

    }

    public override void CompleteHideTransition() {
        gameObject.SetActive(false);
    }

    public override void Show() {
        if(!gameObject.activeInHierarchy) {
            gameObject.SetActive(true);
            AnimationComponent.Play("show-decoration", true).OnComplete(CompleteShowTransition);
        }
    }

    public override void CompleteShowTransition() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = true;
    }
}
