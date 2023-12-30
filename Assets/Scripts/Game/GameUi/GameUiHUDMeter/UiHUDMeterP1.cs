using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHUDMeterP1 : _baseUiMeter
{
    public override IEnumerator Init()
    {
        yield return StartCoroutine(base.Init());
    }

    public override void Setup()
    {
        base.Setup();
        SetText();
    }

    public void SetText() {
        GetComponent<Text>().text = Data.Instance.Game.Player1.Name;
    }
}
