using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[System.Serializable]
public class WaterfallScript : MonoBehaviour
{

    private string shader = "WaterfallScript";
    [Range(0, 1)] public float _Alpha = 1f;
    [HideInInspector] public Texture2D __MainTex2;
    [Range(0.0f, 2f)] public float Liquid = 0f;
    [Range(-2.0f, 4f)] public float Speed = -0.2f;
    [Range(-2f, 2f)] public float EValue = -0.46f;
    [Range(-2f, 2f)] public float TValue = -0.38f;
    public Color LightColor = new Color(0.2f, 0.5f, 1, 1);
    [Range(-1f, 1f)] public float Light = -0.62f;
    Image CanvasImage;
   

    void Awake()
    {
        if (CanvasImage == null)
        {
            CanvasImage = GetComponent<Image>();
        }
        Material tempMaterial = new Material(Shader.Find(shader));
        tempMaterial.hideFlags = HideFlags.None;

        CanvasImage.material = tempMaterial;

        __MainTex2 = Resources.Load("WaterfallTexture") as Texture2D;

        if (__MainTex2)
        {
            __MainTex2.wrapMode = TextureWrapMode.Repeat;
            CanvasImage.material.SetTexture("_MainTex2", __MainTex2);
        }
    }

    void Update()
    {



        CanvasImage.material.SetFloat("_Alpha", _Alpha);
        CanvasImage.material.SetFloat("_Distortion", Liquid);
        CanvasImage.material.SetFloat("_Speed", Speed);
        CanvasImage.material.SetFloat("EValue", EValue);
        CanvasImage.material.SetFloat("TValue", TValue);
        CanvasImage.material.SetFloat("Light", Light);
        CanvasImage.material.SetColor("Lightcolor", LightColor);

    }




}


