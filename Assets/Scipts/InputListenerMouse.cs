using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListenerMouse : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("MouseListener Detected Input MouseButton 0");
        }

        if (Input.GetMouseButton(1))
        {
            Debug.Log("MouseListener Detected Input MouseButton 1");
        }

        if (Input.GetMouseButton(2))
        {
            Debug.Log("MouseListener Detected Input MouseButton 2");
        }
    }
}
