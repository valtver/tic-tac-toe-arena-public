using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _baseUiButtonGroup : _baseCompositionModule
{
    public GameObject refObject;
    public enum GroupMode {
        Static = 0,
        Dynamic = 1
    }
    public GroupMode Mode;

    public enum GroupType {
        Container = 0,
        Selectable = 1
    }
    public GroupType Type;

    public enum GroupState {
        Stable = 0,
        Move = 1
    }
    [SerializeField]
    private GroupState _state;
    public GroupState State{
        get { return _state;}
        set {
            _state = value;
            if(_state == GroupState.Move) {

            }
            if(_state == GroupState.Stable) {

            }
        }
    }

    public override void Reset() {
        base.Reset();
        // Debug.Log(this.gameObject.name + " Reset triggered!");
        for(int i = 0; i < transform.childCount; i++) {
            _baseUiButton btn = transform.GetChild(i).gameObject.GetComponent<_baseUiButton>();
            if(btn)
                btn.State = _baseUiButton.ButtonState.Default;
        }
    }

    public virtual float GetUnitWidth() {
        return refObject.GetComponent<RectTransform>().sizeDelta.x;
    }

    public virtual float GetUnitHeight() {
        return refObject.GetComponent<RectTransform>().sizeDelta.y;
    }

    public virtual GameObject GetUnit(int i) {
        return  transform.GetChild(i).gameObject;
    }
}
