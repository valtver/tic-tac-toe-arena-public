using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocationId {
    public int x;
    public int y;
}

public class GameGridInput : _baseCompositionModule
{
    [Header("Behavior")]
    public LocationId id = new LocationId();
    public bool isWinUnit = false;
    public BoxCollider col;
    public GameData data;
    public SymbolVar sym = new SymbolVar();
    [Header("Unit")]
    public GameUnit gameUnit;
    [Header("Events")]
    public baseEvent eventGridInputClick;
    
    public override IEnumerator Init() {
        sym = new SymbolVar();
        col.enabled = true;
        col = GetComponent<BoxCollider>();
        yield return StartCoroutine(InitUnit());
        Debug.Log("Grid Unit" + id + "init complete");
    }

    public IEnumerator InitUnit() {
        gameUnit = transform.GetChild(0).gameObject.GetComponent<GameUnit>();
        gameUnit.gameObject.SetActive(true);
        yield return StartCoroutine(gameUnit.Init());
    }

    public void Set(SymbolVar symbol) {
        col.enabled = false;
        sym = symbol;
    }

    public void FlagWin() {
        isWinUnit = true;
    }

    public void Click() {
        StopAllCoroutines();
        UserInput.Instance.game = false;
        gameUnit.gameObject.SetActive(true);
        gameUnit.PlayAppear(data.ActivePlayer.symbol);
        Set(data.ActivePlayer.symbol);
        eventGridInputClick.Raise(gameObject);
    }

    public void Highlight(bool dim = true) {
        gameUnit.PlayHighlight(sym, dim);
    }

    public void Dim() {
        gameUnit.PlayDisappear(sym);
    }

    public override void Reset() {
        base.Reset();
        Debug.Log(id.x + " " + id.y);
        col.enabled = true;
        isWinUnit = false;
        if(!gameUnit.gameObject.activeInHierarchy)
            gameUnit.Reset();
        else
            gameUnit.Reset();

        sym = new SymbolVar();
    }

}
