using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Button[] buttons;
    Image[] image;

    public Button lastCaller, nextCaller;

    public void SwitchTab(Button caller)
    {

        TurnOffChildren();
        Color32 norm = new Color32(255, 176, 181, 255);
        if (image == null)
        {
            image = new Image[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                image[i] = buttons[i].transform.GetChild(0).GetComponent<Image>();
            }
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            Button b = buttons[i];
            ColorBlock cb = b.colors;
            if (b == caller)
            {
                cb.normalColor = norm;
                cb.highlightedColor = norm;
                image[i].color = Color.black;
            }
            else
            {
                cb.normalColor = cb.disabledColor;
                cb.highlightedColor = cb.disabledColor;
                image[i].color = Color.white;
            }
            b.colors = cb;

        }
        lastCaller=nextCaller;
        nextCaller = caller;

   
    }

    public void InvokeLast()
    {
StartCoroutine(setLast());
    }
    IEnumerator setLast()
    {
        DressManager dm = transform.root.GetComponent<DressManager>();
        yield return null;
        while (dm.cpa == null)
        {
            yield return null;
        }
        dm.x.onClick.AddListener(lastCaller.onClick.Invoke);


    }


    public void RefreshClothes()
    {
        int index = transform.parent.GetComponent<DressManager>().lastTab;
        transform.GetChild(0).GetChild(0).GetChild(index).GetComponent<Button>().onClick.Invoke();
    }


    void TurnOffChildren()
    {
        int n = transform.childCount;
        for (int i = 0; i < n - buttons.Length; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
//111, 106, 131, 171

//255, 176, 181, 255