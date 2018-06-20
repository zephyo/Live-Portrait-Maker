using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;
using System.Threading;
using ThreadPriority = System.Threading.ThreadPriority;
public class CameraSave : MonoBehaviour
{


    Image view;


    GameObject[] activate;

    Image mat;

    //Thread _thread;

    Button check;
    Image flash;


    Loading l;

    string persis;

    string vidPath;

    bool saved;





    public void Init(Image v, Button crop, Button full, Button check, GameObject[] go, Image master, DressManager dm)
    {
        persis = Application.persistentDataPath;
        view = v;
        l = dm.load;

        UnityAction cropUA = () =>
        {
            view.raycastTarget = true;
            StartCoroutine(listenForCrop(dm));
            dm.TurnOff(mat.transform.root.GetComponent<CanvasGroup>(), false);
        };
        UnityAction fullScreenUA = () =>
        {

            view.rectTransform.anchorMax = Vector2.one;
            view.rectTransform.anchorMin = Vector2.zero;
        };
        crop.onClick.AddListener(cropUA);
        full.onClick.AddListener(fullScreenUA);
        activate = go;
        mat = master;
        this.check = check;

    }

    IEnumerator listenForCrop(DressManager dm)
    {
        RectTransform r = view.rectTransform;
        Vector3 i = Vector3.zero;


#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        i = Input.mousePosition;
        /*r.anchorMin(left, bttom)
       r.anchorMax(right, top) */
        r.anchorMin = new Vector2(i.x / Screen.width, i.y / Screen.height);
        r.anchorMax = r.anchorMin;

        while (Input.GetMouseButton(0))
        {
            //do stuff
            Vector3 n = Input.mousePosition;
            r.anchorMin = new Vector2((n.x < i.x) ? n.x / Screen.width : i.x / Screen.width, (n.y < i.y) ? n.y / Screen.height : i.y / Screen.height);

            r.anchorMax = new Vector2((n.x > i.x) ? n.x / Screen.width : i.x / Screen.width, (n.y > i.y) ? n.y / Screen.height : i.y / Screen.height);

            yield return null;
        }
#elif UNITY_IOS || UNITY_ANDROID
        Touch t = Input.GetTouch(0);
        while (true)
        {

            if (Input.touchCount > 0)
            {
                t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    i = t.position;
                    break;
                }
            }
            yield return null;
        }
        r.anchorMin = new Vector2(i.x / Screen.width, i.y / Screen.height);
        r.anchorMax = r.anchorMin;
        while (true)
        {
            t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                break;
            }
            if (t.phase == TouchPhase.Moved)
            {
                Vector3 n = t.position;
                r.anchorMin = new Vector2((n.x < i.x) ? n.x / Screen.width : i.x / Screen.width, (n.y < i.y) ? n.y / Screen.height : i.y / Screen.height);

                r.anchorMax = new Vector2((n.x > i.x) ? n.x / Screen.width : i.x / Screen.width, (n.y > i.y) ? n.y / Screen.height : i.y / Screen.height);
            }
            yield return null;
        }
#endif
        view.raycastTarget = false;
        yield return null;

        dm.TurnOn(mat.transform.root.GetComponent<CanvasGroup>(), false);

    }

    public void TakeImage()
    {
        OnlySeeCheckButton();
        StartCoroutine(SaveImage());
    }


    UnityEngine.UI.Image createVidJuice(Image check)
    {
        GameObject cropGo = new GameObject("red");
        Image l = cropGo.AddComponent<Image>();
        l.preserveAspect = true;
        l.color = Color.red;
        l.sprite = Resources.Load<Sprite>("ring");
        RectTransform r = l.rectTransform;
        r.SetParent(check.transform, false);
        r.sizeDelta = Vector2.one * 230;
        l.raycastTarget = false;
        l.type = Image.Type.Filled;
        l.fillMethod = Image.FillMethod.Radial360;
        l.fillOrigin = 2;
        l.fillAmount = 0;
        l.fillClockwise = true;
        return l;
    }

    public void TakeVideo(FaceManager fm, UnityEngine.UI.Image checkImg, RectTransform full)
    {

        view.transform.GetChild(0).gameObject.SetActive(false);

        fm.setUpDelegates();
        fm.ChangeLook();

        CamVideo cv = Camera.main.gameObject.AddComponent<CamVideo>();
        Image i = createVidJuice(checkImg);
        cv.init(view.rectTransform, full, (int save) =>
        {
            int w = cv.w;
            int h = cv.h;
            l.Init(false);
            if (save > 0)
            {
                StartCoroutine(saveT(cv, w, h));
            }
            fm.removeAll();
            Destroy(i.gameObject);
            check.interactable = false;
            check.onClick.RemoveAllListeners();
            check.onClick.AddListener(() => TakeVideo(fm, checkImg, full));
            checkImg.sprite = Resources.Load<Sprite>("play");

            //  Directory.Delete(m_FilePath, true); // NativeGallery.GetSavePath(persis, "", "LPM{0}.gif")



        }, i);
        // StartCoroutine(gettingVideo(cv));
        OnlySeeCheckButton();

        check.onClick.RemoveAllListeners();
        check.onClick.AddListener(() => StopVideo(cv));
        checkImg.sprite = Resources.Load<Sprite>("stop");

    }


    IEnumerator SaveVidFile()
    {
        while (saved==false)
        {
            yield return null;
        }
        saved=false;
        byte[] read_ = File.ReadAllBytes(vidPath);
        Debug.Log("SAVING GIF");
        MobileMedia.SaveBytes(read_, "Live Portrait Maker", "LPM{0}", ".gif", true);
        Directory.Delete(vidPath);


    }

    IEnumerator saveT(CamVideo cv, int w, int h)
    {

        Queue<byte[]> frameQueue = cv.frameQueue;
        int savingFrameNumber = cv.savingFrameNumber;
        while (frameQueue.Count > 0)
        {
            // Generate file path
            string path = persis + "/frame" + savingFrameNumber + ".raw";
            File.WriteAllBytes(path, frameQueue.Dequeue());
            savingFrameNumber++;
        }
        Destroy(cv);
        List<Frame> frames = new List<Frame>();
        Texture2D temp = new Texture2D(w, h, TextureFormat.RGB24, false);
        temp.hideFlags = HideFlags.HideAndDontSave;
        temp.wrapMode = TextureWrapMode.Clamp;
        temp.filterMode = FilterMode.Bilinear;
        temp.anisoLevel = 0;
        for (int i = 0; i < savingFrameNumber; i++)
        {
            string BMPpath = persis + "/RECORDING_LPM" + "/frame" + i + ".raw";

            if (File.Exists(BMPpath))
            {
                temp.LoadRawTextureData(File.ReadAllBytes(BMPpath));
                Color32[] colors = temp.GetPixels32();
                yield return null;
                Frame frame = new Frame() { Width = w, Height = h, Data = colors };
                frames.Add(frame);
            }
            else
            {
                break;
            }
        }
        Flush(temp);
        yield return null;
        l.updateThis(() =>
        {
            Directory.Delete(persis + "/RECORDING_LPM", true);
            SeeEverything();
            check.interactable = true;
            view.transform.GetChild(0).gameObject.SetActive(true);
            Application.targetFrameRate = 24;
        });
        ProGifEncoder encoder = new ProGifEncoder(0, 5);

        encoder.SetDelay(70);
   
        StartCoroutine(SaveVidFile());

        ThreadPriority WorkerPriority = ThreadPriority.BelowNormal;
        // GetSavePath(string saveDir, string album, string filenameFormatted)
        string name_ = new FilePathName().GetGifFileName();
       
        ProGifWorker worker = new ProGifWorker(WorkerPriority)
        {
            m_Encoder = encoder,
            m_Frames = frames,
            persisT = persis,
            m_FilePath = Application.persistentDataPath + "/" + name_ + ".gif",
            m_OnFileSaved = Saved,
            m_OnFileSaveProgress = l.setProgress,
        };
        worker.Start();
        //  _thread.Start();

    }

    // void OnDisable()
    // {
    //     if (_threadRunning)
    //     {
    //         // This forces the while loop in the ThreadedWork function to abort.
    //         _threadRunning = false;
    //         _thread.Join();
    //     }
    // }



    void Saved(int id, string path)
    {
        //    MobileMedia.SaveBytes(File.ReadAllBytes(path), "MobileMediaTest", "LPM_{0}",new FilePathName().GetGifFileName(), MobileMedia.ImageFormat.GIF);
        // NativeGallery.SaveVideoToGallery(persis, File.ReadAllBytes(path), "Live Portrait Maker", "LPM_{0}.gif");

        l.stop();
        vidPath = path;
        saved=true;

    }

    void Flush(Texture texture)
    {
        if (RenderTexture.active == texture) return;

#if UNITY_EDITOR
        Texture2D.DestroyImmediate(texture);
#else
		Texture2D.Destroy(texture);
#endif
    }




    public void StopVideo(CamVideo cv)
    {
        // stop getting video data
        cv.done = true;

        //save video
    }
    public Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        return new Rect((Vector2)transform.position - (size * 0.5f), size);
    }
    private IEnumerator SaveImage()
    {

        if (flash == null)
        {
            flash = GameObject.Instantiate(view.gameObject, view.transform.parent, false).GetComponent<Image>();
            flash.color = Color.clear;
            flash.transform.SetSiblingIndex(flash.transform.parent.childCount - 1);
            flash.material = null;
        }
        flash.gameObject.SetActive(false);
        view.transform.GetChild(0).gameObject.SetActive(false);
        check.gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();

        Rect snap = RectTransformToScreenSpace(view.rectTransform);
        Texture2D ss = new Texture2D((int)snap.width, (int)snap.height, TextureFormat.RGB24, false);
        ss.ReadPixels(snap, 0, 0);
        ss.Apply();
        yield return new WaitForEndOfFrame();
        view.transform.GetChild(0).gameObject.SetActive(true);
        check.gameObject.SetActive(true);
        if (flash != null)
            flash.gameObject.SetActive(true);


        LeanTween.value(Camera.main.gameObject, (float val) =>
        {
            if (flash != null)
                flash.color = new Color(0, 0, 0, val);
        }, 0, 0.5f, 0.15f).setEaseInQuad().setLoopPingPong(1).setOnComplete(() =>
        {
            if (flash != null)
                Destroy(flash.gameObject);
        });



        // Save the screenshot to Gallery/Photos
        // NativeGallery.SaveImageToGallery(persis, ss, callback);
        MobileMedia.SaveImage(ss, "Live Portrait Maker", new FilePathName().GetPngFileName(), MobileMedia.ImageFormat.PNG);


        Destroy(ss);
        SeeEverything();
    }

    public void OnlySeeCheckButton()
    {
        for (int i = 0; i < activate.Length; i++)
        {
            activate[i].SetActive(false);
        }

        mat.color = Color.clear;

    }

    public void SeeEverything()
    {
        for (int i = 0; i < activate.Length; i++)
        {
            activate[i].SetActive(true);
        }
        mat.color = Color.white;

    }



}
