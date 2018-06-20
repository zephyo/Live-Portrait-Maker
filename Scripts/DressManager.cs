using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Linq;
using TMPro;
using Kino;

public enum itemType
{
    ebrow,
    bh_air,
    b_ngs,

    e_ye,
    bg,

    CHIN,
    HD,
    BY_ODY,

    w_hites,
    iris,
    se,
    Se,
    xe,
    l_p,
    n_se,

    bronzer,

    FGliter,

    UEye,

    t_clothes,
    blush,

    bDaid,

    JBandage,
    BOW,

    chneckwear,
    glasses,

    freckles,
    starfreckles,
    lippiercing,
    nosepiercing,
    overalls,
    sl1,
    sx_tears,
    bubble,
    EPatch,
    hdphones,

    ctetopband,
    msk,
    scar,
    eear,
    hesidehorn,
    hrclip,
    harts,
    b0odnos,
    bood,
    flower,
    hwrstrand,
    unicorn,
    particle_snow,
    pArticle_sparkle,
    Particle_petal,
    wdEluxeScript,
    wfFallScript,
    GXlitch,
    FRGradient,

    BNry,

    BMlm,

    RPamp,

    CXolor,



    eyelid,
    skin,

}

public struct UndoInfo
{
    public Image set, set2;
    public Sprite before;
    public Color beforeC, beforeC2;
}


//Manager runs when shop is open
//Manages how shop is implemented and how shop connects to character
public class DressManager : MonoBehaviour
{


    List<ShopItem> items;
    public Sprite[] eye, eyes, brows, lips, nose, bangs, hair, clothes, bg, xtra;

    public ColorPicker cpa;


    public Button x;

    RectTransform actualContent;
    [HideInInspector]
    public FaceManager fm;

    bool check = false;

    UnityAction welcome;

    //Switch to a new tab in the shop
    public event Action Switch;


    public int lastTab;

    public Loading load;


    public void Start()
    {




        SwitchButtonColor(GetButtons(), new Color32((byte)PlayerPrefs.GetInt("themeR"), (byte)PlayerPrefs.GetInt("themeG"), (byte)PlayerPrefs.GetInt("themeB"), 255));

        fm = GameObject.FindGameObjectWithTag("Player").GetComponent<FaceManager>();
        items = new List<ShopItem>();
        actualContent = (RectTransform)transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0);
        items.Add(actualContent.GetChild(0).GetComponent<ShopItem>());
        transform.GetChild(0).Find("optMotion").GetChild(0).GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("motion") == 1 ? false : true;
        InitMasterColor();
        dressUp();

        int lang = PlayerPrefs.GetInt("Lang");
        if (lang != 0)
        {
            LanguageSupport ls = GetComponent<LanguageSupport>();
            ls.ChangeLanguage(lang);
            ls.setLanguageDropdown(lang);
        }

    }


    private void Update()
    {
        if (check)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (Input.GetMouseButtonDown(0) &&
            !EventSystem.current.IsPointerOverGameObject())
            {
#elif UNITY_IOS || UNITY_ANDROID
       if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began
       && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)){
#endif
                CanvasGroup du = transform.GetChild(0).GetComponent<CanvasGroup>();
                if (du.interactable)
                {
                    TurnOffEnd(du);
                }
            }
        }
    }

    void InitMasterColor()
    {
        Switch += () =>
        {
            SceneManager.LoadSceneAsync("master", LoadSceneMode.Additive);
            StartCoroutine(waitForMaster());

        };
    }

    IEnumerator waitForMaster()
    {
        yield return null;
        while (!SceneManager.GetSceneByName("master").isLoaded)
        {
            yield return null;
        }

        Transform master = SceneManager.GetSceneByName("master").GetRootGameObjects()[0].transform.GetChild(0);
        cpa = master.GetChild(2).GetComponent<ColorPicker>();
        Debug.Log("grabbing cpa");
        x = master.GetChild(0).GetComponent<Button>();
        master.GetChild(1).GetComponent<Button>().onClick.AddListener(() => TurnOffEnd(master.root.GetComponent<CanvasGroup>()));
        int lang = PlayerPrefs.GetInt("Lang");
        if (lang != 0)
            GetComponent<LanguageSupport>().setMaster(lang, cpa.transform);

    }


    //fade off turn
    public void TurnOff(CanvasGroup turn, bool checkUpdate)
    {
        check = checkUpdate;
        turn.interactable = false;
        turn.blocksRaycasts = false;
        LeanTween.value(Camera.main.gameObject, (float val) =>
        {
            turn.alpha = val;
        }, 1, 0, 0.3f).setEase(LeanTweenType.easeOutCubic);

    }

    //fade off turn, then exit shop
    public void TurnOffEnd(CanvasGroup turn)
    {
        check = false;
        turn.interactable = false;
        turn.blocksRaycasts = false;
        LeanTween.value(Camera.main.gameObject, (float val) =>
        {
            if (turn != null)
                turn.alpha = val;
        }, 1, 0, 0.4f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
        {
            fm.UnloadDressUp();
            if (cpa != null) SceneManager.UnloadSceneAsync("master");

            SceneManager.UnloadSceneAsync(1);
        });

    }

    //fade in turn
    public void TurnOn(CanvasGroup turn, bool checkUpdate)
    {
        LeanTween.value(Camera.main.gameObject, (float val) =>
        {
            turn.alpha = val;
        }, 0, 1, 0.25f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
        {
            turn.interactable = true;
            turn.blocksRaycasts = true;
            check = checkUpdate;
        });
    }

    public void dressUp()
    {
        CanvasGroup du = transform.GetChild(0).GetComponent<CanvasGroup>();
        TurnOn(du, true);

        Button[] butts = transform.GetChild(0).GetChild(0).GetChild(0).GetComponentsInChildren<Button>();
        Button[] tabs = transform.GetChild(0).GetComponent<TabManager>().buttons;
        Button[] combined = new Button[butts.Length + tabs.Length];
        butts.CopyTo(combined, 0);
        tabs.CopyTo(combined, butts.Length);
        welcome = delegate () { TurnOffWelcome(combined); };
        foreach (Button b in combined)
        {
            b.onClick.AddListener(welcome);
        }
    }

    public void TurnOffWelcome(Button[] butts)
    {
        foreach (Button b in butts)
        {
            b.onClick.RemoveListener(welcome);
        }
        Destroy(transform.GetChild(0).GetChild(1).GetChild(2).gameObject);
    }


    public void changeColorTheme()
    {
        StartCoroutine(colorThemeHelp());
    }

    IEnumerator colorThemeHelp()
    {

        if (cpa == null)
        {
            callSwitch();
            yield return null;

        }
        while (!SceneManager.GetSceneByName("master").isLoaded)
        {
            yield return null;
        }
        List<Button> bs = GetButtons();
        Color prev = bs[0].colors.normalColor;
        CanvasGroup cp = cpa.transform.root.GetComponent<CanvasGroup>();

        x.onClick.AddListener(() =>
      {
          SwitchButtonColor(bs, prev);

          TurnOff(cp, true);
          x.onClick.RemoveAllListeners();
      });


        Button check = cpa.transform.parent.GetChild(1).GetComponent<Button>();
        check.onClick.RemoveAllListeners();
        check.onClick.AddListener(() =>
        {
            Color32 newC = cpa.Color;
            PlayerPrefs.SetInt("themeR", newC.r);
            PlayerPrefs.SetInt("themeG", newC.g);
            PlayerPrefs.SetInt("themeB", newC.b);
            PlayerPrefs.Save();

            TurnOff(cp, true);
            check.onClick.RemoveAllListeners();
            check.onClick.AddListener(() => TurnOffEnd(cp));
        });

        cpa.clearUpdateColor();
        cpa.Color = prev;
        cpa.Reset();
        cpa.UpdateColorAction += () =>
        {
            SwitchButtonColor(bs, cpa.Color);
        };
        cpa.gameObject.SetActive(true);
        TurnOn(cp, false);

        yield return null;

    }

    void SwitchButtonColor(List<Button> bs, Color prev)
    {
        ColorBlock cb = bs[0].colors;
        cb.normalColor = prev;
        cb.disabledColor = prev;
        for (int i = 0; i < bs.Count - 5; i++)
        {
            bs[i].colors = cb;
        }
        ColorBlock cb2 = bs[bs.Count - 1].colors;
        cb2.highlightedColor = new Color(prev.r * 1.009f, prev.g * 0.791f, prev.b * 0.6009f, 0.6705f);
        //111,106,131,171 
        //110, 134,218

        cb2.pressedColor = cb2.highlightedColor;
        cb2.disabledColor = cb2.highlightedColor;
        for (int i = bs.Count - 5; i < bs.Count; i++)
        {
            if (bs[i].colors.normalColor.a < 1)
                cb2.normalColor = cb2.disabledColor;
            else
            {
                cb2.normalColor = bs[i].colors.normalColor;
            }
            bs[i].colors = cb2;
        }
    }

    List<Button> GetButtons()
    {
        List<Button> bs = new List<Button>();
        Transform dUp = transform.GetChild(0);
        Transform parent = dUp.GetChild(0).GetChild(0);
        //get buttons
        foreach (Transform t in parent)
        {
            Button b = t.GetComponent<Button>();
            if (b != null) bs.Add(b);
        }
        parent = dUp.GetChild(10).GetChild(0).GetChild(0);
        Debug.Log(parent.name);
        //get saves
        foreach (Transform t in parent)
        {
            Button b = t.GetComponent<Button>();
            if (b != null) bs.Add(b);
        }
        bs.Add(dUp.GetChild(3).GetComponent<Button>());//help
        bs.Add(dUp.GetChild(4).GetComponent<Button>());//opt
        bs.Add(dUp.GetChild(7).GetComponent<Button>());//color theme
        bs.AddRange(transform.GetChild(0).GetComponent<TabManager>().buttons);
        return bs;
    }


    public void colorPick(List<UndoInfo> uiArr)
    {
        CanvasGroup du = transform.GetChild(0).GetComponent<CanvasGroup>();
        CanvasGroup cp = cpa.transform.root.GetComponent<CanvasGroup>();
        TurnOff(du, false);


        x.onClick.AddListener(() =>
        {
            foreach (UndoInfo ui in uiArr)
            {
                UndoChanges(ui);

            }
            TurnOff(cp, false);
            TurnOn(du, true);
            x.onClick.RemoveAllListeners();
        });

        TurnOn(cp, false);
    }


    public void colorPick(UndoInfo ui, itemType it, GameObject equipped)
    {
        CanvasGroup du = transform.GetChild(0).GetComponent<CanvasGroup>();
        CanvasGroup cp = cpa.transform.root.GetComponent<CanvasGroup>();
        TurnOff(du, false);

        Color bg = Camera.main.backgroundColor;
        x.onClick.AddListener(() =>
        {
            UndoChanges(ui);

            if (equipped)
            {
                cpa.transform.parent.GetChild(3).gameObject.SetActive(false);
            }
            TurnOff(cp, false);
            TurnOn(du, true);
            x.onClick.RemoveAllListeners();
        });

        setCPAListeners(ui);
        HashSet<string> s = new HashSet<string>(new string[]{
                          "b_", "ir", "w_", "ey", "bh", "e_", "eb", "l_", "n_", "bg", "BY", "CH", "HD"
        });
        if (equipped.activeSelf && !s.Contains(it.ToString().Substring(0, 2)))
        {


            Button remove = cpa.transform.parent.GetChild(4).GetChild(0).GetComponent<Button>();
            CanvasGroup cg = remove.transform.parent.GetComponent<CanvasGroup>();
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            remove.gameObject.SetActive(true);
            remove.onClick.AddListener(() =>
            {
                equipped.SetActive(false);
                fm.Remove(it.ToString().Substring(0, 2));
                x.onClick.RemoveAllListeners();
                TurnOff(cp, false);
                TurnOn(du, true);
                // remove.transform.parent.GetChild(4).gameObject.SetActive(false);
                remove.gameObject.SetActive(false);
                remove.onClick.RemoveAllListeners();

                cg.alpha = 0;
                cg.interactable = false;
                cg.blocksRaycasts = false;

            });

            x.onClick.AddListener(() =>
            {
                cg.alpha = 0;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            });
        }

        TurnOn(cp, false);
    }

    void UndoChanges(UndoInfo ui)
    {
        if (ui.set != null && ui.set.gameObject.name == "bg")
        {
            /*     hm.beforeC = hm.set.color;
                hm.before = hm.set.sprite;
                hm.beforeC2 = Camera.main.backgroundColor; */
            Camera.main.backgroundColor = ui.beforeC2;
            ui.set.color = ui.beforeC;
            ui.set.sprite = ui.before;
            return;
        }

        if (ui.set != null && ui.before != null)
        {
            if (ui.set.gameObject.name == "head")
            {
                changeSkin(ui.beforeC, ui.beforeC2);
            }

            else
            {
                if (ui.set.sprite.name == "b_92")
                {
                    ui.set.rectTransform.anchoredPosition = new Vector2(ui.set.rectTransform.anchoredPosition.x, 503);
                }
                else if (ui.set == fm.hair)
                {
                    ui.set.rectTransform.sizeDelta = new Vector2(ui.beforeC2.r * 1500, ui.beforeC2.g * 2500);
                }
                UndoHelper(ui);
            }
        }
        else
        {
            if (ui.set != null)
            {
                if (ui.set.sprite != null)
                    fm.Remove(ui.set.sprite.name.Substring(0, 2));
                else
                {
                    if (ui.set.gameObject.name == "wf")
                        ui.set.GetComponent<WaterfallScript>().LightColor = ui.beforeC;
                    else
                        ui.set.color = ui.beforeC;
                }
            }
            else if (ui.before != null && (ui.before.name != "GX" && !ui.before.name.ToLower().StartsWith("pa")))
            {
                string key = ui.before.name.Substring(0, 2);
                fm.Remove(key);
            }
        }
    }

    void UndoHelper(UndoInfo ui)
    {
        ui.set.sprite = ui.before;
        ui.set.color = ui.beforeC;

        setNative(ui.set);
        if (ui.set2 != null)
        {
            ui.set2.sprite = ui.before;
            // Debug.Log("undoing "+ui.set2.name+" with color "+ui.beforeC2);
            ui.set2.color = ui.beforeC2;
            setNative(ui.set2);
        }
    }

    void setNative(Image i)
    {
        string key = i.sprite.name.Substring(0, 2);
        if (key == "e_" || key == "bh" || key == "l_") return;
        i.SetNativeSize();
    }

    public void changeSkin(Color change, Color lips, bool changeMore = true)
    {

        float ratio = 0.15f - Mathf.Clamp((change.r - 0.1f) / 5.5f, 0, 0.15f);
        float degree = Mathf.Clamp(2f - change.r * 2, 1, 5);
        // change = new Color(change.r + ratio, change.g + ratio, change.b + ratio);
        Color light = new Color(change.r * degree + ratio, change.g * degree + ratio, change.b * degree + ratio);
        fm.skin[0].color = light; // nose
        fm.skin[1].color = change; //head

        for (int i = 4; i < fm.skin.Length; i++)
        {
            if (i == 6) continue;
            fm.skin[i].color = change;
        }


        if (changeMore)
        {
            fm.skin[2].color = new Color(light.r + ratio, light.g, light.b); //eye
            fm.skin[3].color = fm.skin[2].color; //eye


            fm.skin[6].color = Color.Lerp(Color.white, new Color(change.r + 0.1f, change.g, change.b), 0.8f);
            float sat = change.r + change.g + change.b;
            sat /= 3;
            fm.lips.color = Color.Lerp(lips, new Color(sat + 0.1f, sat, sat), 0.6f * (1 - sat));
        }

    }


    public void changeSkin(HSBColor change, Color lips)
    {
        change.s *= 0.7f;
        change.b += 0.1f * (1 - change.b);
        Color c = HSBColor.ToColor(change);
        changeSkin(c, lips);
    }


    /*

    cpa.clearUpdateColor();
    cpa.UpdateColorAction += () => { ui.set.color = new Color(1, 1, 1, 1 - cpa.B); };

     */
    void setCPAListeners(UndoInfo ui)
    {


        if (ui.set != null)
        {
            cpa.clearUpdateColor();

            if (ui.set.gameObject.name == "bg")
            {
                if (ui.set.sprite != null && ui.set.sprite.name[2] != 'p')
                {
                    cpa.UpdateColorAction += () => { ui.set.color = new Color(1, 1, 1, 1 - cpa.B); };
                }
                cpa.UpdateColorAction += () => { Camera.main.backgroundColor = cpa.Color; };
            }
            else if (ui.set.gameObject.name == "head")
            {
                cpa.UpdateColorAction += () =>
                {
                    changeSkin(new HSBColor(cpa.H, cpa.S, cpa.B, 1), ui.beforeC2);
                };
            }
            else if (ui.set.gameObject.name == "wf")
            {
                WaterfallScript Waterfall = ui.set.GetComponent<WaterfallScript>();
                cpa.UpdateColorAction += () =>
                {
                    Waterfall.LightColor = cpa.Color;
                };
            }

            else
            {

                cpa.UpdateColorAction += () => { ui.set.color = cpa.Color; };


                if (ui.set2 != null)
                {
                    cpa.UpdateColorAction += () => { ui.set2.color = cpa.Color; };
                }

                if (ui.set.sprite != null && ui.set.sprite.name == "hart")
                {
                    setUpParticles(xtra[18], 1, Color.white);
                }

                if (ui.set.gameObject.name == "bangs")
                {
                    Image shine = ui.set.transform.GetChild(0).GetComponent<Image>();
                    cpa.UpdateColorAction += () =>
                    {
                        shine.color = new Color(shine.color.r, shine.color.g, shine.color.b,
    Mathf.Lerp(1, 0.35f, cpa.B));
                        ;
                    };
                }
            }
        }
        else if (ui.before != null && (ui.before.name.ToLower().StartsWith("pa")))
        {
            cpa.clearUpdateColor();
            setUpParticles(ui.before, 0, Color.white);
        }
    }


    public void setUpParticles(Sprite particle, int index, Color opt, bool ignoreCPA = false)
    {
        if (isEquippedParticle(particle))
        {
            var main2 = GameObject.FindGameObjectWithTag("Finish").transform.Find(particle.name).GetComponent<ParticleSystem>().main;

            if (!ignoreCPA)
            {
                Color original = main2.startColor.colorMin;
                cpa.UpdateColorAction += () =>
              {
                  main2.startColor = new ParticleSystem.MinMaxGradient(cpa.Color,
                  new Color(cpa.Color.r, cpa.Color.g, cpa.Color.b, 0.2f));
              };
                x.onClick.AddListener(() =>
                {
                    main2.startColor = new ParticleSystem.MinMaxGradient(original,
        new Color(original.r, original.g, original.b, 0.2f));
                });

            }
            else
            {
                main2.startColor = new ParticleSystem.MinMaxGradient(opt,
                  new Color(opt.r, opt.g, opt.b, 0.2f));
            }

            return;
        }


        GameObject go = new GameObject();
        go.SetActive(false);
        go.name = particle.name;
        go.transform.SetParent(GameObject.FindGameObjectWithTag("Finish").transform, false);
        ParticleSystem ps = go.AddComponent<ParticleSystem>();



        if (index < go.transform.parent.childCount) go.transform.SetSiblingIndex(index);

        var main = ps.main;
        main.prewarm = true;

        if (!ignoreCPA)
        {
            cpa.UpdateColorAction += () =>
            {
                main.startColor = new ParticleSystem.MinMaxGradient(cpa.Color,
                new Color(cpa.Color.r, cpa.Color.g, cpa.Color.b, 0.2f));
            };
            cpa.Color = opt; cpa.Reset(); cpa.UpdateColor();
        }
        ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();
        Material new_;
        ParticleSystem.EmissionModule em = ps.emission;
        ParticleSystem.ShapeModule sm = ps.shape;



        switch (particle.name)
        {

            case "pArticle_sparkle":
                new_ = Resources.Load<Material>("Star");
                //start size, start speed, shape, lifetime, emission rate, 
                go.transform.localPosition = new Vector3(0, 0, 0);

                main.startLifetime = 5;
                main.maxParticles = 15;
                main.startSpeed = new ParticleSystem.MinMaxCurve(0.05f, 0.6f);
                main.startSize = new ParticleSystem.MinMaxCurve(0.2f, 0.8f);

                em.rateOverTime = new ParticleSystem.MinMaxCurve(3, 6);
                sm.angle = 15.53f;
                sm.radius = 2.8f;
                sm.radiusThickness = 0.7f;
                sm.rotation = new Vector3(-10, 0, 0);

                main.startColor = new ParticleSystem.MinMaxGradient(opt,
               new Color(opt.r, opt.g, opt.b, 0.2f));

                ParticleSystem.ColorOverLifetimeModule c = ps.colorOverLifetime;
                c.enabled = true;
                Gradient ourGradient = new Gradient();
                ourGradient.SetKeys(
                      new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(0, 0.0f), new GradientAlphaKey(1, 0.89f),
                        new GradientAlphaKey(0, 1f) }
                    );
                c.color = new ParticleSystem.MinMaxGradient(ourGradient);
                //transform pos,
                break;
            case "hartic":


                new_ = Resources.Load<Material>("Hart");

                go.transform.localPosition = new Vector3(0, Screen.height / 3.5f, 0);
                main.startLifetime = 3;
                main.maxParticles = 8;
                main.startSpeed = new ParticleSystem.MinMaxCurve(0.1f, 0.4f);
                main.startSize = new ParticleSystem.MinMaxCurve(0.3f, 0.9f);
                main.startRotation = new ParticleSystem.MinMaxCurve(-0.45f, 0.45f);

                main.startColor = new ParticleSystem.MinMaxGradient(opt,
              new Color(opt.r, opt.g, opt.b, 0.2f));


                em.rateOverTime = 3;

                sm.shapeType = ParticleSystemShapeType.SingleSidedEdge;
                sm.radius = 2.473689f;



                ParticleSystem.ColorOverLifetimeModule cc = ps.colorOverLifetime;
                cc.enabled = true;
                Gradient ourGradient2 = new Gradient();
                ourGradient2.SetKeys(
                      new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(0, 0.4f), new GradientAlphaKey(1, 0.89f),
                        new GradientAlphaKey(0, 1f) }
                    );
                cc.color = new ParticleSystem.MinMaxGradient(ourGradient2);
                ParticleSystem.SizeOverLifetimeModule sol = ps.sizeOverLifetime;
                sol.enabled = true;

                break;
            case "Particle_petal":
            case "particle_snow":
                if (particle.name == "Particle_petal")
                {
                    main.maxParticles = 20;
                    em.rateOverTime = 1;
                    main.startRotation = new ParticleSystem.MinMaxCurve(0, 180);
                    main.startSpeed = new ParticleSystem.MinMaxCurve(0.1f, 3);
                    main.startSize = new ParticleSystem.MinMaxCurve(UnityEngine.Random.Range(0.2f, 0.3f),
               UnityEngine.Random.Range(0.45f, 0.6f));
                }
                else
                {
                    main.maxParticles = 40;
                    em.rateOverTime = 3;
                    main.startSpeed = new ParticleSystem.MinMaxCurve(0.1f, 2);
                    main.startSize = new ParticleSystem.MinMaxCurve(UnityEngine.Random.Range(0.2f, 0.4f),
               UnityEngine.Random.Range(0.45f, 0.6f));


                }
                main.startColor = opt;
                new_ = new Material(Resources.Load<Material>("Hart"));
                go.transform.localPosition = new Vector3(0, Screen.height / 2 + 10, 0);
                sm.shapeType = ParticleSystemShapeType.BoxEdge;
                sm.scale = new Vector3(4.5f, 0.045964f, 1);

                main.gravityModifier = 0.01f;


                main.startLifetime = 15.5f;

                ParticleSystem.VelocityOverLifetimeModule vl = ps.velocityOverLifetime;
                vl.enabled = true;
                vl.x = new ParticleSystem.MinMaxCurve(-0.2f, 0.2f);
                break;
            default:
                new_ = new Material(Resources.Load<Material>("Hart"));
                break;

        }
        new_.mainTexture = particle.texture;
        psr.material = new_;
        go.SetActive(true);
    }


    // Sprite[] convertAt(UnityEngine.Object a)
    // {

    //     IEnumerable ae = a as IEnumerable;
    //     List<Sprite> ret = new List<Sprite>();
    //     if (ae != null)
    //     {
    //         foreach (object element in ae)
    //         {
    //             ret.Add((Sprite)element);
    //         }
    //     }
    //     Debug.Log("done with " + a.ToString() + "!");
    //     return ret.ToArray();
    // }
    public void LoadAll()
    {
        if (brows.Length != 0 && lips.Length != 0 && xtra.Length != 0 && nose.Length != 0 &&
        bangs.Length != 0 && hair.Length != 0 && clothes.Length != 0 && bg.Length != 0)
        {
            load.loaddd = false;
            return;
        }

        load.startLoading(false);

        ResourceRequest async = Resources.LoadAsync("");
        // if (xtra.Length == 0)
        // {

        //     ResourceRequest temp = Resources.LoadAsync("xtra", typeof(Sprite[]));
        //     temp.completed += (AsyncOperation ao) =>
        //     {
        //         xtra = convertAt(temp.asset);
        //     };


        // }
        // if (hair.Length == 0)
        // {
        //     ResourceRequest temp = Resources.LoadAsync("bh", typeof(Sprite[]));
        //     temp.completed += (AsyncOperation ao) =>
        //     {
        //         hair = convertAt(temp.asset);
        //     };

        // }
        // if (bangs.Length == 0)
        // {
        //     ResourceRequest temp = Resources.LoadAsync("b_");
        //     temp.completed += (AsyncOperation ao) =>
        //     {
        //         bangs = convertAt(temp.asset);
        //     };

        // }
        // if (eyes.Length == 0)
        // {
        //     ResourceRequest temp = Resources.LoadAsync("ey");
        //     temp.completed += (AsyncOperation ao) =>
        //     {
        //         eyes = convertAt(temp.asset);
        //     };

        // }
        // if (nose.Length == 0)
        // {
        //     ResourceRequest temp = Resources.LoadAsync("n_se");
        //     temp.completed += (AsyncOperation ao) =>
        //     {
        //         nose = convertAt(temp.asset);
        //     };

        // }
        // if (lips.Length == 0)
        // {
        //     ResourceRequest temp = Resources.LoadAsync("l_ps");
        //     temp.completed += (AsyncOperation ao) =>
        //     {
        //         lips = convertAt(temp.asset);
        //     };

        // }
        // if (brows.Length == 0)
        // {
        //     ResourceRequest temp = Resources.LoadAsync("ebrows");
        //     temp.completed += (AsyncOperation ao) =>
        //     {
        //         brows = convertAt(temp.asset);
        //     };

        // }
        // if (bg.Length == 0)
        // {
        //     ResourceRequest temp = Resources.LoadAsync("bg");
        //     temp.completed += (AsyncOperation ao) =>
        //     {
        //         bg = convertAt(temp.asset);
        //     };

        // }
        // if (clothes.Length == 0)
        // {
        //     ResourceRequest temp = Resources.LoadAsync("clothes");
        //     temp.completed += (AsyncOperation ao) =>
        //    {
        //        clothes = convertAt(temp.asset);
        //    }; 

        // }
        StartCoroutine(loadFromResourcesFolder(async));

    }
    IEnumerator loadFromResourcesFolder(ResourceRequest a)
    {

        //Wait till we are done loading
        while (!a.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("done!");
        if (brows.Length == 0)
        {
            brows = Resources.LoadAll<Sprite>("ebrows");
        }
        if (lips.Length == 0)
        {
            lips = Resources.LoadAll<Sprite>("l_ps");
        }
        if (xtra.Length == 0)
        {
            xtra = Resources.LoadAll<Sprite>("xtra");
        }
        if (nose.Length == 0)
        {
            nose = Resources.LoadAll<Sprite>("n_se");
        }
        if (bangs.Length == 0)
        {
            bangs = Resources.LoadAll<Sprite>("b_");
        }
        if (hair.Length == 0)
        {
            hair = Resources.LoadAll<Sprite>("bh");
        }
        if (clothes.Length == 0)
        {
            clothes = Resources.LoadAll<Sprite>("clothes");
        }
        if (bg.Length == 0)
        {
            bg = Resources.LoadAll<Sprite>("bg");
        }
        if (eyes.Length == 0)
        {
            eyes = Resources.LoadAll<Sprite>("ey");
        }

        //Get the loaded data
        load.StopLoading();
    }



    public Sprite getSpriteFromString(itemType it, string name = "")
    {

        if (name == "") return null;

        switch (it)
        {
            case itemType.w_hites:
                return eye[1];
            case itemType.iris:
                return eye[0];
            case itemType.eyelid:
                return eye[2];
            case itemType.se:
            case itemType.Se:
            case itemType.xe:
                return eye.FirstOrDefault(i => i.name == name);
            case itemType.BOW:
            case itemType.chneckwear:
            case itemType.glasses:
            case itemType.t_clothes:
            case itemType.overalls:
                return clothes.FirstOrDefault(i => i.name == name);


            case itemType.b0odnos:
                return xtra[0];
            case itemType.bDaid:
                return xtra[1];
            case itemType.blush:
                return xtra[2];
            case itemType.bood:
                return xtra[3];
            case itemType.bronzer:
                return xtra[4];
            case itemType.bubble:
                return xtra[5];
            case itemType.EPatch:
                return xtra[12];
            case itemType.FGliter:
                return xtra[13];
            case itemType.flower:
                return xtra[14];
            case itemType.freckles:
                return xtra[15];
            case itemType.harts:
                return xtra[17];
            case itemType.hdphones:
                return xtra[19];
            case itemType.hwrstrand:
                return xtra[25];
            case itemType.JBandage:
                return xtra[26];
            case itemType.lippiercing:
                return xtra[27];
            case itemType.msk:
                return xtra[28];
            case itemType.nosepiercing:
                return xtra[29];
            case itemType.Particle_petal:
                return xtra[30];
            case itemType.particle_snow:
                return xtra[31];
            case itemType.pArticle_sparkle:
                return xtra[32];
            case itemType.scar:
                return xtra[33];
            case itemType.sl1:
                return xtra[34];
            case itemType.starfreckles:
                return xtra[35];
            case itemType.sx_tears:
                return xtra[36];
            case itemType.UEye:
                return xtra[37];

            case itemType.unicorn:
                return xtra[38];

            case itemType.e_ye:
                return eyes.FirstOrDefault(i => i.name == name);
            case itemType.l_p:
                return lips.FirstOrDefault(i => i.name == name);
            case itemType.n_se:
                return nose.FirstOrDefault(i => i.name == name);

            case itemType.bg:
                return bg.FirstOrDefault(i => i.name == name);

            case itemType.ebrow:
                return brows.FirstOrDefault(i => i.name == name);
            case itemType.bh_air:
                return hair.FirstOrDefault(i => i.name == name);
            case itemType.b_ngs:
                return bangs.FirstOrDefault(i => i.name == name);

        }
        return xtra.FirstOrDefault(i => i.name == name);
    }

    //helper function for setting up shop tabs
    void DeactivateExtra(int i)
    {
        for (; i < items.Count; i++)
        {
            items[i].gameObject.SetActive(false);
        }
    }



    //helper function for setting up shop tabs
    bool isEquipped(Sprite r)
    {
        string key = r.name.Substring(0, 2);
        return fm.XtraStuff.ContainsKey(key) && (fm.XtraStuff[key].sprite == r);
    }
    //helper function for setting up shop tabs
    bool isEquipped(string key)
    {
        return fm.XtraStuff.ContainsKey(key);
    }
    //helper function for setting up shop tabs
    bool isEquippedParticle(Sprite r)
    {
        string key = r.name.Substring(0, 2);
        return fm.XtraStuff.ContainsKey(key) && GameObject.FindGameObjectWithTag("Finish").transform.Find(r.name) != null;
    }
    //helper function for setting up shop tabs
    void button_helper(Sprite[] arr, itemType it)
    {
        ShopItem next = items[0];
        int i = 0;
        for (; i < arr.Length; i++)
        {
            next = getnext(i, next);
            next.SetItem(arr[i], it, this, isEquipped(arr[i]));

        }
        DeactivateExtra(i);
    }
    //helper function for setting up shop tabs
    void button_helper(Sprite[] arr, itemType it, int i, int len)
    {
        ShopItem next = items[0];
        for (; i < len; i++)
        {
            next = getnext(i, next);
            next.SetItem(arr[i], it, this, isEquipped(arr[i]));
        }
    }
    //helper function for setting up shop tabs
    void button_helper(Sprite[] arr, itemType it, int i, int start, int len)
    {
        int diff = i + (len - start);
        ShopItem next = items[0];
        for (; i < diff; i++, start++)
        {

            next = getnext(i, next);
            next.SetItem(arr[start], it, this, isEquipped(arr[start]));
        }
    }

    void button_single(Sprite[] arr, itemType it, int i, int index)
    {
        ShopItem next = items[0];
        next = getnext(i, next);
        next.SetItem(arr[index], it, this, isEquipped(arr[index]));

    }

    //helper function for setting up shop tabs
    void button_helper_particles(Sprite[] arr, itemType it, int i, int start, int len)
    {
        int diff = i + (len - start);
        ShopItem next = items[0];
        for (; i < diff; i++, start++)
        {

            next = getnext(i, next);
            next.SetItem(arr[start], it, this, isEquippedParticle(arr[start]));
        }
    }

    //helper function for setting up shop tabs
    void button_helper(Sprite[] arr, itemType it, int i, ShopItem next)
    {
        for (; i < arr.Length; i++)
        {
            next = getnext(i, next);
            next.SetItem(arr[i], it, this, isEquipped(arr[i]));

        }
        DeactivateExtra(i);
    }


    //helper function for setting up shop tabs
    ShopItem getnext(int i, ShopItem next)
    {
        if (items.Count > i)
        {
            next = items[i];
        }
        else
        {
            next = Instantiate(next.gameObject, next.transform.parent, false).GetComponent<ShopItem>();
            items.Add(next);
        }
        return next;
    }
    //helper function for setting up shop tabs
    void switching()
    {
        callSwitch();

        actualContent.anchoredPosition = new Vector2(actualContent.anchoredPosition.x, 0);
    }
    public void callSwitch()
    {
        if (Switch != null)
        {
            Switch();
            Switch = null;
        }

    }
    public void eye_()
    {
        lastTab = 0;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());

        switching();
        int index = 0;
        button_single(eye, itemType.iris, index++, 0);
        button_single(eye, itemType.w_hites, index++, 1);
        button_single(eye, itemType.eyelid, index++, 2);
        button_single(eye, itemType.Se, index++, 4);
        button_single(eye, itemType.se, index++, 3);
        button_helper(eye, itemType.se, index, 5, 8);
        index += 3;
        button_helper(eye, itemType.xe, index, 8, 10);
        index += 2;

        DeactivateExtra(index);

    }
    public void eyes_()
    {

        if (eyes.Length == 0)
        {
            eyes = Resources.LoadAll<Sprite>("ey");
        }
        switching();
        lastTab = 1;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());

        button_helper(eyes, itemType.e_ye);
    }

    public void brows_()
    {
        if (brows.Length == 0)
        {
            brows = Resources.LoadAll<Sprite>("ebrows");
        }
        switching();
        lastTab = 2;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());


        button_helper(brows, itemType.ebrow);
    }
    public void lips_()
    {
        if (lips.Length == 0)
        {
            lips = Resources.LoadAll<Sprite>("l_ps");
        }
        if (xtra.Length == 0)
        {
            xtra = Resources.LoadAll<Sprite>("xtra");
        }
        switching();

        lastTab = 3;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());

        button_helper(lips, itemType.l_p);
        button_single(xtra, itemType.sl1, lips.Length, 34);
    }

    public void nose_()
    {
        if (nose.Length == 0)
        {
            nose = Resources.LoadAll<Sprite>("n_se");
        }

        switching();

        lastTab = 4;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());

        button_helper(nose, itemType.n_se);

    }
    public void skin_()
    {
        if (xtra.Length == 0)
        {
            xtra = Resources.LoadAll<Sprite>("xtra");
        }
        switching();

        lastTab = 5;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());


        int index = 0;
        ShopItem next = items[index++];
        next.SetItem(null, itemType.skin, this, true, fm.skin[1].color);
        next = getnext(index++, items[0]);
        next.SetItem(fm.skin[4].sprite, itemType.BY_ODY, this, true, fm.skin[4].color);
        next = getnext(index++, items[0]);
        next.SetItem(fm.skin[1].sprite, itemType.HD, this, true, fm.skin[1].color);
        next = getnext(index++, items[0]);
        next.SetItem(fm.skin[8].sprite, itemType.CHIN, this, true, fm.skin[8].color);


        button_single(xtra, itemType.blush, index++, 2);
        button_single(xtra, itemType.bronzer, index++, 4);
        button_single(xtra, itemType.freckles, index++, 15);
        button_single(xtra, itemType.scar, index++, 33);
        button_single(xtra, itemType.UEye, index++, 37);
        button_single(xtra, itemType.FGliter, index++, 13);


        DeactivateExtra(index);
    }

    public void bangs_()
    {

        if (bangs.Length == 0)
        {
            bangs = Resources.LoadAll<Sprite>("b_");
        }
        switching();

        lastTab = 6;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());

        button_helper(bangs, itemType.b_ngs);

    }
    public void hair_()
    {

        if (hair.Length == 0)
        {
            hair = Resources.LoadAll<Sprite>("bh");
        }

        switching();

        lastTab = 7;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());

        button_helper(hair, itemType.bh_air);

    }

    public void clothes_()
    {
        if (clothes.Length == 0)
        {
            clothes = Resources.LoadAll<Sprite>("clothes");
        }

        if (xtra == null || xtra.Length == 0)
        {
            xtra = Resources.LoadAll<Sprite>("xtra");
        }
        switching();

        lastTab = 8;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());

        int index = 0;
        button_single(clothes, itemType.t_clothes, index++, 9);
        button_single(clothes, itemType.t_clothes, index++, 10);
        button_single(clothes, itemType.t_clothes, index++, 14);
        button_single(clothes, itemType.t_clothes, index++, 16);
        button_single(clothes, itemType.t_clothes, index++, 11);
        button_single(clothes, itemType.t_clothes, index++, 12);
        button_single(clothes, itemType.t_clothes, index++, 13);
        button_single(clothes, itemType.t_clothes, index++, 15);
        button_single(clothes, itemType.BOW, index++, 0);
        button_single(clothes, itemType.BOW, index++, 1);
        button_helper(clothes, itemType.chneckwear, index++, 2, 5);
        index += 2;
        button_helper(clothes, itemType.glasses, index++, 5, 8);
        index += 2;
        button_single(xtra, itemType.EPatch, index++, 12);
        button_single(xtra, itemType.msk, index++, 28);
        button_single(clothes, itemType.overalls, index++, 8);
        DeactivateExtra(index);
    }

    public void bg_()
    {
        if (bg.Length == 0)
        {
            bg = Resources.LoadAll<Sprite>("bg");
        }
        switching(); lastTab = 9;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());

        int index = 0;
        ShopItem next = items[index++];
        next.SetItem(null, itemType.bg, this, false, Camera.main.backgroundColor);

        button_helper(bg, itemType.bg, index++, 6, 10);
        index += 3;
        button_single(bg, itemType.bg, index++, 2);
        button_single(bg, itemType.bg, index++, 1);
        button_single(bg, itemType.bg, index++, 11);
        button_single(bg, itemType.bg, index++, 0);
        button_single(bg, itemType.bg, index++, 5);
        button_single(bg, itemType.bg, index++, 4);
        button_single(bg, itemType.bg, index++, 3);
        button_single(bg, itemType.bg, index++, 10);
        DeactivateExtra(index);



    }
    public void xtra_()
    {

        if (xtra == null || xtra.Length == 0)
        {
            xtra = Resources.LoadAll<Sprite>("xtra");
        }
        switching();
        lastTab = 10;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());

        int index = 0;

        button_single(xtra, itemType.starfreckles, index++, 35);
        button_single(xtra, itemType.hwrstrand, index++, 25);
        button_single(xtra, itemType.bubble, index++, 5);
        button_single(xtra, itemType.hdphones, index++, 19);
        button_helper(xtra, itemType.ctetopband, index++, 6, 8);
        index++;
        button_helper(xtra, itemType.hrclip, index++, 23, 25);
        index++;
        button_helper(xtra, itemType.eear, index++, 8, 12);
        index += 3;
        button_helper(xtra, itemType.hesidehorn, index++, 20, 23);
        index += 2;
        button_single(xtra, itemType.unicorn, index++, 38);
        button_single(xtra, itemType.harts, index++, 17);
        button_single(xtra, itemType.flower, index++, 14);
        button_single(xtra, itemType.lippiercing, index++, 27);
        button_single(xtra, itemType.nosepiercing, index++, 29);
        button_single(xtra, itemType.b0odnos, index++, 0);
        button_single(xtra, itemType.bood, index++, 3);
        button_single(xtra, itemType.JBandage, index++, 26);
        button_single(xtra, itemType.bDaid, index++, 1);
        button_single(xtra, itemType.sx_tears, index++, 36);

        DeactivateExtra(index);

    }


    Sprite CreateRamp(Color one, Color two)
    {
        int size = 256;
        Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point;

        Color[] textureData = new Color[size * size];

        for (int x = 0; x < size; x++)
        {
            for (int y = size - 1; y >= 0; y--)
            {
                Color lerp = Color.Lerp(two, one, y / (float)(size - 1));

                textureData[x + y * size] = lerp;

            }

        }
        texture.SetPixels(textureData);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0f, 1f), 233);
    }

    public void effects()
    {

        if (xtra == null || xtra.Length == 0)
        {
            xtra = Resources.LoadAll<Sprite>("xtra");
        }
        switching();
        lastTab = 11;
        Switch += highlightButton(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(lastTab).GetComponent<Button>());

        int index = 0;
        button_helper_particles(xtra, itemType.Particle_petal, index++, 30, 31);
        button_helper_particles(xtra, itemType.particle_snow, index++, 31, 32);
        button_helper_particles(xtra, itemType.pArticle_sparkle, index++, 32, 33);


        ShopItem next = getnext(index++, items[0]);
        bool e = isEquipped("FR");
        next.SetItem(Resources.Load<Sprite>("wig"), itemType.FRGradient, this, e, Color.white);

        //WaterScript deluxe
        next = getnext(index++, items[0]);
        e = isEquipped("CX");
        next.SetItem(null, itemType.CXolor, this, e, e ? Camera.main.GetComponent<ColorFX>().color : new Color(0.4f, 0.4f, 0.4f, 1));

        next = getnext(index++, items[0]);
        e = isEquipped("BN");
        Texture2D tex = Resources.Load<Texture2D>("BN");
        Sprite Bn = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        Bn.name = "BN";
        next.SetItem(Bn, itemType.BNry, this, e, e ? Camera.main.GetComponent<Binary>().color1 : Color.white);

        next = getnext(index++, items[0]);
        e = isEquipped("RP");
        Sprite fill;
        if (e)
        {
            Ramp r = Camera.main.gameObject.GetComponent<Ramp>();
            fill = CreateRamp(r.SecondColor, r.FirstColor);
        }
        else
        {
            fill = CreateRamp(Color.red, Color.blue);
        }
        fill.name = "RP";
        next.SetItem(fill, itemType.RPamp, this, e, Color.white);

        next = getnext(index++, items[0]);
        e = isEquipped("wd");
        next.SetItem(null, itemType.wdEluxeScript, this, e, e ? fm.XtraStuff["wd"].color : Color.white);
        //WaterfallScript
        next = getnext(index++, items[0]);
        e = isEquipped("wf");
        next.SetItem(null, itemType.wfFallScript, this, e, e ? fm.XtraStuff["wf"].GetComponent<WaterfallScript>().LightColor : Color.white);
        //glitch
        next = getnext(index++, items[0]);
        e = isEquipped("GX");
        next.SetItem(xtra[16], itemType.GXlitch, this, e, Color.white);

        next = getnext(index++, items[0]);
        e = isEquipped("BM");
        next.SetItem(xtra[31], itemType.BMlm, this, e, Color.white);

        DeactivateExtra(index);

    }

    Action highlightButton(Button b)
    {
        ColorBlock cb = b.colors;
        Color n = cb.normalColor;
        float sat = n.r + n.g + n.b;
        sat /= 3;
        Color text, butt;
        if (sat > 0.5f)
        {
            //light
            text = Color.white;
            float ratio = sat - 0.3f;
            butt = new Color(n.r * ratio, n.g * ratio, n.b * ratio, 1);
        }
        else
        {
            text = Color.black;
            float ratio = 0.7f;
            butt = new Color(1 - n.r * ratio, 1 - n.g * ratio, 1 - n.b * ratio, 1);
        }
        cb.normalColor = butt;
        cb.highlightedColor = butt;
        b.colors = cb;
        TextMeshProUGUI t = b.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Color tB = t.color;
        t.color = text;

        return () =>
        {
            cb.normalColor = n;
            cb.highlightedColor = n;
            b.colors = cb;
            t.color = tB;

        };

    }
}
