

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[System.Serializable]
public class Pattern : MonoBehaviour
{
    [Range(0, 1)] public float _Alpha = 1f;

    public Texture2D __MainTex2;
    public float _OffsetX;
    public float _OffsetY;
    Image CanvasImage;

    void Awake()
    {

        CanvasImage = this.gameObject.GetComponent<Image>();
        Material tempMaterial = new Material(Shader.Find("Pattern"));
        tempMaterial.hideFlags = HideFlags.None;
        CanvasImage.material = tempMaterial;
    }




    public void updateParams()
    {
        CanvasImage.material.SetFloat("_OffsetY", _OffsetY);
        CanvasImage.material.SetFloat("_OffsetX", _OffsetX);
        CanvasImage.material.SetFloat("_Alpha", 1 - _Alpha);

    }



    public void setTexture()
    {
        __MainTex2.wrapMode = TextureWrapMode.Repeat;
        CanvasImage.material.SetTexture("_MainTex2", __MainTex2);
    }




}




