using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;

public class InteractAquarium : MonoBehaviour
{

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    private float x = 0.0f;
    private float y = 0.0f;
    public int yMinLimit = -20;
    public int yMaxLimit = 80;
    private Vector3 cameraOriginPosition;
    private Vector3 objectOriginPosition;
    private Quaternion objectOriginRotation;
    private Vector3 lastMousePosition;
    private Vector3 lastRightMousePosition;
    private double lastClickTime;
    private float scale = 0.2f;
    public Transform CMTransform;
    private bool canMovingForward, canMovingBackward, canMovingRight, canMovingLeft, canMovingUp, canMovingDown = false;

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    void Start()
    {
        lastClickTime = Time.realtimeSinceStartup;
        cameraOriginPosition = Camera.main.transform.position;
        objectOriginPosition = transform.position;
        objectOriginRotation = transform.rotation;
    }

    bool Raycast(Vector3 dir) 
    {
        RaycastHit hit;
        if (Physics.Raycast(CMTransform.position, dir, out hit, 1, 6))
        {
            return false;
        }
        return true;
    }

    void Update()
    {
        canMovingForward = Raycast(CMTransform.forward);
        canMovingBackward =  Raycast(-CMTransform.forward);
        canMovingRight = Raycast(CMTransform.right);
        canMovingLeft = Raycast(-CMTransform.right);
        canMovingUp = Raycast(CMTransform.up);
        canMovingDown = Raycast(-CMTransform.up);

        if (Input.GetMouseButton(0))
        {
             x += Input.GetAxis("Mouse X") * xSpeed* 0.02f;
             y -= Input.GetAxis("Mouse Y") * ySpeed*0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            //Quaternion rotation = Quaternion.Euler(y, x, 0);
            Quaternion rotation1 = Quaternion.AngleAxis(y, new Vector3(1,0,0));
            Quaternion rotation2 = Quaternion.AngleAxis(x, new Vector3(0, 1, 0));
            transform.rotation = rotation1 * rotation2;
        }
        else if (Input.GetMouseButtonDown(2))
        {
            if (Time.realtimeSinceStartup - lastClickTime < scale)
            {
                Camera.main.transform.position = cameraOriginPosition;
                transform.position = objectOriginPosition;
                transform.rotation = objectOriginRotation;
            }
            else 
            {
                lastMousePosition = new Vector3(Input.mousePosition.x / Screen.width * 2 - 1, Input.mousePosition.y / Screen.height * 2 - 1, 0);
            }
            lastClickTime = Time.realtimeSinceStartup;
        }
        else if (Input.GetMouseButton(2))
        {
            /*Vector3 wp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            transform.position = new Vector3(wp.x, wp.y , transform.position.z);*/
            Vector3 m_p = new Vector3(Input.mousePosition.x / Screen.width * 2 - 1, Input.mousePosition.y / Screen.height * 2 - 1);
            transform.position += (m_p - lastMousePosition) * 20.0f;
            lastMousePosition = m_p;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if ((Camera.main.transform.position -= Camera.main.transform.position.normalized * 2).magnitude >= 1.0f)
                {
                    CMTransform.position -= CMTransform.position.normalized * 2;
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if ((Camera.main.transform.position).magnitude <= 76.3f)
                    CMTransform.position += CMTransform.position.normalized * 2;
            }
        }
    }


}
