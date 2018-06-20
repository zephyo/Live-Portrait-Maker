using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Kino;
using SoftMasking;

using UnityEngine.SceneManagement;


//Manager runs throughout the whole lifetime of the program
//Manages 2.5D facial animation, looking mechanic, and switching character Image sprites
public class FaceManager : MonoBehaviour
{

    public Image bg;
    /*
    Summary of skin array:
	0: nose
	1: head
	2: Leye
	3: Reye
	4: body
	5: Leyelid 
	6: blush
    7: Reyelid
	 */
    public Image[] skin;

    public Image clothes, hair, bangs, lips;
    /*
 Summary of Eye arrays:
 0 eyewhite
 1 iris
 2 brow
  */


    public List<Image> leftE, rightE;

    //sprites used for blinking
    public Sprite half, blink;


    //holds references to removable clothing/wearable items
    public Dictionary<string, Image> XtraStuff;

    public event Action OnSingleTap;
    public event Action OnDoubleTap;

    //defines the maximum time between two taps to make it double tap
    private float tapThreshold = 0.3f;
    public Action updateDelegate;
    private float tapTimer = 0.0f;
    private bool tap = false;

    //defines where the character should look toward, from the from vector
    Vector2 towards, from;

    Coroutine AnimateRoutine;

    public float HorzEye, VertEye, VertNose, VertLip, VertEB;

    public void UnloadDressUp()
    {
        StartCoroutine(unloadDressHelper());
    }

    IEnumerator unloadDressHelper()
    {

        yield return new WaitForSeconds(0.1f);

        setUpDelegates();
        setUpListeners();
    }


    public void Start()
    {

        setUpDelegates();
        XtraStuff = new Dictionary<string, Image>(StringComparer.Ordinal);
        Application.targetFrameRate = 24;
        XtraStuff.Add("b_", bangs);
        XtraStuff.Add("ir", leftE[1]);
        XtraStuff.Add("ir2", rightE[1]);
        XtraStuff.Add("w_", leftE[0]);
        XtraStuff.Add("w_2", rightE[0]);
        XtraStuff.Add("ey", skin[5]);
        XtraStuff.Add("ey2", skin[7]);
        XtraStuff.Add("bh", hair);
        XtraStuff.Add("e_", skin[2]);
        XtraStuff.Add("e_2", skin[3]);
        XtraStuff.Add("eb", leftE[2]);
        XtraStuff.Add("eb2", rightE[2]);
        XtraStuff.Add("l_", lips);
        XtraStuff.Add("n_", skin[0]);
        XtraStuff.Add("bg", bg);
        XtraStuff.Add("bl", skin[6]);
        XtraStuff.Add("t_", clothes);
        XtraStuff.Add("BY", null);
        XtraStuff.Add("HD", null);
        XtraStuff.Add("CHIN", null);

        UnityEngine.Random.InitState(System.Environment.TickCount);

        from = new Vector2(UnityEngine.Random.Range(0, Screen.width),
             UnityEngine.Random.Range(0, Screen.height));
        towards = new Vector2(UnityEngine.Random.Range(0, Screen.width),
             UnityEngine.Random.Range(0, Screen.height));


        doMotion(PlayerPrefs.GetInt("motion") == 1 ? false : true);
        //if player's first time playing, begin tutorial
        if (PlayerPrefs.GetInt("intro") != 1)
        {
            Intro();
        }
        else
        {
            Destroy(transform.GetChild(2).gameObject);
            setUpListeners();
        }
    }


    public void doMotion(bool on)
    {
        if (on)
        {
            AnimateRoutine = StartCoroutine(animate());
        }
        else
        {
            if (AnimateRoutine != null)
            {
                StopCoroutine(AnimateRoutine);
            }
        }
    }

    IEnumerator animate()
    {
        yield return null;

        while (AnimateRoutine != null)
        {
            float time = UnityEngine.Random.Range(1.4f, 4f);
            towards = new Vector2(UnityEngine.Random.Range(0, Screen.width),
             UnityEngine.Random.Range(0, Screen.height));
            Vector2 to = towards;

            for (float t = 0; t < time; t += Time.deltaTime)
            {
                float tt = t / time;
                tt = tt * tt * (3f - 2f * tt);
                towards = Vector2.Lerp(from, to, tt);
                setFaceAtAngle(towards);
                yield return null;
            }

            from = to;
            if (updateDelegate != null && UnityEngine.Random.value > 0.5f)
            {
                StartCoroutine(blinkAnimate());
            }
        }
    }
    //DO NOT CALL THIS WHILE IN SHOP MODE
    IEnumerator blinkAnimate()
    {
        Sprite s = skin[2].sprite;
        if (s == blink || s.name == "e_hpy" || s.name == "e_sq") yield break;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.9f));
        if (updateDelegate == null) yield break;
        skin[2].sprite = half;
        skin[3].sprite = half;
        yield return null;
        skin[2].sprite = blink;
        skin[3].sprite = blink;
        yield return new WaitForSeconds(0.15f);
        skin[2].sprite = half;
        skin[3].sprite = half;
        yield return null;
        skin[2].sprite = s;
        skin[3].sprite = s;
    }
    IEnumerator look()
    {
        float time = UnityEngine.Random.Range(0.4f, 1f);
        from = towards;

        Vector2 to =
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
          Input.mousePosition;
#elif UNITY_IOS || UNITY_ANDROID
Input.GetTouch(0).position;
#endif
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            float tt = t / time;
            tt = tt * tt * (3f - 2f * tt);
            towards = Vector2.Lerp(from, to, tt);
            setFaceAtAngle(towards);
            yield return null;
        }
        yield return new WaitForSeconds(0.7f);
        from = to;
        if (updateDelegate != null && UnityEngine.Random.value > 0.7f)
        {
            StartCoroutine(blinkAnimate());
        }
        doMotion(PlayerPrefs.GetInt("motion") == 1 ? false : true);
    }

    void setFaceAtAngle(Vector2 towards)
    {

        //hair: pos y from 928.7 to 940.7 to 952.7
        hair.rectTransform.anchoredPosition = new Vector2(turnRatio(towards.x, Screen.width, 12, -12), turnRatio(towards.y, Screen.height, 1100.59f, 1040.59f));
        //bangs: rot x from 9.94 to 0
        bangs.rectTransform.eulerAngles = new Vector3(turnRatio(towards.y, Screen.height, 9.94f, 0), 0, 0);
        // bangs.rectTransform.anchoredPosition = new Vector2(hair.rectTransform.anchoredPosition.x / 5, bangs.rectTransform.anchoredPosition.y);

        //change head rot x from 8.99 to 0
        skin[1].rectTransform.eulerAngles = new Vector3(turnRatio(towards.y, Screen.height, 10, 0), 0, turnRatio(towards.x, Screen.width, -0.95f, 0.95f));
        //eyeright poy -32 to -25 to -18 (+VertEye); rox 0 to 15
        rightE[0].rectTransform.anchoredPosition = new Vector2(turnRatio(towards.x, Screen.width, 169.54f + HorzEye, 175.54f + HorzEye), turnRatio(towards.y, Screen.height, -75.5f + VertEye, -62.5f + VertEye));
        rightE[0].rectTransform.eulerAngles = new Vector3(turnRatio(towards.y, Screen.height, 0, 15), turnRatio(towards.x, Screen.width, -15, 15), 0);
        //X -25.1f, 11.8f
        //Y 1.8f, 21.6f
        //RY -18.6f, 18.6f
        //left is like right, but flipped
        rightE[1].rectTransform.anchoredPosition = new Vector2(turnRatio(towards.x, Screen.width, -24.1f, 10.8f), turnRatio(towards.y, Screen.height, 3.8f, 18.6f));
        rightE[1].rectTransform.eulerAngles = new Vector3(0, turnRatio(towards.x, Screen.width, -18.6f, 18.6f), 0);
        rightE[2].rectTransform.anchoredPosition = new Vector2(rightE[2].rectTransform.anchoredPosition.x, turnRatio(towards.y, Screen.height, 99.5f + VertEB, 114.6f + VertEB));
        //eyeleft poy -32 to -25 to -18(+VertEye); rox 0 to 15
        leftE[0].rectTransform.anchoredPosition = new Vector2(turnRatio(towards.x, Screen.width, -180.1f - HorzEye, -174.1f - HorzEye), rightE[0].rectTransform.anchoredPosition.y);
        leftE[0].rectTransform.eulerAngles = new Vector3(rightE[0].rectTransform.eulerAngles.x, turnRatio(towards.x, Screen.width, 195, 165), 0);

        leftE[1].rectTransform.anchoredPosition = new Vector2(turnRatio(towards.x, Screen.width, 10.8f, -24.1f), rightE[1].rectTransform.anchoredPosition.y);
        leftE[1].rectTransform.eulerAngles = new Vector3(0, rightE[1].rectTransform.eulerAngles.y, 0);
        leftE[2].rectTransform.anchoredPosition = new Vector2(leftE[2].rectTransform.anchoredPosition.x, rightE[2].rectTransform.anchoredPosition.y);

        //lips poy -300.5 to -295.5 to -290.5; rox 18 to 0
        //lips pox -10 to 10; roy -15 to 15
        lips.rectTransform.anchoredPosition = new Vector2(turnRatio(towards.x, Screen.width, -5, 5), turnRatio(towards.y, Screen.height, -350f + VertLip, -333f + VertLip)); //-336.5
        lips.rectTransform.eulerAngles = new Vector3(turnRatio(towards.y, Screen.height, 18, 0), turnRatio(towards.x, Screen.width, -15, 15), 0);

        //nose poy from -140.8f, -128.8f; rox from 0 to 16.9
        //nose pox from -7.68 to 0 to 7.68 (+vertnose); roy from -18, 18
        skin[0].rectTransform.anchoredPosition = new Vector2(turnRatio(towards.x, Screen.width, -7.68f, 7.68f), turnRatio(towards.y, Screen.height, -186f + VertNose, -172 + VertNose)); // -175.8146
        skin[0].rectTransform.eulerAngles = new Vector3(turnRatio(towards.y, Screen.height, 0, 16.9f), turnRatio(towards.x, Screen.width, 5, -5), 0);

    }

    float turnRatio(float a, float max, float min2, float max2)
    {
        return a * ((max2 - min2) / max) + min2;
    }


    public void ChangeLook()
    {
        OnSingleTap += () =>
        {
            if (AnimateRoutine != null) StopCoroutine(AnimateRoutine);
            AnimateRoutine = StartCoroutine(look());
        };
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {

            PlayerPrefs.Save();
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }



    void Intro()
    {

        switch (Application.systemLanguage)
        {
            //0: english 1: chinese 2: japaneese 3: russian 4: spanish 5: thai
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseSimplified:
            case SystemLanguage.ChineseTraditional:
                PlayerPrefs.SetInt("Lang", 1);
                break;
            case SystemLanguage.Japanese:
                PlayerPrefs.SetInt("Lang", 2);
                break;
            case SystemLanguage.Russian:
                PlayerPrefs.SetInt("Lang", 3);
                break;
            case SystemLanguage.Spanish:
                PlayerPrefs.SetInt("Lang", 4);
                break;
            case SystemLanguage.Thai:
                PlayerPrefs.SetInt("Lang", 5);
                break;
            case SystemLanguage.French:
                PlayerPrefs.SetInt("Lang", 6);
                break;
            default:
                PlayerPrefs.SetInt("Lang", 0);
                break;


        }
        PlayerPrefs.SetInt("intro", 1);
        PlayerPrefs.SetInt("themeR", 35);
        PlayerPrefs.SetInt("themeG", 35);
        PlayerPrefs.SetInt("themeB", 35);
        PlayerPrefs.Save();

        Intro i = transform.GetChild(2).GetComponent<Intro>();
        i.Init(this);
    }

    public void setUpDelegates()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        updateDelegate = UpdateEditor;
#elif UNITY_IOS
        updateDelegate = UpdateiOS;
#elif UNITY_ANDROID   
        updateDelegate = UpdateAndroid;       
#endif
    }

    void setUpListeners()
    {
        OnDoubleTap = DressUp;
        ChangeLook();
    }

    public void setUpDressListener()
    {
        OnDoubleTap += DressUp;
    }

    void removeListeners()
    {

        OnDoubleTap = null;


        OnSingleTap = null;
    }

    void DressUp()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        removeAll();
    }

    public void removeAll()
    {
        updateDelegate = null;
        removeListeners();
    }
    private void Update()
    {
        if (updateDelegate != null) { updateDelegate(); }

    }
    private void OnDestroy()
    {
        OnSingleTap = null;
        OnDoubleTap = null;
    }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
    private void UpdateEditor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time < this.tapTimer + this.tapThreshold)
            {
                if (OnDoubleTap != null) { OnDoubleTap(); }
                this.tap = false;
                return;
            }
            this.tap = true;
            this.tapTimer = Time.time;
        }
        if (this.tap == true && Time.time > this.tapTimer + this.tapThreshold)
        {
            this.tap = false;
            if (OnSingleTap != null) { OnSingleTap(); }
        }
    }
#elif UNITY_IOS
    private void UpdateiOS ()
    {
        Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                if(t.tapCount == 2)
                {
                    if(OnDoubleTap != null){ OnDoubleTap();}
                }
                if(t.tapCount == 1)
                {
                    if(OnSingleTap != null) { 
                        OnSingleTap(); 
    
                        }
                }
            }
    }

#elif UNITY_ANDROID

    private void UpdateAndroid ()
    {
            Touch t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began)
        {
         
            if (t.tapCount == 2 || Time.time < this.tapTimer + this.tapThreshold)
            {
                if (OnDoubleTap != null) { OnDoubleTap(); }
                this.tap = false;
              return;
              
            }

            this.tap = true;
            this.tapTimer = Time.time;
        }
        if (this.tap == true)
        {
            this.tap = false;
            if (OnSingleTap != null) { OnSingleTap(); }
        }

    }

#endif

    //Remove the image mapped to key - used when undoing and removing
    public void Remove(string key)
    {
        Image x;
        XtraStuff.TryGetValue(key, out x);
        if (x != null)
        {
            Debug.Log("removoing " + key + " at " + x.gameObject.name);

            HashSet<string> dontDelete = new HashSet<string>(){
                            "b_", "ir", "w_", "ey", "bh", "e_", "eb", "l_", "n_", "bg", "BY", "CH", "HD"
            };
            if (dontDelete.Contains(key))
            {
                return;
            }
            if (key != "t_" && key != "bl")
            {
                Destroy(x.gameObject);
                //check if second
                if (XtraStuff.ContainsKey(key + "2"))
                {
                    Destroy(XtraStuff[key + "2"].gameObject);
                    XtraStuff.Remove(key + "2");
                }
                //check if hart
                else if (key == "ha")
                {
                    Transform ps = GameObject.FindGameObjectWithTag("Finish").transform.Find("hartic");
                    if (ps != null) Destroy(ps.gameObject);
                }

            }
            else
            {
                x.gameObject.SetActive(false);
            }
        }
        else if (key == "pa")
        {
            Transform ps = GameObject.FindGameObjectWithTag("Finish").transform.Find("particle_snow");
            if (ps != null) Destroy(ps.gameObject);
        }
        else if (key == "Pa")
        {
            Transform ps = GameObject.FindGameObjectWithTag("Finish").transform.Find("Particle_petal");
            if (ps != null) Destroy(ps.gameObject);
        }
        else if (key == "pA")
        {
            Transform ps = GameObject.FindGameObjectWithTag("Finish").transform.Find("pArticle_sparkle");
            if (ps != null) Destroy(ps.gameObject);
        }
        else if (key == "GX")
        {
            Camera.main.gameObject.GetComponent<Glitch>().enabled = false;
        }
        else if (key == "BM")
        {
            Camera.main.gameObject.GetComponent<Bloom>().enabled = false;
        }
        else if (key == "CX")
        {
            Camera.main.gameObject.GetComponent<ColorFX>().enabled = false;
        }
        else if (key == "BN")
        {
            Camera.main.gameObject.GetComponent<Binary>().enabled = false;
        }
        else if (key == "RP")
        {
            Camera.main.gameObject.GetComponent<Ramp>().enabled = false;
        }
        else if (key == "FR")
        {
            Destroy(bangs.gameObject.GetComponent<FourGradient>());
            hair.material = null;
            bangs.GetComponent<SoftMask>().enabled = true;
            bangs.transform.GetChild(0).gameObject.SetActive(true);
        }
        XtraStuff.Remove(key);

    }
    public UndoInfo faceSet(Sprite newThang, itemType it)
    {
        //key is the first 2 chars. if 2nd, first 2 chars+"2"
        if (newThang != null) Debug.Log("set face with " + newThang.name + " at " + it);
        UndoInfo hm;
        hm.set = null;
        hm.set2 = null;
        hm.before = null;
        hm.beforeC = Color.white;
        hm.beforeC2 = Color.white;
        string key = "";
        if (newThang != null && it != itemType.BMlm)
            key = newThang.name.Substring(0, 2);

        switch (it)
        {
            case itemType.iris:
                hm.set = leftE[1];
                hm.set2 = rightE[1];
                hm.before = hm.set.sprite;

                break;
            case itemType.w_hites:
                hm.set = leftE[0];
                hm.set2 = rightE[0];
                hm.before = hm.set.sprite;
                break;
            case itemType.eyelid:
                hm.set = skin[5];
                hm.set2 = skin[7];
                break;


            case itemType.se:
                        if (!XtraStuff.ContainsKey(key))
                        {

                            hm.set = newImgAt(key, new Vector2(-3f, 7.783465f),
                            leftE[2], null, 1);
                            hm.set2 = newImgAt(key, new Vector2(-3f, 7.783465f),
                            rightE[2], null, 1);

                        }
                        else
                        {
                            hm.set = XtraStuff[key];
                            hm.set2 = XtraStuff[key + "2"];
                            hm.before = hm.set.sprite;
                        }
                break;
            case itemType.Se:
            Debug.Log("add Se");
                if (!XtraStuff.ContainsKey(key))
                {
                    hm.set = newImgAt(key, new Vector2(9.5f, 0.5f),
                    leftE[2], null, 1);
                    hm.set2 = newImgAt(key, new Vector2(-28.2f, 0.5f),
                    rightE[2], null, 1);
                }
                else
                {
                    hm.set = XtraStuff[key];
                    hm.set2 = XtraStuff[key + "2"];
                    hm.before = hm.set.sprite;
                }
                break;
            case itemType.xe:
                        if (!XtraStuff.ContainsKey(key))
                        {
                            hm.set = newImgAt(key, new Vector2(-4.5f, -5.4f),
                            leftE[2], leftE[1].transform, 1);
                            hm.set2 = newImgAt(key, new Vector2(-4.5f, -5.4f),
                            rightE[2], rightE[1].transform, 1);
                        }
                        else
                        {
                            hm.set = XtraStuff[key];
                            hm.set2 = XtraStuff[key + "2"];
                            hm.before = hm.set.sprite;
                        }
                        Material addY = Resources.Load<Material>("Additive");
                        hm.set.material = addY;
                        hm.set2.material = addY;

                break;
            case itemType.BOW:
                int indexbo;
                if (XtraStuff.ContainsKey("ch") && XtraStuff["ch"].sprite.name == "chsf")
                {
                    indexbo = XtraStuff["ch"].transform.GetSiblingIndex();
                }
                else
                {
                    indexbo = clothes.transform.parent.childCount;
                }
                switch (newThang.name)
                {
                    case "BOW":
                        setHm(new Vector2(10.378f, 453f), clothes, null, indexbo,
                    key, ref hm);
                        break;

                    default:
                        setHm(new Vector2(0f, 0f), clothes, null, indexbo,
                   key, ref hm);
                        break;
                }
                break;

            case itemType.chneckwear:
                int index = 0;
                if (newThang.name == "chsf")
                {
                    index = clothes.transform.parent.childCount;
                }
                else if (newThang.name == "choker")
                {
                    index = -1;
                }
                else if (XtraStuff.ContainsKey("BO"))
                {
                    index = XtraStuff["BO"].transform.GetSiblingIndex();
                }

                switch (newThang.name)
                {
                    case "choker":
                        setHm(new Vector2(10.37823f, 531f), clothes, null, index, key, ref hm);
                        break;
                    case "chokerbow":
                        setHm(new Vector2(10.37823f, 12.9f), clothes, null, index, key, ref hm);
                        break;
                    case "chsf":
                        setHm(new Vector2(-26.94f, 3.26f), clothes, null, index, key, ref hm);
                        break;
                }
                break;
            case itemType.glasses:
                setHm(new Vector2(-7.5f, 72.5f), skin[6], null, 0,
                key, ref hm);
                break;
            case itemType.freckles:
                setHm(new Vector2(0f, -81.5f), skin[6], null, skin[0].transform.GetSiblingIndex() + 1,
                key, ref hm);
                break;
            case itemType.starfreckles:
                setHm(new Vector2(0f, -102f), skin[6], null, skin[6].rectTransform.GetSiblingIndex(),
                key, ref hm);
                break;
            case itemType.lippiercing:
                setHm(new Vector2(0f, 32.9f), skin[6], lips.rectTransform, 4,
                key, ref hm);
                break;
            case itemType.sl1:
                setHm(new Vector2(0.55f, 11.1f), skin[6], lips.rectTransform, 0,
                key, ref hm);
                hm.set.material = Resources.Load<Material>("Additive");
                break;
            case itemType.sx_tears:
                setHm(new Vector2(29.7f, -39.86f), rightE[2], null, rightE[0].transform.childCount,
                key, ref hm);
                break;
            case itemType.bubble:
                setHm(new Vector2(-6.151662f, -271.2f), skin[6], null, 0,
                key, ref hm);
                break;
            case itemType.eear:
                int index2;
                if (XtraStuff.ContainsKey("ha"))
                {
                    index2 = XtraStuff["ha"].transform.GetSiblingIndex();
                }
                else if (XtraStuff.ContainsKey("fl"))
                {
                    index2 = XtraStuff["fl"].transform.GetSiblingIndex();
                }
                else if (XtraStuff.ContainsKey("he"))
                {
                    index2 = XtraStuff["he"].transform.GetSiblingIndex();
                }
                else
                {
                    index2 = 0;
                }
                setHmTwice(new Vector2(-328.1f, 570.4f), new Vector2(329.3f, 570.4f),
               skin[6], null, index2, key, ref hm);
                break;
            case itemType.hesidehorn:
                int index3;
                if (XtraStuff.ContainsKey("ha"))
                {
                    index3 = XtraStuff["ha"].transform.GetSiblingIndex();
                }
                else if (XtraStuff.ContainsKey("fl"))
                {
                    index3 = XtraStuff["fl"].transform.GetSiblingIndex();
                }
                else
                {
                    index3 = 0;
                }
                setHmTwice(new Vector2(-328.1f, 566.2f), new Vector2(329.3012f, 566.2f),
               skin[6], null, index3, key, ref hm);
                break;
            case itemType.EPatch:
                setHm(new Vector2(-225.1f, 52.7f), rightE[2], null, 0,
                key, ref hm);
                break;
            case itemType.hdphones:
                setHmTwice(new Vector2(-238.808f, 468.9f), new Vector2(231f, 468.9f),
               skin[6], null, bangs.transform.GetSiblingIndex(), key, ref hm);
                break;
            case itemType.ctetopband:
                int indexCT;
                if (XtraStuff.ContainsKey("un"))
                {
                    indexCT = XtraStuff["un"].transform.GetSiblingIndex();
                }
                else
                {
                    indexCT = skin[6].transform.parent.childCount;
                }

                switch (newThang.name)
                {
                    case "ctcr":
                        setHmTwice(new Vector2(-101f, 614.2f), new Vector2(101f, 614.2f), skin[6], null, indexCT, key, ref hm);
                        break;
                    case "ctband":
                        setHmTwice(new Vector2(-201f, 600.2f), new Vector2(201f, 600.2f), skin[6], null, indexCT, key, ref hm);
                        break;
                }
                break;
            case itemType.msk:
                int index5;
                if (XtraStuff.ContainsKey("hd"))
                {
                    index5 = XtraStuff["hd"].transform.GetSiblingIndex();
                }
                else
                {
                    index5 = bangs.transform.GetSiblingIndex();
                }
                setHmTwice(new Vector2(-173.0009f, -192.2f), new Vector2(162.4f, -192.2f),
                   skin[6], null, index5, key, ref hm);
                break;
            case itemType.scar:
                setHm(new Vector2(-7.828147f, 16.29871f), rightE[2], null, 0,
                key, ref hm);
                break;
            case itemType.unicorn:
                setHm(new Vector2(44.5f, 607.314f), skin[6], null, 0,
                key, ref hm);
                break;
            case itemType.b0odnos:
                setHm(new Vector2(24.1f, -30.4f), skin[6], skin[0].rectTransform, skin[6].transform.parent.childCount,
                key, ref hm);
                break;
            case itemType.bood:
                setHm(new Vector2(34f, -143.3f), skin[6], null, 0,
                key, ref hm);
                break;
            case itemType.hwrstrand:
                setHm(new Vector2(15f, 629.59f), skin[6], null, 1,
                key, ref hm);
                break;
            case itemType.harts:
                setHm(new Vector2(6f, 524.4f), skin[6], null, 0,
                key, ref hm);
                break;
            case itemType.overalls:


                setHm(Vector2.zero, clothes, null, 1,
                key, ref hm);
                break;
            case itemType.hrclip:
                setHm(new Vector2(354.7f, 263f), skin[6], null, bangs.transform.GetSiblingIndex() + 1,
                    key, ref hm);
                break;
            case itemType.nosepiercing:
                setHm(new Vector2(-50.9f, -51f), skin[6], skin[0].rectTransform, skin[6].transform.parent.childCount,
                   key, ref hm);
                break;
            case itemType.JBandage:
                setHm(new Vector2(-199.7f, -274.8f), skin[6], null, bangs.transform.GetSiblingIndex(),
                   key, ref hm);
                break;

            case itemType.bDaid:
                setHmTwice(new Vector2(-67.8f, 24.8f), new Vector2(67.8f, 24.8f),
                  skin[6], skin[0].rectTransform, 0, key, ref hm);
                break;


            case itemType.FGliter:
                setHmTwice(new Vector2(-122f, 0f), new Vector2(122f, 0f),
                  skin[6], null, 2, key, ref hm);
                Material add = Resources.Load<Material>("Additive");
                hm.set.material = add;
                hm.set2.material = add;
                break;

            case itemType.bronzer:
                setHmTwice(new Vector2(-237f, 108.1f), new Vector2(237f, 108.1f),
                     skin[6], null, 2, key, ref hm);
                break;

            case itemType.UEye:
                setHmTwice(new Vector2(-190f, 24.4f), new Vector2(190f, 24.4f),
                     skin[6], null, 2, key, ref hm);
                break;


            /*
                overall
                scarf
                cutetopband
                unicorn
                tie
                    
                hairclip
                nose ring
                bandage
                bandaid
            
                faceglitter
                undereye
                bronzer
                happy eye/squint

                petal
            backgrounds

             */
            case itemType.flower:
                int index4;
                if (XtraStuff.ContainsKey("ha"))
                {
                    index4 = XtraStuff["ha"].transform.GetSiblingIndex();
                }
                else if (XtraStuff.ContainsKey("ct"))
                {
                    index4 = XtraStuff["ct"].transform.GetSiblingIndex() + 1;
                }
                else
                {
                    index4 = 0;
                }
                setHm(new Vector2(0f, 422.9f), skin[6], null, index4,
                key, ref hm);
                break;
            case itemType.e_ye:
                hm.set = skin[2];
                hm.set2 = skin[3];
                hm.before = hm.set.sprite;
                break;
            case itemType.l_p:
                hm.set = lips;
                hm.before = hm.set.sprite;
                break;
            case itemType.n_se:
                hm.set = skin[0];
                hm.before = hm.set.sprite;
                break;
            case itemType.t_clothes:
                hm.set = clothes;
                clothes.gameObject.SetActive(true);
                XtraStuff[key] = clothes;
                hm.before = hm.set.sprite;
                break;
            case itemType.bg:
                hm.set = bg;
                hm.beforeC = hm.set.color;
                hm.before = hm.set.sprite;
                hm.beforeC2 = Camera.main.backgroundColor;
                if (newThang == null)
                { hm.set.sprite = null; hm.set.color = Color.clear; }
                else
                {
                    hm.beforeC = hm.set.color;
                    if (hm.before == null || (hm.before != null && hm.before.name != newThang.name))
                        hm.set.color = Color.white;
                }

                break;
            case itemType.ebrow:
                hm.set = leftE[2];
                hm.set2 = rightE[2];
                hm.before = hm.set.sprite;
                break;
            case itemType.bh_air:
                hm.set = hair;
                hm.before = hm.set.sprite;
                Vector2 size = hair.rectTransform.sizeDelta;
                hm.beforeC2 = new Color(size.x / 1500, size.y / 2500, 0, 0);

                break;
            case itemType.b_ngs:
                hm.set = bangs;
                hm.before = hm.set.sprite;
                if (newThang.name == "b_92")
                {
                    bangs.rectTransform.anchoredPosition = new Vector2(bangs.rectTransform.anchoredPosition.x, 625.8f);
                }
                else
                {
                    bangs.rectTransform.anchoredPosition = new Vector2(bangs.rectTransform.anchoredPosition.x, 503);
                }
                break;
            case itemType.skin:
                hm.set = skin[1];
                hm.before = hm.set.sprite;
                hm.beforeC2 = lips.color;
                break;
            case itemType.blush:
                hm.set = skin[6];
                hm.set.gameObject.SetActive(true);
                XtraStuff[key] = skin[6];
                hm.before = hm.set.sprite;
                break;
            case itemType.Particle_petal:
            case itemType.pArticle_sparkle:
            case itemType.particle_snow:
                hm.before = newThang;
                XtraStuff[key] = null;
                break;

            case itemType.wdEluxeScript:
                int indexWd = 0;
                if (XtraStuff.ContainsKey("wf"))
                {
                    indexWd = XtraStuff["wf"].transform.GetSiblingIndex();
                }
                setHm(Vector2.zero, bg, bg.transform.parent, indexWd, "wd", ref hm);
                if (hm.set.gameObject.GetComponent<WaterScript>() == null)
                    hm.set.gameObject.AddComponent<WaterScript>();
                hm.set.sprite = null;
                hm.set.gameObject.name = "wd";
                break;
            case itemType.wfFallScript:
                setHm(Vector2.zero, bg, bg.transform.parent, 0, "wf", ref hm);
                WaterfallScript wf = hm.set.gameObject.GetComponent<WaterfallScript>();
                if (wf == null)
                    hm.set.gameObject.AddComponent<WaterfallScript>();
                else
                    hm.beforeC = wf.LightColor;
                hm.set.sprite = null;
                hm.set.gameObject.name = "wf";
                break;
            case itemType.GXlitch:
                if (!XtraStuff.ContainsKey("GX"))
                {
                    Glitch g = Camera.main.gameObject.GetComponent<Glitch>();
                    g.enabled = true;
                    g.colorDrift = 0.25f;
                    XtraStuff["GX"] = null;
                }
                hm.before = newThang;
                break;
            case itemType.CXolor:
            case itemType.BMlm:
            case itemType.BNry:
            case itemType.RPamp:
            case itemType.FRGradient:
                string FXkey = it.ToString().Substring(0, 2);
                XtraStuff[FXkey] = null;
                break;

        }

        if (hm.set != null)
        {
            if (hm.set != bg && it != itemType.wfFallScript) hm.beforeC = hm.set.color;
            if (newThang != null)
            {
                hm.set.sprite = newThang;
                setNative(hm.set, it);
                if (hm.set2 != null)
                {
                    hm.set2.sprite = newThang;
                    hm.beforeC2 = hm.set2.color;
                    setNative(hm.set2, it);
                }
            }
        }
        return hm;
    }

    void setNative(Image i, itemType it)
    {
        if (it == itemType.bg || it == itemType.n_se || it == itemType.l_p)
            return;
        i.SetNativeSize();
    }

    void setHmTwice(Vector2 placement, Vector2 placement2, Image dup, Transform parent, int index, string key, ref UndoInfo hm)
    {
        if (!XtraStuff.ContainsKey(key))
        {
            hm.set = newImgAt(key, placement, dup, parent, index);
            hm.set2 = newImgAt(key, placement2, dup, parent, index);
            hm.set.rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            hm.set = XtraStuff[key];
            hm.set2 = XtraStuff[key + "2"];
            hm.set.rectTransform.anchoredPosition = placement;
            hm.set2.rectTransform.anchoredPosition = placement2;
            hm.before = hm.set.sprite;
        }
    }
    void setHm(Vector2 placement, Image dup, Transform parent, int index, string key, ref UndoInfo hm)
    {
        if (!XtraStuff.ContainsKey(key))
        {
            hm.set = newImgAt(key, placement,
            dup, parent, index);
        }
        else
        {
            hm.set = XtraStuff[key];
            hm.before = hm.set.sprite;
            hm.set.rectTransform.anchoredPosition = placement;
        }
    }

    Image newImgAt(string key, Vector2 pos, Image dup, Transform parent = null, int index = 0)
    {
        Image ret = Instantiate(dup, parent == null ? dup.transform.parent : parent, false).GetComponent<Image>();
        if (index > 0)
        {
            ret.rectTransform.SetSiblingIndex(index);
        }
        else if (index == -1)
        {
            ret.rectTransform.SetSiblingIndex(0);
        }
        ret.rectTransform.anchoredPosition = pos;
        ret.rectTransform.localRotation = Quaternion.Euler(Vector3.zero);

        if (XtraStuff.ContainsKey(key))
        {
            XtraStuff[key + "2"] = ret;
        }
        else
        {
            XtraStuff[key] = ret;


        }
        ret.gameObject.SetActive(true);
        ret.color = Color.white;
        return ret;
    }

}
