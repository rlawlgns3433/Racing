using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Interfaces;
using System;

public class PlayerController : MonoBehaviour
{

    private RaycastHit hit;
    // ���ݶ��̴� 4��
    public WheelCollider[] wheels = new WheelCollider[4];
    public GameObject[] CheckGrounds = new GameObject[4];
    // ���� ���� ���� �κ� 4��
    GameObject[] wheelMesh = new GameObject[4];

    public float power = 200.0f; // ������ ȸ����ų ��
    public float rot = 0; // ������ ȸ�� ����
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
                // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
                wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
            }

            if (currentVelocity < 100)
            {
                rb.velocity *= 1.0025f; // 100 �̸��� �� ������ ����
            }
            else if (currentVelocity < 200)
            {
                rb.velocity *= 1.0015f; // 100 �̻� 200 �̸��� �� ������ �� ����
            }
            else if (currentVelocity >= 200)
            {
                rb.velocity *= 1.00075f; // 200 �̻��� �� ������ ����
            }

            // ���⼭ �ӵ��� �����մϴ�.
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
                // for���� ���ؼ� ���ݶ��̴� ��ü�� �ӵ��� ���� �����.
                wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
            }

            // ���⼭�� �ӵ��� �����մϴ�.
            if (rb.velocity.magnitude > 400f)
            {
                rb.velocity = rb.velocity.normalized * 400f;
            }

            rb.velocity *= 0.995f;
        }

        for (int i = 0; i < 2; i++)
        {
            // �չ����� ������ȯ�� �Ǿ���ϹǷ� for���� �չ����� �ش�ǵ��� �����Ѵ�.
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
    //            // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
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
    //            // for���� ���ؼ� ���ݶ��̴� ��ü�� �ӵ��� ���� �����.
    //            wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
    //            rb.velocity *= 0.999f;
    //        }
    //    }
    //    for (int i = 0; i < 2; i++)
    //    {
    //        // �չ����� ������ȯ�� �Ǿ���ϹǷ� for���� �չ����� �ش�ǵ��� �����Ѵ�.
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