using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMenuButton : _baseUiButton
{
    [Header("Behavior")]
    public baseEvent eventUiShowMenu;
    // Start is called before the first frame update
    public override void Click() {
        base.Click();
        AnimationComponent.Play("button-click", true);
        eventUiShowMenu.Raise(this.gameObject);
    }

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        Debug.Log("..." + this.transform.parent.name + ": Init() call..." );
        if(RaycastComponent == null ) {
            RaycastComponent = this.GetComponent<CanvasGroup>();
        }
        // State = ButtonState.Default;
        RaycastComponent.alpha = 1f;
        yield return null;
    }
}
