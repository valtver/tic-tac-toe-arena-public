using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UiBlurController : MonoBehaviour
{

    public Material blurMaterial;
    [Range(0f, 50f)]
    public float blurValue;
    public Color blurTint;
    // Start is called before the first frame update

    public void updateMaterial() {
        if(blurMaterial.GetFloat("_Size") != blurValue){
            blurMaterial.SetFloat("_Size", blurValue);
        }
        if(blurMaterial.GetColor("_MultiplyColor") != blurTint) {
            blurMaterial.SetColor("_MultiplyColor", blurTint);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateMaterial();
    }
}
