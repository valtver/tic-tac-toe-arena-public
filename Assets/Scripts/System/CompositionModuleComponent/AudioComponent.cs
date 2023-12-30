using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// [RequireComponent(typeof(Animator))]
public class AudioComponent : _baseCompositionModuleComponent
{

    // [System.Serializable]
    // public class Sound {
    //     private string name;
    //     public AudioSource source;
    // }

    public AudioSource[] sounds;
    public AudioSource[] musicLayers;
    public bool musicLayersStopped = false;
    public bool musicLayersPaused = false;
    Dictionary<int, bool> musicLayerPauseState = new Dictionary<int, bool>();
    FloatTween layerVolTween = new FloatTween();

    // public override void Init()
    // {
    //     foreach(AudioSource s in sounds) {
    //         s = gameObject.AddComponent<AudioSource>();
    //     }

    // }

    public void PlaySound(string name) {
        for(int i = 0; i < sounds.Length; i++) {
            if(sounds[i].clip.name == name) {
                sounds[i].pitch = 1f;
                sounds[i].Play();
                return;
            }
        }
        Debug.Log("No sound with name " + name);
    }

    public void PlayRandomPitchSound(string name) {
        for(int i = 0; i < sounds.Length; i++) {
            if(sounds[i].clip.name == name) {
                sounds[i].pitch = Random.Range(0.8f, 1.2f);
                sounds[i].Play();
                return;
            }
        }
        Debug.Log("No sound with name " + name);
    }

    public void PlaySoundPassive(string name) {
        for(int i = 0; i < sounds.Length; i++) {
            if(sounds[i].clip.name == name) {
                if(!sounds[i].isPlaying)
                    sounds[i].Play();
                return;
            }
        }
        Debug.Log("No sound with name " + name);
    }

    public AudioComponent SetMusicLayerVol(int layerIdx, float vol) {
        for(int i = 0; i < musicLayers.Length; i++) {
            if(i <= layerIdx) {
                if(musicLayers[i].volume != vol)
                    musicLayers[i].volume = vol;
            }
            else
                musicLayers[i].volume = 0f;
        };
        return this;
    }

    public AudioComponent SetMusicLayersStart(float newTime) {
        for(int i = 0; i < musicLayers.Length; i++) {
            musicLayers[i].time = newTime;
        };
        return this;
    }

    public AudioComponent FadeUpMusicLayer(int layerIdx) {
        layerVolTween
        .DoUpdate((float value) =>{musicLayers[layerIdx].volume = value;})
        .fromValue(0f)
        .toValue(1f)
        .inTime(1f)
        .play();

        return this;
    }

    public AudioComponent GlobalFadeUpLayers(int untilLayerNr = -1) {
        PlayMusicLayers();

        layerVolTween
        .DoUpdate((float value) =>{
            if(untilLayerNr != -1)
                SetMusicLayerVol(untilLayerNr, value);
            else {
                for(int i = 0; i < musicLayers.Length; i++) {
                    musicLayers[i].volume = value;
                };
            }
        })
        .fromValue(0f)
        .toValue(1f)
        .inTime(1f)
        .play();

        return this;
    }

    public AudioComponent FadeDownMusicLayer(int layerIdx) {
        layerVolTween
        .DoUpdate((float value) =>{musicLayers[layerIdx].volume = value;})
        .fromValue(musicLayers[layerIdx].volume)
        .toValue(0f)
        .inTime(1f)
        .play();

        return this;
    }

    public AudioComponent GlobalFadeDownLayers(int untilLayerNr = -1) {
        layerVolTween
        .DoUpdate((float value) =>{
            if(untilLayerNr != -1)
                SetMusicLayerVol(untilLayerNr, value);
            else {
                for(int i = 0; i < musicLayers.Length; i++) {
                    musicLayers[i].volume = value;
                };
            }
        })
        .fromValue(1f)
        .toValue(0f)
        .inTime(0.5f)
        .OnComplete(()=>{StopMusicLayers();})
        .play();

        return this;
    }

    public AudioComponent PlayMusicLayers() {
        for(int i = 0; i < musicLayers.Length; i++) {
            if(musicLayerPauseState.TryGetValue(i, out bool val)) {
                if(val) {
                    musicLayers[i].UnPause();
                    musicLayerPauseState[i] = false;
                }
            }
            else {
                if(!musicLayers[i].isPlaying) {
                    musicLayers[i].Play();
                }
            }
        }
        musicLayersPaused = false;
        musicLayersStopped = false;
        return this;
    }

    public AudioComponent StopMusicLayers() {
        for(int i = 0; i < musicLayers.Length; i++) {
            musicLayers[i].Stop();
        }
        musicLayersStopped = true;
        return this;
    }

    public AudioComponent PauseMusicLayers() {
        for(int i = 0; i < musicLayers.Length; i++) {
            musicLayers[i].Pause();
            musicLayerPauseState[i] = true;
        }
        musicLayersPaused = true;
        return this;
    }
}
