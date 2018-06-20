using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


//class created to handle randomization and undoing that randomization
public class randomize : MonoBehaviour
{

    /*
    save load not properly working!!!!!!!!


    DUPLICATE SPAWNING OF SHOP -  TF??!?
    
    
     */

    DressManager dm;

    //persistent listener for Randomize button to randomize character
    public void random()
    {

        dm = transform.root.GetComponent<DressManager>();
        dm.LoadAll();
        StartCoroutine(randHelper());
    }

    IEnumerator randHelper()
    {

        if (dm.cpa == null)
        {
            dm.callSwitch();
            yield return null;

        }
        yield return null;
        while (dm.cpa == null)
        {
            yield return null;
        }
        while (dm.load.loaddd)
        {
            yield return null;
        }


        if (dm.cpa != null)
            dm.cpa.gameObject.SetActive(false);
        int length = UnityEngine.Random.Range(0, itemType.GetNames(typeof(itemType)).Length - 10);
        length /= 2;
        HashSet<int> enums = new HashSet<int>();

        enums.Add((int)itemType.ebrow);
        enums.Add((int)itemType.bh_air);
        enums.Add((int)itemType.b_ngs);
        enums.Add((int)itemType.e_ye);
        enums.Add((int)itemType.bg);
        enums.Add((int)itemType.t_clothes);
        for (int i = 0; i < length; i++)
        {
            enums.Add(UnityEngine.Random.Range(5, (int)itemType.particle_snow));
        }
        enums.Add((int)itemType.skin);
        /*
        bugs:
        cpa not loaded in time

        randomize


            test save/load

         */

        List<UndoInfo> uiArr = new List<UndoInfo>();

        itemType[][] BadMatches = new itemType[][]{
                new itemType[]{itemType.msk, itemType.b0odnos, itemType.bood, itemType.JBandage, itemType.lippiercing,itemType.bubble},
                new itemType[]{itemType.bood, itemType.starfreckles},
                new itemType[]{itemType.BOW, itemType.chneckwear},
                new itemType[]{itemType.bronzer, itemType.UEye},
                new itemType[]{itemType.glasses, itemType.EPatch,itemType.scar},
                new itemType[]{itemType.harts, itemType.flower},
                new itemType[]{itemType.freckles,itemType.starfreckles, itemType.bDaid},
                new itemType[]{itemType.hesidehorn, itemType.unicorn, itemType.eear,itemType.flower},

            };

        foreach (itemType[] iList in BadMatches)
        {
            itemType i1 = iList[0];

            foreach (itemType i2 in iList)
            {
                if (enums.Contains((int)i1) && enums.Contains((int)i2))
                {
                    enums.Remove((UnityEngine.Random.value > 0.5f) ? (int)i1 : (int)i2);
                }
            }
        }


        Color bg = dm.fm.bg.color;

        HashSet<string> s = new HashSet<string>(new string[]{
                           "b_", "ir", "w_", "ey", "bh", "e_", "eb", "l_", "n_", "bg", "BY", "CH", "HD","t_",
        });
        Button check = dm.cpa.transform.parent.GetChild(1).GetComponent<Button>();
        check.onClick.RemoveAllListeners();

        foreach (var i in Enum.GetValues(typeof(itemType)))
        {
            itemType it = (itemType)i;
            Sprite newT = getRandomSprite(it);
            string key = it.ToString().Substring(0, 2);

            if (newT != null && !s.Contains(key) && dm.fm.XtraStuff.ContainsKey(key))
            {
                if (dm.fm.XtraStuff[key] != null)
                {
                    dm.fm.XtraStuff[key].gameObject.SetActive(false);
                    //make remove undoable
                    if (dm.fm.XtraStuff.ContainsKey(key + "2"))
                    {
                        dm.fm.XtraStuff[key + "2"].gameObject.SetActive(false);
                        dm.x.onClick.AddListener(() =>
                        {
                            dm.fm.XtraStuff[key].gameObject.SetActive(true);
                            dm.fm.XtraStuff[key + "2"].gameObject.SetActive(true);
                        });
                    }
                    else
                    {
                        dm.x.onClick.AddListener(() =>
                        {
                            dm.fm.XtraStuff[key].gameObject.SetActive(true);
                        });
                    }


                    check.onClick.AddListener(() =>
                    {
                        dm.fm.Remove(key);
                    });
                }
            }
            else if (enums.Contains((int)i))
            {

                uiArr.Add(dm.fm.faceSet(newT, it));
                //if heart..
                if (it == itemType.harts && GameObject.FindGameObjectWithTag("Finish").transform.Find("hartic") == null) dm.setUpParticles(dm.xtra[16], 1, Color.white, true);
                // else if (it == itemType.particles) dm.setUpParticles(uiArr[count].before, 0);
            }

        }

        dm.colorPick(uiArr);
        randomizeParams(dm.fm, dm.x);
        yield return null;
        CanvasGroup cp = dm.cpa.transform.root.GetComponent<CanvasGroup>();
        check.onClick.AddListener(() => { dm.TurnOffEnd(cp); });
        dm.x.onClick.AddListener(() => { check.onClick.RemoveAllListeners(); check.onClick.AddListener(() => dm.TurnOffEnd(cp)); });
        RandomizeColors(uiArr, bg);


    }

    void randomizeParams(FaceManager fm, Button x)
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
        RectTransform hair = fm.hair.rectTransform;

        float val = UnityEngine.Random.Range(-1f, 1f);
        if (!limits.ContainsKey(fm.hair.sprite.name))
        {
            Vector2 origH = hair.sizeDelta;
            int[] ratios = limits[fm.hair.sprite.name];
            hair.sizeDelta = new Vector2(ratios[2] + val * ratios[0], ratios[3] + val * ratios[1]);
            x.onClick.AddListener(() =>
         {
             hair.sizeDelta = origH;
         });

        }
        Dictionary<string, int[]> Banglimits = new Dictionary<string, int[]>(){
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

        RectTransform bang = fm.bangs.rectTransform;

        val = UnityEngine.Random.Range(-0.8f, 0.8f);
        if (!Banglimits.ContainsKey(fm.bangs.sprite.name))
        {
            Vector2 origH = bang.sizeDelta;
            int[] ratios = limits[fm.bangs.sprite.name];
            bang.sizeDelta = new Vector2(fm.bangs.sprite.rect.width + val * ratios[0], fm.bangs.sprite.rect.height + val * ratios[1]);
            x.onClick.AddListener(() =>
         {
             bang.sizeDelta = origH;
         });

        }

        val = UnityEngine.Random.Range(-0.3f, 0.5f);
        RectTransform iris = fm.leftE[1].rectTransform, iris2 = fm.rightE[1].rectTransform;
        Vector2 origI = iris.sizeDelta;
        iris.sizeDelta = new Vector2(120.2f + 25 * val, 107.8f + 25 * val);
        iris2.sizeDelta = iris.sizeDelta;


        val = UnityEngine.Random.Range(-0.6f, 1f);
        Vector2 orig = fm.lips.rectTransform.sizeDelta;
        float origVert = fm.VertLip;
        fm.lips.rectTransform.sizeDelta = new Vector2(fm.lips.sprite.rect.width + val * 24, 132 + UnityEngine.Random.Range(-1f, 1f) * 15);
        fm.VertLip = UnityEngine.Random.Range(-5f, 5f);

        float origX = fm.HorzEye, origY = fm.VertEye;

        RectTransform eye = fm.skin[2].rectTransform, eye2 = fm.skin[3].rectTransform;
        Vector2 origE = eye.sizeDelta;
        val = UnityEngine.Random.Range(-0.4f, 1f);
        eye.sizeDelta = new Vector2(fm.skin[2].sprite.rect.width + val * 14, fm.skin[2].sprite.rect.height + val * 14);
        eye2.sizeDelta = eye.sizeDelta;
        val = UnityEngine.Random.Range(-1f, 1f);
        fm.VertEye = 5.5f * val;
        val = UnityEngine.Random.Range(-0.5f, 1f);
        fm.HorzEye = 4 * val;

        RectTransform head = dm.fm.skin[1].rectTransform;
        Vector2 origSize = head.sizeDelta;
        head.sizeDelta = new Vector2(750 + 30 * UnityEngine.Random.Range(-0.7f, 0.7f), 692 + 35 * UnityEngine.Random.Range(-0.6f, 0.7f));

        float verEB = fm.VertEB;
        val = UnityEngine.Random.Range(-1f, 1f);
        fm.VertEB = val * 15;

        float vertN = fm.VertNose;
        Vector2 sizseN = fm.skin[0].rectTransform.sizeDelta;
        val = UnityEngine.Random.Range(-0.5f, 1f);
        fm.skin[0].rectTransform.sizeDelta = new Vector2(349 + 15 * val, 304 + 15 * val);

        x.onClick.AddListener(() =>
{

    fm.VertEB = verEB;

    fm.VertNose = vertN;
    fm.skin[0].rectTransform.sizeDelta = sizseN;

    iris.sizeDelta = origI;
    iris2.sizeDelta = origI;

    fm.lips.rectTransform.sizeDelta = orig;
    fm.VertLip = origVert;

    fm.VertEye = origY;
    fm.HorzEye = origX;
    eye.sizeDelta = origE;
    eye2.sizeDelta = origE;

    head.sizeDelta = origSize;

});
    }

    //pass in Color bg to ensure color before faceSet
    void RandomizeColors(List<UndoInfo> uiArr, Color bg)
    {
        HashSet<string> s = new HashSet<string>(new string[]{
       "w_hites","eyelid", "bronzer", "UEye", "bg","iris","head", "nose",  "blush","sx","sl1", "bangs", "hair", "eye", "Se", "se1", "se4", "se5", "glasses_hart", "glasses_circle", "se6"
        });

        Color origSkin = dm.fm.skin[4].color, origBlush = dm.fm.skin[6].color,
       origHair = dm.fm.hair.color,
       leftI = dm.fm.leftE[1].color, rightI = dm.fm.rightE[1].color,
       bgCam = Camera.main.backgroundColor, lips = dm.fm.lips.color;

        Color32[] c = getRandColor();
        Color rand = c[UnityEngine.Random.Range(0, c.Length)];
        foreach (UndoInfo ui in uiArr)
        {
            if (ui.set != null && !s.Contains(ui.set.gameObject.name) && ui.set.sprite != null && !s.Contains(ui.set.sprite.name))
            {

                ui.set.color = rand;
                if (ui.set2 != null) ui.set2.color = rand;

                if (ui.set.sprite.name == "hart")
                {
                    var main = GameObject.FindGameObjectWithTag("Finish").transform.Find("hartic").GetComponent<ParticleSystem>().main;
                    main.startColor = new ParticleSystem.MinMaxGradient(rand,
                    new Color(rand.r, rand.g, rand.b, 0.2f));
                }
            }
            rand = c[UnityEngine.Random.Range(0, c.Length)];
        }



        Color newSkin = c[UnityEngine.Random.Range(0, 2)];
        dm.changeSkin(newSkin, lips);
        if (dm.fm.lips.color.r < 180 && (dm.fm.lips.color.g < 180 || dm.fm.lips.color.b < 180)
        && UnityEngine.Random.value > 0.25f
        )
        {

            dm.fm.lips.color = new Color(newSkin.r + 0.2f, newSkin.g + 0.1f, newSkin.b + 0.1f, 1);
            dm.x.onClick.AddListener(() =>
            {
                dm.fm.lips.color = lips;
            });
        }
        Color32 hair = c[UnityEngine.Random.Range(0, c.Length)];

        //change hair color to 2nd color
        dm.fm.leftE[2].color = hair;
        dm.fm.rightE[2].color = hair;
        dm.fm.bangs.color = hair;
        dm.fm.hair.color = hair;

        Color irisL = dm.fm.leftE[0].color, irisR = dm.fm.rightE[0].color;
        if (UnityEngine.Random.value > 0.8f)
        {
            dm.fm.leftE[1].color = c[UnityEngine.Random.Range(0, c.Length / 2)];
            dm.fm.rightE[1].color = dm.fm.leftE[1].color;

        }

        dm.fm.leftE[1].color = c[UnityEngine.Random.Range(0, c.Length / 2)];
        dm.fm.rightE[1].color = c[UnityEngine.Random.Range(0, c.Length / 2)];

        RandomizeBackground((UnityEngine.Random.value > 0.3f) ? c[UnityEngine.Random.Range(0, c.Length)] : new Color32(255, 255, 255, 255), c[UnityEngine.Random.Range(0, c.Length)]);

        dm.x.onClick.AddListener(() =>
        {
            dm.changeSkin(origSkin, Color.white, false);
            dm.fm.skin[6].color = origBlush;
            dm.fm.leftE[1].color = leftI;
            dm.fm.rightE[1].color = rightI;
            dm.fm.bg.color = bg;
            Camera.main.backgroundColor = bgCam;
            dm.fm.leftE[0].color = irisL;
            dm.fm.rightE[0].color = irisR;
        });



    }

    void RandomizeBackground(Color32 one, Color32 two)
    {
        Camera.main.backgroundColor = one;
        if (dm.fm.bg.sprite != null && dm.fm.bg.sprite.name[2] != 'p')
        {
            //change alpha
            dm.fm.bg.color = new Color(1, 1, 1, 1 - HSBColor.FromColor(one).b);
        }
        else
        {
            dm.fm.bg.color = two;
        }
    }
    Color32[] getRandColor()
    {
        int num = UnityEngine.Random.Range(0, 32);
        //switch to returning array based on num (swiytch case)
        switch (num)
        {
            case 0:
                return new Color32[] { new Color32(228, 240, 245, 255), new Color32(158, 153, 153, 255), new Color32(70, 62, 74, 255), new Color32(193, 212, 227, 255), new Color32(131, 153, 181, 255), new Color32(68, 79, 107, 255), };
            case 1: return new Color32[] { new Color32(176, 128, 106, 255), new Color32(241, 233, 216, 255), new Color32(208, 211, 170, 255), new Color32(176, 186, 141, 255), new Color32(238, 209, 121, 255), new Color32(108, 88, 91, 255) };
            case 2: return new Color32[] { new Color32(245, 234, 234, 255), new Color32(236, 217, 229, 255), new Color32(219, 236, 222, 255), new Color32(205, 233, 218, 255), new Color32(228, 210, 226, 255), new Color32(239, 249, 229, 255), };
            case 3: return new Color32[] { new Color32(191, 173, 154, 255), new Color32(215, 213, 216, 255), new Color32(165, 192, 170, 255), new Color32(104, 146, 128, 255), new Color32(59, 114, 104, 255), new Color32(28, 56, 52, 255), };
            case 4: return new Color32[] { new Color32(237, 234, 229, 255), new Color32(244, 226, 182, 255), new Color32(192, 223, 223, 255), new Color32(219, 99, 116, 255), new Color32(137, 151, 163, 255), new Color32(159, 203, 214, 255), };
            case 5: return new Color32[] { new Color32(235, 219, 181, 255), new Color32(197, 176, 151, 255), new Color32(224, 180, 128, 255), new Color32(65, 166, 210, 255), new Color32(183, 227, 240, 255), new Color32(219, 228, 227, 255), new Color32(65, 84, 89, 255), };
            case 6: return new Color32[] { new Color32(254, 219, 203, 255), new Color32(206, 190, 195, 255), new Color32(205, 195, 230, 255), new Color32(134, 185, 198, 255), new Color32(29, 77, 171, 255), new Color32(164, 88, 151, 255), new Color32(57, 44, 90, 255) };
            case 7: return new Color32[] { new Color32(253, 239, 239, 255), new Color32(210, 188, 171, 255), new Color32(229, 206, 214, 255), new Color32(250, 230, 235, 255), new Color32(154, 172, 130, 255), new Color32(85, 114, 86, 255), new Color32(43, 64, 61, 255) };
            case 8: return new Color32[] { new Color32(240, 216, 187, 255), new Color32(102, 76, 62, 255), new Color32(217, 143, 94, 255), new Color32(55, 55, 55, 255), new Color32(110, 182, 190, 255), new Color32(56, 114, 119, 255), };
            case 9: return new Color32[] { new Color32(219, 206, 205, 255), new Color32(236, 214, 220, 255), new Color32(206, 207, 220, 255), new Color32(174, 190, 209, 255), new Color32(106, 125, 122, 255), new Color32(207, 108, 87, 255), new Color32(75, 69, 70, 255), };
            case 10: return new Color32[] { new Color32(242, 230, 225, 255), new Color32(247, 243, 239, 255), new Color32(78, 163, 163, 255), new Color32(55, 120, 150, 255), new Color32(182, 176, 189, 255), new Color32(43, 92, 122, 255) };
            case 11: return new Color32[] { new Color32(216, 198, 159, 255), new Color32(245, 245, 245, 255), new Color32(242, 235, 167, 255), new Color32(227, 227, 227, 255), new Color32(227, 181, 29, 255), new Color32(105, 105, 86, 255), new Color32(199, 199, 201, 255) };
            case 12: return new Color32[] { new Color32(254, 230, 225, 255), new Color32(206, 158, 163, 255), new Color32(254, 183, 207, 255), new Color32(224, 109, 137, 255), new Color32(35, 43, 57, 255), new Color32(108, 252, 253, 255), new Color32(185, 238, 244, 255), };
            case 13: return new Color32[] { new Color32(254, 230, 225, 255), new Color32(206, 158, 163, 255), new Color32(254, 183, 207, 255), new Color32(224, 109, 137, 255), new Color32(35, 43, 57, 255), new Color32(108, 252, 253, 255), new Color32(185, 238, 244, 255), };
            case 14: return new Color32[] { new Color32(208, 167, 146, 255), new Color32(229, 203, 188, 255), new Color32(206, 203, 206, 255), new Color32(236, 225, 218, 255), new Color32(230, 232, 228, 255), new Color32(127, 125, 126, 255) };
            case 15: return new Color32[] { new Color32(238, 246, 235, 255), new Color32(203, 183, 142, 255), new Color32(221, 212, 155, 255), new Color32(238, 238, 206, 255), new Color32(226, 112, 131, 255), new Color32(216, 177, 214, 255), new Color32(185, 225, 238, 255) };
            case 16: return new Color32[] { new Color32(234, 229, 219, 255), new Color32(208, 167, 146, 255), new Color32(175, 205, 199, 255), new Color32(99, 153, 152, 255), new Color32(43, 62, 55, 255), new Color32(100, 40, 48, 255), new Color32(237, 73, 90, 255) };
            case 17: return new Color32[] { new Color32(201, 178, 154, 255), new Color32(128, 98, 86, 255), new Color32(235, 228, 221, 255), new Color32(69, 52, 63, 255), new Color32(86, 96, 112, 255), new Color32(188, 203, 209, 255) };
            case 18: return new Color32[] { new Color32(255, 255, 255, 255), new Color32(233, 233, 233, 255), new Color32(26, 25, 26, 255), new Color32(76, 92, 106, 255), new Color32(149, 167, 176, 255), new Color32(206, 206, 206, 255), new Color32(222, 223, 223, 255) };
            case 19: return new Color32[] { new Color32(209, 192, 175, 255), new Color32(97, 87, 83, 255), new Color32(227, 223, 218, 255), new Color32(120, 104, 120, 255), new Color32(170, 191, 188, 255), new Color32(211, 216, 232, 255) };
            case 20: return new Color32[] { new Color32(182, 168, 155, 255), new Color32(244, 243, 241, 255), new Color32(236, 227, 227, 255), new Color32(212, 193, 196, 255), new Color32(42, 43, 44, 255), new Color32(225, 223, 211, 255) };
            case 21: return new Color32[] { new Color32(186, 181, 181, 255), new Color32(231, 227, 222, 255), new Color32(208, 206, 199, 255), new Color32(249, 249, 245, 255), new Color32(242, 242, 235, 255), new Color32(233, 234, 238, 255) };
            case 22: return new Color32[] { new Color32(168, 151, 143, 255), new Color32(102, 76, 77, 255), new Color32(37, 31, 48, 255), new Color32(202, 202, 204, 255), new Color32(227, 227, 232, 255), new Color32(125, 44, 56, 255) };
            case 23: return new Color32[] { new Color32(230, 222, 214, 255), new Color32(117, 84, 95, 255), new Color32(174, 186, 160, 255), new Color32(74, 67, 64, 255), new Color32(79, 88, 120, 255), new Color32(155, 163, 181, 255) };
            case 24: return new Color32[] { new Color32(168, 151, 143, 255), new Color32(100, 73, 71, 255), new Color32(178, 172, 226, 255), new Color32(138, 116, 182, 255), new Color32(83, 58, 90, 255), new Color32(47, 47, 47, 255), };
            case 25: return new Color32[] { new Color32(235, 231, 211, 255), new Color32(222, 196, 171, 255), new Color32(194, 108, 127, 255), new Color32(130, 62, 86, 255), new Color32(56, 49, 84, 255), new Color32(149, 180, 207, 255), };
            case 26: return new Color32[] { new Color32(236, 230, 209, 255), new Color32(108, 96, 112, 255), new Color32(240, 244, 241, 255), new Color32(225, 182, 204, 255), new Color32(39, 33, 60, 255), new Color32(71, 47, 113, 255), };
            //236,230,209  108,96,112  240,244,241  225, 182, 204  39,33,60  71,47,113

            case 27: return new Color32[] { new Color32(170, 107, 103, 255), new Color32(237, 224, 213, 255), new Color32(117, 149, 157, 255), new Color32(96, 109, 113, 255), new Color32(221, 175, 150, 255), new Color32(240, 234, 245, 255), };
            //170,107,103  237,224,213   117,149,157  96,109,113   221,175,150  240,234, 245
            case 28: return new Color32[] { new Color32(247, 193, 164, 255), new Color32(251, 238, 226, 255), new Color32(205, 119, 95, 255), new Color32(250, 220, 234, 255), new Color32(230, 229, 244, 255), new Color32(222, 230, 255, 255), };
            //247, 193,164   251,238, 226   205, 119,95     250,220,234   230,229,244   222, 230, 255

            case 29: return new Color32[] { new Color32(151, 134, 129, 255), new Color32(194, 181, 168, 255), new Color32(229, 223, 215, 255), new Color32(239, 239, 238, 255), new Color32(60, 86, 108, 255), new Color32(136, 175, 197, 255), };
                //151,134,129  194,181,168    229,223,215   239,239,238     60,86,108      136,175,197
        }
        return new Color32[] { new Color32(247, 232, 228, 255), new Color32(113, 107, 110, 255), new Color32(253, 254, 240, 255), new Color32(130, 144, 157, 255), new Color32(181, 193, 204, 255), new Color32(226, 229, 234, 255) };

    }

    Sprite getRandomSprite(itemType it)
    {
        /*
                faceglitter
                undereye
                bronzer
                happy eye/squint


            backgrounds

        
         */

        switch (it)
        {
            case itemType.iris:
                return dm.eye[0];
            case itemType.w_hites:
                return dm.eye[1];
            case itemType.Se:
                return dm.eye[3];
            case itemType.se:
                return dm.eye[UnityEngine.Random.Range(4, 8)];
            case itemType.xe:
                return dm.eye[UnityEngine.Random.Range(8, 10)];
            case itemType.BOW:
                return dm.clothes[UnityEngine.Random.Range(0, 2)];
            case itemType.chneckwear:
                return dm.clothes[UnityEngine.Random.Range(2, 4)];
            case itemType.glasses:
                return dm.clothes[UnityEngine.Random.Range(5, 8)];
            case itemType.overalls:
                return dm.clothes[8];
            case itemType.t_clothes:
                return dm.clothes[UnityEngine.Random.Range(9, 17)];
            case itemType.hrclip:
                return dm.xtra[UnityEngine.Random.Range(23, 25)];
            case itemType.ctetopband:
                return dm.xtra[UnityEngine.Random.Range(6, 8)];
            case itemType.eear:
                return dm.xtra[UnityEngine.Random.Range(8, 12)];
            case itemType.hesidehorn:
                return dm.xtra[UnityEngine.Random.Range(20, 23)];

            case itemType.b0odnos:
                return dm.xtra[0];
            case itemType.bDaid:
                return dm.xtra[1];
            case itemType.blush:
                return dm.xtra[2];
            case itemType.bood:
                return dm.xtra[3];
            case itemType.bronzer:
                return dm.xtra[4];
            case itemType.bubble:
                return dm.xtra[5];
            case itemType.EPatch:
                return dm.xtra[12];
            case itemType.FGliter:
                return dm.xtra[13];
            case itemType.flower:
                return dm.xtra[14];
            case itemType.freckles:
                return dm.xtra[15];
            case itemType.harts:
                return dm.xtra[17];
            case itemType.hdphones:
                return dm.xtra[19];
            case itemType.hwrstrand:
                return dm.xtra[25];
            case itemType.JBandage:
                return dm.xtra[26];
            case itemType.lippiercing:
                return dm.xtra[27];
            case itemType.msk:
                return dm.xtra[28];
            case itemType.nosepiercing:
                return dm.xtra[29];
            case itemType.particle_snow:
            case itemType.Particle_petal:
            case itemType.pArticle_sparkle:
                return dm.xtra[UnityEngine.Random.Range(30, 33)];

            case itemType.scar:
                return dm.xtra[33];
            case itemType.sl1:
                return dm.xtra[34];
            case itemType.starfreckles:
                return dm.xtra[35];
            case itemType.sx_tears:
                return dm.xtra[36];
            case itemType.UEye:
                return dm.xtra[37];

            case itemType.unicorn:
                return dm.xtra[38];


            case itemType.e_ye:
                return dm.eyes[UnityEngine.Random.Range(0, dm.eyes.Length)];
            case itemType.l_p:
                return dm.lips[UnityEngine.Random.Range(0, dm.lips.Length)];
            case itemType.n_se:
                return dm.nose[UnityEngine.Random.Range(0, dm.nose.Length)];

            case itemType.bg:
                int rand = UnityEngine.Random.Range(0, dm.bg.Length + 1);
                if (rand == dm.bg.Length) return null;
                return dm.bg[rand];
            case itemType.ebrow:
                return dm.brows[UnityEngine.Random.Range(0, dm.brows.Length)];
            case itemType.bh_air:
                int index = UnityEngine.Random.Range(0, dm.hair.Length);
                if (dm.hair[index].name == "bh2" && UnityEngine.Random.value > 0.5f) { index = UnityEngine.Random.Range(index, dm.hair.Length); }
                return dm.hair[index];
            case itemType.b_ngs:
                return dm.bangs[UnityEngine.Random.Range(0, dm.bangs.Length)];
        }
        return null;
    }
}
