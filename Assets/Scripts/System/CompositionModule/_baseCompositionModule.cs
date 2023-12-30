using System.Collections;
using UnityEngine;

public class _baseCompositionModule : MonoBehaviour
{
    [Header("Module Components")]
    public AnimationComponent AnimationComponent;

    public AudioComponent AudioComponent;
    
    public virtual IEnumerator Init() {
        for(int i = 0; i < this.transform.childCount; i++) {
            _baseCompositionModule[] module = this.transform.GetChild(i).gameObject.GetComponents<_baseCompositionModule>();
            if(module.Length > 0) {
                // Debug.Log(this.transform.parent.name + " -> " + this.gameObject.name + ": _baseModule Init() call..." );
                for(int j = 0; j < module.Length; j++) {
                    yield return StartCoroutine(module[j].Init());
                }
            }
        }
        yield return null;

        if(AnimationComponent == null) {
            if(GetComponent<AnimationComponent>() != null) {
                AnimationComponent = GetComponent<AnimationComponent>();
            }
        }

        if(AudioComponent == null) {
            if(GetComponent<AudioComponent>() != null) {
                AudioComponent = GetComponent<AudioComponent>();
                AudioComponent.Init();
            }
        }
    }

    public virtual void Setup() {
        for(int i = 0; i < this.transform.childCount; i++) {
            _baseCompositionModule module = this.transform.GetChild(i).gameObject.GetComponent<_baseCompositionModule>();
            if(module != null) {
                // Debug.Log(this.transform.parent.gameObject.name + " -> " + this.gameObject.name + ": _baseModule Setup() call..." );
                module.Setup();
            }
        }
    }

    public virtual void Reset() {
        for(int i = 0; i < this.transform.childCount; i++) {
            _baseCompositionModule module = this.transform.GetChild(i).gameObject.GetComponent<_baseCompositionModule>();
            if(module != null) {
                // Debug.Log(this.transform.parent.name + " -> " + this.gameObject.name + ": _baseModule Setup() call..." );
                module.Reset();
            }
        }
    }
}
