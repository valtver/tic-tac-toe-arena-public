using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Data : MonoBehaviour
{
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static Data _instance = null;
    private static bool m_ShuttingDown = false;
 
    // A static property that finds or creates an instance of the manager object and returns it.
    public static Data Instance
    {

        get
        {
            if (m_ShuttingDown)
            {
                // Debug.LogWarning("EventSystem Instance " +
                //     "already destroyed. Returning null.");
                return null;
            }
            if (_instance == null)
            {
                // FindObjectOfType() returns the first AManager object in the scene.
                _instance = FindObjectOfType(typeof(Data)) as Data;
            }
            // If it is still null, create a new instance
            if (_instance == null)
            {
                Debug.LogWarning("Data INSTANTIATED!");
                var obj = Resources.Load("Data/Data", typeof(GameObject));
                GameObject inst = Instantiate(obj) as GameObject;
                inst.name = obj.name;
                Data.Instance.Init();
            }
            return _instance;
        }
    }

    public void Init() {

    }
 
    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnDestroy()
    {
        m_ShuttingDown = true;
        _instance = null;
        Debug.LogWarning("Loader Singleton DESTROYED!");
    }

    public AppData App;
    public GameData Game;
    public GameUiData Ui;

    // [System.Serializable]
    // public playerData playerData;

    // Add your own code here...
}