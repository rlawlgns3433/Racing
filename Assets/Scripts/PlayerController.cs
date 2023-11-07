using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Interfaces;
using System;

public class PlayerController : MonoBehaviour
{

    private RaycastHit hit;
    // 휠콜라이더 4개
    public WheelCollider[] wheels = new WheelCollider[4];
    public GameObject[] CheckGrounds = new GameObject[4];
    // 차량 모델의 바퀴 부분 4개
    GameObject[] wheelMesh = new GameObject[4];

    public float power = 200.0f; // 바퀴를 회전시킬 힘
    public float rot = 0; // 바퀴의 회전 각도
    Rigidbody rb;
    
    /// <summary>
    /// test axis
    /// </summary>
    public float axiss;

    private Vector3 oldPosition;
    private Vector3 currentPosition;
    public double velocity;
    public float currentVelocity;
    void Start()
    {
        rot = 45f;
        rb = GetComponent<Rigidbody>();
        // 무게 중심을 y축 아래방향으로 낮춘다.
        rb.centerOfMass = new Vector3(0, -1, 0);
        oldPosition = transform.position;

        // 바퀴 모델을 태그를 통해서 찾아온다.(차량이 변경되더라도 자동으로 찾기위해서)
        wheelMesh = GameObject.FindGameObjectsWithTag("WheelMesh");
        CheckGrounds = GameObject.FindGameObjectsWithTag("CheckGround");
        for (int i = 0; i < wheelMesh.Length; i++)
        {	// 휠콜라이더의 위치를 바퀴메쉬의 위치로 각각 이동시킨다.
            wheels[i].transform.position = wheelMesh[i].transform.position;
        }
        
    }

    private void Update()
    {
        GetVelocity();
        if(GetObjectFromCar().tag == "Road") 
        {
            // blabla
        }
        else if(GetObjectFromCar().tag == "Sand")
        {
            // blabla
        }
        else if(GetObjectFromCar().tag == "Grass")
        {
            // blabla
        }
    }

    private void FixedUpdate()
    {
        WheelPosAndAni();
        MovingMachanism();
    }
    public void MovingMachanism()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
                wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
            }

            if (currentVelocity < 100)
            {
                rb.velocity *= 1.0025f; // 100 미만일 때 가속을 높임
            }
            else if (currentVelocity < 200)
            {
                rb.velocity *= 1.0015f; // 100 이상 200 미만일 때 가속을 더 높임
            }
            else if (currentVelocity >= 200)
            {
                rb.velocity *= 1.00075f; // 200 이상일 때 가속을 높임
            }

            // 여기서 속도를 제한합니다.
            if (currentVelocity >= 400f)
            {
                rb.velocity = rb.velocity.normalized * 400f;
                currentVelocity = 400f;
            }

        }
        else if (Input.GetAxis("Vertical") == 0)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                // for문을 통해서 휠콜라이더 전체의 속도를 점점 낮춘다.
                wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
            }

            // 여기서도 속도를 제한합니다.
            if (rb.velocity.magnitude > 400f)
            {
                rb.velocity = rb.velocity.normalized * 400f;
            }

            rb.velocity *= 0.995f;
        }

        for (int i = 0; i < 2; i++)
        {
            // 앞바퀴만 각도전환이 되어야하므로 for문을 앞바퀴만 해당되도록 설정한다.
            wheels[i].steerAngle = Input.GetAxis("Horizontal") * rot;
        }
        currentVelocity = rb.velocity.magnitude * 4;
        axiss = Input.GetAxis("Vertical");
    }

    //public void MovingMachanism()
    //{
    //    if (Input.GetAxis("Vertical") != 0)
    //    {
    //        for (int i = 0; i < wheels.Length; i++)
    //        {
    //            // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
    //            wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
    //            if(velocity < 100 ) rb.velocity *= 1.002f;
    //            else if(velocity < 200) rb.velocity *= 1.0001f;
    //            else if(velocity >= 200) rb.velocity *= 1.00001f;
    //        }
    //    }
    //    else if (Input.GetAxis("Vertical") == 0)
    //    {
    //        for (int i = 0; i < wheels.Length; i++)
    //        {
    //            // for문을 통해서 휠콜라이더 전체의 속도를 점점 낮춘다.
    //            wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
    //            rb.velocity *= 0.999f;
    //        }
    //    }
    //    for (int i = 0; i < 2; i++)
    //    {
    //        // 앞바퀴만 각도전환이 되어야하므로 for문을 앞바퀴만 해당되도록 설정한다.
    //        wheels[i].steerAngle = Input.GetAxis("Horizontal") * rot;
    //    }
    //    axiss = Input.GetAxis("Vertical");
    //}

    void WheelPosAndAni()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }

    void GetVelocity()
    {
        currentPosition = transform.position;
        var dis = (currentPosition - oldPosition);
        var distance = Math.Sqrt(Math.Pow(dis.x, 2) + Math.Pow(dis.y, 2) + Math.Pow(dis.z, 2));
        velocity = distance / Time.deltaTime;
        oldPosition = currentPosition;
    }

    GameObject GetObjectFromCar()
    {
        for(int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(CheckGrounds[i].transform.position, -CheckGrounds[i].transform.up, out hit))
            {
                Debug.DrawRay(CheckGrounds[i].transform.position, -CheckGrounds[i].transform.up * hit.distance, Color.red);
            }
            else
            {
                Debug.DrawRay(CheckGrounds[i].transform.position, -CheckGrounds[i].transform.up * 1000f, Color.red);
            }
        }
        return hit.collider.gameObject;
    }
}