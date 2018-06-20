using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadUI : MonoBehaviour
{

    int profile = -1;
    NotesUI nU;


    public void StartNotesEditor()
    {
        if (profile == -1) return;
        nU.TurnOn(profile);
    }



    public void SaveName(string name)
    {
        if (profile == -1) return;
        PlayerPrefs.SetString("SAVE" + profile, name);
        PlayerPrefs.Save();

    }

    public void TurnOff()
    {
        transform.root.GetChild(0).GetComponent<CanvasGroup>().interactable = true;
        CanvasGroup cg = GetComponent<CanvasGroup>();
        LeanTween.value(Camera.main.gameObject, (float val) =>
  {
      cg.alpha = val;
  }, 1, 0, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
   {
       cg.interactable = false;
       cg.blocksRaycasts = false;
   });
    }

    public void TurnOn(int p, SaveManager sm, NotesUI notesU, Button caller)
    {
        profile = -1;
        string updateTXT, lTXT, cancelTXT;
        nU = notesU;
        switch (PlayerPrefs.GetInt("Lang"))
        {
            // "cancel",
            // "delete",
            // "load",

            case 1:
                updateTXT = "更新";
                lTXT = "打开";
                cancelTXT = "取消";
                //chinese
                break;
            case 2:
                updateTXT = "更新";
                lTXT = "開く";
                cancelTXT = "中止";
                //ja
                break;
            case 3:
                updateTXT = "Обновить";
                lTXT = "Загрузить файл";
                cancelTXT = "Отмена";
                //rus
                break;

            case 4:
                updateTXT = "actualizar";
                lTXT = "cargar";
                cancelTXT = "cancelar";
                //spanish
                break;
            case 5:
                updateTXT = "ปรับปรุง";
                lTXT = "โหลด";
                cancelTXT = "ยกเลิก";
                //thai
                break;
            case 6:
                updateTXT = "réactualiser";
                lTXT = "charge";
                cancelTXT = "Annuler";
                //french
                break;
            default:
                updateTXT = "update";
                lTXT = "Load";
                cancelTXT = "Cancel";
                //english
                break;
        }


        Button load, update, delete, notesOk;

        load = transform.GetChild(0).GetComponent<Button>();
        update = transform.GetChild(1).GetComponent<Button>();
        delete = transform.GetChild(2).GetComponent<Button>();
        notesOk = nU.transform.GetChild(0).GetComponent<Button>();


        TMP_InputField name = transform.GetChild(4).GetComponent<TMP_InputField>();
        transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString(p + "NOTES");
        name.text = PlayerPrefs.GetString("SAVE" + p);

        if (cancelTXT != "Cancel")
        {
            load.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lTXT;
            update.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = updateTXT;
            transform.GetChild(transform.childCount - 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = cancelTXT;
        }
        setButtonColors(load, update);
        //listeners stuff
        removeAllListeners(load, update, delete, name, notesOk);
        load.onClick.AddListener(() =>
        {
            sm.Load(p);
        }
        );


        update.onClick.AddListener(() =>
                {
                    Debug.Log("update "+p);
                    sm.DeleteSubstance(p);
                    sm.Save(null, p, 1);
                }
                );


        delete.onClick.AddListener(() =>
            {
                sm.Delete(p);
                Destroy(caller.gameObject);

            });

        notesOk.onClick.AddListener(() =>
        {
           transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = nU.transform.GetChild(nU.transform.childCount-1).GetComponent<TMP_InputField>().text;
       
        });



        name.onEndEdit.AddListener((string val) =>
        {
            caller.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = val;
        });




        CanvasGroup cg = GetComponent<CanvasGroup>();

        profile = p;
        cg.interactable = true;
        cg.blocksRaycasts = true;

        LeanTween.value(Camera.main.gameObject, (float val) =>
{
    cg.alpha = val;
}, 0, 1, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
{

});


    }

    void removeAllListeners(Button load, Button update, Button delete, TMP_InputField ifd, Button n)
    {
        load.onClick.RemoveAllListeners();
        update.onClick.RemoveAllListeners();
        delete.onClick.RemoveAllListeners();
        ifd.onEndEdit.RemoveAllListeners();
        n.onClick.RemoveAllListeners();
    }


    void setButtonColors(Button load, Button update)
    {
        ColorBlock cb = load.colors;

        cb.normalColor = new Color32((byte)Mathf.Clamp(PlayerPrefs.GetInt("themeR"), 160, 200), (byte)Mathf.Clamp(PlayerPrefs.GetInt("themeG"), 100, 200), (byte)Mathf.Clamp(PlayerPrefs.GetInt("themeB"), 90, 200), 255);
        cb.highlightedColor = cb.normalColor;
        load.colors = cb;
        update.colors = cb;
    }

}
