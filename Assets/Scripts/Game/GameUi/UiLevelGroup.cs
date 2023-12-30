using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class UiLevelGroup : _baseCompositionModule
{

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        GameObject refObject;
        Debug.Log("..." + this.transform.parent.name + ": Init() call..." );

        refObject = transform.GetChild(0).gameObject;

        for(int i = 0; i < System.Enum.GetValues(typeof(LevelVar)).Length; i++) {
            UiLevel btn;
            if(i < transform.childCount) {
                btn = transform.GetChild(i).gameObject.GetComponent<UiLevel>();
            }
            else {
                btn = Instantiate(refObject, transform).GetComponent<UiLevel>();
            }   
            btn.ImageObject.GetComponent<Image>().sprite = Data.Instance.Ui.LevelSprite[i];
            btn.name = refObject.name;
        }
        
        yield return null;
    }

}
