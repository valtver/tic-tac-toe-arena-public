using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SymbolVar {
    X = 1,
    O = -1
};

[CreateAssetMenu(fileName = "New GamePlayerSymbolVar", menuName = "Data/GamePlayerSymbolVar", order = 1)]
public class GamePlayerSymbolVar : Variable<SymbolVar>
{
    
}
