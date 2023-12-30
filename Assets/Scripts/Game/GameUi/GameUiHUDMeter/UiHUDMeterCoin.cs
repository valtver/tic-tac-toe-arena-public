using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHUDMeterCoin : _baseUiMeter
{

    public override IEnumerator Init()
    {
        yield return StartCoroutine(base.Init());

    }

    public override void Setup()
    {
        base.Setup();
    }

    public void ShowCoin(int choice) {
        if(choice == (int)SymbolVar.X) {
            AnimationComponent.SpineUiComponent.Play("show-coin-o");
        }
        else {
            AnimationComponent.SpineUiComponent.Play("show-coin-x");
        }
    }
}
