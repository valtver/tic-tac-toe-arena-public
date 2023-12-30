using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameData", menuName = "Data/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Generic")]
    public GameLevelVar Level;
    public GameGridSizeVar GridSize;
    public GamePlayerVersusVar PlayerVersus;
    public GamePlayerSymbolVar Symbol;
    [Header("Load")]
    public LoaderData Loader;
        [Header("----Generated Data----")]
    [Header("Players")]
    public GamePlayerData Player1;
    public GamePlayerData Player2;
    [Header("Active Player")]
    public GamePlayerData ActivePlayer;
    public GamePlayerData Winner;
    [Header("Other")]
    public SymbolVar LastRoundWinner;
    public int WinSize;
    public int Round;
    public int MAX_ROUND = 5;

    public void init() {
        Loader.preloadList.Clear();
        Loader.preloadList.Add(Loader.preloadDir + (int)Level.RuntimeValue + "/" + "GameInputGrid" + (int)Level.RuntimeValue + GridSize.RuntimeValue.ToString());
        Loader.preloadList.Add(Loader.preloadDir + (int)Level.RuntimeValue + "/" + "GameField" + (int)Level.RuntimeValue + GridSize.RuntimeValue.ToString());

        Player1.role = Versus.P;
        Player1.symbol = (SymbolVar)((int)Symbol.RuntimeValue);
        Player1.Name = Player1.role.ToString() + 1;
        Player1.lastRoundScore = 0;

        Player2.role = PlayerVersus.RuntimeValue;
        Player2.symbol = (SymbolVar)(-(int)Symbol.RuntimeValue);
        Player2.lastRoundScore = 0;
        if(Player2.role == Versus.AI)
            Player2.Name = Player2.role.ToString();
        else
            Player2.Name = Player2.role.ToString() + 2;

        if(GridSize.RuntimeValue.ToString() == "S")
            WinSize = 3;
        else if(GridSize.RuntimeValue.ToString() == "M")
            WinSize = 4;
        else if(GridSize.RuntimeValue.ToString() == "L")
            WinSize = 5;

        Round = 0;
        Winner = null;
        LastRoundWinner = new SymbolVar();
    }
}
