using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUiRoundEnd : _baseUiScreen
{
    public baseEvent eventGameReady;
    public GameObject roundText;

    public override IEnumerator Init() {
        gameObject.SetActive(false);
        yield return null;
    }

    public override void Hide() {
        // AnimationComponent.Play("hide-screen", true).OnComplete(CompleteHideTransition);
        CompleteHideTransition();
    }

    public void SetRoundText(string txt) {
        roundText.GetComponent<Text>().text = txt;
    }

    public override void CompleteHideTransition() {
        gameObject.SetActive(false);
    }

    public override void Show() {
        gameObject.SetActive(true);
        AnimationComponent.Play("show-round-end-text").OnComplete(CompleteShowTransition);
        // AnimationComponent.Play("show-screen", true).OnComplete(CompleteShowTransition);
    }

    public override void CompleteShowTransition() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = true;
        eventGameReady.Raise();
        AnimationComponent.Play("hide-round-end-text").OnComplete(() => {gameObject.SetActive(false);});
        // UserInput.Instance.ui = true;
    }

}
