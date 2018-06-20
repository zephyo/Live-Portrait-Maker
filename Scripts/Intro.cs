using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class Intro : MonoBehaviour
{


    public Image face, speechBbl, pointer;
    public TextMeshProUGUI speech;

    Queue<Action> nextActions;

    FaceManager d;

    int l;

    public void Init(FaceManager fm)
    {
        d = fm;
        d.OnSingleTap += Change;
        gameObject.SetActive(true);
        happy();
        nextActions = new Queue<Action>();
        nextActions.Enqueue(hello);
        nextActions.Enqueue(tapOnce);
        nextActions.Enqueue(tap);
        nextActions.Enqueue(tapTwice);
        nextActions.Enqueue(tapT);
        nextActions.Enqueue(thatsIt);
        l = PlayerPrefs.GetInt("Lang");
        Change();
    }

    public void Change()
    {

        if (nextActions.Count == 0)
        {
            d.OnSingleTap -= Change;
            GameObject game = GameObject.FindGameObjectWithTag("Respawn");
            if (game != null) d.updateDelegate = null;
            LeanTween.value(speechBbl.gameObject, (float val) =>
       {
           speechBbl.rectTransform.localScale = Vector3.one * val;
           speechBbl.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(-90, 0, val));
       }, 1f, 0.3f, 0.3f).setEaseInQuart().setOnComplete(() =>
                {
                    LeanTween.cancel(face.gameObject);
                    Destroy(gameObject);
                }
                );
        }
        else
        {
            nextActions.Dequeue()();
        }
    }


    void hello()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        string h;
        switch (l)
        {
            case 1:
                //chinese
                h = "你好~";
                break;
            case 2:
                //ja
                h = "こんにちは~";
                break;
            case 3:
                //rus
                h = "Всем привет~";
                break;
            case 4:
                h = "Hola~";
                break;
            //spanish
            case 5:
                //thai
                h = "สวัสดี~";
                break;
            case 6:
                h = "bonjour~";
                break;
            default:
                //english
                h = "hello hello~";
                break;
        }

        speech.text = h;

        //rescale + rotate
        LeanTween.value(Camera.main.gameObject, (float val) =>
        {
            speechBbl.rectTransform.localScale = Vector3.one * val;
            speechBbl.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(-90, 0, val));
            cg.alpha = val;
        }, 0.3f, 1f, 0.6f).setEaseOutQuart().setOnComplete(() =>
            {
                StartCoroutine(bounce());
            });
    }

    void tapOnce()
    {
        d.OnSingleTap -= Change;

        string h;
        switch (l)
        {
            case 1:
                //chinese
                h = "单击观看";
                break;
            case 2:
                //ja
                h = "見るためにタップ";
                break;
            case 3:
                //rus
                h = "Нажмите, чтобы посмотреть";
                break;
            case 4:
                h = "Pulse para buscar";
                break;
            //spanish
            case 5:
                //thai
                h = "แตะเพื่อดู";
                break;
            case 6:
                h = "Appuyez sur pour rechercher";
                break;
            default:
                //english
                h = "tap once to look";
                break;
        }


        //rescale + rotate
        LeanTween.value(speechBbl.gameObject, (float val) =>
        {
            speechBbl.rectTransform.localScale = Vector3.one * val;
            speechBbl.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(-90, 0, val));
        }, 1f, 0.3f, 0.3f).setEaseInQuart().setOnComplete(() =>
        {
            speech.text = h;
            LeanTween.value(speechBbl.gameObject, (float val) =>
            {

                speechBbl.rectTransform.localScale = Vector3.one * val;
                speechBbl.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(-90, 0, val));
            }, 0.3f, 1f, 0.6f).setEaseOutQuart().setOnComplete(() =>
            {
                d.OnSingleTap += Change;
                StartCoroutine(bounce());
            });
        });
    }
    void tap()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        LeanTween.value(speechBbl.gameObject, (float val) =>
      {
          speechBbl.rectTransform.localScale = Vector3.one * val;
          speechBbl.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(-90, 0, val));
          cg.alpha = val;
      }, 1f, 0f, 0.4f).setEaseOutQuart().setOnComplete(() =>
      {
          d.ChangeLook();
          cg.alpha = 0;
          pointer.gameObject.SetActive(true);
          face.gameObject.SetActive(false);
          speechBbl.gameObject.SetActive(false);
          cg.alpha = 1;
          Image sparks = pointer.transform.GetChild(0).GetComponent<Image>();
          LeanTween.value(pointer.gameObject, (float val) =>
      {
          //change width from 267.6 to 240
          //change x rotation from 0 to -17.79
          //change sparks fill from 0 to 1
          pointer.rectTransform.eulerAngles = new Vector3(val * -17.79f, 0, 0);
          pointer.rectTransform.sizeDelta = new Vector2(267.6f - 27.6f * val, pointer.rectTransform.sizeDelta.y);
          sparks.fillAmount = val;
      }, 0, 1, 0.7f).setEaseInOutQuart().setDelay(0.3f).setLoopPingPong();
      });
    }

    void tapTwice()
    {

        string h;
        switch (l)
        {
            case 1:
                //chinese
                h = "双击打开菜单";
                break;
            case 2:
                //ja
                h = "ダブルタップしてメニューを開";
                break;
            case 3:
                //rus
                h = "Дважды нажмите, чтобы открыть меню";
                break;
            case 4:
                h = "Toca dos veces para abrir el menú!";
                break;
            //spanish
            case 5:
                //thai
                h = "แตะสองครั้งเพื่อเปิดเมนู!";
                break;
            case 6:
                h = "Appuyez deux fois sur pour ouvrir le menu!";
                break;
            default:
                //english
                h = "tap twice to change things up!";
                break;
        }



        d.OnSingleTap -= Change;
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        LeanTween.cancel(pointer.gameObject);
        pointer.gameObject.SetActive(false);
        face.gameObject.SetActive(true);
        speechBbl.gameObject.SetActive(true);
        speech.text = h;
        //rescale + rotate

        LeanTween.value(speechBbl.gameObject, (float val) =>
        {
            cg.alpha = val;
            speechBbl.rectTransform.localScale = Vector3.one * val;
            speechBbl.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(-90, 0, val));
        }, 0.3f, 1f, 0.4f).setEaseOutQuart().setOnComplete(() =>
        {
            d.OnSingleTap += Change;
            d.OnDoubleTap+=Change;
            StartCoroutine(bounce());
        });

    }

    void tapT()
    {
        d.OnDoubleTap-=Change;
        CanvasGroup cg = GetComponent<CanvasGroup>();
        LeanTween.value(speechBbl.gameObject, (float val) =>
      {
          speechBbl.rectTransform.localScale = Vector3.one * val;
          speechBbl.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(-90, 0, val));
          cg.alpha = val;
      }, 1f, 0, 0.5f).setEaseOutQuart().setOnComplete(() =>
      {
          cg.alpha = 0;
          pointer.gameObject.SetActive(true);
          face.gameObject.SetActive(false);
          speechBbl.gameObject.SetActive(false);

          Image sparks = pointer.transform.GetChild(0).GetComponent<Image>();
          sparks.fillAmount = 0;
          cg.alpha = 1;
          StartCoroutine(taptwice(sparks));

          d.setUpDressListener();
          d.OnDoubleTap += Change;
          d.OnSingleTap -= Change;
      });

    }

    IEnumerator taptwice(Image sparks)
    {

        while (pointer.gameObject.activeSelf == true)
        {
            LeanTween.value(pointer.gameObject, (float val) =>
        {
            pointer.rectTransform.eulerAngles = new Vector3(val * -17.79f, 0, 0);
            pointer.rectTransform.sizeDelta = new Vector2(267.6f - 27.6f * val, pointer.rectTransform.sizeDelta.y);
            sparks.fillAmount = val;
        }, 0, 1, 0.6f).setEaseInOutQuart();
            yield return new WaitForSeconds(0.6f);

            LeanTween.value(pointer.gameObject, (float val) =>
            {
                pointer.rectTransform.eulerAngles = new Vector3(val * -17.79f, 0, 0);
                pointer.rectTransform.sizeDelta = new Vector2(267.6f - 27.6f * val, pointer.rectTransform.sizeDelta.y);
                sparks.fillAmount = val * 0.7f;
            }, 1, 0.5f, 0.05f).setEaseInQuart();
            yield return new WaitForSeconds(0.05f);

            LeanTween.value(pointer.gameObject, (float val) =>
            {
                pointer.rectTransform.eulerAngles = new Vector3(val * -17.79f, 0, 0);
                pointer.rectTransform.sizeDelta = new Vector2(267.6f - 27.6f * val, pointer.rectTransform.sizeDelta.y);
                sparks.fillAmount = val * 0.7f;
            }, 0.5f, 1, 0.1f).setEaseOutQuart();
            yield return new WaitForSeconds(0.1f);

            LeanTween.value(pointer.gameObject, (float val) =>
            {
                pointer.rectTransform.eulerAngles = new Vector3(val * -17.79f, 0, 0);
                pointer.rectTransform.sizeDelta = new Vector2(267.6f - 27.6f * val, pointer.rectTransform.sizeDelta.y);
                sparks.fillAmount = val;
            }, 1, 0, 1f).setEaseInOutQuart();
            yield return new WaitForSeconds(1f);
        }
    }

    void thatsIt()
    {

        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        LeanTween.cancel(pointer.gameObject);
        pointer.gameObject.SetActive(false);
        face.gameObject.SetActive(true);
        speechBbl.gameObject.SetActive(true);
        string h;
        switch (l)
        {
            case 1:
                //chinese
                h = "就这些了，希望你喜欢！:D";
                break;
            case 2:
                //ja
                h = "それでおしまい！ 楽しむ！:D";
                break;
            case 3:
                //rus
                h = "это оно; повеселись! :D";
                break;
            case 4:
                h = "Eso es; ¡que te diviertas! :D";
                break;
            //spanish
            case 5:
                //thai
                h = "แค่นั้นแหละ; มีความสุข! :D";
                break;
            case 6:
                h = "c'est tout; s'amuser! :D";
                break;
            default:
                //english
                h = "that's it; have fun! :D";
                break;
        }


        speech.text = h;
        d.OnDoubleTap -= Change;
        //rescale + rotate
        StartCoroutine(wait(cg));



    }

    IEnumerator wait(CanvasGroup cg)
    {
        yield return new WaitForSeconds(0.2f);
        transform.SetParent(GameObject.FindGameObjectWithTag("Respawn").transform, false);

        d.setUpDelegates();

        LeanTween.value(speechBbl.gameObject, (float val) =>
        {
            cg.alpha = val;
            speechBbl.rectTransform.localScale = Vector3.one * val;
            speechBbl.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(-90, 0, val));
        }, 0.3f, 1f, 0.6f).setEaseOutQuart().setOnComplete(() =>
        {
            d.OnSingleTap += Change;
            StartCoroutine(bounce());
        });

    }




    void happy()
    {
        float orig = face.rectTransform.eulerAngles.z;
        LeanTween.value(face.gameObject, (float val) =>
        {
            face.rectTransform.eulerAngles = new Vector3(0, 0, val);
        }, orig, -4.67f, 0.9f).setEaseOutSine().setOnComplete(() =>
        {
            LeanTween.value(face.gameObject, (float val) =>
            {
                face.rectTransform.eulerAngles = new Vector3(0, 0, val);
            }, -4.67f, 8.48f, 0.9f).setEaseOutSine().setLoopPingPong();
        });
    }

    IEnumerator bounce()
    {

        LeanTween.cancel(face.gameObject);
        yield return null;
        LeanTween.value(face.gameObject, (float val) =>
        {
            face.rectTransform.anchoredPosition = new Vector2(face.rectTransform.anchoredPosition.x, val);
        }, 24.5f, 70f, 0.5f).setEaseOutBack().setOnComplete(() =>
        {
            LeanTween.value(face.gameObject, (float val) =>
            {
                face.rectTransform.anchoredPosition = new Vector2(face.rectTransform.anchoredPosition.x, val);
            }, 70f, 24.5f, 0.8f).setEaseOutBounce().setOnComplete(() =>
             {
                 happy();
             });


        });

    }
}
