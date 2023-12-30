using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelVar {
    Dungeon = 0,
    Mayan = 1,
    Japan = 2
};

[CreateAssetMenu(fileName = "New GameLevelVar", menuName = "Data/GameLevelVar", order = 1)]
public class GameLevelVar : Variable<LevelVar>
{
    
}
