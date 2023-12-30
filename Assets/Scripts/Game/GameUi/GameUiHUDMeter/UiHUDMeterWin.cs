using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHUDMeterWin : _baseUiMeter
{
    public GameObject winDotReference;

    public override IEnumerator Init()
    {
        yield return StartCoroutine(base.Init());
    }

    public override void Setup()
    {
        base.Setup();
        SetLine();
    }

    private void SetLine() {
        if(this.transform.childCount > 0) {
            return;
        }
        if(winDotReference == null) {
            Debug.LogError("No reference to Instantiate!");
        }
        else {
            for(int i = 0; i < (int)Data.Instance.Game.WinSize; i++) {
                Instantiate(winDotReference, this.transform);
            }
        }
    }
}
