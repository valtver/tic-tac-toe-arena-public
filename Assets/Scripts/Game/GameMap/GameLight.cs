using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLight : _baseCompositionModule
{

    public override IEnumerator Init() {
        yield return StartCoroutine(base.Init());
    }

    public void PlayAction() {
        AnimationComponent.Play("light-action", true);
    }

}
