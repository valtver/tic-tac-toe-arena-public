using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gameState {
    None,
    LevelStartIntro,
    UiStartIntro,

    RoundStart,

    RoundLevelIntro,
    RoundUiIntro,

    RoundTurn,
    RoundNextTurn,
    RoundTurnComplete,

    RoundLevelOutro,
    RoundUiOutro, 

    // LevelEnd,
    LevelEndOutro,
    UiEndOutro
}

public class Game : _baseCompositionModule
{
    public gameState State;
    public int coinValue;
    [Header("GameData")]
    public GameData data;
    [Header("GameMap")]
    public GameObject GameMap;
    [Header("GameField")]
    public GameObject GameField;
    [Header("GameInputGrid")]
    public GameObject GameInputGrid;
    private GameGrid gameGrid;
    private GameUi gameUi;
    private GameMap gameMap;
    [Header("Events")]
    public baseEvent eventAppShowGame;
    public baseEvent eventGameUiRoundIntro;
    public baseEvent eventGameUiRoundOutro;
    public baseEvent eventGameUiEndOutro;
    public baseEvent eventGameUiHUDUpdate;
    public baseEvent eventGridAiMove;
    public baseEvent eventGridStateCheck;
    public baseEvent eventGridRoundOutro;
    public baseEvent eventGridLevelOutro;
    public baseEvent eventGameLevelIntro;
    public baseEvent eventGameUiStartIntro;

    // public baseEvent eventGameSetActivePlayer;
    // public GameMap map;

    void OnEnable() {
        if(GameObject.Find("App") == null) {
            Data.Instance.Game.init();
            StartCoroutine(Init());
        }
    }

    public override IEnumerator Init() {
        yield return StartCoroutine(InitData());
        yield return StartCoroutine(InitMap());
        yield return StartCoroutine(InitUi());
        yield return StartCoroutine(InitGameField());
        yield return StartCoroutine(InitInputGrid());
        yield return StartCoroutine(RaiseEventAppShowGame());
    }

    public IEnumerator RaiseEventAppShowGame() {
        Shader.SetGlobalFloat("_Size", 0f);
        eventAppShowGame.Raise();
        StartGame();
        yield return null;
    }

    // public IEnumerator DelayOnReady() {
    //     yield return new WaitForSeconds(1f);
    //     OnReady();
    // }

    public void StartGame() {
        Debug.Log("Game init complete! Starting Game!");
        gameMap.PlayIdle();
        gameMap.PlayRoundMusicIntro();
        State = gameState.LevelStartIntro;
        // State = gameState.UiEndOutro;

        OnReady();
    }

    private IEnumerator InitData() {
        yield return null;
    }

    private IEnumerator InitMap() {
        gameMap = GameMap.GetComponent<GameMap>();
        yield return StartCoroutine(gameMap.Init());
        yield return null;
    }

    public void OnReady() {
        if(State == gameState.LevelStartIntro) {
            State = gameState.UiStartIntro;
            eventGameLevelIntro.Raise(); //WAIT FOR ONREADY() WHEN COMPLETED
            OnReady();
            //OR
            // OnReady();
        }
        else if(State == gameState.UiStartIntro) {
            State = gameState.RoundUiIntro;
            eventGameUiStartIntro.Raise();
        }
        else if(State == gameState.RoundLevelIntro) {
            
        }
        else if(State == gameState.RoundUiIntro) {
            Data.Instance.Game.Round += 1;
            coinValue = CoinFlip();
            State = gameState.RoundStart;
            eventGameUiRoundIntro.Raise(this.gameObject);
            // gameUi.ShowRoundStart(Data.Instance.Game.Round, coinValue);
        }
        else if(State == gameState.RoundStart) {
            State = gameState.RoundTurn;
            SetPlayer(coinValue);
            if(!gameUi.IsHUDActive()) {
                gameUi.ShowHUD();
            }
            gameMap.PlayRoundMusic();
            eventGameUiHUDUpdate.Raise();
            gameGrid.Reset();
            // OnReady();
            //SHOW COIN FLIP
            //UI SHOW FIRST TURN
        }
        else if(State == gameState.RoundTurn) {
            State = gameState.RoundTurnComplete;
            if(data.ActivePlayer.role == Versus.P) {
                Debug.Log("PLAYER GOES!");
                UserInput.Instance.game = true;
            }
            else if(data.ActivePlayer.role == Versus.AI) {
                Debug.Log("PC GOES!");
                eventGridAiMove.Raise();
                // StartCoroutine(gameGrid.AIMove());
            }
        }
        else if(State == gameState.RoundTurnComplete) {
            State = gameState.RoundNextTurn;
            eventGridStateCheck.Raise();
        }
        else if(State == gameState.RoundNextTurn) {
            State = gameState.RoundTurn;
            SwitchPlayer();
            eventGameUiHUDUpdate.Raise();
            OnReady();
        }
        else if(State == gameState.RoundLevelOutro) {
            State = gameState.RoundUiOutro;
            Debug.Log("Animating level outro");
            eventGridRoundOutro.Raise();
            Invoke("OnReady", 0.7f);
            //LEVEL ANIMATIONS
            // OnReady();
        }
        else if(State == gameState.RoundUiOutro) {
            State = gameState.RoundUiIntro; //GAME START/REPEAT
            eventGameUiRoundOutro.Raise();
            Debug.Log("Animating Ui outro");
        }
        else if(State == gameState.LevelEndOutro) {
            State = gameState.UiEndOutro;
            eventGridLevelOutro.Raise();
            OnReady();
        }
        else if(State == gameState.UiEndOutro) {
            eventGameUiEndOutro.Raise();
        }
    }

    public void OnRoundWin(GameObject sender) {

        SymbolVar winSymbol = Data.Instance.Game.LastRoundWinner;
        if(Data.Instance.Game.Player1.symbol == winSymbol) {
            Data.Instance.Game.Player1.lastRoundScore += 1;
        }
        else if(Data.Instance.Game.Player2.symbol == winSymbol) {
            Data.Instance.Game.Player2.lastRoundScore += 1;
        }

        RoundCheck();
        // gameUi.ShowRoundEnd();
        // OnReady();
        //SHOW ROUND END
        // Restart();
        
    }

    public void OnRoundDraw() {
        RoundCheck();
    }

    public void RoundCheck() {
        if(Data.Instance.Game.Round >= Data.Instance.Game.MAX_ROUND) { 
            if(Data.Instance.Game.Player1.lastRoundScore > Data.Instance.Game.Player2.lastRoundScore) {
                Data.Instance.Game.Winner = Data.Instance.Game.Player1;
            }
            else if(Data.Instance.Game.Player1.lastRoundScore < Data.Instance.Game.Player2.lastRoundScore) {
                Data.Instance.Game.Winner = Data.Instance.Game.Player2;
            }
            else {
                Data.Instance.Game.Winner = null;
            }
            
            State = gameState.LevelEndOutro;
        }
        else {
            State = gameState.RoundLevelOutro;
            if(gameUi.IsHUDActive()) {
                gameUi.HideHUD();
            }
        }
        Invoke("OnReady", 0.5f);
        // OnReady();
    }

    public int CoinFlip() {
        int choice = (int)Mathf.RoundToInt(Random.value);
        if(choice == 0) {
            return -1;
        }
        else {
            return choice;
        }
    }

    public void SetPlayer(int n) {
        if(n == (int)Data.Instance.Game.Player1.symbol) {
            Data.Instance.Game.ActivePlayer = Data.Instance.Game.Player1;
        }
        else {
            Data.Instance.Game.ActivePlayer = Data.Instance.Game.Player2;
        }
        // eventGameSetActivePlayer.Raise();
    }

    public void SwitchPlayer(int force = -2) {
        if(Data.Instance.Game.ActivePlayer == Data.Instance.Game.Player2) {
            Data.Instance.Game.ActivePlayer = Data.Instance.Game.Player1;
        }
        else {
            Data.Instance.Game.ActivePlayer = Data.Instance.Game.Player2;
        }
        // eventGameSetActivePlayer.Raise();
    }

    private IEnumerator InitInputGrid() {
        if(GameInputGrid.transform.childCount < 1) {
            GameObject prefab = Loader.Instance.GetAssetByPath(data.Loader.preloadDir + (int)data.Level.RuntimeValue + "/" + "GameInputGrid" + (int)data.Level.RuntimeValue + data.GridSize.RuntimeValue.ToString());
            gameGrid = Instantiate<GameObject>(prefab, GameInputGrid.transform).GetComponent<GameGrid>();
            gameGrid.name = prefab.name;
        }
        else {
            gameGrid = GameInputGrid.transform.GetChild(0).gameObject.GetComponent<GameGrid>();
        }
        yield return StartCoroutine(gameGrid.Init());
    }

    private IEnumerator InitGameField() {
        if(GameField.transform.childCount < 1) {
            GameObject prefab = Loader.Instance.GetAssetByPath(data.Loader.preloadDir + (int)data.Level.RuntimeValue + "/" + "GameField" + (int)data.Level.RuntimeValue + data.GridSize.RuntimeValue.ToString());
            GameObject gameField = Instantiate<GameObject>(prefab, GameField.transform);
            gameField.name = prefab.name;
            yield return null;
        }
    }

    private IEnumerator InitUi() {
        gameUi = GameObject.Find("GameUi").GetComponent<GameUi>();
        yield return StartCoroutine(gameUi.Init());
        yield return null;
    }

    public void Restart() {
        // gameGrid.Reset();
    }
}
