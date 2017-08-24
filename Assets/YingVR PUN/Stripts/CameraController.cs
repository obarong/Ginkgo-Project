//ljr

using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class CameraController : Photon.MonoBehaviour
{
    public float moveSpeed = 2f;

    public const float rotateSpeed = 500f;
    public Camera myCamera;
    public Rigidbody rb;
    Vector3 targetDirection;

    // 记录手指触屏的位置
    Vector2 m_screenpos = new Vector2();

    // Use this for initialization
    void Start()
    {
        // 允许多点触控
        Input.multiTouchEnabled = true;

        // 允许陀螺仪
        Input.gyro.enabled = true;

        if (myCamera)
        {
            if (photonView.isMine == false && PhotonNetwork.connected == true)
            {
                foreach (var component in myCamera.GetComponents<Component>())
                {
                    if (component.GetType() == typeof(AudioListener))
                    {
                        Destroy(component);
                    }
                    else if (component.GetType() == typeof(PhysicsRaycaster))
                    {
                        Destroy(component);
                    }
                    else if (component.GetType() == typeof(Camera))
                    {
                        myCamera.enabled = false;
                        continue;
                    }
                }
            }
        }

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine == false && PhotonNetwork.connected == true)
            return;

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        MobileInput(); 
#else
        DesktopInput();
#endif
    }

    void FixedUpdate()
    {
        if (photonView.isMine == false && PhotonNetwork.connected == true)
            return;

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        rb.MovePosition(transform.position + targetDirection * Time.deltaTime * moveSpeed / 5f);
#else
        rb.MovePosition(transform.position + targetDirection * Time.deltaTime * moveSpeed);
#endif
        targetDirection = Vector3.zero;
    }

    void OnGUI()
    {
        /*GUI.Label(new Rect(5, 5, 400, 20), "X = " + String.Format("{0:0.000}", Input.gyro.attitude.x));
        GUI.Label(new Rect(5, 30, 400, 20), "Y = " + String.Format("{0:0.000}", Input.gyro.attitude.y));
        GUI.Label(new Rect(5, 55, 400, 20), "Z = " + String.Format("{0:0.000}", Input.gyro.attitude.z));*/
    }

    // 桌面系统操作
    void DesktopInput()
    {
        // 记录鼠标左键的移动距离
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        if (mx != 0 || my != 0)
        {
            //松开鼠标左键 
            if (Input.GetMouseButton(0))
            {
                //myCamera.transform.Translate(new Vector3(mx * Time.deltaTime, my * Time.deltaTime, 0));
                myCamera.transform.Rotate(Vector3.right * Time.deltaTime * my * rotateSpeed);
                myCamera.transform.Rotate(Vector3.up * Time.deltaTime * mx * -rotateSpeed, Space.World);
                //myCamera.transform.Rotate(new Vector3(my * Time.deltaTime * rotateSpeed, mx * Time.deltaTime * -rotateSpeed, 0), Space.Self);
            }
        }

        //键盘操作
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h != 0 || v != 0)
        {
            targetDirection = new Vector3(h, 0, v);
            float y = myCamera.transform.rotation.eulerAngles.y;
            targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;
        }
    }

    // 移动平台触屏操作
    void MobileInput()
    {
        Quaternion temp;
        temp.w = Input.gyro.attitude.w;
        temp.x = -Input.gyro.attitude.x;
        temp.y = -Input.gyro.attitude.y;
        temp.z = Input.gyro.attitude.z;
        myCamera.transform.localRotation = temp;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.touchCount <= 0)
            return;

        // 1个手指触摸屏幕
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                // 记录手指触屏的位置
                m_screenpos = Input.touches[0].position;
            }
            // 手指移动
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                float h = Input.touches[0].deltaPosition.x;
                float v = Input.touches[0].deltaPosition.y;
                if (h != 0 || v != 0)
                {
                    targetDirection = new Vector3(h, 0, v);
                    float y = myCamera.transform.rotation.eulerAngles.y;
                    targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;
                }
            }

            // 手指离开屏幕 判断移动方向
            if (Input.touches[0].phase == TouchPhase.Ended &&
                Input.touches[0].phase != TouchPhase.Canceled)
            {

                Vector2 pos = Input.touches[0].position;

                // 手指水平移动
                if (Mathf.Abs(m_screenpos.x - pos.x) > Mathf.Abs(m_screenpos.y - pos.y))
                {
                    if (m_screenpos.x > pos.x)
                    {
                        //手指向左划动
                    }
                    else
                    {
                        //手指向右划动
                    }
                }
                else   // 手指垂直移动
                {
                    if (m_screenpos.y > pos.y)
                    {
                        //手指向下划动
                    }
                    else
                    {
                        //手指向上划动
                    }
                }
            }
        }
        else if (Input.touchCount > 1)
        {
            // 记录两个手指的位置
            Vector2 finger1 = new Vector2();
            Vector2 finger2 = new Vector2();

            // 记录两个手指的移动
            Vector2 mov1 = new Vector2();
            Vector2 mov2 = new Vector2();

            for (int i = 0; i < 2; i++)
            {
                Touch touch = Input.touches[i];

                if (touch.phase == TouchPhase.Ended)
                    break;

                if (touch.phase == TouchPhase.Moved)
                {
                    float mov = 0;
                    if (i == 0)
                    {
                        finger1 = touch.position;
                        mov1 = touch.deltaPosition;

                    }
                    else
                    {
                        finger2 = touch.position;
                        mov2 = touch.deltaPosition;

                        if (finger1.x > finger2.x)
                        {
                            mov = mov1.x;
                        }
                        else
                        {
                            mov = mov2.x;
                        }

                        if (finger1.y > finger2.y)
                        {
                            mov += mov1.y;
                        }
                        else
                        {
                            mov += mov2.y;
                        }

                        //transform.Translate(0, 0, mov * Time.deltaTime);
                    }
                }
            }
        }
    }
}
