using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHUDMeterRoundWin : _baseUiMeter
{
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
        GetComponent<Text>().text = Data.Instance.Game.Player1.lastRoundScore.ToString() + ":" + Data.Instance.Game.Player2.lastRoundScore.ToString();
    }
}
