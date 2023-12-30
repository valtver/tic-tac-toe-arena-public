using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLevelButton : _baseUiButton
{
    public enum ButtonSubState {
        None = 0,
        Replay = 1
    }

    public ButtonSubState _subState;
    public ButtonSubState SubState {
        get{ return _subState;}
        set{
            _subState = value;
            if(_subState == ButtonSubState.Replay) {
                ReplayObject.SetActive(true);
            }
            else if(_subState == ButtonSubState.None) {
                // ReplayObject.SetActive(false);
            }
        }
    }

    [Header("Behavior")]
    public LevelVar Level;
    public GameObject ReplayObject;
    public GameObject ImageObject;
    public baseEvent eventDataLevelChange;

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        if(ReplayObject == null) {
            for(int i = 0; i < transform.childCount; i++) {
                if(transform.GetChild(i).gameObject.name == "ReplayImage") {
                    ReplayObject = transform.GetChild(i).gameObject;
                }
                if(transform.GetChild(i).gameObject.name == "ButtonImage") {
                    ImageObject = transform.GetChild(i).gameObject;
                }
            }
        }
        SubState = ButtonSubState.None;
        yield return null;
    }

    public override void Click() {
        base.Click();
        AnimationComponent.Play("button-click", true);
        State = ButtonState.Selected;
        eventDataLevelChange.Raise(this.gameObject);
    }

}
