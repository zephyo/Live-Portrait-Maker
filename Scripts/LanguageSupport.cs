using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LanguageSupport : MonoBehaviour
{

    // Use this for initialization

    public void setLanguageDropdown(int i)
    {
        if (i ==1 ) i = 3;//chinese
        else if (i == 2) i = 4;//japan
        else if (i == 3) i = 5;//russian
        else if (i == 4) i = 1;//spanish
        else if (i == 5) i = 6;//thai
        else if (i == 6) i = 2;//french
        transform.GetChild(0).GetChild(5).GetComponent<TMP_Dropdown>().value = i;
    }

    public void languageWrapper(int i)
    {
                Debug.Log(i);
         if (i ==3) i = 1;//chinese
        else if (i == 4) i = 2;//japan
        else if (i == 5) i = 3;//russian
        else if (i == 1) i = 4;//spanish
        else if (i == 6) i = 5;//thai
        else if (i == 2) i = 6;//french

        ChangeLanguage(i);
    }
    public void ChangeLanguage(int i)
    {

        string[] vals;
        switch (i)
        {
            case 0:
                vals = new string[]{
            "<size=140%><b>hi there!</b></size>\n"+
"click the top or left things to get started",
            "eye",
            "eyelash",
            "eyebrow",
            "lips",
            "nose",
            "body",
            "bangs",
            "hair",
            "clothes",
            "bg",
            "misc",
            "effects",
            // "save portrait as:",
            // "ok",
            // "cancel",
            // "delete",
            // "load",
            "help/about",
            "options",
            "motion",
            "change theme",
             "<size=160%><b><color=#ffd4d1>hello~! ^_^</b></size></color>\n"+
"<size=120%>\n\n<color=#ff9e9e><b>Tap</b></color> to look\n"+
"<color=#ff9e9e><b>Double tap</b></color> to open menu</size>\n"+
"<u><color=#ffd4d1>Use your creations however you like, but do not commercialize them./u></color>\n"+
"<size=90%>This app is by <b>Angela He</b> - find me on twitter (@zephybite) + tumblr (zephyo)!",

            "customize",
            "randomize",
            "save/load portraits",
            "help/about/options",

            };
                break;
            case 1:
                vals = new string[]{

"<size=140%><b>嗨，你好!</b></size>\n"+ 
"点击上方或者左边的选项就可以开始了",
"眼睛",
"眼睛",
"眉毛",
"嘴唇",
"刘海",
"鼻子",
"身体",
"头发",
"衣服",
"背景",
"其他",
"效果",
// "保存人像为:",
// "确定",
// "取消",
// "删除",
// "打开",
"帮助/关于",
"设置",
"可动",
"主调",
"<size=160%><b><color=#ffd4d1>哈罗~! ^_^</b></size></color>\n"+
"<size=120%>\n\n<color=#ff9e9e><b>单击观看\n"+
"双击打开菜单</b></color></size>\n"+
"<u><color=#ffd4d1>你的作品可以用于你想用的任何地方，但不要将它们商业化。</u></color>\n"+
"<size=90%>开发者Angela He。 大家可以到推特(@zephybite)或者tumblr(zephyo)关注我。",


"自定义",
"随机定义",
"保存肖像",
"帮助/关于/设置",
};
                //    Debug.Log("Chinese with length " + vals.Length + "!");
                break;
            case 2:

                vals = new string[]{

                       "<size=140%><b>こんにちは！</b></size>\n上または左のものをクリックして開始する",
"眼",
"まつげ",
"眉",
"唇",
"鼻",
"体",
"前髪",
"ヘア",
"衣類",
"背景",
"雑多",
"視覚効果",
// "ポートレートを次のように保存：",
// "はい",
// "中止",
// "削除",//delete
// "開く",//load
"助けて" ,//help about
"設定",
"移動",
"色のテーマを変更する",


"<size=160%><b><color=#ffd4d1>こんにちは~! ^_^</b></size></color>\n<size=120%>\n\n"+
"<color=#ff9e9e><b>見るためにタップ\nダブルタップしてメニューを開く</b></color></size>\n"+
"<u><color=#ffd4d1>好きなようにあなたの作品を使用しますが、それらを商品化しないでください。</u></color>\n"+
"<size=90%>このアプリはAngela Heさんです - twitter（@zephybite）+ tumblr（zephyo）で私を見つけてください！",


"カスタマイズ",
"ランダム",
"ポートレート",
"助けて/設定",

                };
                break;
            case 3:


                vals = new string[]{
"<size=140%><b>Всем привет!</b></size>\nнажмите на верхнюю или левую часть, чтобы начать",
"глаз",
"ресница",
"Бровь",
"губы",
"Нос",
"тело",
"челка",
"Волосы",
"одежда",
"сцена",
"Разное"
,"эффекты"
// ,"Сохранить портрет как:"
// ,"ОК"
// ,"Отмена"
// ,"Удалить"
// ,"Загрузить файл"
,"Помощь/о"
,"настройки"
,"движение"
,"Менять тему",

"<size=160%><b><color=#ffd4d1>Здравствуйте~! ^_^</b></size></color>\n<size=120%>\n\n\n"+
"<color=#ff9e9e><b>Нажмите, чтобы посмотреть\nДважды нажмите, чтобы открыть меню</b></color></size>\n"+
"<u><color=#ffd4d1>Используйте свои творения, как вам нравится, но не коммерциализируйте их.</u></color>\n"+
"<size=90%>Это приложение Angela He - найдите меня на twitter (@zephybite) + tumblr (zephyo)!"


,"изменение"
,"рандомизации"
,"Сохранить Портрет",
"Помощь/о/настройки",


 };
                Debug.Log("Russian with length " + vals.Length + "!");
                break;
            case 4:
                vals = new string[]{
                 "<size=140%><b>¡Hola!</b></size>\n" +
"haz clic en la parte superior o izquierda para comenzar",
"ojo",
"pestaña",
"ceja",
"labios",
"nariz",
"cuerpo",
"flequillo",
"cabello",
"ropa",
"ambiente",
"misc",
"efectos",
            // "guardar retrato como:",
            // "ok",
            // "cancelar",
            // "borrar",
            // "cargar",
            "ayuda/sobre",
"opciones",
"movimiento",
"cambiar el tema",
"<size=160%><b><color=#ffd4d1> hola~! ^_^</b></size></color>\n" +
"<size=120%>\n\n<color=#ff9e9e><b>Pulse</b></color> para buscar\n" +
"<color=#ff9e9e><b>Toca dos veces</b></color> para abrir el menú</size>\n" +
"<u><color=#ffd4d1>Usa tus creaciones como quieras, pero no las comercialices.</u></color>\n" +
"<size=90%>Esta aplicación es de <b>Angela He</b> - ¡encuéntreme en Twitter (@zephybite) + tumblr (zephyo)!",

"<nobr>personalizar</nobr>",
"aleatorizar",
"Guardar carga",
"ayuda/sobre/opciones",
              };
                break;
            case 5:
                vals = new string[]{
"<size=140%><b>สวัสดี!</b></size>\nคลิกที่ด้านบนหรือด้านซ้า\nยเพื่อเริ่มต้นใช้งาน",
"ตา",
"ขนตา",
"คิ้ว",
"โอษฐ์",
"จมูก",
"ร่างกาย",
"หน้าม้า",
"ผม",
"เสื้อผ้า",
"พื้นหลัง",
"อื่น ๆ",
"เอฟเฟก",
// "บันทึกภาพเป็น:",
// "ตกลง",
// "ยกเลิก",
// "ลบ",
// "โหลด",
"ช่วยเหลือ/เกี่ยวกับ",
"การตั้งค่า",
"การเคลื่อนไหว",
"เปลี่ยนธีม",

"<size=160%><b><color=#ffd4d1>สวัสดี~! ^_^</b></size></color>\n<size=120%>\n\n"+
"<color=#ff9e9e><b>แตะเพื่อดู\nแตะสองครั้งเพื่อเปิดเมนู</b></color></size>\n"+
"<u><color=#ffd4d1>ใช้งานสร้างสรรค์ของคุณ แต่คุณต้องการ แต่อย่าทำเป็นเชิงพาณิชย์</u></color>\n"+
"<size=90%>แอปพลิเคชันนี้เป็นโดย Angela He - หาฉันใน twitter (@ zephybite) + tumblr (zephyo)!",

"ปรับแต่ง",
"สุ่ม",
"บันทึกภาพบุคคล",
"ช่วยเหลือ/เกี่ยวกับ/การตั้งค่า",

                 };
                break;
            case 6:
                vals = new string[]{
            "<size=140%><b>salut là-bas!</b></size>\n"+
"cliquez sur les choses en haut ou à gauche pour commencer",
         "œil",
"cil",
"sourcil",
"lèvres",
"nez",
"corps",
"frange",
"cheveux",
"habits",//èê
            "scène",
"misc",
"effets",
            // "enregistrer le portrait sous:",
            // "D'accord",
            // "Annuler",
            // "effacer",
            // "charge",
            "aide/sur",
"options",
"mouvement",
"change le thème",
             "<size=160%><b><color=#ffd4d1>bonjour~! ^_^</b></size></color>\n\n"+
"<size=120%>\n\n<color=#ff9e9e><b>Appuyez sur</b></color> pour rechercher\n"+
"<color=#ff9e9e><b>Appuyez deux fois sur</b></color> pour ouvrir le menu</size>\n"+
"<u><color=#ffd4d1>Utilisez vos créations comme vous le souhaitez, mais ne les commercialisez pas.</u></color>\n"+
"<size=90%>Cette application est par <b>Angela He</b> - me trouver sur twitter (@zephybite) + tumblr (zephyo)!",

        "personnaliser",
"randomiser",
"enregistrer des portraits",
"aide/sur/options",
            };
                break;
                case 7:
                 vals = new string[]{
                 "<size=140%><b>안녕하세요!</b></size>\n"+
"시작하려면 상단 또는 왼쪽을 클릭하십시오",
            "눈",
            "속눈썹",
            "눈썹",
            "입술",
            "코",
            "신체",
            "앞머리",
            "머리",
            "천",
            "bg",
            "기타",
            "효과",
            // "다음과 같이 사진 저장 :",
            // "ok",
            // "cancel",
            // "delete",
            // "로드",
            "도움",
            "옵션",
            "운동",
            "테마 변경",
             "<size=160%><b><color=#ffd4d1>hello ~! ^_^</b></size></color>\n"+
"<size=120%>\n\n<color=#ff9e9e><b>보기</b></color> "+
"<color=#ff9e9e><b>두 번 탭</b></color>하여 메뉴 </size>를여십시오.\n"+
"<u><color=#ffd4d1>원하는대로 작품을 사용하되 상용화하지 마십시오 .</u></color>\n"+
"<size=90%>이 응용 프로그램은 <b>Angela He</b> 님 - 트위터 (@zephybite) + tumblr (zephyo)에서 나를 찾습니다!",

            "사용자 정의",
            "무작위 화",
            "인물 저장 / 불러 오기",
            "help / about / options",
                 };
                break;

            default:
                return;
        }
        //get all relevant buttons
        List<TextMeshProUGUI> t = GetLanguageInputs();
        for (int index = 0; index < t.Count; index++)
        {
            if (t[index] != null)
                t[index].text = vals[index];
        }

        PlayerPrefs.SetInt("Lang", i);
        PlayerPrefs.Save();


        DressManager dm = GetComponent<DressManager>();
        if (dm.cpa != null)
        {
            setMaster(i, dm.cpa.transform);
        }

    }

    public void setMaster(int i, Transform cpa)
    {
        string remove;
        TextMeshProUGUI r = cpa.transform.parent.GetChild(4).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        switch (i)
        {
            case 1:
                //chinese
                remove = "删除";
                break;
            case 2:
                //ja
                remove = "削除";
                break;
            case 3:
                //rus
                remove = "Удалить";
                break;
            case 4:
                //thai
                remove = "borrar";
                break;
            case 5:
                //thai
                remove = "ลบ";
                break;
            case 6:
                remove = "effacer";
                break;

            default:
                //english
                remove = "remove";
                break;
        }
        r.text = remove;



    }


    List<TextMeshProUGUI> GetLanguageInputs()
    {
        List<TextMeshProUGUI> t = new List<TextMeshProUGUI>();
        Transform dressup = transform.GetChild(0);
        bool welcome = dressup.GetChild(1).childCount > 2;
        t.Add(welcome ? dressup.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>() : null);
        Transform content = dressup.GetChild(0).GetChild(0);
        for (int i = 0; i < content.childCount; i++)
        {
            t.Add(content.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>());
        }
        //             "save portrait as:",
        //             "ok",
        //             "cancel",
        //             "delete",
        //             "load",
        t.Add(dressup.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>());

        t.Add(dressup.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>());

        t.Add(dressup.GetChild(6).GetComponent<TextMeshProUGUI>());

        t.Add(dressup.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>());
        Transform about = dressup.GetChild(8);
        //             "<size=160%><b><color=#ffd4d1>hello~! ^_^</b></size></color>\n"+
        // "<size=120%>\n\n<color=#ff9e9e><b>Tap</b></color> to look\n"+
        // "<color=#ff9e9e><b>Double tap</b></color> to open menu</size>\n"+
        // "<u><color=#ffd4d1>Use your creations however you like</u></color> - for profile pictures, character designs, and anything else.\n"+
        // "<size=90%>This app is by <b>Angela He</b> - find me on twitter (@zephybite) + tumblr (zephyo)!"
        t.Add(about.GetComponent<TextMeshProUGUI>());
        //             "customize",
        //             "randomize",
        //             "save/load portraits",
        //             "help/about/options",
        if (about.childCount > 0)
        {
            for (int i = 0; i < about.childCount; i++)
            {
                Transform c = about.GetChild(i);
                if (c.childCount > 0) t.Add(c.GetChild(0).GetComponent<TextMeshProUGUI>());
            }
        }
        return t;

    }
}
