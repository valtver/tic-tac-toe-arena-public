using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUiEnd : _baseUiScreen
{

    public GameObject UiEndCross;
    public GameObject UiEndCircle;
    public GameObject UiEndWinnerText;
    public GameObject UiEndScoreText;

    public baseEvent eventGameMusicStop;

    public override IEnumerator Init() {
        gameObject.SetActive(false);
        yield return null;
    }

    public override void Hide() {
        AnimationComponent.Play("hide-screen", true).OnComplete(CompleteHideTransition);
    }

    public override void CompleteHideTransition() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = true;
        gameObject.SetActive(false);
    }

    public override void Show() {

        gameObject.SetActive(true);
        AudioComponent.PlaySound("total-screen-show");
        if(Data.Instance.Game.Winner != null) {
            if(Data.Instance.Game.Winner.symbol == SymbolVar.X) {
                AnimationComponent.Play("show-end-screen-x", true).OnComplete(CompleteShowX);
                UiEndWinnerText.GetComponent<Text>().text = Data.Instance.Game.Winner.Name;
            }
            else if(Data.Instance.Game.Winner.symbol == SymbolVar.O) {
                AnimationComponent.Play("show-end-screen-o", true).OnComplete(CompleteShowO);
                UiEndWinnerText.GetComponent<Text>().text = Data.Instance.Game.Winner.Name;
            }
            if(Data.Instance.Game.Player1.lastRoundScore > Data.Instance.Game.Player2.lastRoundScore) {
                UiEndScoreText.GetComponent<Text>().text = Data.Instance.Game.Player1.lastRoundScore + ":" + Data.Instance.Game.Player2.lastRoundScore;
            }
            else {
                UiEndScoreText.GetComponent<Text>().text = Data.Instance.Game.Player2.lastRoundScore + ":" + Data.Instance.Game.Player1.lastRoundScore;
            }
        }
        else {
            AnimationComponent.Play("show-end-screen-draw", true).OnComplete(CompleteShowDraw);
            UiEndScoreText.GetComponent<Text>().text = Data.Instance.Game.Player1.lastRoundScore + ":" + Data.Instance.Game.Player2.lastRoundScore;
        }
    }

    private void CompleteShowX() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = true;
        UiEndCross.GetComponent<AnimationComponent>().SpineUiComponent.Play("x-show").OnComplete(()=>{
            UiEndCross.GetComponent<AnimationComponent>().SpineUiComponent.Loop("x-loop");
        });
         AnimationComponent.Play("continue-show-end-screen-x", true).OnComplete(CompleteShowXTransition);
    }

    private void CompleteShowO() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = true;
        UiEndCircle.GetComponent<AnimationComponent>().SpineUiComponent.Play("o-show").OnComplete(()=>{
            UiEndCircle.GetComponent<AnimationComponent>().SpineUiComponent.Loop("o-loop");
        });
         AnimationComponent.Play("continue-show-end-screen-o", true).OnComplete(CompleteShowOTransition);
    }

    private void CompleteShowDraw() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = true;
        UiEndCross.GetComponent<AnimationComponent>().SpineUiComponent.Play("x-show").OnComplete(()=>{
            UiEndCross.GetComponent<AnimationComponent>().SpineUiComponent.Loop("x-loop");
        });
        UiEndCircle.GetComponent<AnimationComponent>().SpineUiComponent.Play("o-show").OnComplete(()=>{
            UiEndCircle.GetComponent<AnimationComponent>().SpineUiComponent.Loop("o-loop");
        });
         AnimationComponent.Play("continue-show-end-screen-draw", true).OnComplete(CompleteShowDrawTransition);
    }

    public void CompleteShowXTransition() {
        AnimationComponent.Play("show-end-screen-x-loop", true);
        UserInput.Instance.game = false;
        UserInput.Instance.ui = true;
    }

    public void CompleteShowOTransition() {
        AnimationComponent.Play("show-end-screen-o-loop", true);
        UserInput.Instance.game = false;
        UserInput.Instance.ui = true;
    }

    public void CompleteShowDrawTransition() {
        AnimationComponent.Play("show-end-screen-draw-loop", true);
        UserInput.Instance.game = false;
        UserInput.Instance.ui = true;
    }

    public void MusicStopEvent() {
        eventGameMusicStop.Raise();
    }
}
