using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUiStartIntro : _baseUiScreen
{
    public GameObject LevelImageObject;

    public override IEnumerator Init() {
        if(LevelImageObject == null) {
            for(int i = 0; i < transform.childCount; i++) {
                if(transform.GetChild(i).gameObject.name == "LevelImage") {
                    LevelImageObject = transform.GetChild(i).gameObject;
                    LevelImageObject.GetComponent<Image>().sprite = Data.Instance.Ui.LevelSprite[(int)Data.Instance.Game.Level.RuntimeValue];
                }
            }
        }
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

    public override void Show() {
        gameObject.SetActive(true);
        AnimationComponent.Play("show-ui-intro-" + Data.Instance.Game.WinSize).OnComplete(CompleteShowTransition);
        // AnimationComponent.Play("show-screen", true).OnComplete(CompleteShowTransition);
    }

    public override void CompleteShowTransition() {
        gameObject.SetActive(false);
        // UserInput.Instance.ui = true;
    }

}
