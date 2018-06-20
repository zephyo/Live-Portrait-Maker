using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class ColorPicker : MonoBehaviour
{



    public float H = 0.5f, S = 0, B = 1;
    public Color Color;


    public event Action UpdateColorAction;





    public Image img;
    public Image satu;

    bool reseting;

    public void clearUpdateColor() { UpdateColorAction = null; }


    void Start()
    {
       
        createTexture(H);

    }


    public void Reset()
    {
        Debug.Log("reseting with "+Color);
        reseting = true;
        HSBColor hsb = HSBColor.FromColor(Color);
        H = hsb.h;
        S = hsb.s;
        B = hsb.b;
        img.color = Color;
        transform.GetChild(0).GetComponent<Slider>().value = H;
        transform.GetChild(1).GetComponent<Slider>().value = S;
        transform.GetChild(2).GetComponent<Slider>().value = B;
        createTexture(H);
        reseting = false;
    }



    void createTexture(float val)
    {
        HSBColor color = new HSBColor(val, S, 1);
        int size = 256;
        Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point;

        Color[] textureData = new Color[size * size];
        color.s = 0;

        for (int x = 0; x < size; x++)
        {
            for (int y = size - 1; y >= 0; y--)
            {
                color.s = Mathf.Clamp(y / (float)(size - 1), 0, 1);

                textureData[x + y * size] = color.ToColor();

            }

        }

        texture.SetPixels(textureData);
        texture.Apply();

        satu.sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0f, 1f), 233);

    }





    public void coloringSpectrum(float val)
    {
        if (!reseting)
        {
            HSBColor color = new HSBColor(val, S, B);
            createTexture(val);
            UpdateColor(color);
        }

    }
    public void Saturation(float val)
    {
        if (!reseting)
        {
            HSBColor color = new HSBColor(H, val, B);
            UpdateColor(color);
        }
    }

    public void Brightness(float val)
    {
        if (!reseting)
        {

            HSBColor color = new HSBColor(H, S, val);
            UpdateColor(color);
        }
    }


    private void UpdateColor(HSBColor color)
    {
        Color = color.ToColor();

        H = color.h;
        S = color.s;
        B = color.b;

        UpdateColor();
    }

    public void UpdateColor()
    {
        img.color = Color;
       if (UpdateColorAction!=null) UpdateColorAction();


    }

     public Iris getLeftRight()
    {
        Transform ret = transform.parent.GetChild(4).GetChild(2);
        ret.gameObject.SetActive(true);
        return ret.GetComponent<Iris>();

    }



    //Generates a 256x256 texture with all variations for the selected HUE

}
