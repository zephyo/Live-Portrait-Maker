using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class WaterScript : MonoBehaviour
{

    private string shader = "WaterScript";
    [Range(0, 1)] public float _Alpha = 0.57f;

    [Range(0.0f, 4f)] public float Heat = 0.2f;
    [Range(0.0f, 4f)] public float Speed = 0.46f;
    [Range(0.0f, 1f)] public float EValue = 0.393f;
    [Range(-4.0f, 4f)] public float Light = 0.97f;
    Image CanvasImage;


    void Awake()
    {
        if (CanvasImage == null)
        {
            CanvasImage = this.gameObject.GetComponent<Image>();
        }

        Material tempMaterial = new Material(Shader.Find(shader));
        tempMaterial.hideFlags = HideFlags.None;
        CanvasImage.material = tempMaterial;

    }


    void Update()
    {
        CanvasImage.material.SetFloat("_Alpha", 1 - _Alpha);
        CanvasImage.material.SetFloat("_Distortion", Heat);
        CanvasImage.material.SetFloat("_Speed", Speed);
        CanvasImage.material.SetFloat("EValue", EValue);
        CanvasImage.material.SetFloat("Light", Light);
    }

}


