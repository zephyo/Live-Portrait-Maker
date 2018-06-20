using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Kino;
using SoftMasking;



public class ShopItem : MonoBehaviour
{

    // Use this for initialization
    public void SetItem(Sprite smth, itemType i,
   DressManager dm, bool equipped)
    {
        Image img = GetComponent<Image>();
        string key = i.ToString().Substring(0, 2);
        if (equipped)
        {
            Debug.Log("set color!");
            if (dm.fm.XtraStuff.ContainsKey(key) && dm.fm.XtraStuff[key] != null)
                img.color = dm.fm.XtraStuff[key].color;
        }
        else
        {

            img.color = Color.white;
        }
        Helper(img, smth, i, dm, equipped);
    }

    public void SetItem(Sprite smth, itemType i,
        DressManager dm, bool equipped, Color change)
    {
        Image im = GetComponent<Image>();
        im.color = change;
        Helper(im, smth, i, dm, equipped);

    }

    void Helper(Image im, Sprite smth, itemType i,
      DressManager dm, bool equipped)
    {
        im.sprite = smth;
        Button b = GetComponent<Button>();
        SetUpButton(b, smth, i, dm);


        if (equipped)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        if (i == itemType.wfFallScript)
        {
            Texture2D tex = Resources.Load<Texture2D>("WaterfallTexture");
            im.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        }
        else if (i == itemType.wdEluxeScript)
        {
            im.sprite = Resources.Load<Sprite>("Water");

        }
        else if (i == itemType.wdEluxeScript)
        {
            im.sprite = Resources.Load<Sprite>("Water");

        }
        else if (i == itemType.BNry)
        {
            im.type = Image.Type.Tiled;
            dm.Switch += () =>
            {
                im.type = Image.Type.Simple;

            };

        }
        else if (i == itemType.FRGradient)
        {
            FourGradient prev = gameObject.AddComponent<FourGradient>();
            prev.opacity = 1;
            if (dm.fm.XtraStuff.ContainsKey("FR"))
            {
                FourGradient fg = dm.fm.bangs.GetComponent<FourGradient>();
                prev._Color1 = fg._Color1; prev._Color2 = fg._Color2; prev._Color3 = fg._Color3; prev._Color4 = fg._Color4;
            }

            prev.updateParams();
            dm.Switch += () =>
       {
           Debug.Log("destroy prev!");
           Destroy(prev);

       };

        }
        else if (i == itemType.GXlitch || i == itemType.BMlm)
        {
            im.type = Image.Type.Sliced;
            dm.Switch += () =>
            {
                im.type = Image.Type.Simple;

            };

        }
        gameObject.SetActive(true);
    }

    bool isEquipped()
    {
        return transform.GetChild(0).gameObject.activeSelf;
    }


    void SpecialFX(itemType i, DressManager dm, UndoInfo fs)
    {
        int l = PlayerPrefs.GetInt("Lang");
        ColorPicker cpa = dm.cpa;
        string key = i.ToString().Substring(0, 2);
        switch (i)
        {
            case itemType.BNry:
                Binary b = Camera.main.gameObject.GetComponent<Binary>();
                if (!isEquipped())
                {
                    dm.x.onClick.AddListener(() =>
                            {
                                dm.fm.Remove("BN");
                            });
                }
                else
                {
                    Color c0 = b.color0, c1 = b.color1;
                    float alpha = b.Opacity;
                    dm.x.onClick.AddListener(() =>
                           {
                               b.color0 = c0;
                               b.color1 = c1;
                               b.Opacity = alpha;
                               b.updateColor();

                           });
                }
                cpa.Color = b.color1;
                cpa.Reset();



                Iris lr3 = cpa.getLeftRight();
                TurnCanvas(lr3.transform, true);
                string light, dark;
                switch (l)
                {
                    case 1:
                        //chinese
                        light = "光"; dark = "黑暗";
                        break;
                    case 2:
                        //ja
                        light = "光"; dark = "闇";
                        break;
                    case 3:
                        //rus
                        light = "легкий"; dark = "темно";
                        break;
                    case 4:
                        light = "ligero"; dark = "oscuro";
                        break;
                    case 5:
                        //thai
                        light = "สีอ่อน"; dark = "สีเข้ม";
                        break;
                    case 6:
                        light = "lumière"; dark = "foncé";
                        break;
                    default:
                        //english
                        light = "light"; dark = "dark";
                        break;
                }


                lr3.fillButtons(light, dark,
                () => { b.color1 = cpa.Color; b.updateColor(); },
                 () => { b.color0 = cpa.Color; b.updateColor(); }
                );

                Slider s = getOpacitySlider(cpa.transform);
                s.value = b.Opacity;
                s.onValueChanged.AddListener((float val) =>
                {
                    b.Opacity = val;
                    b.updateColor();
                });


                dm.x.onClick.AddListener(() =>
                {
                    s.gameObject.SetActive(false);
                    lr3.gameObject.SetActive(false);
                    TurnCanvas(lr3.transform, false);

                });

                b.enabled = true;
                lr3.setListeners();


                break;
            case itemType.BMlm:
                Bloom bl = Camera.main.gameObject.GetComponent<Bloom>();
                Slider[] t = getSliders(cpa);
                t[0].onValueChanged.RemoveAllListeners();
                t[1].onValueChanged.RemoveAllListeners();


                if (!isEquipped())
                {
                    dm.x.onClick.AddListener(() =>
                            {
                                dm.fm.Remove("BM");
                            });
                }
                else
                {
                    float intensity = bl.intensity;
                    float radius = bl.radius;
                    dm.x.onClick.AddListener(() =>
                           {
                               bl.intensity = intensity;
                               bl.radius = radius;
                               bl.UpdateParam();
                           });
                }

                float ratio = 0.3f;
                float ratio2 = 6f;
                // 20- 48

                Image oneI = t[0].transform.GetChild(1).GetComponent<Image>();
                Sprite before = oneI.sprite;
                oneI.sprite = Resources.Load<Sprite>("random");
                oneI.rectTransform.sizeDelta = Vector2.one * 64;
                t[0].value = (bl.radius - 1) / ratio2;
                t[1].value = bl.intensity / ratio;


                dm.x.onClick.AddListener(() =>
                {
                    t[1].transform.parent.gameObject.SetActive(false);
                    oneI.sprite = before;
                    oneI.rectTransform.sizeDelta = Vector2.one * 80.99f;
                });

                t[0].onValueChanged.AddListener((float val) =>
                 {
                     val = val / 2 + 0.5f;
                     bl.radius = val * ratio2 + 1;
                     bl.UpdateParam();
                 });
                t[1].onValueChanged.AddListener((float val) =>
                {
                    val = val / 2 + 0.5f;
                    bl.intensity = val * ratio;
                    bl.UpdateParam();
                });
                dm.colorPick(fs, i, transform.GetChild(0).gameObject);
                bl.enabled = true;
                return;

            case itemType.RPamp:
                Ramp r = Camera.main.gameObject.GetComponent<Ramp>();
                if (!isEquipped())
                {
                    dm.x.onClick.AddListener(() =>
                            {
                                dm.fm.Remove("RP");
                            });
                    cpa.Color = Color.blue;
                }
                else
                {

                    cpa.Color = r.FirstColor;
                    Color c0 = r.FirstColor, c1 = r.SecondColor;
                    float a = r._opacity;
                    dm.x.onClick.AddListener(() =>
                          {
                              r.FirstColor = c0;
                              r.SecondColor = c0;
                              r._opacity = a;
                              r.updateColors();
                          });

                }



                Iris lr2 = cpa.getLeftRight();
                TurnCanvas(lr2.transform, true);

                string bo, to;
                switch (l)
                {
                    case 1:
                        //chinese
                        bo = "降低"; to = "上";
                        break;
                    case 2:
                        //ja
                        bo = "低い"; to = "アッパー";
                        break;
                    case 3:
                        //rus
                        bo = "ниже"; to = "Вверх";
                        break;
                    case 4:
                        bo = "inferior"; to = "superior";
                        break;
                    case 5:
                        //thai
                        bo = "โคน"; to = "ด้านบน";
                        break;
                    case 6:
                        bo = "inférieur"; to = "supérieur";
                        break;
                    default:
                        //english
                        bo = "bottom"; to = "top";
                        break;
                }



                lr2.fillButtons(bo, to,
                 () => { r.FirstColor = cpa.Color; r.updateColors(); },
                  () => { r.SecondColor = cpa.Color; r.updateColors(); }
                 );

                Slider s2 = getOpacitySlider(cpa.transform);
                s2.value = r._opacity;
                s2.onValueChanged.AddListener((float val) =>
                {
                    r._opacity = val;
                    r.updateColors();
                });

                dm.x.onClick.AddListener(() =>
                {
                    s2.gameObject.SetActive(false);
                    lr2.gameObject.SetActive(false);
                    TurnCanvas(lr2.transform, false);

                });


                cpa.Reset();
                lr2.setListeners();

                r.enabled = true;


                break;
            case itemType.CXolor:
                ColorFX fx = Camera.main.gameObject.GetComponent<ColorFX>();
                if (!isEquipped())
                {
                    dm.x.onClick.AddListener(() =>
                    {
                        dm.fm.Remove("CX");
                    });
                    cpa.Color = new Color(0.4f, 0.4f, 0.4f, 1);

                }
                else
                {
                    cpa.Color = fx.color;
                    Color prev = fx.color;
                    dm.x.onClick.AddListener(() =>
                   {
                       fx.color = prev;
                       fx.updateColor();
                   });

                }
                cpa.Reset();
                cpa.clearUpdateColor();
                cpa.UpdateColorAction += () => { fx.color = cpa.Color; fx.updateColor(); };


                Slider s3 = getOpacitySlider(cpa.transform);
                TurnCanvas(s3.transform, true);
                s3.value = fx.Amount;
                s3.onValueChanged.AddListener((float val) =>
                {
                    fx.Amount = val;
                    fx.updateColor();
                });

                dm.x.onClick.AddListener(() =>
              {
                  s3.gameObject.SetActive(false);
                  TurnCanvas(s3.transform, false);
              });

                fx.enabled = true;
                break;




            case itemType.wdEluxeScript:

                if (!isEquipped())
                {
                    dm.x.onClick.AddListener(() =>
                            {
                                dm.fm.XtraStuff.Remove(key);
                                Destroy(dm.fm.transform.Find(key).gameObject);
                            });
                }
                else
                {
                    cpa.Color = fs.set.color;
                    cpa.Reset();
                }
                break;
            case itemType.wfFallScript:

                if (!isEquipped())
                {
                    dm.x.onClick.AddListener(() =>
                        {
                            dm.fm.XtraStuff.Remove(key);
                            Destroy(dm.fm.transform.Find(key).gameObject);
                        });
                }
                else
                {
                    cpa.Color = fs.set.GetComponent<WaterfallScript>().LightColor;
                    cpa.Reset();
                }
                break;

            case itemType.FRGradient:
                FourGradient fg;

                bool e = isEquipped();
                if (!e)
                {
                    dm.fm.bangs.GetComponent<SoftMask>().enabled = false;
                    dm.fm.bangs.transform.GetChild(0).gameObject.SetActive(false);
                    fg = dm.fm.bangs.gameObject.AddComponent<FourGradient>();
                    dm.fm.hair.material = dm.fm.bangs.materialForRendering;
                    dm.x.onClick.AddListener(() =>
                            {
                                dm.fm.Remove(key);

                            });
                }
                else
                {
                    fg = dm.fm.bangs.gameObject.GetComponent<FourGradient>();

                    cpa.Color = fg._Color1;
                    Color c0 = fg._Color1, c1 = fg._Color2, c2 = fg._Color3, c3 = fg._Color4;
                    float o = fg.opacity;
                    dm.x.onClick.AddListener(() =>
                          {

                              fg._Color1 = c0; fg._Color2 = c1; fg._Color3 = c2; fg._Color4 = c3;
                              fg.opacity = o;
                              fg.updateParams();


                          });

                }
                Iris lr4 = cpa.getLeftRight();
                TurnCanvas(lr4.transform, true);

                string bo2, to2, bo3, to3; //t l, t r, b l , b r
                switch (l)
                {
                    case 1:
                    case 2:
                        bo2 = "左上"; to2 = "右上"; bo3 = "左下"; to3 = "右下";
                        break;
                    case 3:
                        //rus
                        bo2 = "верхний левый"; to2 = "правом верхнем"; bo3 = "Нижняя левая"; to3 = "внизу справа";
                        break;
                    case 4:
                        bo2 = "izquierda superior"; to2 = "derecha superior"; bo3 = "izquierda inferior"; to3 = "derecha inferior";
                        //spansih
                        break;
                    case 5:
                        //thai
                        bo2 = "บนซ้าย"; to2 = "ด้านบนขวา"; bo3 = "ล่างซ้าย"; to3 = "ล่างขวา";
                        break;
                    case 6:
                        bo2 = "haut gauche"; to2 = "haut droite"; bo3 = "bas gauche"; to3 = "bas droite";

                        break;
                    default:
                        //english
                        bo2 = "top left"; to2 = "top right"; bo3 = "bottom left"; to3 = "bottom right";
                        break;
                }



                lr4.fillButtons(bo2, to2,
                 () =>
                 {
                     fg._Color1 = cpa.Color; fg.updateSingleParam(1);
                 },
                  () =>
                  {
                      fg._Color2 = cpa.Color; fg.updateSingleParam(2);
                  }
                 );

                lr4.fill2MoreButtons(bo3, to3,
                    () =>
                    {
                        fg._Color3 = cpa.Color; fg.updateSingleParam(3);
                    },
                  () =>
                  {
                      fg._Color4 = cpa.Color; fg.updateSingleParam(4);
                  },
                   dm.x, e ? lr4.transform.parent.GetChild(0).GetComponent<Button>() : null
                );

                Slider sFG = getOpacitySlider(cpa.transform);
                sFG.value = fg.opacity;
                sFG.onValueChanged.AddListener((float val) =>
                {
                    fg.opacity = val * 0.9f; fg.updateParams();
                });

                dm.x.onClick.AddListener(() =>
                {
                    sFG.gameObject.SetActive(false);
                    lr4.gameObject.SetActive(false);
                    TurnCanvas(lr4.transform, false);

                });


                cpa.Reset();

                lr4.setListeners();
                break;

        }

        cpa.gameObject.SetActive(true);
        dm.colorPick(fs, i, transform.GetChild(0).gameObject);

    }

    bool isSpecialFX(itemType i)
    {
        int test = (int)i;
        return test >= (int)itemType.wdEluxeScript && test <= (int)itemType.CXolor && i != itemType.GXlitch;
    }


    void SetUpButton(Button b, Sprite smth, itemType i,
      DressManager dm)
    {
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() =>
        {
            ColorPicker cpa = dm.cpa;

            UndoInfo fs = dm.fm.faceSet(smth, i);

            if (isSpecialFX(i))
            {
                SpecialFX(i, dm, fs);
                return;
            }
            HashSet<itemType> slidersOnly = new HashSet<itemType>(){
                itemType.GXlitch, itemType.n_se, itemType.eyelid, itemType.CHIN, itemType.BY_ODY, itemType.HD
            };

            if (slidersOnly.Contains(i))
            {
                handleSlidersOnly(cpa, dm, fs, i);

            }
            else
            {
                if (fs.set != null)
                {
                    if (i == itemType.w_hites || i == itemType.hesidehorn || i == itemType.eear)
                    {
                        handleSeparateColors(cpa, fs, dm.x);

                    }

                    else if (i == itemType.bg)
                    {

                        if (smth != null && smth.name[2] == 'p')
                        {
                            handleBackground(cpa, fs, dm.x);
                        }
                        else if (smth != null && (fs.before == null || fs.before.name != fs.set.sprite.name))
                        {
                            Camera.main.backgroundColor = Color.black;

                        }
                        cpa.Color = Camera.main.backgroundColor; cpa.Reset();
                    }
                    else
                    {
                        HashSet<itemType> transformThese = new HashSet<itemType>(){
itemType.bh_air, itemType.b_ngs, itemType.e_ye, itemType.iris, itemType.l_p, itemType.ebrow
                        };
                        if (transformThese.Contains(i))
                        {
                            handleExclusive(cpa, dm.x, dm.fm, i, fs);
                        }

                        cpa.Color = fs.set.color;
                        cpa.Reset();
                    }
                }
                else
                {
                    if (i.ToString().ToLower().Substring(0, 2) == "pa")
                    {
                        handleParticle(cpa, smth.name, dm);
                    }
                }
                cpa.gameObject.SetActive(true);
            }
            dm.colorPick(fs, i, transform.GetChild(0).gameObject);
        });

    }

    void TurnCanvas(Transform child, bool on)
    {
        CanvasGroup cg = child.transform.parent.GetComponent<CanvasGroup>();
        TurnThis(cg, on);
    }

    void TurnThis(CanvasGroup cg, bool on)
    {
        cg.blocksRaycasts = on;
        cg.interactable = on;
        cg.alpha = on ? 1 : 0;
    }


    Slider[] getSliders(ColorPicker cpa)
    {
        cpa.gameObject.SetActive(false);
        GameObject g = cpa.transform.parent.GetChild(3).gameObject;
        g.SetActive(true);
        return new Slider[]{ g.transform.GetChild(0).GetComponent<Slider>(),
        g.transform.GetChild(1).GetComponent<Slider>() };
    }


    Slider getOpacitySlider(Transform cpa)
    {
        Slider ret = cpa.transform.parent.GetChild(4).GetChild(1).GetComponent<Slider>();
        ret.gameObject.SetActive(true);
        ret.onValueChanged.RemoveAllListeners();
        return ret;

    }

    void handleExclusive(ColorPicker cpa, Button x, FaceManager fm, itemType i, UndoInfo fs)
    {
        Iris lr = cpa.getLeftRight();
        TurnCanvas(lr.transform, true);
        string lt, rt;
        switch (PlayerPrefs.GetInt("Lang"))
        {
            case 1:
                //chinese
                lt = "颜色"; rt = "转变";
                break;
            case 2:
                //ja
                lt = "色"; rt = "変更";
                break;
            case 3:
                //rus
                lt = "цвет"; rt = "менять";
                break;
            case 4:
                lt = "color"; rt = "transformar";
                break;
            case 5:
                //thai
                lt = "สี"; rt = "แปลง";
                break;
            case 6:
                lt = "Couleur"; rt = "transformer";
                break;
            default:
                //english
                lt = "color"; rt = "transform";
                break;
        }
        CanvasGroup cpaCG = cpa.GetComponent<CanvasGroup>();

        Slider[] temp = getSliders(cpa);
        Slider one = temp[0];
        Slider two = temp[1];
        CanvasGroup arrCG = one.transform.parent.GetComponent<CanvasGroup>();
        TurnThis(arrCG, false);

        one.onValueChanged.RemoveAllListeners();
        two.onValueChanged.RemoveAllListeners();

        UnityAction lftA = () => { TurnThis(cpaCG, true); TurnThis(arrCG, false); }
        ,
        rtA = () => { TurnThis(cpaCG, false); TurnThis(arrCG, true); };

        if (i == itemType.bh_air)
        {
            Dictionary<string, int[]> limits = new Dictionary<string, int[]>(){
                {"bh1", new int[]{72,348,1080,1229 } } ,//ratio x, ratio y, original x, original y
                 {"bh2", new int[]{ 41, 311, 1125, 1228} } ,
                 {"bh3", new int[]{ 42,492,1085,1509} } ,
                 {"bh4", new int[]{ 52,435, 1064,1554 } } ,
                 {"bh5", new int[]{ 80,431,1146, 1418} } ,
                 {"bh6", new int[]{ 35,243, 1105,1908} } ,
                 {"bh7", new int[]{ 49,148,1227,1930} } ,
                 {"bh8", new int[]{ 37,177,1080,1045} } ,
                 {"bh9", new int[]{ 62,126,1059,967} } ,
                 {"bh99", new int[]{ 65,249,1051,1861} } ,
            };

            RectTransform hair = fs.set.rectTransform;
            if (!limits.ContainsKey(fs.set.sprite.name)) return;
            int[] ratios = limits[fs.set.sprite.name];

            one.value = (hair.sizeDelta.x - ratios[2]) / ratios[0];
            two.value = (hair.sizeDelta.y - ratios[3]) / ratios[1];

            Vector2 orig = hair.sizeDelta;
            two.onValueChanged.AddListener((float val) =>
            {
                hair.sizeDelta = new Vector2(hair.sizeDelta.x, ratios[3] + val * ratios[1]);
            });
            one.onValueChanged.AddListener((float val) =>
            {
                hair.sizeDelta = new Vector2(ratios[2] + val * ratios[0], hair.sizeDelta.y);
            });

            x.onClick.AddListener(() =>
           {
               hair.sizeDelta = orig;
           });
        }
        else if (i == itemType.l_p)
        {
            RectTransform lip = fs.set.rectTransform;
            float width = 24, height = 15, posY = 11;
            float origW = fs.set.sprite.rect.width;
            one.value = (lip.sizeDelta.x - origW) / width;
            two.value = (lip.sizeDelta.y - 132) / height;

            float xx = lip.sizeDelta.x, y = lip.sizeDelta.y;
            float origY = fm.VertLip;

            one.onValueChanged.AddListener((float val) =>
            {
                lip.sizeDelta = new Vector2(origW + val * width, lip.sizeDelta.y);
            });

            Slider three = GameObject.Instantiate(two.gameObject, two.transform.parent, false).GetComponent<Slider>();
            two.onValueChanged.AddListener((float val) =>
             {
                 lip.sizeDelta = new Vector2(lip.sizeDelta.x, 132 + val * height);
             });
            three.GetComponent<Image>().color = new Color(0.74f, 0.61f, 0.815f, 1);
            three.onValueChanged.AddListener((float val) =>
            {
                fm.VertLip = val * posY;
            });
            two.transform.GetChild(1).eulerAngles = new Vector3(0, 0, 45);


            x.onClick.AddListener(() =>
           {
               fs.set.rectTransform.sizeDelta = new Vector2(xx, y);
               fm.VertLip = origY;
               Destroy(three.gameObject);
               two.transform.GetChild(1).eulerAngles = new Vector3(0, 0, 90);
           });

        }
        else if (i == itemType.e_ye)
        {
            float ratio = 15;


            one.value = fm.HorzEye / -ratio;
            two.value = fm.VertEye / ratio;

            Slider o = one;
            Slider t = two;
            one = GameObject.Instantiate(two.gameObject, two.transform.parent, false).GetComponent<Slider>();
            two = GameObject.Instantiate(two.gameObject, two.transform.parent, false).GetComponent<Slider>();


            float origX = fm.HorzEye;
            float origY = fm.VertEye;
            t.onValueChanged.AddListener((float val) =>
            {
                // eye1.anchoredPosition = new Vector2(eye1.anchoredPosition.x, -25 + ratio * val);
                // eye2.anchoredPosition = new Vector2(eye2.anchoredPosition.x, -25 + ratio * val);
                fm.VertEye = ratio * val;
            });
            o.onValueChanged.AddListener((float val) =>
            {
                fm.HorzEye = ratio * val;
            });

            x.onClick.AddListener(() =>
           {
               fm.VertEye = origY;
               fm.HorzEye = origX;

           });

            one.transform.GetChild(1).eulerAngles = new Vector3(0, 0, 45);
            Image img = two.transform.GetChild(1).GetComponent<Image>();
            Sprite before = img.sprite;
            img.sprite = Resources.Load<Sprite>("random");
            img.rectTransform.sizeDelta = Vector2.one * 64;
            img.rectTransform.eulerAngles = Vector3.zero;
            float sizeRatio = 25, zRatio = 5, origH = fs.set.sprite.rect.height, origW = fs.set.sprite.rect.width;


            RectTransform eye = (RectTransform)fs.set.rectTransform;
            RectTransform eye2 = (RectTransform)fs.set2.rectTransform;

            one.value = (eye.sizeDelta.y - origH) / sizeRatio;
            two.value = eye.localRotation.z / zRatio;



            one.onValueChanged.AddListener((float val) =>
            {
                eye.sizeDelta = new Vector2(origW + val * sizeRatio, origH + val * sizeRatio);
                eye2.sizeDelta = eye.sizeDelta;
            });
            two.onValueChanged.AddListener((float val) =>
            {
                eye.localRotation = Quaternion.Euler(0, eye.localRotation.y, val * zRatio);
                eye2.localRotation = Quaternion.Euler(0, eye2.localRotation.y, val * zRatio);
            });
            Vector2 orig = eye.sizeDelta;
            float origZ = eye.localRotation.z;


            t.GetComponent<Image>().color = new Color(0.74f, 0.61f, 0.815f, 1);
            two.GetComponent<Image>().color = new Color(0.568f, 0.737f, 0.838f, 1);


            x.onClick.AddListener(() =>
           {
               eye.sizeDelta = orig;
               eye2.sizeDelta = orig;
               eye.localRotation = Quaternion.Euler(0, eye.localRotation.y, origZ);
               eye2.localRotation = Quaternion.Euler(0, eye2.localRotation.y, origZ);
               t.GetComponent<Image>().color = new Color(0.619f, 0.624f, 0.8156f, 1);
               Destroy(one.gameObject);
               Destroy(two.gameObject);

           });

        }
        else if (i == itemType.ebrow)
        {

            RectTransform eb = fs.set.rectTransform;
            RectTransform eb2 = fs.set2.rectTransform;
            float yRatio = 15, zRatio = 10;
            Image img = two.transform.GetChild(1).GetComponent<Image>(), img2 = one.transform.GetChild(1).GetComponent<Image>();
            Sprite before = img.sprite;
            img.sprite = Resources.Load<Sprite>("random");
            img.rectTransform.sizeDelta = Vector2.one * 64;
            Vector3 bf = img.rectTransform.eulerAngles;
            img.rectTransform.eulerAngles = Vector3.zero;
            img2.rectTransform.eulerAngles = bf;


            one.value = fm.VertEB / yRatio;
            two.value = eb.localRotation.z / zRatio;

            two.onValueChanged.AddListener((float val) =>
            {
                eb.localRotation = Quaternion.Euler(0, eb.localRotation.y, val * zRatio);
                eb2.localRotation = Quaternion.Euler(0, eb2.localRotation.y, val * zRatio);

                //    hair.sizeDelta = new Vector2(hair.sizeDelta.x, ratios[3] + val * ratios[1]);
            });
            one.onValueChanged.AddListener((float val) =>
            {
                fm.VertEB = val * yRatio;


                //    hair.sizeDelta = new Vector2(ratios[2] + val * ratios[0], hair.sizeDelta.y);
            });

            float origZ = eb.localRotation.z;
            float origVert = fm.VertEB;

            x.onClick.AddListener(() =>
           {
               img.sprite = before;
               img.rectTransform.sizeDelta = Vector2.one * 80.99f;
               img.rectTransform.eulerAngles = bf;
               img2.rectTransform.eulerAngles = Vector3.zero;
               eb.localRotation = Quaternion.Euler(0, eb.localRotation.y, origZ);
               eb2.localRotation = Quaternion.Euler(0, eb2.localRotation.y, origZ);
               fm.VertEB = origVert;


           });
        }

        else if (i == itemType.b_ngs)
        {
            Dictionary<string, int[]> limits = new Dictionary<string, int[]>(){
                {"b_1", new int[]{72,200 } } ,//ratio x, ratio y, original x, original y
                 {"b_2", new int[]{ 30, 200} } ,
                 {"b_3", new int[]{ 150,150} } ,
                 {"b_4", new int[]{ 50,278 } } ,
                 {"b_5", new int[]{ 50,200} } ,
                 {"b_6", new int[]{ 60,200} } ,
                 {"b_7", new int[]{ 70,308} } ,
                 {"b_8", new int[]{ 50,190} } ,
                 {"b_9", new int[]{ 62,200} } ,
                 {"b_91", new int[]{ 90,200} } ,
                  {"b_92", new int[]{ 65,200} } ,
            };

            RectTransform hair = fs.set.rectTransform;
            if (!limits.ContainsKey(fs.set.sprite.name)) return;
            int[] ratios = limits[fs.set.sprite.name];
            float w = fs.set.sprite.rect.width, h = fs.set.sprite.rect.height;

            one.value = (hair.sizeDelta.x - w) / ratios[0];
            two.value = (hair.sizeDelta.y - h) / ratios[1];

            Vector2 orig = hair.sizeDelta;
            two.onValueChanged.AddListener((float val) =>
            {
                hair.sizeDelta = new Vector2(hair.sizeDelta.x, h + val * ratios[1]);
            });
            one.onValueChanged.AddListener((float val) =>
            {
                hair.sizeDelta = new Vector2(w + val * ratios[0], hair.sizeDelta.y);
            });

            x.onClick.AddListener(() =>
           {
               hair.sizeDelta = orig;
           });
        }
        else if (i == itemType.iris)
        {

            float ratio = 25;
            RectTransform iris = fs.set.rectTransform, iris2 = fs.set2.rectTransform;

            one.value = (iris.sizeDelta.x - 120.2f) / ratio;

            Vector2 orig = iris.sizeDelta;
            two.gameObject.SetActive(false);

            one.onValueChanged.AddListener((float val) =>
            {
                iris.sizeDelta = new Vector2(120.2f + ratio * val, 107.8f + ratio * val);
                iris2.sizeDelta = iris.sizeDelta;
            });


            one.transform.GetChild(1).eulerAngles = new Vector3(0, 0, 45);
            string ltRT, rtRT;
            switch (PlayerPrefs.GetInt("Lang"))
            {
                case 1:
                    //chinese
                    ltRT = "左"; rtRT = "右";
                    break;
                case 2:
                    //ja
                    ltRT = "左"; rtRT = "右";
                    break;
                case 3:
                    //rus
                    ltRT = "слева"; rtRT = "направо";
                    break;
                case 4:
                    //thai
                    ltRT = "izquierda"; rtRT = "derecho";
                    break;
                case 5:
                    //thai
                    ltRT = "ไปทางซ้าย"; rtRT = "ทางขวา";
                    break;
                case 6:
                    //thai
                    ltRT = "gauche"; rtRT = "droite";
                    break;
                default:
                    //english
                    ltRT = "left"; rtRT = "right";
                    break;
            }

            lr.fill2MoreButtons(ltRT, rtRT, () => { fs.set.color = cpa.Color; },
             () => { fs.set2.color = cpa.Color; }, x, null);

            lr.imgs[2].transform.SetAsLastSibling();
            lr.imgs[3].transform.SetAsLastSibling();

            lftA = () =>
            {
                TurnThis(cpaCG, true);
                TurnThis(arrCG, false);
                lr.imgs[2].gameObject.SetActive(true);
                lr.imgs[3].gameObject.SetActive(true);
            };

            rtA = () =>
            {
                TurnThis(cpaCG, false);
                TurnThis(arrCG, true);
                lr.imgs[2].gameObject.SetActive(false);
                lr.imgs[3].gameObject.SetActive(false);
            };

            x.onClick.AddListener(() =>
           {
               two.gameObject.SetActive(true);
               one.transform.GetChild(1).eulerAngles = Vector3.zero;
               iris.sizeDelta = orig;
               iris2.sizeDelta = orig;
           });

        }


        lr.exclusiveButtons(lt, rt, lftA,
       rtA,
         x
        );

        x.onClick.AddListener(() =>
        {
            lr.gameObject.SetActive(false);
            TurnCanvas(lr.transform, false);
            TurnThis(cpaCG, true);
            TurnThis(arrCG, true);
            one.transform.parent.gameObject.SetActive(false);

        });

    }

    void handleParticle(ColorPicker cpa, string smth, DressManager dm)
    {
        if (isEquipped())
        {
            cpa.Color = GameObject.FindGameObjectWithTag("Finish").transform.Find(smth).GetComponent<ParticleSystem>().main.startColor.colorMin;
            cpa.Reset();
        }
        else
        {
            dm.x.onClick.AddListener(() =>
            {
                string key = smth.Substring(0, 2);
                dm.fm.Remove(key);
            });
        }
    }

    void handleBackground(ColorPicker cpa, UndoInfo fs, Button x)
    {
        Iris lr = cpa.getLeftRight();
        TurnCanvas(lr.transform, true);
        string lt, rt;
        switch (PlayerPrefs.GetInt("Lang"))
        {
            case 1:
                //chinese
                lt = "模式"; rt = "背景";
                break;
            case 2:
                //ja
                lt = "模様"; rt = "背景";
                break;
            case 3:
                //rus
                lt = "шаблон"; rt = "сцена";
                break;
            case 4:
                //spanish
                lt = "patrón"; rt = "ambiente";
                break;
            case 5:
                //thai
                lt = "แบบแผน"; rt = "พื้นหลัง";
                break;
            case 6:
                //french
                lt = "modèle"; rt = "scène";
                break;
            default:
                //english
                lt = "pattern"; rt = "bg";
                break;
        }

        lr.fillButtons(lt, rt,
        () => { fs.set.color = cpa.Color; },
         () => { Camera.main.backgroundColor = cpa.Color; }
        );
        cpa.Color = fs.set.color;
        Color patern = fs.set.color;
        x.onClick.AddListener(() =>
        {
            fs.set.color = patern;
            lr.gameObject.SetActive(false);
            TurnCanvas(lr.transform, false);

        });
        cpa.gameObject.SetActive(true);
        cpa.Reset();
    }



    void handleSeparateColors(ColorPicker cpa, UndoInfo fs, Button x)
    {
        Iris lr = cpa.getLeftRight();
        TurnCanvas(lr.transform, true);
        string lt, rt;
        switch (PlayerPrefs.GetInt("Lang"))
        {
            case 1:
                //chinese
                lt = "左"; rt = "右";
                break;
            case 2:
                //ja
                lt = "左"; rt = "右";
                break;
            case 3:
                //rus
                lt = "слева"; rt = "направо";
                break;
            case 4:
                //thai
                lt = "izquierda"; rt = "derecho";
                break;
            case 5:
                //thai
                lt = "ไปทางซ้าย"; rt = "ทางขวา";
                break;
            case 6:
                //thai
                lt = "gauche"; rt = "droite";
                break;
            default:
                //english
                lt = "left"; rt = "right";
                break;
        }

        lr.fillButtons(lt, rt,
        () => { fs.set.color = cpa.Color; },
         () => { fs.set2.color = cpa.Color; }
        );
        cpa.Color = fs.set.color;
        x.onClick.AddListener(() =>
        {
            lr.gameObject.SetActive(false);
            TurnCanvas(lr.transform, false);

        });
        cpa.gameObject.SetActive(true);
        cpa.Reset();
    }

    void handleSlidersOnly(ColorPicker cpa, DressManager dm, UndoInfo fs, itemType i)
    {
        Slider[] temp = getSliders(cpa);
        Slider one = temp[0];
        Slider two = temp[1];
        one.onValueChanged.RemoveAllListeners();
        two.onValueChanged.RemoveAllListeners();

        dm.x.onClick.AddListener(() =>
        {
            one.transform.parent.gameObject.SetActive(false);
        });


        if (i == itemType.n_se)
        {
            float ratio = 20;
            float ratio2 = 35f;

            one.transform.GetChild(1).eulerAngles = new Vector3(0, 0, 45);

            one.value = (fs.set.rectTransform.sizeDelta.y - 304) / ratio2;
            two.value = dm.fm.VertNose / ratio;

            Vector2 origSize = fs.set.rectTransform.sizeDelta;
            float VertNose = dm.fm.VertNose;

            dm.x.onClick.AddListener(() =>
            {
                one.transform.GetChild(1).eulerAngles = Vector3.zero;
                fs.set.rectTransform.sizeDelta = origSize;
                dm.fm.VertNose = VertNose;

            });

            one.onValueChanged.AddListener((float val) =>
            {
                fs.set.rectTransform.sizeDelta = new Vector2(349 + ratio2 * val, 304 + ratio2 * val);
            });
            two.onValueChanged.AddListener((float val) =>
            {
                dm.fm.VertNose = ratio * val;
            });
        }
        else if (i == itemType.GXlitch)
        {
            float ratio = 0.4f;
            float ratio2 = 0.2f;
            Glitch ag = Camera.main.GetComponent<Glitch>();

            one.value = ag.colorDrift;
            two.value = ag.verticalJump / ratio2;

            one.onValueChanged.AddListener((float val) =>
            {
                ag.colorDrift = val;
                ag.scanLineJitter = val * ratio;
            });
            two.onValueChanged.AddListener((float val) =>
            {
                ag.verticalJump = val * ratio2;
            });



            if (!isEquipped())
            {
                dm.x.onClick.AddListener(() =>
                    {
                        dm.fm.Remove("GX");
                    });
            }
            else
            {
                float cD = ag.colorDrift;
                float vJ = ag.verticalJump;
                dm.x.onClick.AddListener(() =>
                    {
                        ag.colorDrift = cD;
                        ag.verticalJump = vJ;
                    });
            }
        }
        else if (i == itemType.CHIN)
        {
            float ratio2 = 25f;
            one.gameObject.SetActive(false);
            RectTransform chin = dm.fm.skin[8].rectTransform;

            two.value = (chin.sizeDelta.y - 233) / ratio2;

            Vector2 origSize = chin.sizeDelta;
            dm.x.onClick.AddListener(() =>
            {
                one.gameObject.SetActive(true);
                chin.sizeDelta = origSize;

            });
            two.onValueChanged.AddListener((float val) =>
            {
                chin.sizeDelta = new Vector2(chin.sizeDelta.x, 233 + ratio2 * val);
            });

        }
        else if (i == itemType.BY_ODY)
        {

            float ratio = 30;
            float ratio2 = 35f;

            RectTransform body = dm.fm.skin[4].rectTransform;

            one.value = (body.sizeDelta.x - 1240) / ratio2;
            two.value = (body.sizeDelta.y - 866) / ratio;

            Vector2 origSize = body.sizeDelta;

            dm.x.onClick.AddListener(() =>
            {
                body.sizeDelta = origSize;
            });

            one.onValueChanged.AddListener((float val) =>
            {
                body.sizeDelta = new Vector2(1240 + ratio * val, body.sizeDelta.y);
            });
            two.onValueChanged.AddListener((float val) =>
            {
                body.sizeDelta = new Vector2(body.sizeDelta.x, 823 + ratio2 * val);
            });

        }
        else if (i == itemType.HD)
        {

            float ratio = 30;
            float ratio2 = 35f;

            RectTransform head = dm.fm.skin[1].rectTransform;

            one.value =(head.sizeDelta.x - 750) / ratio2;
            two.value =  (head.sizeDelta.y - 692) / ratio;

            Vector2 origSize = head.sizeDelta;

            dm.x.onClick.AddListener(() =>
            {
                head.sizeDelta = origSize;
            });

            one.onValueChanged.AddListener((float val) =>
            {
                head.sizeDelta = new Vector2(750 + ratio * val, head.sizeDelta.y);
            });
            two.onValueChanged.AddListener((float val) =>
            {
                head.sizeDelta = new Vector2(head.sizeDelta.x, 692 + ratio2 * val);
            });

        }
        else if (i == itemType.eyelid)
        {
            float ratio = 10f;
            float ratio2 = 14f;
            // 20- 48

            Slider s = getOpacitySlider(cpa.transform);
            s.value = fs.set.color.a;
            TurnCanvas(s.transform, true);
            s.onValueChanged.AddListener((float val) =>
            {
                Color skin = fs.set.color;
                fs.set.color = new Color(skin.r, skin.g, skin.b, val);
                fs.set2.color = fs.set.color;
            });




            Image oneI = one.transform.GetChild(1).GetComponent<Image>();
            Sprite before = oneI.sprite;
            oneI.sprite = Resources.Load<Sprite>("random");
            oneI.rectTransform.sizeDelta = Vector2.one * 64;
            float origY = fs.set.rectTransform.anchoredPosition.y;

            one.value = fs.set.rectTransform.localRotation.z / ratio;
            two.value = (fs.set.rectTransform.anchoredPosition.y - 34f) / ratio2;

            Quaternion origZ = fs.set.rectTransform.localRotation;

            dm.x.onClick.AddListener(() =>
            {
                s.gameObject.SetActive(false);
                TurnCanvas(s.transform, false);
                oneI.sprite = before;
                oneI.rectTransform.sizeDelta = Vector2.one * 80.99f;
                fs.set.rectTransform.localRotation = origZ;
                fs.set2.rectTransform.localRotation = origZ;

                fs.set.rectTransform.anchoredPosition = new Vector2(fs.set.rectTransform.anchoredPosition.x, origY);

                fs.set2.rectTransform.anchoredPosition = new Vector2(fs.set2.rectTransform.anchoredPosition.x, origY);

            });


            one.onValueChanged.AddListener((float val) =>
            {
                fs.set.rectTransform.localRotation = Quaternion.Euler(0, 0, ratio * val);
                fs.set2.rectTransform.localRotation = Quaternion.Euler(0, 0, ratio * val);
            });
            two.onValueChanged.AddListener((float val) =>
            {
                fs.set.rectTransform.anchoredPosition = new Vector2(fs.set.rectTransform.anchoredPosition.x, 34f + ratio2 * val);
                fs.set2.rectTransform.anchoredPosition = new Vector2(fs.set2.rectTransform.anchoredPosition.x, 34f + ratio2 * val);
            });
        }




    }

}
