using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListenerKeyBoard : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("KeyBoardListener Detected Input" + KeyCode.W);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("KeyBoardListener Detected Input" + KeyCode.A);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("KeyBoardListener Detected Input" + KeyCode.S);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("KeyBoardListener Detected Input" + KeyCode.D);
        }
    }
}
