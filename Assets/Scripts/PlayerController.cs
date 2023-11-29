using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Interfaces;
using System;

public class PlayerController : MonoBehaviour
{

    // private 
    private GameObject[] wheelMesh = new GameObject[4];
    private Vector3 oldPosition;
    private Vector3 currentPosition;
    private Rigidbody rb;
    private RaycastHit hit;
    public WheelCollider FL_Wheel, FR_Wheel;
    private const float kmhToMs = 0.277778f; // km/h를 m/s로 변환하기 위한 상수



    // public
    
    public WheelCollider[] wheels = new WheelCollider[4]; // 휠콜라이더 4개
    public GameObject[] CheckGrounds = new GameObject[4];
    public CarData mCarData;
    public float maxSpeedKmPerHour = 100f; // 최대 속도 (km/h)
    public float timeTo100KmPerHour = 7f; // 예시로 7초로 설정  // 0에서 100km/h까지의 가속 시간 계산 (일반적인 예시)
    //public float CurrentSpeedKmPerHour => rb.velocity.magnitude * kmhToMs * 3.6f;
    //// 속도를 km/h 단위로 표시


    /// <summary>
    /// CarData 대체 부분
    ///// </summary>
    //public float power = 300.0f; // 바퀴를 회전시킬 힘
    //public float rot = 0; // 바퀴의 회전 각도
    //public float axiss;
    //public double velocity;
    //public float currentVelocity;
    ////public AnimationCurve torqueCurve; // 속도에 따른 토크 변화를 위한 곡선
    //public float maxSpeed = 100f;
    public float m_countdown = 3f;




    void Start()
    {
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

        FL_Wheel = GameObject.Find("Wheel_FL").GetComponent<WheelCollider>();
        FR_Wheel = GameObject.Find("Wheel_FR").GetComponent<WheelCollider>();
    }

    private void Update()
    {
        m_countdown -= Time.deltaTime;

        if(m_countdown > 0)
        {
            Input.ResetInputAxes();
        }

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


    float AccelerationRate()
    {
        // 0에서 100km/h까지의 가속 거리 계산 (등가속 운동의 이동 거리 계산식 사용)
        float distanceTo100KmPerHour = 0.5f * (100f / kmhToMs) * timeTo100KmPerHour;
        return 2 * distanceTo100KmPerHour / (timeTo100KmPerHour * timeTo100KmPerHour); // 가속도 = 2s / t^2
    }

    // 속도에 따른 토크를 계산하는 함수
    //float CalculateTorqueByVelocity(float currentVelocity)
    //{
    //    float normalizedVelocity = currentVelocity / maxSpeed; // 현재 속도를 정규화합니다. (0에서 1 사이의 값으로)

    //    // 속도에 따른 토크 변화를 정의한 곡선을 사용하여 토크 값을 가져옵니다.
    //    float torque = torqueCurve.Evaluate(normalizedVelocity);
    //    return torque;
    //}

    public void MovingMachanism()
    {
        float acceleration = Input.GetAxis("Vertical") * AccelerationRate();

        if (Input.GetAxis("Vertical") > 0)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
                wheels[i].motorTorque = Input.GetAxis("Vertical") * mCarData.power;
            }

            // 여기서 속도를 제한합니다.
            if (mCarData.currentVelocity >= 100f)
            {
                rb.velocity = rb.velocity.normalized * 100f;
                mCarData.currentVelocity = 100f;
            }

        }
        else if(Input.GetAxis("Vertical") < 0)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
                wheels[i].motorTorque = Input.GetAxis("Vertical") * mCarData.power;
            }

            // 여기서 속도를 제한합니다.
            if (mCarData.currentVelocity >= 100f)
            {
                rb.velocity = rb.velocity.normalized * 100f;
                mCarData.currentVelocity = 100f;
            }
        }
        else if (Input.GetAxis("Vertical") == 0)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                // for문을 통해서 휠콜라이더 전체의 속도를 점점 낮춘다.
                wheels[i].motorTorque = Input.GetAxis("Vertical") * mCarData.power;
            }

            // 여기서도 속도를 제한합니다.
            if (rb.velocity.magnitude > 100f)
            {
                rb.velocity = rb.velocity.normalized * 100f;
            }

            rb.velocity *= 0.99f;
        }


        FL_Wheel.steerAngle = Input.GetAxis("Horizontal") * mCarData.rot;
        FR_Wheel.steerAngle = Input.GetAxis("Horizontal") * mCarData.rot;

        mCarData.currentVelocity = rb.velocity.magnitude;
        mCarData.axiss = Input.GetAxis("Vertical");
    }


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
        mCarData.velocity = distance / Time.deltaTime;
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