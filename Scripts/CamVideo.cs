using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

//using System.Drawing.Imaging;
using UnityEngine.UI;
class BitmapEncoder
{
    // public static void WriteBitmap(Stream stream, int width, int height, byte[] imageData)
    // {
    //     using (BinaryWriter bw = new BinaryWriter(stream))
    //     {

    //         // define the bitmap file header
    //         bw.Write((UInt16)0x4D42);                               // bfType;
    //         bw.Write((UInt32)(14 + 40 + (width * height * 4)));     // bfSize;
    //         bw.Write((UInt16)0);                                    // bfReserved1;
    //         bw.Write((UInt16)0);                                    // bfReserved2;
    //         bw.Write((UInt32)14 + 40);                              // bfOffBits;

    //         // define the bitmap information header
    //         bw.Write((UInt32)40);                               // biSize;
    //         bw.Write((Int32)width);                                 // biWidth;
    //         bw.Write((Int32)height);                                // biHeight;
    //         bw.Write((UInt16)1);                                    // biPlanes;
    //         bw.Write((UInt16)32);                                   // biBitCount;
    //         bw.Write((UInt32)0);                                    // biCompression;
    //         bw.Write((UInt32)(width * height * 4));                 // biSizeImage;
    //         bw.Write((Int32)0);                                     // biXPelsPerMeter;
    //         bw.Write((Int32)0);                                     // biYPelsPerMeter;
    //         bw.Write((UInt32)0);                                    // biClrUsed;
    //         bw.Write((UInt32)0);                                    // biClrImportant;

    //         // switch the image data from RGB to BGR
    //         for (int imageIdx = 0; imageIdx < imageData.Length; imageIdx += 3)
    //         {
    //             bw.Write(imageData[imageIdx + 2]);
    //             bw.Write(imageData[imageIdx + 1]);
    //             bw.Write(imageData[imageIdx + 0]);
    //             bw.Write((byte)255);
    //         }

    //     }
    // }

}
public class CamVideo : MonoBehaviour
{

    public bool done = false;
    public Queue<byte[]> frameQueue;

    public string persistentDataPath;
    private Thread encoderThread;
    public int maxFrames = 60;
    // Texture Readback Objects
    private RenderTexture tempRenderTexture;
    private Texture2D tempTexture2D;

    // Timing Data
    private int frameNumber;
    public int savingFrameNumber;

    Rect r;

    Action<int> cb;

    UnityEngine.UI.Image fill;

    public int w, h;

    public bool threadIsProcessing;
    private bool terminateThreadWhenDone;


    public void init(RectTransform view, RectTransform fullview, Action<int> callback, UnityEngine.UI.Image fi)
    {
        Application.targetFrameRate = 14;
        persistentDataPath = Application.persistentDataPath + "/RECORDING_LPM";

        if (!System.IO.Directory.Exists(persistentDataPath))
        {
            System.IO.Directory.CreateDirectory(persistentDataPath);
        }
        cb = callback;
        fill = fi;
        // Debug.Log("aspect : "+ Camera.main.aspect * Mathf.Clamp(view.rect.height,0,800));



        frameQueue = new Queue<byte[]>();
        r = RectTransformToScreenSpace(view, view.transform.lossyScale);
        h = (int)r.height; w = (int)r.width;
        Rect ff = RectTransformToScreenSpace(fullview, fullview.transform.lossyScale);
        int fullw = (int)ff.width; int fullh = (int)ff.height;
        if (h > 600)
        {

            float ratio = 600f / h;
            w = (int)(r.width * ratio);
            h = 600;
            fullw = (int)(fullw * ratio);
            fullh = (int)(fullh * ratio);
            // r = RectTransformToScreenSpace(view, new Vector3(ratio,ratio,1));

            //RectTransformToScreenSpace(view, new Vector3(ratio, ratio, 1));
            // Vector2 size = Vector2.Scale(view.rect.size, new Vector3(ratio, ratio, 1));

            r = new Rect(r.x * ratio, r.y * ratio, w, h);
            //1222.800
        }


        Debug.Log("stat:" + w + ", " + h);

        tempRenderTexture = new RenderTexture(fullw, fullh, 0);
        tempTexture2D = new Texture2D(w, h, TextureFormat.RGB24, false);
        tempTexture2D.hideFlags = HideFlags.HideAndDontSave;
        tempTexture2D.wrapMode = TextureWrapMode.Clamp;
        tempTexture2D.filterMode = FilterMode.Bilinear;
        tempTexture2D.anisoLevel = 0;


        threadIsProcessing = true;
        terminateThreadWhenDone = false;
        encoderThread = new Thread(EncodeAndSave);
        encoderThread.Start();

    }

    private void OnDisable()
    {
        if (threadIsProcessing)
        {
            terminateThreadWhenDone = true;
            encoderThread.Join();
        }

    }
    public Rect RectTransformToScreenSpace(RectTransform transform, Vector3 scale)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, scale);
        // size /= r;
        Rect r = new Rect((Vector2)transform.position - (size * 0.5f), size);

        return r;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (done || frameNumber > maxFrames)
        {
            terminateThreadWhenDone = true;
            cb(savingFrameNumber);
            Destroy(fill.gameObject);
            this.enabled = false;
        }

        if (frameQueue.Count < 10)
        {
            UnityEngine.Graphics.Blit(source, tempRenderTexture);
            RenderTexture.active = tempRenderTexture;
            tempTexture2D.ReadPixels(r, 0, 0);
            tempTexture2D.Apply();
            // if ((int)r.width != w)
            // {
            //     tempTexture2D.Resize(w, h, TextureFormat.RGB24, false);
            //     tempTexture2D.Apply();
            // }
            RenderTexture.active = null;
            frameQueue.Enqueue(tempTexture2D.GetRawTextureData());
            tempRenderTexture.DiscardContents();
            // if ((int)r.width != w)
            // {
            //     tempTexture2D.Resize((int)r.width, (int)r.height, TextureFormat.RGB24, false);
            //     tempTexture2D.Apply();
            // }

            frameNumber++;

        }


        UnityEngine.Graphics.Blit(source, destination);

        fill.fillAmount = (float)frameNumber / (float)maxFrames;
    }


    private void EncodeAndSave()
    {

        while (threadIsProcessing)
        {
            if (frameQueue.Count > 0)
            {
                // Generate file path
                string path = persistentDataPath + "/frame" + savingFrameNumber + ".raw";

                File.WriteAllBytes(path, frameQueue.Dequeue());

                // Done
                savingFrameNumber++;

            }

            if (terminateThreadWhenDone)
            {

                break;
            }


        }

        terminateThreadWhenDone = false;
        threadIsProcessing = false;


    }





}
