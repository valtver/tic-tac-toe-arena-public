using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GamePlayerData", menuName = "Data/GamePlayerData", order = 1)]
public class GamePlayerData : ScriptableObject
{
    public string Name;
    public Versus role;
    public SymbolVar symbol;
    public GamePlayerStats Stats;
    public int lastRoundScore;
}
