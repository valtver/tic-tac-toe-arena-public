using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class App : _baseCompositionModule
{

    GameObject LoadLogo;
    public bool isIntro = true;

    [Header("Audio")]
    public AudioMixer AudioMixer;

    [Header("Behavior")]
    [SerializeField]
    private baseEvent eventBlockerHide;
    [SerializeField]
    private baseEvent eventBlockerShow;
    [SerializeField]
    private baseEvent eventBlockerShowIntroStart;
    [SerializeField]
    private baseEvent eventBlockerShowLoad;
    [SerializeField]
    private baseEvent eventBlockerHideLoad;

    void OnEnable() {
        ApplyApplicationSettings();
    }

    void Start()
    {
        Debug.Log("Application starts...");
        LoadUserSettings();
        ApplyUserSettings();
        UserInput.Instance.Init();
        UserInput.Instance.ui = false;
        UserInput.Instance.game = false;
        eventBlockerShowIntroStart.Raise(this.gameObject);
        // eventBlockerShow.Raise(this.gameObject);
    }

    public void ApplyApplicationSettings() {
        Application.targetFrameRate = 60;
    }

    public void ApplicationQuit() {
        SaveUserSettings();
        Application.Quit();
    }

    public void ApplyUserSettings() {
        Debug.Log("Applying sound: " + (int)Data.Instance.App.Sound.RuntimeValue + " Applying music: " + (int)Data.Instance.App.Music.RuntimeValue);
        if((int)Data.Instance.App.Sound.RuntimeValue > 0)            AudioMixer.SetFloat("SoundVolume", 0f);
        if((int)Data.Instance.App.Sound.RuntimeValue < 0)            AudioMixer.SetFloat("SoundVolume", -80f);
        if((int)Data.Instance.App.Music.RuntimeValue > 0)            AudioMixer.SetFloat("MusicVolume", 0f);
        if((int)Data.Instance.App.Music.RuntimeValue < 0)            AudioMixer.SetFloat("MusicVolume", -80f);
        SaveUserSettings();
    }

    public void LoadUserSettings() {
        if(PlayerPrefs.HasKey("sound")) Data.Instance.App.Sound.RuntimeValue = (AudioVar)PlayerPrefs.GetInt("sound");
        if(PlayerPrefs.HasKey("music")) Data.Instance.App.Music.RuntimeValue = (AudioVar)PlayerPrefs.GetInt("music");
        if(PlayerPrefs.HasKey("level")) Data.Instance.Game.Level.RuntimeValue = (LevelVar)PlayerPrefs.GetInt("level");
        if(PlayerPrefs.HasKey("gridsize")) Data.Instance.Game.GridSize.RuntimeValue = (GridSize)PlayerPrefs.GetInt("gridsize");
        if(PlayerPrefs.HasKey("playerversus")) Data.Instance.Game.PlayerVersus.RuntimeValue = (Versus)PlayerPrefs.GetInt("playerversus");
        if(PlayerPrefs.HasKey("symbol")) Data.Instance.Game.Symbol.RuntimeValue = (SymbolVar)PlayerPrefs.GetInt("symbol");
    }

    public void SaveUserSettings() {
        if(Data.Instance.App.Sound.RuntimeValue != (AudioVar)PlayerPrefs.GetInt("sound")) PlayerPrefs.SetInt("sound", (int)Data.Instance.App.Sound.RuntimeValue);
        if(Data.Instance.App.Music.RuntimeValue != (AudioVar)PlayerPrefs.GetInt("music")) PlayerPrefs.SetInt("music", (int)Data.Instance.App.Music.RuntimeValue);
        if(Data.Instance.Game.Level.RuntimeValue != (LevelVar)PlayerPrefs.GetInt("level")) PlayerPrefs.SetInt("level", (int)Data.Instance.Game.Level.RuntimeValue);
        if(Data.Instance.Game.GridSize.RuntimeValue != (GridSize)PlayerPrefs.GetInt("gridsize")) PlayerPrefs.SetInt("gridsize", (int)Data.Instance.Game.GridSize.RuntimeValue);
        if(Data.Instance.Game.PlayerVersus.RuntimeValue != (Versus)PlayerPrefs.GetInt("playerversus")) PlayerPrefs.SetInt("playerversus", (int)Data.Instance.Game.PlayerVersus.RuntimeValue);
        if(Data.Instance.Game.Symbol.RuntimeValue != (SymbolVar)PlayerPrefs.GetInt("symbol")) PlayerPrefs.SetInt("symbol", (int)Data.Instance.Game.Symbol.RuntimeValue);
    }

    public void LoadGameSession() {
        Debug.Log("Loading game...");
        Data.Instance.Game.init();
        Debug.Log(Data.Instance.Game.Loader.preloadList.Count);
        Loader.Instance.Load(Data.Instance.Game.Loader);
    }

    public void InitGameSession() {
        Debug.Log("Loaded. Game init started");
        StartCoroutine(InitGame());
    }

    public IEnumerator InitGame() {
        Game Game = GameObject.Find("Game").GetComponent<Game>();
        yield return StartCoroutine(Game.Init());
    }

    public void GameInitComplete() {
        Debug.Log("Game init complete! Hide blocker");
        eventBlockerHide.Raise();
        if(isIntro) {
            isIntro = false;
        }
        else {
            eventBlockerHideLoad.Raise();
        }
    }

    public void ApplicationLoadChange() {
        eventBlockerShowLoad.Raise(this.gameObject);
    }

    public override IEnumerator Init() {
        Debug.Log("init");
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable() {

    }

}
