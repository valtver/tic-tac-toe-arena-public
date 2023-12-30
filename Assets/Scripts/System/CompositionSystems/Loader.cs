using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static Loader _instance = null;
    private static bool m_ShuttingDown = false;
 
    // A static property that finds or creates an instance of the manager object and returns it.
    public static Loader Instance
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
                _instance = FindObjectOfType(typeof(Loader)) as Loader;
            }
 
            // If it is still null, create a new instance
            if (_instance == null)
            {
                Debug.LogWarning("Loader Singleton INSTANTIATED!");
                var obj = Resources.Load("Loader/Loader", typeof(GameObject));
                GameObject inst = Instantiate(obj) as GameObject;
                inst.name = obj.name;
                Loader.Instance.Init();
            }
            return _instance;
        }
    }
 
    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnDestroy()
    {
        m_ShuttingDown = true;
        _instance = null;
        Debug.LogWarning("Loader Singleton DESTROYED!");
    }

    [Header("Behavior")]
    public baseEvent eventLoaderComplete;
    // public appData data;
    [SerializeField]
    private List<string> preloadData = new List<string>();
    [SerializeField]
    public static Dictionary<string, GameObject> preloadedAssets = new Dictionary<string, GameObject>();

    void OnEnable() {
        preloadData.Clear();
        preloadedAssets.Clear();
    }

    public void Init() {

    }

    public void Load(LoaderData loaderData) {
        Debug.Log(this.gameObject + " called Load!");
        StartCoroutine(LoadOrder(loaderData));
    }

    IEnumerator LoadOrder(LoaderData loaderData) {
        if(SceneManager.GetActiveScene().name != "Application") {
            yield return StartCoroutine(unloadSceneAsync(SceneManager.GetActiveScene().name));
        }
        
        yield return StartCoroutine(unloadAssetsAsync());
        yield return StartCoroutine(preloadPaths(loaderData));
        yield return StartCoroutine(loadSceneAsync(((int)loaderData.sceneName.RuntimeValue).ToString()));

        // Data.Instance.Session = session;

        eventLoaderComplete.Raise();
        Debug.Log("PRELOADED ASSETS COUNT: " + preloadedAssets.Count);
        StopAllCoroutines();
    }

    IEnumerator unloadSceneAsync(string sceneName) {
        AsyncOperation asyncUnload;
        asyncUnload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene(), UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        while ( !asyncUnload.isDone) {
            Debug.Log("Progress of async UNLOAD scene: " + asyncUnload.progress*100f);
            yield return null;
        }
        Debug.Log("Scene unloaded");
    }

    IEnumerator unloadAssetsAsync() {
        if(preloadedAssets.Count > 0) {
            Debug.Log("Preloaded assets exist. Clearing.");
            preloadedAssets.Clear();
            yield return null;
        }

        GC.Collect(); //DUNNO :/

        AsyncOperation asyncUnloadAssets;
        asyncUnloadAssets = Resources.UnloadUnusedAssets();
        while ( !asyncUnloadAssets.isDone) {
            Debug.Log("Progress of async UNLOAD assets: " + asyncUnloadAssets.progress*100f);
            yield return null;
        }
        Debug.Log("Assets unloaded");
    }

    IEnumerator loadSceneAsync(string sceneName)
    {
        Debug.Log("Loading scene async: " + sceneName);
        AsyncOperation asyncLoad;
        asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            Debug.Log("Progress of async LOAD scene: " + asyncLoad.progress*100f);
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        Debug.Log("Active scene is " + SceneManager.GetActiveScene().name);
        
    }

    IEnumerator preloadPaths(LoaderData loaderData) {
        if(loaderData.preloadList.Count > 0) {
            Debug.Log("Preload paths found! Preloading...");
            foreach(string dataPath in loaderData.preloadList) {
                yield return StartCoroutine(getAsyncAsset(dataPath));
            }
        }
        yield return null;
    }

    IEnumerator getAsyncAsset(string path) {
        Debug.Log("Async preload started: " + path);
        ResourceRequest resourcesRequest = Resources.LoadAsync(path);
        while(!resourcesRequest.isDone) {
            Debug.Log(path + " async preload: " + resourcesRequest.progress*100f + "%");
            yield return null;
        }
        preloadedAssets.Add(path,  resourcesRequest.asset as GameObject);
        Debug.Log("Async preload complete: " + path + " --- " + resourcesRequest.asset);
        resourcesRequest = null;
    }

    public void populatePreloadStatePaths(LoaderData loaderData) {
        preloadData.Clear();
        preloadData.AddRange(loaderData.preloadList);
        // if(state.Name == "ui") {
        //     foreach(levelData lvl in DataBase.Instance.data.level) {
        //         if(lvl.uiPrefabPath != "") {
        //             preloadData.Add(lvl.uiPrefabPath);
        //         }
        //         foreach(subLevelData subLvl in lvl.subLevel) {
        //             if(subLvl.uiPrefabPath != "") {
        //                 preloadData.Add(subLvl.uiPrefabPath);
        //             }
        //         }
        //     }
        //     foreach(handData hnd in DataBase.Instance.data.hand) {
        //         if(hnd.uiPrefabPath != "") {
        //             preloadData.Add(hnd.uiPrefabPath);
        //         }                
        //     }
        //     foreach(achData ach in DataBase.Instance.data.ach) {
        //         if(ach.uiPrefabPath != "") {
        //             preloadData.Add(ach.uiPrefabPath);
        //         }                
        //     }
        //     Debug.Log("UI Paths set!");
        // }
        // if(state.Name == "game") {
        //     string path;

        //     var lvlIdx = DataBase.Instance.data.session.levelIndex;
        //     path = DataBase.Instance.data.level[lvlIdx].gamePrefabPath;
        //     if(path != "") {
        //         preloadData.Add(path);
        //     }

        //     var subLvlIdx = DataBase.Instance.data.session.subLevelIndex;
        //     path = DataBase.Instance.data.level[lvlIdx].subLevel[subLvlIdx].gamePrefabPath;
        //     if(path != "") {
        //         preloadData.Add(path);
        //     }

        //     var handIdx = DataBase.Instance.data.session.handIndex;
        //     path = DataBase.Instance.data.hand[handIdx].gamePrefabPath;
        //     if(path != "") {
        //         preloadData.Add(path);
        //     }

        //     if(DataBase.Instance.data.purchase.isActive()) {
        //         path = DataBase.Instance.data.level[lvlIdx].gorePrefabPath;
        //         if(path != "") {
        //             preloadData.Add(path);
        //         }
        //         path = DataBase.Instance.data.level[lvlIdx].subLevel[subLvlIdx].gorePrefabPath;
        //         if(path != "") {
        //             preloadData.Add(path);
        //         }
        //     }
        // }
    }

    public GameObject GetAssetByPath(string path) {
        GameObject obj;
        if(preloadedAssets.TryGetValue(path, out obj))
        {
            Debug.Log("Preloaded assets count: " + preloadedAssets.Count + ". Found: " + obj + " at " + path);
            return obj;
        }
        else
        {
            Debug.LogWarning(path + " No such asset path...trying to load now");
            obj = Resources.Load(path) as GameObject;
            Debug.Log(obj + " is loaded");
            return obj;
        }
    }

    void OnDisable() {
        preloadData.Clear();
        preloadedAssets.Clear();
    }

}