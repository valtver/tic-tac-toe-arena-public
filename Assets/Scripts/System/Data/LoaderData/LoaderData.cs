using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LoaderData", menuName = "Data/LoaderData", order = 1)]
public class LoaderData : ScriptableObject
{
    public GameLevelVar sceneName;
    public string sceneFormat = ".unity";
    public string sceneDir = "Scenes";

    public GameGridSizeVar preloadName;
    public string preloadDir = "Game";

    public List<string> preloadList = new List<string>();


}
