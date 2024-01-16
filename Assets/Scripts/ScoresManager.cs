using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScoresManager : MonoBehaviour
{
    public GameObject listItemPrefab;
    public Transform contentPanel;
    public float itemSpacing = 50f;

    //Этот метод выводит путем цикла значение из массива элементов, создавая новый элемент списка из префаба content и записывает данные в дочерний компонент префаба
    void Start()
    {
        float currentY = 0f;
        string[] item = File.ReadAllLines("Records.json");
        for (int i=0;i<item.Length;i++)
        {
            GameObject newListItem = Instantiate(listItemPrefab, contentPanel);
            newListItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, currentY);
            Text textComponent = newListItem.GetComponentInChildren<Text>();
            textComponent.text = item[i];

            currentY -= itemSpacing;
        }

    }

   
}
