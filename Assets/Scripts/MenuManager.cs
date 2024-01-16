using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Text RecordText;
    public InputField inputName;

    //Этот метод вызывается одновременно с экземпляром или же со сценой Menu этот метод записывает в поле input имя последнего игрока и вызывает метод выставления рекорда
    private void Awake()
    {
        string[] name = File.ReadAllLines("Name.json");
        inputName.text = name[0];
        SetRecord();
    }

    //Данный метод считывает очки из файла при помощи конвертации парсом записывает их в целочисленный массив сортирует их потом реверсирует массив и записывает в поле текст текущий рекорд
    private void SetRecord()
    {
        string[] jsonStrings = File.ReadAllLines("Scores.json");
        int[] numbers = jsonStrings.Select(s => int.Parse(s)).ToArray();
        Array.Sort(numbers);
        Array.Reverse(numbers);
        string record= numbers[0].ToString();
        RecordText.text = RecordText.text+ " \n" + record;
    }
}
