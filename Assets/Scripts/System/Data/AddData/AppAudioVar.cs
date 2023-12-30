using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioVar {OFF = -1, ON = 1};

[CreateAssetMenu(fileName = "New AppAudioVar", menuName = "Data/AppAudioVar", order = 1)]
public class AppAudioVar : Variable<AudioVar>
{
    
}
