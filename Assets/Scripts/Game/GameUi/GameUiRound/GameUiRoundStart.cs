using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUiRoundStart : _baseUiScreen
{
    public GameObject roundText;
    public GameObject coin;
    public int choice = 0;
    public baseEvent eventGameReady;

    public override IEnumerator Init() {
        roundText.SetActive(false);
        coin.SetActive(false);
        gameObject.SetActive(false);
        yield return null;
    }

    public override void Hide() {
        // AnimationComponent.Play("hide-screen", true).OnComplete(CompleteHideTransition);
        CompleteHideTransition();
    }

    public override void CompleteHideTransition() {
        gameObject.SetActive(false);
    }

    public void SetRoundText(string txt) {
        roundText.GetComponent<Text>().text = txt;
    }

    public void SetCoin(int coinSide) {
        choice = coinSide;
    }

    public override void Show() {
        gameObject.SetActive(true);
        AnimationComponent.Play("show-round-text").OnComplete(ShowCoin);
        // AnimationComponent.Play("show-screen", true).OnComplete(CompleteShowTransition);
    }

    public override void CompleteShowTransition() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = true;
        eventGameReady.Raise();
        gameObject.SetActive(false);
        // UserInput.Instance.ui = true;
    }

    public void ShowCoin() {
        coin.gameObject.SetActive(true);
        AnimationComponent.Play("hide-round-text");
        AudioComponent.PlaySound("coin-ui");
        if(choice == (int)SymbolVar.X) {
            coin.GetComponent<AnimationComponent>().SpineUiComponent.Play("show-coin-x").OnComplete(CompleteShowTransition);
        }
        else {
            coin.GetComponent<AnimationComponent>().SpineUiComponent.Play("show-coin-o").OnComplete(CompleteShowTransition);
        }
        choice = 0;
    }
}
