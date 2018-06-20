using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[System.Serializable]
public class FourGradient : MonoBehaviour
{
    public Color _Color1 = new Color(0f, 0f, 0f, 1f);
    public Color _Color2 = new Color(0.54f, 0f, 0f, 1f);
    public Color _Color3 = new Color(1f, 0.54f, 0.68f, 1f);
    public Color _Color4 = new Color(0.69f, 0.91f, 1f, 1f);

    public float opacity = 0.55f;

    Material m;
    void Awake()
    {
        Image CanvasImage = gameObject.GetComponent<Image>();
        m = new Material(Shader.Find("4Gradient"));
        m.hideFlags = HideFlags.None;

        CanvasImage.material = m;
        m = CanvasImage.materialForRendering;
        updateParams();
    }

    Color withOpacity(Color input)
    {
        return new Color(input.r, input.g, input.b, opacity);
    }

    public void updateParams()
    {
        m.SetColor("_Color1", withOpacity(_Color1));
        m.SetColor("_Color2", withOpacity(_Color2));
        m.SetColor("_Color3", withOpacity(_Color3));
        m.SetColor("_Color4", withOpacity(_Color4));
    }

    public void updateSingleParam(int i)
    {
        switch (i)
        {
            case 1:
                m.SetColor("_Color1", withOpacity(_Color1));
                break;
            case 2:
                m.SetColor("_Color2", withOpacity(_Color2));
                break;
            case 3:
                m.SetColor("_Color3", withOpacity(_Color3));
                break;
            case 4:
                m.SetColor("_Color4", withOpacity(_Color4));
                break;
        }
    }
    void OnDestroy()
    {
        GetComponent<Image>().material=null;
    }



    //  void OnEnable()
    //     {
    //         if (this.gameObject.GetComponent<Image>() != null)
    //         {
    //             if (CanvasImage == null) CanvasImage = this.gameObject.GetComponent<Image>();
    //         }


    //         if (defaultMaterial == null)
    //         {
    //             defaultMaterial = new Material(Shader.Find("Sprites/Default"));
    //         }

    //         if (ForceMaterial == null)
    //         {
    //             ActiveChange = true;
    //             tempMaterial = new Material(Shader.Find(shader));
    //             tempMaterial.hideFlags = HideFlags.None;

    //             if (this.gameObject.GetComponent<SpriteRenderer>() != null)
    //             {
    //                 this.GetComponent<Renderer>().sharedMaterial = tempMaterial;
    //             }
    //             else if (this.gameObject.GetComponent<Image>() != null)
    //             {
    //                 CanvasImage.material = tempMaterial;
    //             }
    //         }
    //         else
    //         {
    //             ForceMaterial.shader = Shader.Find(shader);
    //             ForceMaterial.hideFlags = HideFlags.None;
    //             if (this.gameObject.GetComponent<SpriteRenderer>() != null)
    //             {
    //                 this.GetComponent<Renderer>().sharedMaterial = ForceMaterial;
    //             }
    //             else if (this.gameObject.GetComponent<Image>() != null)
    //             {
    //                 CanvasImage.material = ForceMaterial;
    //             }
    //         }   

    //     }
}

