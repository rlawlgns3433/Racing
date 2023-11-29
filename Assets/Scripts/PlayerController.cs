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
    private const float kmhToMs = 0.277778f; // km/h�� m/s�� ��ȯ�ϱ� ���� ���



    // public
    
    public WheelCollider[] wheels = new WheelCollider[4]; // ���ݶ��̴� 4��
    public GameObject[] CheckGrounds = new GameObject[4];
    public CarData mCarData;
    public float maxSpeedKmPerHour = 100f; // �ִ� �ӵ� (km/h)
    public float timeTo100KmPerHour = 7f; // ���÷� 7�ʷ� ����  // 0���� 100km/h������ ���� �ð� ��� (�Ϲ����� ����)
    //public float CurrentSpeedKmPerHour => rb.velocity.magnitude * kmhToMs * 3.6f;
    //// �ӵ��� km/h ������ ǥ��


    /// <summary>
    /// CarData ��ü �κ�
    ///// </summary>
    //public float power = 300.0f; // ������ ȸ����ų ��
    //public float rot = 0; // ������ ȸ�� ����
    //public float axiss;
    //public double velocity;
    //public float currentVelocity;
    ////public AnimationCurve torqueCurve; // �ӵ��� ���� ��ũ ��ȭ�� ���� �
    //public float maxSpeed = 100f;
    public float m_countdown = 3f;




    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // ���� �߽��� y�� �Ʒ��������� �����.
        rb.centerOfMass = new Vector3(0, -1, 0);
        oldPosition = transform.position;

        // ���� ���� �±׸� ���ؼ� ã�ƿ´�.(������ ����Ǵ��� �ڵ����� ã�����ؼ�)
        wheelMesh = GameObject.FindGameObjectsWithTag("WheelMesh");
        CheckGrounds = GameObject.FindGameObjectsWithTag("CheckGround");
        for (int i = 0; i < wheelMesh.Length; i++)
        {	// ���ݶ��̴��� ��ġ�� �����޽��� ��ġ�� ���� �̵���Ų��.
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
        // 0���� 100km/h������ ���� �Ÿ� ��� (��� ��� �̵� �Ÿ� ���� ���)
        float distanceTo100KmPerHour = 0.5f * (100f / kmhToMs) * timeTo100KmPerHour;
        return 2 * distanceTo100KmPerHour / (timeTo100KmPerHour * timeTo100KmPerHour); // ���ӵ� = 2s / t^2
    }

    // �ӵ��� ���� ��ũ�� ����ϴ� �Լ�
    //float CalculateTorqueByVelocity(float currentVelocity)
    //{
    //    float normalizedVelocity = currentVelocity / maxSpeed; // ���� �ӵ��� ����ȭ�մϴ�. (0���� 1 ������ ������)

    //    // �ӵ��� ���� ��ũ ��ȭ�� ������ ��� ����Ͽ� ��ũ ���� �����ɴϴ�.
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
                // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
                wheels[i].motorTorque = Input.GetAxis("Vertical") * mCarData.power;
            }

            // ���⼭ �ӵ��� �����մϴ�.
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
                // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
                wheels[i].motorTorque = Input.GetAxis("Vertical") * mCarData.power;
            }

            // ���⼭ �ӵ��� �����մϴ�.
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
                // for���� ���ؼ� ���ݶ��̴� ��ü�� �ӵ��� ���� �����.
                wheels[i].motorTorque = Input.GetAxis("Vertical") * mCarData.power;
            }

            // ���⼭�� �ӵ��� �����մϴ�.
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