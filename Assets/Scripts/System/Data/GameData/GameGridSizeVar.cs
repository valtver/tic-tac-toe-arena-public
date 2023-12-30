using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum GridSize {S = 3, M = 5, L = 7};

[CreateAssetMenu(fileName = "New GameGridSizeVar", menuName = "Data/GameGridSizeVar", order = 1)]
public class GameGridSizeVar : Variable<GridSize>
{
    
}
