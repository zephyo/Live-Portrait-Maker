using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NotesUI : MonoBehaviour
{

    int profile = -1;
    public TMP_InputField tmp;

    public void SaveNotes()
    {
        if (profile == -1) return;
        PlayerPrefs.SetString(profile + "NOTES", tmp.text);
        PlayerPrefs.Save();
    }

    public void TurnOff()
    {
        SaveNotes();
        StartCoroutine(waitForSave());
    }
    IEnumerator waitForSave()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.interactable = false;

        yield return new WaitForSeconds(0.1f);

        LeanTween.value(Camera.main.gameObject, (float val) =>
{
    cg.alpha = val;
}, 1, 0, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
   {
       cg.blocksRaycasts = false;
   });
    }

    public void TurnOn(int p)
    {
        profile = -1;
        transform.GetChild(transform.childCount - 1).GetComponent<TMP_InputField>().text = PlayerPrefs.GetString(p + "NOTES");
        string OK, notes;
        switch (PlayerPrefs.GetInt("Lang"))
        {

            case 1:

                OK = "确定";

                notes = "注释";
                //chinese
                break;
            case 2:

                OK = "はい";

                notes = "ノート";
                //ja
                break;
            case 3:

                OK = "ОК";

                notes = "заметки";
                //rus
                break;
            case 4:
                //    SPT = "guardar retrato como:";
                OK = "ok";

                notes = "Notas";
                break;
            case 5:
                // SPT = "บันทึกภาพเป็น:";
                OK = "ตกลง";
                // cTXT = "ยกเลิก";
                // name = "ชื่อ?";
                notes = "บันทึก";
                //thai
                break;
            case 6:
                // SPT = "enregistrer le portrait sous:";
                OK = "bien";
                // cTXT = "Annuler";
                // name = "prénom?";
                notes = "Remarques";
                //thai
                break;
            default:
                // SPT = "save portrait as:";
                OK = "Ok";
                // cTXT = "Cancel";
                // name = "name?";
                notes = "Notes";

                //english
                break;
        }
        if (notes != "Notes")
        {
            transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = OK;

            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = notes;
        }
        profile = p;


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


}
