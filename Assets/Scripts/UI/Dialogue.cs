using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue: MonoBehaviour
{
    [Header("UI���")]
    public TMP_Text TextLabel;
    public Image Face;
    [Header("�ı�����")]
    public TextAsset TextFile;
    public Sprite face1;
    public Sprite face2;
    public int Index;
    public float TextSpeed=0.1f;

     List<string>textList= new List<string>();
     bool textIsFinshed;
    private void Awake()
    {
        textIsFinshed = true;
        GetTextFromFile(TextFile);
        Index = 0;
    }
    private void OnEnable()
    {
        /*TextLabel.text = textList[Index];
        Index++;*/
        StartCoroutine(SetTextUI());
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F) && Index == textList.Count)
        {
           
            gameObject.SetActive(true);
            Index = 0;
            return;
        }
        if (Input.GetKeyDown(KeyCode.F)&&textIsFinshed)
        {
            /*Debug.Log(1);
            TextLabel.text = textList[Index];
            Index++;*/
            StartCoroutine(SetTextUI());
        }
        
    }

    void GetTextFromFile(TextAsset textAsset)
    {
        textList.Clear();//Ϊ��ֹÿ�ε��÷���ʱ��lsit�����ݲ��ϱ��
        Index = 0;
        var textline = textAsset.text.Split('\n');
        foreach (var line in textline)
        {
            textList.Add(line);
        }
    }
    IEnumerator SetTextUI()
    {   textIsFinshed=false;
        TextLabel.text = "";
        switch (textList[Index])
        {
            case "A":
                Face.sprite = face1;
                Index++;
                break;
            case "B":
                Face.sprite = face2;
                Index++;
                break;
        }
        //��ȡÿһ���ַ�
        for (int i = 0; i < textList[Index].Length; i++)
        {
            TextLabel.text += textList[Index][i];
            yield return new WaitForSeconds(TextSpeed);
        }
        textIsFinshed = true;
        Index++;
    }
}
