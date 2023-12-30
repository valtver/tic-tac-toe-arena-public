using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiHUDMeterRound : _baseUiMeter
{

    public roundLetter rLetter;

    public override IEnumerator Init()
    {
        yield return StartCoroutine(base.Init());
    }

    public override void Setup()
    {
        base.Setup();
        SetRound();
    }

    public void SetRound() {
        rLetter = (roundLetter)Data.Instance.Game.Round;
        GetComponent<Text>().text = rLetter.ToString() + "/V";
    }
}
