using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLevel : _baseCompositionModule
{
    [Header("Behavior")]
    public GameObject ReplayObject;
    public GameObject ImageObject;


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
        yield return null;
    }
}
