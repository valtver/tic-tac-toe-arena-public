using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UserInputComponent : _baseCompositionModuleComponent
{
    [Header("Options")]
    public bool draggable = false;
    public bool inDrag = false;
    [SerializeField]
    public PointerData pointerData;

    [SerializeField]
    private baseResponse ClickDownResponse;
    [SerializeField]
    private baseResponse ClickUpResponse;
    [SerializeField]
    private baseResponse DragResponse;
    [SerializeField]
    private baseResponse DragOffResponse;

    // public void SetPointerData(PointerData pointer) {
    //     pointerData = pointer;
    // }

    void Update() {

    }

    public void ClickDown () {
        if(ClickDownResponse != null) {
            ClickDownResponse.Invoke(gameObject);
        }
    }

    public void ClickUp () {
        if(ClickUpResponse != null && !inDrag) {
            ClickUpResponse.Invoke(gameObject);
            Debug.Log(this.name + inDrag + " No DRAG! Click!");
        }
        else if(DragOffResponse != null) {
            DragOffResponse.Invoke(gameObject);
            Debug.Log(this.name + inDrag +  " Drag off.");
        }
    }

    public void Drag() {
        if(draggable) {
            if(DragResponse != null) {
                DragResponse.Invoke(gameObject);
            }
        }
    }

    public void DragOff() {
        if(draggable) {
            if(DragOffResponse != null) {
                DragOffResponse.Invoke(gameObject);
            }
        }
    }
}
