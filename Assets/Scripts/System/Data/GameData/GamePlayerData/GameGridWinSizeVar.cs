using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum GameGridWinSize {S = 3, M = 4, L = 5}

[CreateAssetMenu(fileName = "New GameGridWinSizeVar", menuName = "Data/GameGridWinSizeVar", order = 1)]
public class GameGridWinSizeVar : Variable<GameGridWinSize>
{

}
