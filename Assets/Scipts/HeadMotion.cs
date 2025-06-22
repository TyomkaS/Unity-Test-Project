using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMotion : MonoBehaviour
{
    public delegate void RotateAround(GameObject target);
    public static event RotateAround onRotateArround;

    public delegate void StopRotateAround();
    public static event StopRotateAround offRotateArround;


    private GameObject[] Objects;

    private float speed = 3f;
    private float mouseSens = 300f;

    private float xCoord;
    private float xRotation = 50f;
    private float yRotation = 0f;

    private bool isTargetEnable;
    private bool isTurnAroundTarget;

    public GameObject cameraTarget;
    public MenuScript menuscript;

    //Камеры
    private Camera MainCamera;
    private Camera CameraAround;


    void Awake()
    {
        Debug.Log("HeadMotion Awake Started ========= ");
        Debug.Log("HeadMotion Awake Finished ======== ");
        //Подключение класса MenuScript, чтобы отправить в него выбранный объект
        menuscript = GameObject.Find("Canvas").GetComponent<MenuScript>();

        //Инициализация значений
        isTargetEnable = true;      //Отслеживание прицеливания камеры (Менаяется в методе SetIsTargetEnable, подписанным на события MenuScript)
        cameraTarget = null;        //Изначально null, инициализируется в методе Update
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        CameraAround = GameObject.Find("CameraAround").GetComponent<Camera>();  //Вторая камера для облёта

        Objects = GameObject.FindGameObjectsWithTag("Test");

        Debug.Log("HeadMotion Awake Finished ======== ");

    }

    private void Start()
    {
        isTargetEnable = true;
        isTurnAroundTarget = false;
        CameraAround.enabled = false;   //Выключение второй камеры

    }

    // Update is called once per frame
    void Update()
    {
        if (isTurnAroundTarget)
        {
            //Вращение вокруг объекта
            Debug.Log("Turn Around Object");
            if (MainCamera.enabled)
            {
                MainCamera.enabled = false;
                CameraAround.enabled = true;
                onRotateArround?.Invoke(cameraTarget);

            }
            
            if (Input.GetMouseButton(1))
            {
                offRotateArround?.Invoke();
                MainCamera.enabled = true;
                CameraAround.enabled = false;
                isTurnAroundTarget = false;
            }
        }
        else
        {
            //Передвижение
            if (Input.GetMouseButton(1))
            {
                transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
                //Debug.Log(transform.position);
            }

            if (!Input.GetMouseButton(2)) //Так сделано, что бы можно было зажать среднюю кнопку или колесо, и подвинуть курсор
            {
                if (isTargetEnable)     //Когда курсор наведён на панель управления isTargetEnable==false, сделано для удобства, работы с панелью управления и направлением камеры на объект
                {
                    //Вращение
                    float mouseX = Input.GetAxisRaw("Mouse X") * mouseSens * Time.deltaTime;
                    float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSens * Time.deltaTime;
                    //Debug.Log("mouseX" + mouseX + "/mouseY" + mouseY);

                    mouseX = mouseX * mouseSens * Time.deltaTime;
                    mouseY = mouseY * mouseSens * Time.deltaTime;
                    //Debug.Log("MouseX" + mouseX + "/MouseY" + mouseY);

                    xRotation = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
                    yRotation += Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
                    yRotation = Mathf.Clamp(yRotation, -90f, 90f);
                    transform.localEulerAngles = new Vector3(-yRotation, xRotation, 0);
                }
                
            }


            //Выбор объекта
            if (isTargetEnable)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    cameraTarget = CameraTarget.gameobj;
                    if (cameraTarget != null)
                    {
                        if (cameraTarget.tag == "Test")
                        {
                            menuscript.SetSelectedObj(cameraTarget);
                            Debug.Log("Нацелено на: " + cameraTarget.name);
                        }

                        else
                        {
                            Debug.Log("Нацелено на: нередактируемый объект");
                        }
                    }
                    else
                    {
                        Debug.Log("Не нацелено ни на что.");
                    }
                }
            }
        }   
    }

    private void OnEnable()
    {
        MenuScript.onInElem += SetIsTargetEnable;
        MenuScript.onOutElem += SetIsTargetEnable;
        MenuScript.onTurnAround += TurnAroundTarget;
        MenuScript.onTargetOut += TargetOut;
        MenuScript.offTurnAround += StopTurnAroudTarget;
        ChooseObjPanelHandler.onObjectButtonClick += ChooseObjectHandler;
    }

    private void OnDisable()
    {
        MenuScript.onInElem -= SetIsTargetEnable;
        MenuScript.onOutElem -= SetIsTargetEnable;
        MenuScript.onTurnAround -= TurnAroundTarget;
        MenuScript.onTargetOut -= TargetOut;
        MenuScript.offTurnAround -= StopTurnAroudTarget;
        ChooseObjPanelHandler.onObjectButtonClick -= ChooseObjectHandler;
    }

    public void ChooseObjectHandler(GameObject obj)
    {
        cameraTarget = obj;
        menuscript.SetSelectedObj(cameraTarget);
    }

    public void SetIsTargetEnable(MenuScript ms,bool value)
    { isTargetEnable = value; }

    public void TurnAroundTarget(MenuScript ms, GameObject obj)
    {
        isTurnAroundTarget = true;
        cameraTarget = obj;
    }

    public void StopTurnAroudTarget()
    {
        isTurnAroundTarget = false;
    }

    public void TargetOut()
    {
        isTurnAroundTarget = false;
        cameraTarget = null;
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision");
    }
}
