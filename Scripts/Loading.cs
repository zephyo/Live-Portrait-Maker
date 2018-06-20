using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

public class Loading : MonoBehaviour
{

    // Use this for initialization


    public bool loaddd;

    bool check;
    public Image l;
    float pr;
    public void startLoading(bool check, float totalTime = 1.5f)
    {
        Init(check);
        StartCoroutine(load(totalTime));

    }

    public void Init(bool check)
    {
        loaddd = true;
        this.check = check;
        Color32[] rand = new Color32[]{
            new Color32(108,252,253,230),
                new Color32(255,222,202,230),
                 new Color32(255,190,185,230),
                   new Color32(113,178,181,230),
                new Color32(177,207,221,230),
                 new Color32(235,98,115,230),

        };
        l.color = rand[UnityEngine.Random.Range(0, rand.Length)];
        l.gameObject.SetActive(true);
    }

    public void setProgress(int s, float progress)
    {
       pr = progress;
    }

    public void updateThis(Action cb){
        l.fillAmount=0;
           Color32[] rand = new Color32[]{
            new Color32(255,247,211,230),
                new Color32(255,222,202,230),
                 new Color32(255,190,185,230),
                   new Color32(113,178,181,230),
                new Color32(177,207,221,230),
                 new Color32(213,213,229,230),

        };
        l.color = rand[UnityEngine.Random.Range(0, rand.Length)];   
        StartCoroutine(u(cb));
    }
    IEnumerator u(Action cb){
        while (loaddd){
            yield return null;
            l.fillAmount=pr;
        }
         l.gameObject.SetActive(false);
         l.fillAmount=0;
        cb();
    }

    public void stop(){
        loaddd=false;
       
    }




    IEnumerator load(float totalTime)
    {
        float time = 0;
        while (loaddd)
        {
            float t = time / totalTime;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            l.fillAmount = Mathf.Lerp(0, 1, t);
            time += Time.deltaTime;
            if (time > totalTime) time = time - totalTime;

            yield return null;
        }
        if (check)
        {
            Debug.LogWarning("check!");
            l.rectTransform.eulerAngles = Vector3.zero;
            l.sprite = Resources.Load<Sprite>("tick");
            l.color = new Color32(200, 255, 220, 255);
            LeanTween.value(Camera.main.gameObject, (float val) =>
            {
                l.fillAmount = val;
            }, 0, 1, 0.3f).setEaseInOutQuint().setOnComplete(() =>
            {
                LeanTween.value(Camera.main.gameObject, (float val) =>
         {
             l.color = new Color(l.color.r, l.color.g, l.color.b, val);
         }, 1, 0, 0.4f).setEaseInOutCubic().setOnComplete(() =>
                         {
                             l.sprite = Resources.Load<Sprite>("ring");
                             l.gameObject.SetActive(false);
                             enabled = false;
                         });
            });

            yield break;
        }
         l.gameObject.SetActive(false);
    }

    public void StopLoading()
    {
        loaddd = false;
    }
}
