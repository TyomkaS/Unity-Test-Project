using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour 
{
    public float distance = 100f;
    public static GameObject gameobj;
    private Camera cam;
   


    void Start()
    {
        cam = GetComponent<Camera>();
        gameobj = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 beginRay = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            Vector3 endRay = cam.transform.forward * distance;
            Debug.DrawLine(beginRay, endRay * 100, Color.red);

            Ray ray = new Ray(beginRay, endRay);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, distance))
            {
                gameobj = hit.collider.gameObject;
            }

        }
    }
}
