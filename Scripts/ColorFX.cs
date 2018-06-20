using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColorFX : MonoBehaviour
{
    [Range(0, 1)]
    public float Amount = 1.0f;

    public Color color;

    Material curMaterial;

    Material material
    {
        get
        {
            if (curMaterial == null)
            {
                curMaterial = new Material(Shader.Find("SepiaFX"));
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial;
        }
    }

    private void Start()
    {
        color=new Color(0.4f,0.4f,0.4f,1);
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
    }

    public void updateColor()
    {
        material.SetFloat("_EffectAmount", Amount);
        material.SetFloat("_r", color.r);

        material.SetFloat("_g", color.g);
        material.SetFloat("_b", color.b);



    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);

    }

    private void OnDisable()
    {
        if (curMaterial)
            DestroyImmediate(curMaterial);
    }
}
