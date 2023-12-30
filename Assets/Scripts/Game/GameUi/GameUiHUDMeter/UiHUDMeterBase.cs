using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHUDMeterBase : _baseUiMeter
{
    public override IEnumerator Init()
    {
        yield return StartCoroutine(base.Init());

    }

    public override void Setup()
    {
        base.Setup();
        FlipHUD();
    }

    private void FlipHUD() {
        Vector3 newScale = new Vector3();
        newScale = GetComponent<RectTransform>().localScale;
        newScale.x = (int)Data.Instance.Game.Symbol.RuntimeValue;
        GetComponent<RectTransform>().localScale  = newScale;
    }

    public void SetPlayer() {
        
    }
}
