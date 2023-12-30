using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Versus {AI = 1, P = -1};

[CreateAssetMenu(fileName = "New GamePlayerVersusVar", menuName = "Data/GamePlayerVersusVar", order = 1)]
public class GamePlayerVersusVar : Variable<Versus>
{
    
}
