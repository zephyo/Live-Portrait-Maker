using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class Iris : MonoBehaviour
{

    public DressManager dM;

    //List<bool> bools; //left, right, bttom left, bottom right

    public List<Image> imgs;
    List<UnityAction> listeners;

    bool exclusive;

    private void Awake()
    {
        float cellWidth = ((RectTransform)transform.parent).rect.width / 2;
        GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellWidth, 123.6f);


        if (dM == null)
        {
            dM = GameObject.FindGameObjectWithTag("Respawn").GetComponent<DressManager>();
            listeners = new List<UnityAction>(4);
            imgs = new List<Image>(4);
            imgs.Add(transform.GetChild(0).GetComponent<Image>());
            imgs.Add(transform.GetChild(1).GetComponent<Image>());
            listeners.Add(null);
            listeners.Add(null);
        }
    }


    public void fillButtons(string l, string r, UnityAction la, UnityAction ra)
    {
        gameObject.SetActive(true);
        Image etL = imgs[0];
        Image etR = imgs[1];
        etL.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = l;
        etR.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = r;
        listeners[0] = la;
        listeners[1] = ra;
    }

    public void exclusiveButtons(string l, string r, UnityAction la, UnityAction ra, Button x, Button remove = null)
    {
        gameObject.SetActive(true);
        Image etL = imgs[0];
        Image etR = imgs[1];

        etL.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = l;
        etR.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = r;
        listeners[0] = la;
        listeners[1] = ra;
        Debug.Log("exclusive!");
        exclusive = true;
        Sprite oldButt = etL.sprite;
        etL.sprite = Resources.Load<Sprite>("butt3");
        etR.sprite = etL.sprite;
        Color orig = etL.color;
        etL.color = Color.white;
        etR.color = new Color(0f, 0f, 0f, 0.466f);

        UnityAction changeBack = () =>
       {
           etL.color = orig; etR.color = orig;
           etL.sprite = oldButt; etR.sprite = oldButt; exclusive = false;
       };
        x.onClick.AddListener(changeBack);
        if (remove != null)
        {
            remove.onClick.AddListener(changeBack);
        }

    }


    public void fill2MoreButtons(string l, string r, UnityAction la, UnityAction ra, Button x, Button remove)
    {
        Image bottomL = GameObject.Instantiate(imgs[0].transform, transform, false).GetComponent<Image>();
        Image bottomR = GameObject.Instantiate(bottomL, transform, false).GetComponent<Image>();
        bottomR.transform.SetAsFirstSibling();
        bottomL.transform.SetAsFirstSibling();
        bottomL.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = l;
        bottomR.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = r;
        listeners.Insert(2, la);
        listeners.Insert(3, ra);
        imgs.Add(bottomL);
        imgs.Add(bottomR);

        UnityAction removeL = () =>
        {

            listeners.Remove(la);
            listeners.Remove(ra);
            imgs.Remove(bottomL);
            imgs.Remove(bottomR);
            Destroy(bottomL.gameObject);
            Destroy(bottomR.gameObject);
        };
        x.onClick.AddListener(removeL);
        if (remove != null)
        {
            remove.onClick.AddListener(removeL);
        }

    }

    public void setListeners(int index = 0)
    {
        dM.cpa.clearUpdateColor();
        for (int i = index; i < listeners.Count; i++)
        {
            UnityAction temp = listeners[i];
            if (imgs[i].color == Color.black)
            {
                dM.cpa.UpdateColorAction += () => temp();
            }
        }
    }

    public void change(Image img)
    {
        if (exclusive)
        {
            if (img.color != Color.white)
            {
                if (img.color == Color.black)
                {
                    img.color = new Color(0.49f, 0.5f, 0.5f, 0.55f);
                    setListeners(2);
                }
                else if (img.color.a == 0.55f)
                {
                    img.color = Color.black;
                    setListeners(2);

                }


                else
                {
                    img.color = Color.white;
                    if (imgs[0] == img)
                    {
                        listeners[0]();
                        imgs[1].color = new Color(0f, 0f, 0f, 0.466f);
                    }
                    else
                    {
                        listeners[1]();
                        imgs[0].color = new Color(0f, 0f, 0f, 0.466f);
                    }
                }

            }
            return;
        }
        if (img.color == Color.black)
        {
            img.color = new Color(0.49f, 0.5f, 0.5f, 0.55f);
        }
        else
        {
            img.color = Color.black;
        }
        setListeners();

    }


}
