using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class portraitUI : MonoBehaviour
{


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

    public void TurnOn(SaveManager sm, NotesUI nu, int p)
    {
        string SPT, OK, cTXT, name, AddNotes;
        switch (PlayerPrefs.GetInt("Lang"))
        {

            case 1:
                SPT = "保存人像为:";
                OK = "确定";
                cTXT = "取消";
                name = "名称？";
                AddNotes = "添加注释";
                //chinese
                break;
            case 2:
                SPT = "名前を付けて保存：";
                OK = "はい";
                cTXT = "中止";
                name = "名？";
                AddNotes = "ノート";
                //ja
                break;
            case 3:
                SPT = "сохранить как:";
                OK = "ОК";
                cTXT = "Отмена";
                name = "имя?";
                AddNotes = "заметки";
                //rus
                break;
            case 4:
                SPT = "guardar retrato como:";
                OK = "ok";
                cTXT = "cancelar";
                name = "¿nombre?";
                AddNotes = "agregar notas";
                break;
            case 5:
                SPT = "บันทึกภาพเป็น:";
                OK = "ตกลง";
                cTXT = "ยกเลิก";
                name = "ชื่อ?";
                AddNotes = "เพิ่มบันทึก";
                //thai
                break;
            case 6:
                SPT = "enregistrer le portrait sous:";
                OK = "bien";
                cTXT = "Annuler";
                name = "prénom?";
                AddNotes = "ajouter notes";
                //french
                break;
            default:
                SPT = "save portrait as:";
                OK = "Ok";
                cTXT = "Cancel";
                name = "name?";
                AddNotes = "add notes";

                //english
                break;
        }

        Button ok, addnotes;

        ok = transform.GetChild(0).GetComponent<Button>();
        addnotes = transform.GetChild(1).GetComponent<Button>();

        if (AddNotes != "add notes")
        {
            ok.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = OK;
            addnotes.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = AddNotes;
            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = SPT;
            transform.GetChild(transform.childCount - 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = cTXT;
        }

        TMP_InputField t = transform.GetChild(3).GetComponent<TMP_InputField>();

        setButtonColors(ok, addnotes);
        //listeners stuff
        removeAllListeners(ok, addnotes);


        ok.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(t.text))
            {
                sm.Save(t, p);
                TurnOff();

            }
            else
            {
                t.text = name;
            }
        }
        );

    
        addnotes.onClick.AddListener(() =>
    {
        nu.TurnOn(p);
    });

        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.interactable = true;
        cg.blocksRaycasts = true;

        LeanTween.value(Camera.main.gameObject, (float val) =>
{
    cg.alpha = val;
}, 0, 1, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
{

});




    }

    void removeAllListeners(Button load, Button update)
    {
        load.onClick.RemoveAllListeners();
        update.onClick.RemoveAllListeners();
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
