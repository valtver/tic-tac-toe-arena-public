using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum roundLetter {
    N = 0,
    I = 1,
    II = 2,
    III = 3,
    IV = 4,
    V = 5
}

public class GameUi : _baseCompositionModule
{
    [Header ("Data")]
    public GameUiData GameUiData;
    [Header("Behavior")]
    public GameUiDecoration DecorationScreen;
    public GameUiHUD HUDScreen;
    public GameUiMain MainScreen;
    public GameUiSettings SettingsScreen;
    public GameUiInfo InfoScreen;
    public GameUiHelp HelpScreen;

    public GameUiRoundStart RoundStart;
    public GameUiRoundEnd RoundEnd;
    public GameUiStartIntro StartIntro;

    public GameUiEnd EndScreen;
    public GameUiQuit QuitScreen;

    public baseEvent eventGamePauseMusic;
    public baseEvent eventGameContinueMusic;
    public baseEvent eventGameUiHideMenu;

    private List<_baseUiScreen> Screens = new List<_baseUiScreen>();

    public List<_baseUiScreen> ScreenHistory = new List<_baseUiScreen>();

    public override IEnumerator Init() {
        DecorationScreen.gameObject.SetActive(true);
        yield return StartCoroutine(DecorationScreen.Init());
        Screens.Add(DecorationScreen);
    
        HUDScreen.gameObject.SetActive(true);
        yield return StartCoroutine(HUDScreen.Init());
        Screens.Add(HUDScreen);

        MainScreen.gameObject.SetActive(true);
        yield return StartCoroutine(MainScreen.Init());        
        Screens.Add(MainScreen);

        SettingsScreen.gameObject.SetActive(true);
        yield return StartCoroutine(SettingsScreen.Init());
        Screens.Add(SettingsScreen);

        InfoScreen.gameObject.SetActive(true);
        yield return StartCoroutine(InfoScreen.Init());
        Screens.Add(InfoScreen);

        HelpScreen.gameObject.SetActive(true);
        yield return StartCoroutine(HelpScreen.Init());
        Screens.Add(HelpScreen);

        RoundStart.gameObject.SetActive(true);
        yield return StartCoroutine(RoundStart.Init());

        RoundEnd.gameObject.SetActive(true);
        yield return StartCoroutine(RoundEnd.Init());

        StartIntro.gameObject.SetActive(true);
        yield return StartCoroutine(StartIntro.Init());
        
        EndScreen.gameObject.SetActive(true);
        yield return StartCoroutine(EndScreen.Init());
        Screens.Add(EndScreen);

        QuitScreen.gameObject.SetActive(true);
        yield return StartCoroutine(QuitScreen.Init());
        Screens.Add(QuitScreen);


        yield return null;
    }

    private void HideActiveScreen() {
        for(int i = 0; i < Screens.Count; i++) {
            if(Screens[i].gameObject.activeInHierarchy && (Screens[i] != DecorationScreen)) {
                Screens[i].Hide();
            }
        }
    }

    public void AppBackButtonClick() {
        if((ScreenHistory[ScreenHistory.Count-1] != QuitScreen)
        && (ScreenHistory[ScreenHistory.Count-1] == MainScreen)
        || (ScreenHistory[ScreenHistory.Count-1] == HUDScreen)
        ) {
            eventGamePauseMusic.Raise();
            ShowQuit();
        }
    }

    public void ShowQuit() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        HideActiveScreen();
        QuitScreen.Setup();
        QuitScreen.Show();
        ScreenHistory.Add(QuitScreen);
    }

    public void ShowMain() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        HideActiveScreen();
        MainScreen.NoBackButton = false;
        MainScreen.Setup();
        MainScreen.EnableBackButton();
        MainScreen.Show();
        DecorationScreen.Show();
        ScreenHistory.Add(MainScreen);
        eventGamePauseMusic.Raise();
    }

    public void ShowMainEnd() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        ScreenHistory.RemoveAt(ScreenHistory.Count-1);
        HideActiveScreen();
        MainScreen.Setup();
        MainScreen.DisableBackButton();
        MainScreen.Show();
        DecorationScreen.Show();
        ScreenHistory.Add(MainScreen);
    }

    public void ShowHUD() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        HideActiveScreen();
        HUDScreen.Setup();
        HUDScreen.Show();
        ScreenHistory.Add(HUDScreen);
    }

    public void HideHUD() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        HideActiveScreen();
        ScreenHistory.Clear();
    }

    public void ShowStartIntro() {
        StartIntro.gameObject.SetActive(true);
        StartIntro.Show();
        Debug.Log("UI INTRO STARTS");
    }

    public void ShowRoundStart(GameObject go) {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        Debug.Log("Show UI Round Intro");
        RoundStart.gameObject.SetActive(true);
        RoundStart.SetRoundText("ROUND " + ((roundLetter)Data.Instance.Game.Round).ToString() );
        RoundStart.SetCoin(go.GetComponent<Game>().coinValue);
        RoundStart.Show();
    }

    public void ShowRoundOutro() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        RoundEnd.gameObject.SetActive(true);
        RoundEnd.Show();
        if((int)Data.Instance.Game.LastRoundWinner == 0) {
            RoundEnd.SetRoundText("DRAW");
        }
        else {
            if(Data.Instance.Game.LastRoundWinner == Data.Instance.Game.Player1.symbol)
                RoundEnd.SetRoundText(Data.Instance.Game.Player1.Name + " WINS" );
            else
                RoundEnd.SetRoundText(Data.Instance.Game.Player2.Name + " WINS" );
        }
    }

    public void StartUi() {
        // HUDScreen.Setup();
        // HUDScreen.Show();
        // ScreenHistory.Add(HUDScreen);
    }

    public void ShowSettings() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        HideActiveScreen();
        SettingsScreen.Setup();
        SettingsScreen.Show();
        ScreenHistory.Add(SettingsScreen);
    }

    public void ShowInfo() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        HideActiveScreen();
        InfoScreen.Show();
        ScreenHistory.Add(InfoScreen);
    }

    public void ShowHelp() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        HideActiveScreen();
        HelpScreen.Show();
        ScreenHistory.Add(HelpScreen);
    }

    public void ShowEnd() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        HideActiveScreen();
        DecorationScreen.Show();
        EndScreen.Show();
        ScreenHistory.Add(EndScreen);
    }

    public void ShowBack() {
        UserInput.Instance.game = false;
        UserInput.Instance.ui = false;
        HideActiveScreen();
        ScreenHistory.RemoveAt(ScreenHistory.Count-1);
        var screenToShow = ScreenHistory[ScreenHistory.Count-1];
        if(screenToShow == HUDScreen) {
            screenToShow.Setup();
        }
        screenToShow.Show();
        if(ScreenHistory[ScreenHistory.Count-1] == HUDScreen) {
            if(DecorationScreen.gameObject.activeInHierarchy)
                DecorationScreen.Hide();
            eventGameUiHideMenu.Raise();
            eventGameContinueMusic.Raise();
        }
    }

    public bool IsHUDActive() {
        if((ScreenHistory.Count > 0) && (ScreenHistory[ScreenHistory.Count-1] == HUDScreen)) {
            return true;
        }
        else {
            return false;
        }
    }
}
