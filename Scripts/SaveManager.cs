using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Kino;
using SoftMasking;

public class SaveManager : MonoBehaviour
{


    public GameObject SaveProfile, LoadProfile, NewProfile, NoteEditor;

    private LoadUI lu; private NotesUI nu; private portraitUI pu;

    private void Start()
    {
        populateButtons();
    }

    void populateButtons()
    {
        Color32 prev = Color.white;
        prev = new Color32((byte)PlayerPrefs.GetInt("themeR"), (byte)PlayerPrefs.GetInt("themeG"), (byte)PlayerPrefs.GetInt("themeB"), 255);

        int n = PlayerPrefs.GetInt("SAVEn");
        for (int i = 0; i < n; i++)
        {
            string val = PlayerPrefs.GetString("SAVE" + i.ToString());
            if (!string.IsNullOrEmpty(val))
            {
                int temp = i;
                Button b = InstantiateButton(val, temp);

                ColorBlock cb = b.colors;
                cb.normalColor = prev;
                cb.disabledColor = prev;
                cb.highlightedColor = prev;
                b.colors = cb;

            }
        }
    }


    Button InstantiateButton(string val, int i)
    {
        Button b = GameObject.Instantiate(SaveProfile, transform, false).GetComponent<Button>();
        TextMeshProUGUI title = b.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        title.text = val;
        b.onClick.AddListener(() => LoadNotify(i, b));
        return b;
    }

    public void SaveNotify()
    {
        int n = getNext();
        if (pu == null)
        {

            pu = GameObject.Instantiate(NewProfile, transform.root, false).GetComponent<portraitUI>();


        }
        if (nu == null)
        {
            nu = GameObject.Instantiate(NoteEditor, transform.root, false).GetComponent<NotesUI>();

        }
        else
        {
            nu.transform.SetAsLastSibling();
        }
        pu.TurnOn(this, nu, n);
        transform.root.GetChild(0).GetComponent<CanvasGroup>().interactable = false;

    }



    public void Save(TMP_InputField t, int n, int overwrite = -1)
    {
        n = (overwrite == -1) ? getNext() : n;
        DressManager dm = transform.root.GetComponent<DressManager>();
        dm.load.startLoading(true);


        string num = n.ToString();
        string key = "SAVE" + num;
        if (overwrite == -1)
        {
            Debug.LogWarning("saving at " + key);
            PlayerPrefs.SetString(key, t.text);
        }


        //save all face things with key + "face", key + "bh"; etc




        foreach (var i in Enum.GetValues(typeof(itemType)))
        {
            itemType it = (itemType)i;
            string keyItem = i.ToString().Substring(0, 2);
            if (keyItem == "CH")
            {
                Vector2 chin = dm.fm.skin[8].rectTransform.sizeDelta;
                PlayerPrefs.SetString(num + keyItem, chin.y + "");
                continue;
            }
            else if (keyItem == "BY")
            {
                Vector2 body = dm.fm.skin[4].rectTransform.sizeDelta;
                PlayerPrefs.SetString(num + keyItem, body.x + "|" + body.y);
                continue;
            }
            else if (keyItem == "HD")
            {
                Vector2 head = dm.fm.skin[1].rectTransform.sizeDelta;
                PlayerPrefs.SetString(num + keyItem, head.x + "|" + head.y);
                continue;
            }
            if (dm.fm.XtraStuff.ContainsKey(keyItem))
            {
                string colors;
                string name;
                if (dm.fm.XtraStuff[keyItem] != null && dm.fm.XtraStuff[keyItem].sprite != null)
                {
                    name = dm.fm.XtraStuff[keyItem].sprite.name;
                }
                else
                {
                    name = " ";
                }
                if (it == itemType.bg)
                {
                    Color32 tempC = Camera.main.backgroundColor;
                    colors = tempC.r + "," + tempC.g + "," + tempC.b;
                    if (name != " " && name[2] == 'p')
                    {
                        tempC = dm.fm.XtraStuff[keyItem].color;
                        colors += "|" + tempC.r + "," + tempC.g + "," + tempC.b;
                    }
                }
                else if (it == itemType.GXlitch)
                {
                    Glitch g = Camera.main.GetComponent<Glitch>();
                    colors = g.colorDrift + "|" + g.verticalJump;
                }
                else if (it == itemType.CXolor)
                {
                    ColorFX g = Camera.main.GetComponent<ColorFX>();
                    Color32 tempC = g.color;
                    colors = tempC.r + "," + tempC.g + "," + tempC.b + "|" + g.Amount;
                }
                else if (it == itemType.BNry)
                {
                    Binary g = Camera.main.GetComponent<Binary>();
                    Color32 tempC = g.color0;
                    Color32 tempC2 = g.color1;
                    colors = tempC.r + "," + tempC.g + "," + tempC.b + "|" + tempC2.r + "," + tempC2.g + "," + tempC2.b + "|" + g.Opacity;
                }
                else if (it == itemType.RPamp)
                {
                    Ramp g = Camera.main.GetComponent<Ramp>();
                    Color32 tempC = g.FirstColor;
                    Color32 tempC2 = g.SecondColor;
                    colors = tempC.r + "," + tempC.g + "," + tempC.b + "|" + tempC2.r + "," + tempC2.g + "," + tempC2.b + "|" + g._opacity;
                }
                else if (it == itemType.BMlm)
                {
                    Bloom g = Camera.main.GetComponent<Bloom>();
                    colors = g.intensity + "|" + g.radius;
                }
                else
                {
                    Color32 tempC;
                    if (keyItem.ToLower() == "pa")
                    {
                        Transform ps = GameObject.FindGameObjectWithTag("Finish").transform.Find(it.ToString());
                        if (ps == null) continue;
                        tempC = ps.GetComponent<ParticleSystem>().main.startColor.colorMin;
                        colors = tempC.r + "," + tempC.g + "," + tempC.b;
                    }
                    else if (keyItem == "wf")
                    {
                        tempC = dm.fm.XtraStuff[keyItem].GetComponent<WaterfallScript>().LightColor;
                        colors = tempC.r + "," + tempC.g + "," + tempC.b;
                    }
                    else if (keyItem == "FR")
                    {
                        FourGradient fg = dm.fm.bangs.GetComponent<FourGradient>();
                        tempC = fg._Color1;
                        colors = tempC.r + "," + tempC.g + "," + tempC.b + "|";
                        tempC = fg._Color2;
                        colors += tempC.r + "," + tempC.g + "," + tempC.b + "|";
                        tempC = fg._Color3;
                        colors += tempC.r + "," + tempC.g + "," + tempC.b + "|";
                        tempC = fg._Color4;
                        colors += tempC.r + "," + tempC.g + "," + tempC.b + "|";
                        colors += fg.opacity.ToString();
                    }
                    else
                    {
                        tempC = dm.fm.XtraStuff[keyItem].color;

                        colors = tempC.r + "," + tempC.g + "," + tempC.b;

                        if (keyItem == "bh")
                        {
                            Vector2 size = dm.fm.XtraStuff[keyItem].rectTransform.sizeDelta;
                            colors += "|" + size.x + "," + size.y;
                        }
                        else if (keyItem == "b_")
                        {
                            Vector2 size = dm.fm.XtraStuff[keyItem].rectTransform.sizeDelta;
                            colors += "|" + size.x + "," + size.y;
                        }
                        else if (keyItem == "l_")
                        {
                            Vector2 size = dm.fm.XtraStuff[keyItem].rectTransform.sizeDelta;
                            colors += "|" + size.x + "," + size.y + "|" + dm.fm.VertLip;
                        }
                        else if (keyItem == "eb")
                        {
                            colors += "|" + dm.fm.XtraStuff[keyItem].rectTransform.eulerAngles.z + "|" + dm.fm.VertEB;
                        }
                        else if (keyItem == "e_")
                        {
                            RectTransform r = dm.fm.XtraStuff[keyItem].rectTransform;
                            colors += "|" + r.sizeDelta.x + "," + r.sizeDelta.y + "|" + r.eulerAngles.z;

                        }
                        else if (keyItem == "ey")
                        {

                            RectTransform r2 = dm.fm.skin[5].rectTransform;
                            colors += "|" + r2.anchoredPosition.y + "|" + r2.eulerAngles.z;
                        }

                        else if (dm.fm.XtraStuff.ContainsKey(keyItem + "2"))
                        {
                            Image im2 = dm.fm.XtraStuff[keyItem + "2"];
                            if (im2 != null)
                            {
                                tempC = im2.color;
                                colors += "|" + tempC.r + "," + tempC.g + "," + tempC.b;
                                if (keyItem == "ir")
                                {
                                    colors += "|" + dm.fm.leftE[1].rectTransform.sizeDelta.x + "," + dm.fm.leftE[1].rectTransform.sizeDelta.y;
                                }//iris.sizeDelta = new Vector2(120.2f + ratio * val, 107.8f + ratio * val);
                            }
                        }




                    }
                }

                string val = name + "|" + colors;
                PlayerPrefs.SetString(num + keyItem, val);
                Debug.LogWarning("save" + num + keyItem + " as: " + val);


            }
        }
        PlayerPrefs.SetFloat(num + "HE", dm.fm.HorzEye);
        PlayerPrefs.SetFloat(num + "VE", dm.fm.VertEye);
        PlayerPrefs.SetFloat(num + "VN", dm.fm.VertNose);
        PlayerPrefs.SetFloat(num + "SN", dm.fm.skin[0].rectTransform.sizeDelta.y - 304);
        // fs.set.rectTransform.sizeDelta = new Vector2(349 + ratio2 * val, 304 + ratio2 * val);
        //    public float HorzEye, VertEye, VertNose, noseSize
        //   Color32 c = dm.fm.leftE[1].color;
        // PlayerPrefs.SetString(num + "lI", c.r + "," + c.g + "," + c.b);
        // c = dm.fm.rightE[1].color;
        // PlayerPrefs.SetString(num + "rI", c.r + "," + c.g + "," + c.b);
        // c = dm.fm.leftE[0].color;
        // PlayerPrefs.SetString(num + "W", c.r + "," + c.g + "," + c.b);
        // c = dm.fm.rightE[0].color;
        // PlayerPrefs.SetString(num + "Wr", c.r + "," + c.g + "," + c.b);
        Color32 c = dm.fm.skin[1].color;
        PlayerPrefs.SetString(num + "S", c.r + "," + c.g + "," + c.b);


        /*
        
          hm.set = leftE[1];
                        hm.set2 = rightE[1];
                        hm.before = hm.set.sprite;
                        break;
                    case "whites":
                        hm.set = leftE[0];
                        hm.set2 = rightE[0];
         */



        if (overwrite == -1)
        {
            Button b = InstantiateButton(PlayerPrefs.GetString(key), n);

            Color32 prev = new Color32((byte)PlayerPrefs.GetInt("themeR"), (byte)PlayerPrefs.GetInt("themeG"), (byte)PlayerPrefs.GetInt("themeB"), 255);
            ColorBlock cb = b.colors;
            cb.normalColor = prev;
            cb.disabledColor = prev;
            b.colors = cb;

            setNext(n);
        }
        PlayerPrefs.Save();
        dm.load.StopLoading();

    }

    int getNext()
    {
        int max = PlayerPrefs.GetInt("SAVEn");
        for (int i = 0; i < max; i++)
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("SAVE" + i.ToString())))
            {
                return i;
            }
        }
        return max;
    }
    void setNext(int n)
    {
        if (n == PlayerPrefs.GetInt("SAVEn"))
            PlayerPrefs.SetInt("SAVEn", n + 1);

    }


    public void LoadNotify(int p, Button caller)
    {
        if (lu == null)
        {

            lu = GameObject.Instantiate(LoadProfile, transform.root, false).GetComponent<LoadUI>();


        }
        if (nu == null)
        {
            nu = GameObject.Instantiate(NoteEditor, transform.root, false).GetComponent<NotesUI>();

        }
        else
        {
            nu.transform.SetAsLastSibling();
        }
        lu.TurnOn(p, this, nu, caller);
        transform.root.GetChild(0).GetComponent<CanvasGroup>().interactable = false;
    }

    public void Delete(int p)
    {

        Debug.LogWarning("deleting at .." + "SAVE" + p.ToString());
        PlayerPrefs.DeleteKey("SAVE" + p.ToString());
        PlayerPrefs.DeleteKey(p + "NOTES");

        DeleteSubstance(p);
        PlayerPrefs.Save();
    }

    public void DeleteSubstance(int p)
    {


        foreach (var i in Enum.GetValues(typeof(itemType)))
        {
            string keyItem = i.ToString().Substring(0, 2);
            PlayerPrefs.DeleteKey(p + keyItem);
        }
        string[] otherKeys = new string[]{
            "HE", "VE", "VN", "SN", "lI", "rI", "W","Wr", "S"
        };
        foreach (string s in otherKeys)
        {
            PlayerPrefs.DeleteKey(p + s);
        }
        /*


            PlayerPrefs.SetFloat(num + "HE", dm.fm.HorzEye);
                PlayerPrefs.SetFloat(num + "VE", dm.fm.VertEye);
                PlayerPrefs.SetFloat(num + "VN", dm.fm.VertNose);
                PlayerPrefs.SetFloat(num + "SN", dm.fm.skin[0].rectTransform.sizeDelta.y - 304);
                // fs.set.rectTransform.sizeDelta = new Vector2(349 + ratio2 * val, 304 + ratio2 * val);
                //    public float HorzEye, VertEye, VertNose, noseSize
                Color32 c = dm.fm.leftE[1].color;
                PlayerPrefs.SetString(num + "lI", c.r + "," + c.g + "," + c.b);
                c = dm.fm.rightE[1].color;
                PlayerPrefs.SetString(num + "rI", c.r + "," + c.g + "," + c.b);
                c = dm.fm.leftE[0].color;
                PlayerPrefs.SetString(num + "W", c.r + "," + c.g + "," + c.b);
                c = dm.fm.rightE[0].color;
                PlayerPrefs.SetString(num + "Wr", c.r + "," + c.g + "," + c.b);
                c = dm.fm.skin[1].color;
                PlayerPrefs.SetString(num + "S", c.r + "," + c.g + "," + c.b);


         */

    }


    public void Load(int profile)
    {
        StartCoroutine(Loading(profile));
    }
    IEnumerator Loading(int profile)
    {



        //load all face things with key + "face", key + "bh"; etc
        DressManager dm = transform.root.GetComponent<DressManager>();
        dm.load.startLoading(false);
        dm.LoadAll();
        yield return null;
        while (dm.load.loaddd)
        {
            yield return null;
        }
        foreach (var i in Enum.GetValues(typeof(itemType)))
        {
            itemType it = (itemType)i;
            string keyItem = i.ToString().Substring(0, 2);
            if (PlayerPrefs.HasKey(profile + keyItem))
            {
                string full = PlayerPrefs.GetString(profile + keyItem);
                Debug.LogWarning("load " + profile + keyItem + ":" + full);
                string[] halves = full.Split(new string[] { "|", "," }, StringSplitOptions.RemoveEmptyEntries);
                Sprite s = dm.getSpriteFromString(it, halves[0]);

                UndoInfo hm = dm.fm.faceSet(s, it);

                if (keyItem == "CH")
                {
                    dm.fm.skin[8].rectTransform.sizeDelta = new Vector2(dm.fm.skin[8].rectTransform.sizeDelta.x, float.Parse(halves[0]));
                   // PlayerPrefs.GetString(profile + keyItem, chin.y + "");
                    continue;
                }
                else if (keyItem == "BY")
                {
                     dm.fm.skin[4].rectTransform.sizeDelta = new Vector2(float.Parse(halves[0]), float.Parse(halves[1]));
                  
                  ///  PlayerPrefs.SetString(num + keyItem, body.x + "|" + body.y);
                    continue;
                }
                else if (keyItem == "HD")
                {
                    dm.fm.skin[1].rectTransform.sizeDelta = new Vector2(float.Parse(halves[0]), float.Parse(halves[1]));
                  
                 //   PlayerPrefs.SetString(num + keyItem, head.x + "|" + head.y);
                    continue;
                }

                if (it == itemType.GXlitch)
                {
                    Glitch g = Camera.main.GetComponent<Glitch>();
                    g.enabled = true;
                    g.colorDrift = float.Parse(halves[1]);
                    g.verticalJump = float.Parse(halves[2]);
                    float ratio = 0.4f;
                    g.scanLineJitter = g.colorDrift * ratio;
                    continue;
                }
                if (it == itemType.CXolor)
                {
                    ColorFX g;
                    g = Camera.main.gameObject.GetComponent<ColorFX>();
                    g.enabled = true;
                    g.color = convertAt(halves, 1, g.color);
                    g.Amount = float.Parse(halves[4]);
                    g.updateColor();

                    continue;
                }
                if (it == itemType.BNry)
                {
                    Binary g;
                    g = Camera.main.gameObject.GetComponent<Binary>();
                    g.enabled = true;
                    g.color0 = convertAt(halves, 1, g.color0);
                    g.color1 = convertAt(halves, 4, g.color1);
                    g.Opacity = float.Parse(halves[7]);
                    g.updateColor();
                    continue;
                }
                if (it == itemType.RPamp)
                {
                    Ramp g;
                    g = Camera.main.gameObject.GetComponent<Ramp>();
                    g.enabled = true;
                    g.FirstColor = convertAt(halves, 1, g.FirstColor);
                    g.SecondColor = convertAt(halves, 4, g.SecondColor);
                    g._opacity = float.Parse(halves[7]);
                    g.updateColors();
                    continue;
                }
                if (it == itemType.BMlm)
                {
                    Bloom g = Camera.main.gameObject.GetComponent<Bloom>();
                    g.enabled = true;
                    g.intensity = float.Parse(halves[1]);
                    g.radius = float.Parse(halves[2]);
                    g.UpdateParam();
                    continue;
                }


                if (keyItem.ToLower() == "pa") { checkParticles(convertAt(halves, 1, Color.white), dm, it); continue; }


                if (it == itemType.bg)
                {
                    Camera.main.backgroundColor = convertAt(halves, 1, Color.white);
                    if (halves[0] == " ")
                    {
                        hm.set.color = Color.clear;
                    }
                    else if (halves[0][2] != 'p')
                    {
                        hm.set.color = new Color(1, 1, 1, 1 - HSBColor.FromColor(Camera.main.backgroundColor).b);
                    }
                    else
                    {
                        hm.set.color = convertAt(halves, 4, hm.set.color);
                    }
                }
                else if (keyItem == "wf")
                {
                    hm.set.GetComponent<WaterfallScript>().LightColor = convertAt(halves, 1, hm.set.color);
                }
                else if (keyItem == "FR")
                {
                    FourGradient fg = dm.fm.bangs.GetComponent<FourGradient>();
                    if (fg == null)
                    {
                        dm.fm.bangs.GetComponent<SoftMask>().enabled = false;
                        dm.fm.bangs.transform.GetChild(0).gameObject.SetActive(false);
                        fg = dm.fm.bangs.gameObject.AddComponent<FourGradient>();
                        dm.fm.hair.material = dm.fm.bangs.materialForRendering;
                    }
                    int index = 1;
                    fg._Color1 = convertAt(halves, index, fg._Color1);
                    index += 3;
                    fg._Color2 = convertAt(halves, index, fg._Color2);
                    index += 3;
                    fg._Color3 = convertAt(halves, index, fg._Color3);
                    index += 3;
                    fg._Color4 = convertAt(halves, index, fg._Color4);
                    index += 3;
                    fg.opacity = float.Parse(halves[index]);
                    fg.updateParams();
                }
                else
                {
                    if (hm.set != null)
                    {

                        hm.set.color = convertAt(halves, 1, hm.set.color);
                        hm.set.gameObject.SetActive(true);

                        if (keyItem == "bh" && halves.Length > 4)
                        {
                            Vector2 size = new Vector2(float.Parse(halves[4]), float.Parse(halves[5]));
                            dm.fm.XtraStuff[keyItem].rectTransform.sizeDelta = size;
                        }
                        else if (keyItem == "b_" && halves.Length > 4)
                        {
                            Vector2 size = new Vector2(float.Parse(halves[4]), float.Parse(halves[5]));
                            dm.fm.XtraStuff[keyItem].rectTransform.sizeDelta = size;
                        }
                        else if (keyItem == "l_" && halves.Length > 4)
                        {
                            Vector2 size = new Vector2(float.Parse(halves[4]), float.Parse(halves[5]));
                            dm.fm.XtraStuff[keyItem].rectTransform.sizeDelta = size;
                            dm.fm.VertLip = float.Parse(halves[6]);
                        }
                        else if (keyItem == "eb" && halves.Length == 6)
                        {
                            RectTransform eb1 = dm.fm.XtraStuff[keyItem].rectTransform, eb2 = dm.fm.XtraStuff[keyItem + "2"].rectTransform;
                            float z = float.Parse(halves[4]);
                            eb1.localRotation = Quaternion.Euler(0, 0, z);
                            eb2.localRotation = Quaternion.Euler(0, 0, z);
                            dm.fm.VertEB = float.Parse(halves[5]);
                            hm.set2.color = hm.set.color;
                            continue;
                        }
                        else if (keyItem == "e_" && halves.Length > 4)
                        {
                            RectTransform r = dm.fm.XtraStuff[keyItem].rectTransform;
                            Vector2 size = new Vector2(float.Parse(halves[4]), float.Parse(halves[5]));
                            float z = float.Parse(halves[6]);
                            RectTransform e = dm.fm.XtraStuff[keyItem].rectTransform, e2 = dm.fm.XtraStuff[keyItem + "2"].rectTransform;
                            e.sizeDelta = size;
                            e2.sizeDelta = size;
                            e.localRotation = Quaternion.Euler(0, 0, z);
                            e2.localRotation = Quaternion.Euler(0, 0, z);
                            hm.set2.color = hm.set.color;
                            continue;

                        }

                        if (keyItem == "ey" && halves.Length > 4)
                        {
                            Debug.Log("set eyelid");
                            RectTransform r0 = dm.fm.skin[5].rectTransform, r2 = dm.fm.skin[7].rectTransform;
                            r0.anchoredPosition = new Vector2(r0.anchoredPosition.x, float.Parse(halves[4])); r2.anchoredPosition = r0.anchoredPosition;
                            r0.localRotation = Quaternion.Euler(0, r0.localRotation.y, float.Parse(halves[5])); r2.localRotation = Quaternion.Euler(0, r2.localRotation.y, r0.eulerAngles.z);
                            hm.set2.color = hm.set.color;
                            continue;

                        }
                        if (hm.set2 != null)
                        {
                            if (halves.Length > 4)
                            {
                                hm.set2.color = convertAt(halves, 4, hm.set2.color);
                                if (keyItem == "ir" && halves.Length == 9)
                                {   //rs += "|" + dm.fm.leftE[1].rectTransform.sizeDelta.x + "," + dm.fm.leftE[1].rectTransform.sizeDelta.y;
                                    hm.set.rectTransform.sizeDelta = new Vector2(float.Parse(halves[7]), float.Parse(halves[8]));
                                    hm.set2.rectTransform.sizeDelta = hm.set.rectTransform.sizeDelta;
                                }
                                //new Color32(Convert.ToByte(halves[4]), Convert.ToByte(halves[5]), Convert.ToByte(halves[6]), 255);
                            }
                            else
                            {
                                hm.set2.color = hm.set.color;
                            }
                            hm.set2.gameObject.SetActive(true);
                        }

                    }
                }
            }
            else
            {
                dm.fm.Remove(keyItem);
            }
            dm.load.StopLoading();

        }

        // "HE", "VE", "VN", "SN", "lI", "rI", "W", "S"
        //    public float HorzEye, VertEye, VertNose, noseSize


        dm.fm.HorzEye = PlayerPrefs.GetFloat(profile + "HE");
        dm.fm.VertEye = PlayerPrefs.GetFloat(profile + "VE");
        dm.fm.VertNose = PlayerPrefs.GetFloat(profile + "VN");
        float SN = PlayerPrefs.GetFloat(profile + "SN");
        dm.fm.skin[0].rectTransform.sizeDelta = new Vector2(349 + SN, 304 + SN);
        // fs.set.rectTransform.sizeDelta = new Vector2(349 + ratio2 * val, 304 + ratio2 * val);


        string[] halves2 = //PlayerPrefs.GetString(profile + "lI").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                           // dm.fm.leftE[1].color = convertAt(halves2, 0, dm.fm.leftE[1].color);
                           // halves2 = PlayerPrefs.GetString(profile + "rI").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                           // dm.fm.rightE[1].color = convertAt(halves2, 0, dm.fm.rightE[1].color);
                           // halves2 =
                           //  PlayerPrefs.GetString(profile + "W").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                           // dm.fm.leftE[0].color = convertAt(halves2, 0, dm.fm.leftE[0].color);
                           // halves2 = PlayerPrefs.GetString(profile + "Wr").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                           // dm.fm.rightE[0].color = convertAt(halves2, 0, dm.fm.rightE[0].color);



        // halves2 = 
        PlayerPrefs.GetString(profile + "S").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        dm.changeSkin(convertAt(halves2, 0, dm.fm.skin[1].color), Color.white, false);
    }

    Color32 convertAt(string[] halves, int i, Color32 orig)
    {
        if (halves.Length <= i + 2) return orig;
        return new Color32(Convert.ToByte(halves[i]), Convert.ToByte(halves[i + 1]), Convert.ToByte(halves[i + 2]), 255);

    }



    void checkParticles(Color c, DressManager dm, itemType it)
    {
        Debug.Log(c);
        dm.setUpParticles(dm.getSpriteFromString(it), 0, c, true);
    }
}
