using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System;
[SerializeField]
public class item
{
    public string no;
    public int no_int;
    public int frame = 0;
    public string question;
    public string answer;
    public string equestion;
    public string eanswer;
    public item(int _i, int _f, string _q, string _a, string _eq, string _ea)
    {
        no = "s" + _i.ToString("0000");
        no_int = _i;
        frame = _f;
        question = _q;
        answer = _a;
        equestion = _eq;
        eanswer = _ea;
    }
    public item(string _i, string _f, string _q, string _a, string _eq, string _ea)
    {
        int i = int.Parse(_i);

        no = "s" + i.ToString("0000");
        no_int = i;
        frame = int.Parse(_f);
        question = _q;
        answer = _a;
        equestion = _eq;
        eanswer = _ea;
    }
    
    
}

public class CSVReader
{

    public static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    public static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    public static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}


 
public class go_data : MonoBehaviour
{
    public AnimationClip ac;
    public Text questionText;
    
    public List<item> items = new List<item>();
    public List<item> items_long = new List<item>();
    public string _exp;
    public int category = 0;
    public List<int> dataAll = new List<int>();  //질문종류 목록
    public DF2ClientAudioTester dF;
    public lerp mylerp;
    public List<int> questionNo3 = new List<int>();
    // Start is called before the first frame update

    public int getFrame(string _no)
    {
        //Debug.Log(items.Count);
        int frame = 0;
        foreach (item i in items)
        {
             
            if (i.no_int.ToString() == _no)
            {

                Debug.Log("getFrame " + _no + " " + i.frame);
                frame = i.frame;

            }

        }
        return frame;
    }
    public string GetQuestion(int  _i)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].no_int == _i)
            {
                return items[i].question;
            }
        }
        return "--";
    }

    public void Pop3() //3개 뽑기
    {

        dataAll.Clear();
        //switch (category) {
        //    case 0:
        //        for (int i = 1; i < 1001; i++) { dataAll.Add(i); }
        //        break;
        //    case 1:
        //        for (int i = 1; i < 33; i++) { dataAll.Add(i); }
        //        for (int i = 901; i < 1001; i++) { dataAll.Add(i); }
        //        break;
        //    case 2:
        //        for (int i = 33; i < 267; i++) { dataAll.Add(i); }
        //        break;
        //    case 3:
        //        for (int i = 267; i < 408; i++) { dataAll.Add(i); }
        //        break;
        //    case 4:
        //        for (int i = 408; i < 900; i++) { dataAll.Add(i); }
        //        break;
        //    case 5:
        //        for (int i = 1; i < 1001; i++) { dataAll.Add(i); }
        //        break;
        //}
        //if (category == 5)
        //{

        //        int i = Random.Range(1, 31);
        //        int j = Random.Range(491, 527);
        //        int k = Random.Range(901, 1000);
        //        questionNo3[0] = dataAll[i];
        //        questionNo3[1] = dataAll[j];
        //        questionNo3[2] = dataAll[k];

        //}
        //else
        //{
        //    for (int j = 0; j < 3; j++)
        //    {

        //        int i = Random.Range(0, dataAll.Count);

        //        questionNo3[j] = dataAll[i];
        //        dataAll.RemoveAt(i);
        //    }
        //}
        for (int i = 0; i < items.Count; i++) { 
            if(items[i].no_int < 1000)
                dataAll.Add( items[i].no_int); 
        }
        //for (int j = 0; j < 3; j++)
        //{
        //    int i = Random.Range(0, dataAll.Count);

        //    questionNo3[j] = dataAll[i];
        //    dataAll.RemoveAt(i);
        //}
         
        questionNo3 = dataAll.OrderBy(arg => Guid.NewGuid()).Take(3).ToList();
        



    }
 
    void Start()
    {
        //리스트 만들기



        //List<Dictionary<string, object>> data = CSVReader.Read("gojong_all_han_eng");
        List<Dictionary<string, object>> data = CSVReader.Read("go_v2_csv");

        for (var i = 0; i < data.Count; i++)
        {
            //Debug.Log("index " + (i).ToString() + " : " + data[i]["id"] + " " + data[i]["frame"] + " " + data[i]["q"]);
            items.Add(
                new item( 
                    data[i]["id"].ToString(), 
                    data[i]["frame"].ToString(), 
                    data[i]["q"].ToString(), 
                    data[i]["a"].ToString(),
                    data[i]["eq"].ToString(),
                    data[i]["ea"].ToString()
                )
                );
        }
        //SetCategory(5);
        category = 5;
        Pop3();


        //init();
        //if (questionText) StartCoroutine(QuestionRef());
    }
    public void QuestionRef() //질문 바꾸기
    {

        Pop3(); //3개 랜덤넘버 추출

        //if (dF.isEnglish)
        //questionText.text = "- " + items[questionNo3[0]].equestion + "\n- " + items[questionNo3[1]].equestion + "\n- " + items[questionNo3[2]].equestion;
        //else

        //Debug.Log("questionNo3                    " + questionNo3.Count);

        questionText.text = "- " + GetQuestion(questionNo3[0]) + "\n- ";
        questionText.text = questionText.text  + GetQuestion(questionNo3[1]) + "\n- ";
        questionText.text = questionText.text  + GetQuestion(questionNo3[2]);
       
        mylerp.last_change_time = Time.time;
    }
    public void SetCategory(int _i)//카테고리변경
    {
        category = _i;
        QuestionRef();
    }


    public void SaveTxt(string _q, string _a)
    {
        string text = File.ReadAllText("E:/go/facial/temp.json");
        text = text.Replace("_txt01_", _q);
        text = text.Replace("_txt02_", _a);
        File.WriteAllText("E:/go/facial/NewAgent/intents/"+ _q  + ".json", text);

        string text2 = File.ReadAllText("E:/go/facial/temp_usersays_ko.json");
        text2 = text2.Replace("_txt03_", _q);
        File.WriteAllText("E:/go/facial/NewAgent/intents/" + _q + "_usersays_ko.json", text2);

        Debug.Log(text);
         


    }

}



