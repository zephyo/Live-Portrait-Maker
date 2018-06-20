using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class opacity : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Image i = GetComponent<Image>();
        i.sprite = CreateRamp(new Color(0.3f, 0.3f, 0.3f, 1), Color.clear);
        i.rectTransform.sizeDelta = new Vector2(((RectTransform)transform.parent).rect.width , i.rectTransform.sizeDelta.y);


        Destroy(this);
    }

    Sprite CreateRamp(Color one, Color two)
    {
        int size = 256;
        Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point;

        Color[] textureData = new Color[size * size];

        for (int y = size - 1; y >= 0; y--)
        {
            for (int x = 0; x < size; x++)
            {

                Color lerp = Color.Lerp(two, one, y / (float)(size - 1));

                textureData[y + x * size] = lerp;

            }

        }
        texture.SetPixels(textureData);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0f, 1f), 233);
    }


}
