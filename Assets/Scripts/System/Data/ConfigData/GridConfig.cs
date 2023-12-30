using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GridConfig", menuName = "Config/GridConfig", order = 1)]
public class GridConfig : ScriptableObject
{
    public List<Vector3> unitPositionList;
    public List<Vector3> unitScaleList;
}
