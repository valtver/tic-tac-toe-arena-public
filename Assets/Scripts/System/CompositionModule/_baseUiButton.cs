using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class _baseUiButton : _baseCompositionModule
{
    public enum ButtonState {
        Default = 0,
        Selected = 1,
        Inactive = 2
    }

    [Header("UI Button")]
    [SerializeField]
    public baseEvent eventUiButtonClick;
    private ButtonState _state;
    public CanvasGroup RaycastComponent;
//STATE CONTROL
    public ButtonState State {
        get { return _state; }
        set { 
            _state = value; 
            RaycastComponent.interactable = (value != ButtonState.Inactive);
            RaycastComponent.blocksRaycasts = (value != ButtonState.Inactive);
            RaycastComponent.alpha = Data.Instance.App.ButtonStateColor[(int)_state].a;
            return;
        }
    }

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        Debug.Log("..." + this.transform.parent.name + ": Init() call..." );
        if(RaycastComponent == null ) {
            RaycastComponent = this.GetComponent<CanvasGroup>();
        }
        State = ButtonState.Default;
        yield return null;
    }

//DEFAULT VISUALS
    public virtual void Click() {
        _baseUiButtonGroup parentGroup;
        if(transform.parent.gameObject.GetComponent<_baseUiButtonGroup>()) {
            parentGroup = transform.parent.gameObject.GetComponent<_baseUiButtonGroup>();
            parentGroup.Reset();
        }
        if(eventUiButtonClick)
            eventUiButtonClick.Raise();
    }

}
