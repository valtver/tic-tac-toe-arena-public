using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class UiLevelButtonGroup : _baseUiButtonGroup 
{

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        Debug.Log("..." + this.transform.parent.name + ": Init() call..." );

        if(Mode == GroupMode.Dynamic) {
            if(transform.GetChild(0) != null) {
                refObject = transform.GetChild(0).gameObject;
            }
            for(int i = 0; i < System.Enum.GetValues(typeof(LevelVar)).Length; i++) {
                UiLevelButton btn;
                if(i < transform.childCount) {
                    btn = transform.GetChild(i).gameObject.GetComponent<UiLevelButton>();
                }
                else {
                    btn = Instantiate(refObject, transform).GetComponent<UiLevelButton>();
                }   
                btn.Level = (LevelVar)i;
                btn.ImageObject.GetComponent<Image>().sprite = Data.Instance.Ui.LevelSprite[i];
                btn.name = refObject.name;
                btn.State = _baseUiButton.ButtonState.Default;
            }
        }
        yield return null;
    }

    public override void Setup()
    {
        base.Setup();
        for(int i = 0; i < System.Enum.GetValues(typeof(LevelVar)).Length; i++) {
            UiLevelButton btn = transform.GetChild(i).gameObject.GetComponent<UiLevelButton>();
            if(i == (int)Data.Instance.Game.Level.RuntimeValue) {
                // btn.SubState = UiLevelButton.ButtonSubState.Replay;
            }
            else {
                btn.SubState = UiLevelButton.ButtonSubState.None;
            }
            btn.State = _baseUiButton.ButtonState.Default;
        }
    }

}
