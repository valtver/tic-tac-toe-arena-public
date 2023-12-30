using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : _baseCompositionModule
{

    public baseEvent eventGameMapIntroComplete;

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
        AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;
    }

    void OnAudioConfigurationChanged(bool deviceWasChanged) {
            AudioComponent.PlaySound("ambient");
            if(!AudioComponent.musicLayersStopped) {
                StopMusic();
                PlayMusic();
            }
    }

    public void PlayIdle() {
        AnimationComponent.Play("map-idle", true);
        AudioComponent.PlaySound("ambient");
    }

    public void PlayIntro() {
        // AnimationComponent.Play("map-idle", true).OnComplete(()=>{
            eventGameMapIntroComplete.Raise();
        // });
        
    }

    public void PlayRoundMusicIntro() {
        AudioComponent.SetMusicLayersStart(2.5f);
        AudioComponent.SetMusicLayerVol(0, 1f);
        AudioComponent.PlayMusicLayers();
    }

    public void PlayRoundMusic() {
        AudioComponent.FadeUpMusicLayer(Data.Instance.Game.Round);
        // AudioComponent.SetMusicLayerVol(round, 1f);
    }

    public void PauseMusic() {
        AudioComponent.GlobalFadeDownLayers(Data.Instance.Game.Round);
        // AudioComponent.PauseMusicLayers();
    }

    public void PlayMusic() {
        AudioComponent.GlobalFadeUpLayers(Data.Instance.Game.Round);
        // AudioComponent.PlayMusicLayers();
    }

    public void StopMusic() {
        AudioComponent.StopMusicLayers();
    }

    void OnDisable() {
        AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigurationChanged;
    }
}
