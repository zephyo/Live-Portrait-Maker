using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class CamSaverHelper : MonoBehaviour
{

    public void CameraTab()
    {
        DressManager dm = transform.root.GetComponent<DressManager>();
        StartCoroutine(setUpInitial(dm));
    }

    IEnumerator setUpInitial(DressManager dm)
    {
        if (dm.cpa == null)
        {
            dm.callSwitch();
            yield return null;

        }
        while (!SceneManager.GetSceneByName("master").isLoaded)
        {
            yield return null;
        }

        dm.TurnOff(dm.transform.GetChild(0).GetComponent<CanvasGroup>(), false);
        dm.TurnOn(dm.cpa.transform.root.GetComponent<CanvasGroup>(), false);


        //set up canvas - 
        Button crop, fullscreen;
        crop = GameObject.Instantiate(dm.x.gameObject, dm.x.transform.parent, false).GetComponent<Button>();
        fullscreen = GameObject.Instantiate(dm.x.gameObject, dm.x.transform.parent, false).GetComponent<Button>();
        Button check = dm.cpa.transform.parent.GetChild(1).GetComponent<Button>();
        crop.onClick.RemoveAllListeners();
        fullscreen.onClick.RemoveAllListeners();

        fullscreen.GetComponent<Image>().sprite = Resources.Load<Sprite>("crop");
        crop.GetComponent<Image>().sprite = Resources.Load<Sprite>("cropbutt");



        GameObject cropGo = new GameObject("crop");
        Image rc = cropGo.AddComponent<Image>();

        rc.raycastTarget = false;
        RectTransform r = rc.rectTransform;
        r.SetParent(transform.root, false);
        r.offsetMax = Vector2.zero;
        r.offsetMin = Vector2.zero;
        r.anchorMax = Vector2.one;
        r.anchorMin = Vector2.zero;



        Image cropC = GameObject.Instantiate(cropGo, cropGo.transform.parent, false).GetComponent<Image>();
        cropC.name = "cropC";
        Image cropFront = GameObject.Instantiate(cropC, cropGo.transform, false).GetComponent<Image>();
        rc.color = Color.black;
        cropC.material = new Material(Shader.Find("UIMasked"));
        rc.material = new Material(Shader.Find("UIMask"));

        cropFront.sprite = Resources.Load<Sprite>("crop");
        cropFront.color = new Color(0.15f, 0.15f, 0.15f, 1);
        cropFront.type = Image.Type.Sliced;

        Image watermark = GameObject.FindGameObjectWithTag("Top").transform.GetChild(0).GetComponent<Image>();
        r = watermark.rectTransform;
        r.SetParent(cropGo.transform, false);
        Vector2 wmAP = r.anchoredPosition, wmSD = r.sizeDelta;
        r.anchoredPosition = new Vector2(282.7f, 78);
        r.sizeDelta = new Vector2(559.8f, 1146.5f);

        cropC.material.SetColor("_Color", new Color(0, 0, 0, 0.31f));

        CameraSave cs = gameObject.AddComponent<CameraSave>();

        Iris lr = dm.cpa.getLeftRight();

        cs.Init(rc, crop, fullscreen, check, new GameObject[]{
            crop.gameObject, fullscreen.gameObject, lr.gameObject, dm.x.gameObject,
        }, dm.x.transform.parent.GetComponent<Image>(), dm);
        //150 150 180 180



        check.onClick.RemoveAllListeners();
        check.onClick.AddListener(() =>
        {
            cs.TakeImage();
        });

        string photo, video = "gif";

        switch (PlayerPrefs.GetInt("Lang"))
        {

            case 1:
                photo = "照片";
                break;
            case 2:
                photo = "写真";
                break;
            case 3:
                photo = "Фото";
                //rus
                break;
            case 4:
                photo = "foto";
                //spanish
                break;
            case 5:
                photo = "ภาพถ่าย";
                //thai
                break;
            case 6:
                photo = "photo";
                //french
                break;
            default:
                photo = "photo";
                //english
                break;
        }

        Image checkIMg = check.GetComponent<Image>();
        Sprite checkS = checkIMg.sprite;
        checkIMg.sprite = Resources.Load<Sprite>("camera");


        UnityAction photoUA = () =>
            {

                check.onClick.RemoveAllListeners();
                check.onClick.AddListener(() =>
                {

                    cs.TakeImage();

                });
                checkIMg.color = Color.white;
                checkIMg.sprite = Resources.Load<Sprite>("camera");
                crop.GetComponent<Image>().color = Color.white;
                fullscreen.GetComponent<Image>().color = Color.white;

            };

        UnityAction videoUA = () =>
            {
                check.onClick.RemoveAllListeners();
                check.onClick.AddListener(() =>
                {
                    cs.TakeVideo(dm.fm, checkIMg, cropC.rectTransform);

                });
                Color32 yell = new Color32(255, 248, 194, 255);
                checkIMg.sprite = Resources.Load<Sprite>("play");
                crop.GetComponent<Image>().color = yell;
                fullscreen.GetComponent<Image>().color = yell;
            };


        lr.exclusiveButtons(photo, video, photoUA, videoUA, dm.x);
        CanvasGroup cg = lr.transform.parent.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;

        lr.imgs[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 248, 194, 255);


        r = (RectTransform)check.transform;
        Vector2 pos = r.anchoredPosition, sD = r.sizeDelta;
        r.anchorMin = new Vector2(0.5f, 0.5f);
        r.anchorMax = r.anchorMin;
        r.anchoredPosition = Vector2.zero;
        r.sizeDelta = new Vector2(175.8f, 219.7f);

        r = (RectTransform)dm.cpa.transform.parent;
        Vector2 pos2 = r.anchoredPosition, sD2 = r.sizeDelta;
        r.anchoredPosition = new Vector2(r.anchoredPosition.x, 107.9f);
        r.sizeDelta = new Vector2(r.sizeDelta.x, 215.9f);

        r = (RectTransform)dm.x.transform;
        Vector2 sD3 = r.sizeDelta;
        r.sizeDelta = new Vector2(r.sizeDelta.x, 215.9f);


        r = (RectTransform)crop.transform;
        r.anchorMin = new Vector2(0.6964788f, 0);
        r.anchorMax = new Vector2(0.7631065f, 1);
        r.offsetMax = new Vector2(24.1f, 0);
        r.offsetMin = new Vector2(-40.1f, 0);

        r = (RectTransform)fullscreen.transform;
        r.anchorMin = new Vector2(0.8555213f, 0);
        r.anchorMax = new Vector2(1, 1);
        r.offsetMax = new Vector2(-44.1f, 0);
        r.offsetMin = new Vector2(-13.9f, 0);
        /*
         trs.offsetMin = new Vector2(left, bottom);
    trs.offsetMax = new Vector2(-right, -top);
         */

        dm.cpa.gameObject.SetActive(false);




        dm.x.onClick.AddListener(() =>
        {
            RectTransform rt = watermark.rectTransform;
            rt.SetParent(GameObject.FindGameObjectWithTag("Top").transform, false);
            rt.anchoredPosition = wmAP;
            rt.sizeDelta = wmSD;
            checkIMg.sprite = checkS;
            Destroy(crop.gameObject); Destroy(fullscreen.gameObject); Destroy(cropGo.gameObject); Destroy(cropC.gameObject); Destroy(cs);
            dm.TurnOff(dm.cpa.transform.root.GetComponent<CanvasGroup>(), true);
            dm.TurnOn(dm.transform.GetChild(0).GetComponent<CanvasGroup>(), true);
            cs.SeeEverything();
            dm.x.onClick.RemoveAllListeners();
            check.onClick.RemoveAllListeners();
            CanvasGroup cp = dm.cpa.transform.root.GetComponent<CanvasGroup>();
            check.onClick.AddListener(() =>
            {
                dm.TurnOffEnd(cp);
            });
            lr.imgs[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;


            rt = (RectTransform)check.transform;


            r.sizeDelta = sD;
            rt.anchoredPosition = pos;
            rt.anchorMax = new Vector2(1, 0.5f);
            rt.anchorMin = new Vector2(0.905f, 0.5f);
            rt.offsetMax = new Vector2(-38.65f, rt.offsetMax.y);
            rt.offsetMin = new Vector2(-87.35f, rt.offsetMin.y);


            r = (RectTransform)dm.cpa.transform.parent;
            r.sizeDelta = sD2;
            r.anchoredPosition = pos2;






            cg.interactable = false;
            cg.blocksRaycasts = false;
            cg.alpha = 0;



        });


        yield return null;

    }





}
