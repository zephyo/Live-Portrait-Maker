using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{

    private void Start()
    {
        Button help = transform.GetChild(0).GetComponent<TabManager>().buttons[3];
        help.onClick.AddListener(() => LoadAbout(help));
    }

    public void Motion(bool on)
    {
        PlayerPrefs.SetInt("motion", on ? 0 : 1);
        PlayerPrefs.Save();
        GetComponent<DressManager>().fm.doMotion(on);
    }

    public void LoadAbout(Button remove)
    {
        TextMeshProUGUI about =
            transform.GetChild(0).GetChild(8).GetComponent<TextMeshProUGUI>();

        Vector2 a = new Vector2(0.5f, 0.89f);
        string[] titles;


        switch (PlayerPrefs.GetInt("Lang"))
        {

            case 1:
                titles = new string[]{
              "自定义",
                "随机定义",
                "保存肖像",
                "帮助/关于/设置",
            };
                //chinese
                break;
            case 2:
                titles = new string[]{
"カスタマイズ",
"ランダム",
"ポートレート",
"助けて/設定",
            };
                //ja
                break;
            case 3:
                titles = new string[]{
"изменение"
,"рандомизации"
,"Сохранить Портрет",
"Помощь/о/настройки",
            };
                //rus
                break;
            case 4:
                titles = new string[]{

"<nobr>personalizar</nobr>",
"aleatorizar",
"Guardar carga",
"ayuda/sobre/opciones",

            };
                //thai
                break;
            case 5:
                titles = new string[]{

                    "ปรับแต่ง",
"สุ่ม",
"บันทึกภาพบุคคล",
"ช่วยเหลือ/การตั้งค่า",

            };
                //thai
                break;
            case 6:
                titles = new string[]{
"personnaliser",
"randomiser",
"enregistrer des portraits",
"aide/sur/options",
                   };
                break;
            default:
                titles = new string[]{
 "customize",
                    "randomize",
                    "save/load portraits",
                    "help/about/options"
            };
                //english
                break;
        }


        TextMeshProUGUI descript = GameObject.Instantiate(about, about.transform, false).GetComponent<TextMeshProUGUI>();
        descript.gameObject.SetActive(true);
        descript.fontStyle = FontStyles.Underline;
        descript.enableAutoSizing = false;
        descript.fontSize = 38.5f;
        descript.lineSpacing = 0;

        TabManager tm = transform.GetChild(0).GetComponent<TabManager>();
        //250
        int i = 0;
        descript.text = titles[i];
        Image tab = GameObject.Instantiate(tm.buttons[i].transform.GetChild(0).gameObject, about.transform, false).GetComponent<Image>();
        tab.color = Color.white;
        tab.rectTransform.anchorMax = a;
        tab.rectTransform.anchorMin = a;
        tab.rectTransform.anchoredPosition = new Vector2(-375 + i * 250, -45.5f);

        descript.transform.SetParent(tab.transform, false);

        descript.rectTransform.anchorMin = new Vector2(0.5f, 0);
        descript.rectTransform.anchorMax = descript.rectTransform.anchorMin;
        descript.rectTransform.sizeDelta = new Vector2(248.6f, 143.1f);
        descript.rectTransform.anchoredPosition = new Vector2(0, -97.5f);

        i++;

        for (; i < 4; i++)
        {

            tab = GameObject.Instantiate(tm.buttons[i].transform.GetChild(0).gameObject, about.transform, false).GetComponent<Image>();
            tab.color = Color.white;
            tab.rectTransform.anchorMax = a;
            tab.rectTransform.anchorMin = a;
            tab.rectTransform.anchoredPosition = new Vector2(-375 + i * 250, -45.5f);

            descript = GameObject.Instantiate(descript, tab.transform, false).GetComponent<TextMeshProUGUI>();
            descript.text = titles[i];
            // descript.rectTransform.anchoredPosition = new Vector2(0, -97.5f);


        }
        remove.onClick.RemoveAllListeners();
    }
}
