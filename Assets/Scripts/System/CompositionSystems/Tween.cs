using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tween : MonoBehaviour
{
    [SerializeField]
    int activeTweenCounterFloat = 0;
    [SerializeField]
    int activeTweenCounterVector = 0;
    [SerializeField]
    int activeTweenCounterColor = 0;
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    public List<FloatTween> ftUpdateList = null;
    public List<VectorTween> vtUpdateList = null;
    public List<ColorTween> clUpdateList = null;

    private List<FloatTween> ftUpdateListSnapshot = null;
    private List<VectorTween> vtUpdateListSnapshot = null;
    private List<ColorTween> clUpdateListSnapshot = null;

    private static Tween _instance = null;
    private static bool m_ShuttingDown = false;
 
    // A static property that finds or creates an instance of the manager object and returns it.
    public static Tween Instance
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
                _instance = (Tween)FindObjectOfType(typeof(Tween));
            }
            // If it is still null, create a new instance
            if (_instance == null)
            {
                Debug.LogWarning("DEBUG Tween INSTANTIATED!");

                var obj = Resources.Load("Tween/Tween", typeof(GameObject));
                GameObject inst;
                if(GameObject.Find("App")) {
                    inst = Instantiate(obj, GameObject.Find("App").transform) as GameObject;
                }
                else {
                    inst = Instantiate(obj) as GameObject;
                }
                inst.name = obj.name;
                Tween.Instance.Init();
            }
            return _instance;
        }
    }
 
    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnDestroy()
    {
        m_ShuttingDown = true;
        _instance = null;
        Debug.LogWarning("Tween Singleton DESTROYED!");
    }

    public void Init() {
        ftUpdateList = new List<FloatTween>();
        vtUpdateList = new List<VectorTween>();
        clUpdateList = new List<ColorTween>();

        ftUpdateListSnapshot = new List<FloatTween>();
        vtUpdateListSnapshot = new List<VectorTween>();
        clUpdateListSnapshot = new List<ColorTween>();
    }

    void Update() {
        // Debug.Log("ftUpdateList: " + ftUpdateList.Count);
        // Debug.Log("vtUpdateList: " + vtUpdateList.Count);
        // Debug.Log("clUpdateList: " + clUpdateList.Count);

        if(ftUpdateList != null && ftUpdateList.Count > 0) {
            ftUpdateListSnapshot.AddRange(ftUpdateList); 
            for(int i = 0; i < ftUpdateListSnapshot.Count; i++) {
                if(ftUpdateListSnapshot[i].stateSet == FloatTween.tweenState.play)
                    ftUpdateListSnapshot[i].update();
                else
                    ftUpdateList.Remove(ftUpdateListSnapshot[i]);
            }
            ftUpdateListSnapshot.Clear();
            activeTweenCounterFloat = ftUpdateList.Count; //DEBUG DATA
        }
        if(vtUpdateList != null && vtUpdateList.Count > 0) {
            vtUpdateListSnapshot.AddRange(vtUpdateList); 
            for(int i = 0; i < vtUpdateListSnapshot.Count; i++) {
                if(vtUpdateListSnapshot[i].stateSet == VectorTween.tweenState.play)
                    vtUpdateListSnapshot[i].update();
                else
                    vtUpdateList.Remove(vtUpdateListSnapshot[i]);
            }
            vtUpdateListSnapshot.Clear();
            activeTweenCounterVector = vtUpdateList.Count; //DEBUG DATA
        }
        if(clUpdateList != null && clUpdateList.Count > 0) {
            clUpdateListSnapshot.AddRange(clUpdateList); 
            for(int i = 0; i < clUpdateListSnapshot.Count; i++) {
                if(clUpdateListSnapshot[i].stateSet == ColorTween.tweenState.play)
                    clUpdateListSnapshot[i].update();
                else
                    clUpdateList.Remove(clUpdateListSnapshot[i]);
            }
            clUpdateListSnapshot.Clear();
            activeTweenCounterColor = clUpdateList.Count; //DEBUG DATA
        }
    }
}

//----------------------------------------

public class FloatTween
{
    public delegate void _Cb();
    public delegate void _Call(float value);
    private _Call onUpdate = null;
    private _Cb onComplete = null;

    public enum tweenState {
        play = 1,
        pause = 2,
        stop = 3
    }

    public tweenState stateSet;

    private float deltaT;// = 1f;
    private float timeIn;// = 1f;
    private EasingFunction.Function ease;// = EasingFunction.Ease.Linear;

    private float from, to;
    private bool isLoop;

    public FloatTween DoUpdate(_Call call) {
        onUpdate = call;
        return this;
    }

    public FloatTween OnComplete(_Cb cback) {
        onComplete = cback;
        return this;
    }

    public FloatTween() {
        stateSet = tweenState.stop;
        deltaT = 1f;
        timeIn = 1f;
        ease = EasingFunction.GetEasingFunction(EasingFunction.Ease.Linear);
        isLoop = false;
    }

    public void play() {
        this.RemoveTween();
        if(stateSet == tweenState.stop || stateSet == tweenState.play) {
            deltaT = 0f;
            stateSet = tweenState.play;
            this.AddTween();
        }
        else if(stateSet == tweenState.pause) {
            stateSet = tweenState.play;
            this.AddTween();
        }
    }

    public void pause() {
        if(stateSet == tweenState.play) {
            stateSet = tweenState.pause;
        }
    }

    public void stop() {
        if(stateSet == tweenState.play) {
            stateSet = tweenState.stop;
            deltaT = 0;
            onComplete = null;
        }
    }

    public void complete() {
        if (onUpdate != null)  {
            onUpdate(to);
        }
        if(onComplete != null) {
            onComplete();
            onComplete = null;
        }
        if(isLoop) {
            play();
            return;
        }
        stateSet = tweenState.stop;
        deltaT = 1f;
    }

    public FloatTween fromValue(float val) {
        from = val;
        return this;
    }
    public FloatTween toValue(float val) {
        to = val;
        return this;
    }

    public FloatTween inTime(float sec) {
        timeIn = sec;
        return this;
    }

    public FloatTween inLoop() {
        isLoop = true;
        return this;
    }

    public FloatTween easeLike(EasingFunction.Ease eType) {
        ease = EasingFunction.GetEasingFunction(eType);
        return this;
    }
    
    public void update() {
            if(deltaT < 1f) {
                deltaT += Time.unscaledDeltaTime/timeIn;
                if(deltaT >= 1f) {
                    complete();
                }
                if (onUpdate != null) 
                    onUpdate(ease(from, to, deltaT));
            }
    }
}
//--------------------------------------------VECTOR3 TWEEN--------------------------------------------------
public class VectorTween
{
    public delegate void _Cb();
    public delegate void _Call(Vector3 value);
    public _Call onUpdate = null;
    public _Cb onComplete = null;

    public enum tweenState {
        play = 1,
        pause = 2,
        stop = 3
    }

    public tweenState stateSet;

    private float deltaT;// = 1f;
    private float timeIn;// = 1f;
    private EasingFunction.Function ease;// = EasingFunction.Ease.Linear;

    private Vector3 from, to;
    private bool isLoop;

    public VectorTween DoUpdate(_Call call) {
        onUpdate = call;
        return this;
    }
        
    public VectorTween OnComplete(_Cb cback) {
        onComplete = cback;
        return this;
    }

    public VectorTween() {
        stateSet = tweenState.stop;
        deltaT = 1f;
        timeIn = 1f;
        ease = EasingFunction.GetEasingFunction(EasingFunction.Ease.Linear);
        isLoop = false;
    }

    public void play() {
        this.RemoveTween();
        if(stateSet == tweenState.stop || stateSet == tweenState.play) {
            deltaT = 0f;
            stateSet = tweenState.play;
            this.AddTween();
        }
        else if(stateSet == tweenState.pause) {
            stateSet = tweenState.play;
            this.AddTween();
        }
    }

    public void pause() {
        if(stateSet == tweenState.play) {
            stateSet = tweenState.pause;
        }
    }

    public void stop() {
        if(stateSet == tweenState.play) {
            stateSet = tweenState.stop;
            deltaT = 0;
            onComplete = null;
        }
    }

    public void complete() {
        if (onUpdate != null) 
            onUpdate(to);
        if(onComplete != null) {
            onComplete();
            onComplete = null;
        }
        if(isLoop) {
            play();
            return;
        }
        stateSet = tweenState.stop;
        deltaT = 1f;
    }

    public VectorTween fromValue(Vector3 vec) {
        from = vec;
        return this;
    }
    public VectorTween toValue(Vector3 vec) {
        to = vec;
        return this;
    }

    public VectorTween inTime(float sec) {
        timeIn = sec;
        return this;
    }

    public VectorTween inLoop() {
        isLoop = true;
        return this;
    }

    public VectorTween easeLike(EasingFunction.Ease eType) {
        ease = EasingFunction.GetEasingFunction(eType);
        return this;
    }
    
    public void update() {
            if(deltaT < 1f) {
                deltaT += Time.unscaledDeltaTime/timeIn;
                if(deltaT >= 1f) {
                    complete();
                }
                if (onUpdate != null) 
                    onUpdate(new Vector3(ease(from.x, to.x, deltaT), ease(from.y, to.y, deltaT), ease(from.z, to.z, deltaT)));
            }
    }
}
//---------------------------------------------COLOR TWEEN------------------------------------------
public class ColorTween
{
    public delegate void _Cb();
    public delegate void _Call(Color value);
    public _Call onUpdate = null;
    public _Cb onComplete = null;

    public enum tweenState {
        play = 1,
        pause = 2,
        stop = 3
    }

    public tweenState stateSet;

    private float deltaT;// = 1f;
    private float timeIn;// = 1f;
    private EasingFunction.Function ease;// = EasingFunction.Ease.Linear;

    private Color from, to;
    private bool isLoop;

    public ColorTween DoUpdate(_Call call) {
        onUpdate = call;
        return this;
    }

    public ColorTween OnComplete(_Cb cback) {
        onComplete = cback;
        return this;
    }

    public ColorTween() {
        stateSet = tweenState.stop;
        deltaT = 1f;
        timeIn = 1f;
        ease = EasingFunction.GetEasingFunction(EasingFunction.Ease.Linear);
        isLoop = false;
    }

    public void play() {
        this.RemoveTween();
        if(stateSet == tweenState.stop || stateSet == tweenState.play) {
            deltaT = 0f;
            stateSet = tweenState.play;
            this.AddTween();
        }
        else if(stateSet == tweenState.pause) {
            stateSet = tweenState.play;
            this.AddTween();
        }
    }

    public void pause() {
        if(stateSet == tweenState.play) {
            stateSet = tweenState.pause;
        }
    }

    public void stop() {
        if(stateSet == tweenState.play) {
            stateSet = tweenState.stop;
            deltaT = 0;
            onComplete = null;
        }
    }

    public void complete() {
        if (onUpdate != null) 
            onUpdate(to);
        if(onComplete != null) {
            onComplete();
            onComplete = null;
        }
        if(isLoop) {
            play();
            return;
        }
        stateSet = tweenState.stop;
        deltaT = 1f;
    }

    public ColorTween fromValue(Color col) {
        from = col;
        return this;
    }
    public ColorTween toValue(Color col) {
        to = col;
        return this;
    }

    public ColorTween inTime(float sec) {
        timeIn = sec;
        return this;
    }

    public ColorTween inLoop() {
        isLoop = true;
        return this;
    }

    public ColorTween easeLike(EasingFunction.Ease eType) {
        ease = EasingFunction.GetEasingFunction(eType);
        return this;
    }
    
    public void update() {

            if(deltaT < 1f) {
                deltaT += Time.unscaledDeltaTime/timeIn;
                if(deltaT >= 1f) {
                    complete();
                }
                if(onUpdate != null) 
                    onUpdate(new Color(
                        ease(from.r, to.r, deltaT),
                        ease(from.g, to.g, deltaT), 
                        ease(from.b, to.b, deltaT), 
                        ease(from.a, to.a, deltaT))
                    );
            }
        
    }
}

public static class TweenHelper {
    public static void AddTween(this FloatTween ft) {
        Tween.Instance.ftUpdateList.Add(ft);
    }
    public static void AddTween(this VectorTween vt) {
        Tween.Instance.vtUpdateList.Add(vt);
    }
    public static void AddTween(this ColorTween cl) {
        Tween.Instance.clUpdateList.Add(cl);
    }
    public static void RemoveTween(this FloatTween ft) {
        Debug.Log(ft);
        Tween.Instance.ftUpdateList.Remove(ft);
    }
    public static void RemoveTween(this VectorTween vt) {
        Tween.Instance.vtUpdateList.Remove(vt);
    }
    public static void RemoveTween(this ColorTween cl) {
        Tween.Instance.clUpdateList.Remove(cl);
    }
}