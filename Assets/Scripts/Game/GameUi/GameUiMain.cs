using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUiMain : _baseUiScreen
{
    public struct DataChange {
        public LevelVar Level;
        public GridSize GridSize;
        public Versus PlayerVersus;
        public SymbolVar Symbol;
    }

    public DataChange DataUpdate;
    public baseEvent eventApplicationChangeLoad;
    public baseEvent eventApplicationSaveData;
    public bool GameRestart = false;
    public bool NoBackButton = false;

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        gameObject.SetActive(false);
        yield return null;
    }

    public override void Hide() {
        AnimationComponent.Play("hide-screen", true).OnComplete(CompleteHideTransition);
    }

    public override void CompleteHideTransition() {
        gameObject.SetActive(false);
    }

    public override void Show() {
        // NoBackButton = false;
        gameObject.SetActive(true);
        AnimationComponent.Play("show-screen", true).OnComplete(CompleteShowTransition);
        if(NoBackButton) {
            transform.GetComponentInChildren<UiBackButtonGroup>(true).gameObject.SetActive(false);
            transform.GetComponentInChildren<UiReplayButtonGroup>(true).gameObject.SetActive(true);
        }
        else {
            transform.GetComponentInChildren<UiBackButtonGroup>(true).gameObject.SetActive(true);
            transform.GetComponentInChildren<UiReplayButtonGroup>(true).gameObject.SetActive(false);
        }
    }

    public override void Setup()
    {
        base.Setup();
        GameRestart = false;
        DataUpdate.Level = Data.Instance.Game.Level.RuntimeValue;
        DataUpdate.GridSize = Data.Instance.Game.GridSize.RuntimeValue;
        DataUpdate.PlayerVersus = Data.Instance.Game.PlayerVersus.RuntimeValue;
        DataUpdate.Symbol = Data.Instance.Game.Symbol.RuntimeValue;
        transform.GetComponentInChildren<UiConfirmButtonGroup>(true).gameObject.SetActive(false);
        transform.GetComponentInChildren<UiConfirmReplayButtonGroup>(true).gameObject.SetActive(false);
        // transform.GetComponentInChildren<UiBackButtonGroup>(true).gameObject.SetActive(true);
    }

    public override void CompleteShowTransition() {
        UserInput.Instance.ui = true;
    }

    public void DataSet(GameObject changeSource) {
        if(changeSource.GetComponent<UiReplayButton>() != null) {
            GameRestart = true;
        }
        else if(changeSource.GetComponent<UiLevelTab>() != null) {
            DataUpdate.Level = changeSource.GetComponent<UiLevelTab>().Level;     
            Debug.Log(DataUpdate.Level);
        }
        else if(changeSource.GetComponent<UiGridSizeButton>() != null) {
            DataUpdate.GridSize = changeSource.GetComponent<UiGridSizeButton>().GridSize;
            Debug.Log(DataUpdate.GridSize);
        }
        else if(changeSource.GetComponent<UiVersusButton>() != null) {
            DataUpdate.PlayerVersus = changeSource.GetComponent<UiVersusButton>().Versus;
        }
        else if(changeSource.GetComponent<UiSymbolButton>() != null) {
            DataUpdate.Symbol = changeSource.GetComponent<UiSymbolButton>().Symbol;
        }
        UiConfirmGroupChange();
    }

    public void UiConfirmGroupChange() {
        if(GameRestart) {
            ShowConfirmReplayButtonGroup();
            HideReplayButton();
            GameRestart = false;
        }
        else if(DataUpdate.Level != Data.Instance.Game.Level.RuntimeValue) {
            ShowConfirmButtonGroup();
            HideConfirmReplayButtonGroup();
            HideBackButton();
            HideReplayButton();
        }
        else if(DataUpdate.GridSize != Data.Instance.Game.GridSize.RuntimeValue) {
            ShowConfirmButtonGroup();
            HideConfirmReplayButtonGroup();
            HideBackButton();
            HideReplayButton();
        }
        else if(DataUpdate.PlayerVersus != Data.Instance.Game.PlayerVersus.RuntimeValue) {
            ShowConfirmButtonGroup();
            HideConfirmReplayButtonGroup();
            HideBackButton();
            HideReplayButton();
        }
        else if(DataUpdate.Symbol != Data.Instance.Game.Symbol.RuntimeValue) {
            ShowConfirmButtonGroup();
            HideConfirmReplayButtonGroup();
            HideBackButton();
            HideReplayButton();
        }
        else {
            DeclineChanges();
        }
    }

    public void ShowConfirmButtonGroup() {
        UiConfirmButtonGroup ConfirmGroup = transform.GetComponentInChildren<UiConfirmButtonGroup>(true);

        if(!ConfirmGroup.gameObject.activeInHierarchy) {
            ConfirmGroup.gameObject.SetActive(true);
            ConfirmGroup.Show();
        }
    }

    public void ShowConfirmReplayButtonGroup() {
        UiConfirmReplayButtonGroup ConfirmGroup = transform.GetComponentInChildren<UiConfirmReplayButtonGroup>(true);

        if(!ConfirmGroup.gameObject.activeInHierarchy) {
            ConfirmGroup.gameObject.SetActive(true);
            ConfirmGroup.Show();
        }
    }

    public void DisableBackButton() {
        transform.GetComponentInChildren<UiBackButtonGroup>(true).gameObject.SetActive(false);
        NoBackButton = true;
    }

    public void EnableBackButton() {
        transform.GetComponentInChildren<UiReplayButtonGroup>(true).gameObject.SetActive(false);
        NoBackButton = false;
    }

    public void HideConfirmButtonGroup() {
        UiConfirmButtonGroup ConfirmGroup = transform.GetComponentInChildren<UiConfirmButtonGroup>(true);

        if(ConfirmGroup.gameObject.activeInHierarchy) {
            ConfirmGroup.Hide();
        }
    }

    public void HideConfirmReplayButtonGroup() {
        UiConfirmReplayButtonGroup ConfirmGroup = transform.GetComponentInChildren<UiConfirmReplayButtonGroup>(true);

        if(ConfirmGroup.gameObject.activeInHierarchy) {
            ConfirmGroup.Hide();
        }
    }

    public void HideBackButton() {
        UiBackButtonGroup BackButton = transform.GetComponentInChildren<UiBackButtonGroup>(true);

        if(!NoBackButton) {
            if(!BackButton.hidden)
                BackButton.Hide();
        }
    }

    public void HideReplayButton() {
        UiReplayButtonGroup ReplayButton = transform.GetComponentInChildren<UiReplayButtonGroup>(true);

        if(NoBackButton) {
            if(!ReplayButton.hidden)
                ReplayButton.Hide();
        }
    }

    public void ShowBackButton() {
        UiBackButtonGroup BackButton = transform.GetComponentInChildren<UiBackButtonGroup>(true);

        if(!NoBackButton) {
            BackButton.gameObject.SetActive(true);
            BackButton.Show();
        }
    }

    public void ShowReplayButton() {
        UiReplayButtonGroup ReplayButton = transform.GetComponentInChildren<UiReplayButtonGroup>(true);

        if(NoBackButton) {
            ReplayButton.gameObject.SetActive(true);
            ReplayButton.Show();
        }
    }

    public void UpdateData() {
        Data.Instance.Game.Level.RuntimeValue = DataUpdate.Level;
        Data.Instance.Game.GridSize.RuntimeValue = DataUpdate.GridSize;
        Data.Instance.Game.PlayerVersus.RuntimeValue = DataUpdate.PlayerVersus;
        Data.Instance.Game.Symbol.RuntimeValue = DataUpdate.Symbol;
    }

    public void ConfirmChanges() {
        UserInput.Instance.ui = false;
        UserInput.Instance.game = false;
        UpdateData();
        eventApplicationSaveData.Raise();
        eventApplicationChangeLoad.Raise(this.gameObject);
    }

    public void DeclineChanges() {
        Setup();
        HideConfirmButtonGroup();
        HideConfirmReplayButtonGroup();
        ShowBackButton();
        ShowReplayButton();
    }
}
