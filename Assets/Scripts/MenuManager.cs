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

    //���� ����� ���������� ������������ � ����������� ��� �� �� ������ Menu ���� ����� ���������� � ���� input ��� ���������� ������ � �������� ����� ����������� �������
    private void Awake()
    {
        string[] name = File.ReadAllLines("Name.json");
        inputName.text = name[0];
        SetRecord();
    }

    //������ ����� ��������� ���� �� ����� ��� ������ ����������� ������ ���������� �� � ������������� ������ ��������� �� ����� ����������� ������ � ���������� � ���� ����� ������� ������
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
