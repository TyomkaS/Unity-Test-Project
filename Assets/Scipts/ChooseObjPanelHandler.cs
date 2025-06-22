using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseObjPanelHandler : MonoBehaviour
{
    //Этот класс создан, т.к. не получалось обратится к UI элементу DropDown (и вообще он вызывал ошибку при клике по нему, т.к. удалялся(не понятно когда))
    public delegate void ObjectChoosed(GameObject obj);
    public static event ObjectChoosed onObjectButtonClick;

    private Button LChooserButton;
    private Button ObjectButton;
    private Button RChooserButton;
    private GameObject[] Objects;
    private bool isAllnull = true;
    private int ObjectsIterator;

    void Awake()
    {
        ObjectsInitializer();

        LChooserButton = transform.GetChild(0).GetComponent<Button>();
        ObjectButton = transform.GetChild(1).GetComponent<Button>();
        RChooserButton = transform.GetChild(2).GetComponent<Button>();

    }

    private void Start()
    {
        LChooserButton.onClick.AddListener(LButtonClickHandler);
        ObjectButton.onClick.AddListener(ObjectButtonClickHandler);
        RChooserButton.onClick.AddListener(RButtonClickHandler);

        ObjectsIterator = 0;
        ObjectButtonSetText();

    }

    private void LButtonClickHandler()
    {
        if (Objects!=null)
        {
            if (ObjectsIterator > -1)
            {
                ObjectsIterator--;
                ObjectButtonSetText();
            }
        }
    }

    private void ObjectButtonClickHandler()
    {
        Debug.Log("ObjButtonClick ");
        if (ObjectsIterator != -1)
        {
            onObjectButtonClick?.Invoke(Objects[ObjectsIterator]);
        }
        else
        {
            onObjectButtonClick?.Invoke(null);
        }   
    }

    private void RButtonClickHandler()
    {
        if (Objects != null)
        {
            if (ObjectsIterator < Objects.Length - 1)
            {
                ObjectsIterator++;
                ObjectButtonSetText();
            }
        }
    }
    private void ObjectButtonSetText()
    {
        if (Objects != null || isAllnull)
        {
            if (ObjectsIterator == -1)
            {
                ObjectButton.GetComponentInChildren<Text>().text = "None";
            }
            else
            {
                ObjectButton.GetComponentInChildren<Text>().text = Objects[ObjectsIterator].name;
            }
        }
        else
        {
            ObjectButton.GetComponentInChildren<Text>().text = "Empty List";
        }
    }

    private void ObjectsInitializer()
    {
        /*
         * Метод сделан потому что, если использовать Objects = GameObject.FindGameObjectsWithTag("Test");
         * массив заполняется непонятно в каком подрядке 
        */
        Objects = new GameObject[8];
        //Objects[0] = null;

        for (int i = 0; i < Objects.Length; i++)
        {
            Debug.Log("iteration"+ i+1.ToString() + "|Object to find is:" + "Object" + (i+1).ToString());
            Objects[i] = GameObject.Find("Object" + (i+1).ToString());
            if (Objects[i]!=null)
            {
                isAllnull = false;
            }
        }
    }
}
