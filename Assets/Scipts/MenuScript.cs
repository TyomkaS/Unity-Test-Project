using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void IsOutUIElem(MenuScript ms,bool value);
    public static event IsOutUIElem onOutElem;
    public static event IsOutUIElem onInElem;

    public delegate void TargetObg(MenuScript ms, GameObject obj);
    public static event TargetObg onTurnAround;

    public delegate void StopTurnArondTarget();
    public static event StopTurnArondTarget offTurnAround;

    public delegate void TargetObjOut();
    public static event TargetObjOut onTargetOut;

    //public Dropdown dropD;

    private Text ObjectCalledText;
    private Button LookAroundButton;
    private Button HideButton;
    private Button CloseObjectButton;
    private Button HidePanelButton;
    private Button ShowPanelButton;
    private Button UnHideButton;

    private Slider TransparencySlider;
    private Slider RedColorSlider;
    private Slider GreenColorSlider;
    private Slider BlueColorSlider;

    private Text TransparencyTextValue;
    private Text RedTextValue;
    private Text GreenTextValue;
    private Text BlueTextValue;

    private GameObject PanelMenu;
    private GameObject SelectedObj;
    private Color SelectedObjcolor;

    private bool isGettingNewObject;
    public bool isTurnAround;

    private void Awake()
    {
        Debug.Log("MenuScript Awake Started ========= ");
        Debug.Log("PanelMenu Object Initialisation=== ");
        //Инициализация оббъектов расположенных на PanelMenu
        //Кнопки
        HideButton = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        CloseObjectButton = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        LookAroundButton = transform.GetChild(0).GetChild(2).GetComponent<Button>();
        UnHideButton = transform.GetChild(0).GetChild(18).GetComponent<Button>();
        //Слайдеры
        TransparencySlider = transform.GetChild(0).GetChild(6).GetComponent<Slider>();
        SetSliderValueLimit(TransparencySlider,0f, 1f);
        RedColorSlider = transform.GetChild(0).GetChild(7).GetComponent<Slider>();
        SetSliderValueLimit(RedColorSlider, 0f, 1f);
        GreenColorSlider = transform.GetChild(0).GetChild(8).GetComponent<Slider>();
        SetSliderValueLimit(GreenColorSlider, 0f, 1f);
        BlueColorSlider = transform.GetChild(0).GetChild(9).GetComponent<Slider>();
        SetSliderValueLimit(BlueColorSlider, 0f, 1f);
        //Подписи к слайдерам
        TransparencyTextValue = transform.GetChild(0).GetChild(14).GetComponent<Text>();
        RedTextValue = transform.GetChild(0).GetChild(15).GetComponent<Text>();
        GreenTextValue = transform.GetChild(0).GetChild(16).GetComponent<Text>();
        BlueTextValue = transform.GetChild(0).GetChild(17).GetComponent<Text>();

        //Debug.Log("Hide button name = " + HideButton.name);
        //Debug.Log("CloseObject button name = " + CloseObjectButton.name);
        //Debug.Log("LookAround button name = "+ LookAroundButton.name);
        //Debug.Log("UnHide button name = " + UnHideButton.name);

        //Инициализация ObjectCalledText
        ObjectCalledText = transform.GetChild(0).GetChild(3).GetComponent<Text>();
        //Debug.Log("Object Called Text name = " + ObjectCalledText.name);

        //Инициализация кнопок расположенных на Canvas
        //Debug.Log("Canvas Object Initialisation====== ");
        HidePanelButton = transform.GetChild(3).GetComponent<Button>();
        ShowPanelButton = transform.GetChild(4).GetComponent<Button>();
        //Debug.Log("Hide Panel Button name = " + HidePanelButton.name);
        //Debug.Log("Show Panel Button name = " + ShowPanelButton.name);

        //Инициализация панели PanelMenu расположенной на Canvas
        PanelMenu = GameObject.Find("PanelMenu");
        //Debug.Log("Panel Menu name = " + PanelMenu.name);

        Debug.Log("MenuScript Awake Finished ======== ");
    }
    // Start is called before the first frame update
    void Start()
    {
        
        // Добавление прослушивателей на кнопки в PanelMenu
        HideButton.onClick.AddListener(HideObject);
        CloseObjectButton.onClick.AddListener(CloseObject);
        LookAroundButton.onClick.AddListener(LookAround);
        UnHideButton.onClick.AddListener(UnHideObject);
        UnHideButton.gameObject.SetActive(false);       //Изначально устанавливается значение false
        // Добавление прослушивателей на кнопки в скрывающих и показывающих PanelMenu
        HidePanelButton.onClick.AddListener(HidePanelMenu);
        ShowPanelButton.onClick.AddListener(ShowPanelMenu);
        ShowPanelButton.gameObject.SetActive(false);       //Изначально устанавливается значение false
        //Добавление прослушивателей на Sliders
        TransparencySlider.onValueChanged.AddListener(onTransperentSliderValueChange);
        RedColorSlider.onValueChanged.AddListener(onRedSliderValueChange);
        GreenColorSlider.onValueChanged.AddListener(onGreenSliderValueChange);
        BlueColorSlider.onValueChanged.AddListener(onBlueSliderValueChange);
        TransparencyTextValue.text="0";
        RedTextValue.text = "0";
        GreenTextValue.text = "0";
        BlueTextValue.text = "0";

        //Установка значения Text (выбранного объекта)
        SelectedObj = null;
        ObjectCalledText.text = "None";
        //dropdown.value = 0;

        isTurnAround = false;
        isGettingNewObject = false;

    }

    public void CloseObject()
    {
        Debug.Log("Close Object works");

        //При сбросе объекта активность/не активность этих кнопок устанавливается в начальное положение
        HideButton.gameObject.SetActive(true);
        UnHideButton.gameObject.SetActive(false);

        //Обнуление SelectedObj и ObjectCalledText
        SelectedObj = null;
        ObjectCalledText.text = "None";
        onTargetOut?.Invoke();
    }

    public void HideObject()
    {
        if (SelectedObj!=null)
        {
            Debug.Log("Hide Object works");
            UnHideButton.gameObject.SetActive(true);
            HideButton.gameObject.SetActive(false);
            SelectedObj.SetActive(false);
        }

    }

    public void HidePanelMenu()
    {
        PanelMenu.SetActive(false);
        HidePanelButton.gameObject.SetActive(false);
        ShowPanelButton.gameObject.SetActive(true);
    }

    public void LookAround()
    {

        Debug.Log("Look around works");

        isTurnAround = !isTurnAround;   //перевод из состояния true в false и обратно

        if (isTurnAround)
        {
            if (SelectedObj != null)
            {
                onTurnAround?.Invoke(this, SelectedObj);
            }
        }
        else
        {
            offTurnAround?.Invoke();
        }
    }

    void onTransperentSliderValueChange(float value)
    {
        if (SelectedObj != null)
        {
            int val = (int)(value * 255);
            TransparencyTextValue.text = val.ToString();
            if (!isGettingNewObject)
            {
                SelectedObjcolor.a = value;
                Debug.Log("Transperent change " + value);
                SelectedObj.GetComponent<Renderer>().material.color = SelectedObjcolor;
            }
        }

    }

    void onRedSliderValueChange(float value)
    {
        if (SelectedObj != null)
        {
            int val = (int)(value * 255);
            RedTextValue.text = val.ToString();
            if (!isGettingNewObject)
            {
                SelectedObjcolor.r = value;
                Debug.Log("Red change " + value);
                SelectedObj.GetComponent<Renderer>().material.color = SelectedObjcolor;
            }
        }

    }

    void onGreenSliderValueChange(float value)
    {
        if (SelectedObj != null)
        {
            int val = (int)(value * 255);
            GreenTextValue.text = val.ToString();
            if (!isGettingNewObject)
            {
                SelectedObjcolor.g = value;
                Debug.Log("Green change " + value);
                SelectedObj.GetComponent<Renderer>().material.color = SelectedObjcolor;
            }
        }

    }

    void onBlueSliderValueChange(float value)
    {
        if (SelectedObj != null)
        {
            int val = (int)(value * 255);
            BlueTextValue.text = val.ToString();
            if (!isGettingNewObject)
            {
                SelectedObjcolor.b = value;
                Debug.Log("Blue change " + value);
                SelectedObj.GetComponent<Renderer>().material.color = SelectedObjcolor;
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Действие, которое нужно выполнить при наведении
        //Debug.Log("Наведен на " + eventData.pointerEnter.name);
        if (eventData.pointerEnter.tag == "UI")
        {
            onInElem?.Invoke(this, false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Действие, которое нужно выполнить при уходе курсора
        //Debug.Log("Ушел с " + eventData.pointerEnter.name);
        if (eventData.pointerEnter.tag == "UI")
        {
            onOutElem?.Invoke(this, true);
        }
    }

    public void SetText(string text)
    {
        ObjectCalledText.text = text;
    }

    public void SetSelectedObj(GameObject obj)
    {
        isGettingNewObject = true;  //Устанавливается в true, чтобы не срабатывали обработчики изменения значения
        SelectedObj = obj;
        if (SelectedObj != null)
        {
            ObjectCalledText.text = SelectedObj.name;

            if (SelectedObj.activeSelf)
            {
                HideButton.gameObject.SetActive(true);
                UnHideButton.gameObject.SetActive(false);
            }
            else
            {
                HideButton.gameObject.SetActive(false);
                UnHideButton.gameObject.SetActive(true);
            }


            //Получение цвета для изменения цвета объекта слайдерами
            SelectedObjcolor = SelectedObj.GetComponent<Renderer>().material.color;
            //Настройка изначального значения слайдеров
            SetSliderValue(TransparencySlider, SelectedObjcolor.a);
            SetSliderValue(RedColorSlider, SelectedObjcolor.r);
            SetSliderValue(GreenColorSlider, SelectedObjcolor.g);
            SetSliderValue(BlueColorSlider, SelectedObjcolor.b);
        }
        else
        {
            ObjectCalledText.text = "None";
            SetSliderValue(TransparencySlider, 0);
            SetSliderValue(RedColorSlider, 0);
            SetSliderValue(GreenColorSlider, 0);
            SetSliderValue(BlueColorSlider, 0);

        }
        isGettingNewObject = false;  //Устанавливается в false, чтобы обработчики изменения значения отслеживали изменения
    }

    private void SetSliderValueLimit(Slider slider,float min, float max)
    {
        if (slider != null)
        {
            slider.minValue = min;
            slider.maxValue = max;
        }
    }

    private void SetSliderValue(Slider slider, float value)
    {
        slider.value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
    }
    public void ShowPanelMenu()
    {
        PanelMenu.SetActive(true);
        HidePanelButton.gameObject.SetActive(true);
        ShowPanelButton.gameObject.SetActive(false);
    }


    public void UnHideObject()
    {
        if (SelectedObj!=null)
        {
            Debug.Log("UnHide Object works");
            UnHideButton.gameObject.SetActive(false);
            HideButton.gameObject.SetActive(true);
            SelectedObj.SetActive(true);
        }

    }
}
