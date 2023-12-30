using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New AppData", menuName = "Data/AppData", order = 1)]
public class AppData : ScriptableObject
{
    public AppAudioVar Sound;
    public AppAudioVar Music;
    public Color[] ButtonStateColor;
}
