using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Interfaces;
using System;

public class PlayerController : MonoBehaviour
{
    // ���ݶ��̴� 4��
    public WheelCollider[] wheels = new WheelCollider[4];
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

    void Start()
    {
        rot = 45f;
        rb = GetComponent<Rigidbody>();
        // ���� �߽��� y�� �Ʒ��������� �����.
        rb.centerOfMass = new Vector3(0, -1, 0);
        oldPosition = transform.position;

        // ���� ���� �±׸� ���ؼ� ã�ƿ´�.(������ ����Ǵ��� �ڵ����� ã�����ؼ�)
        wheelMesh = GameObject.FindGameObjectsWithTag("WheelMesh");

        for (int i = 0; i < wheelMesh.Length; i++)
        {	// ���ݶ��̴��� ��ġ�� �����޽��� ��ġ�� ���� �̵���Ų��.
            wheels[i].transform.position = wheelMesh[i].transform.position;
        }
    }

    private void Update()
    {
        GetVelocity();
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
                rb.velocity *= 1.001f;
            }
        }
        else if (Input.GetAxis("Vertical") == 0)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                // for���� ���ؼ� ���ݶ��̴� ��ü�� �ӵ��� ���� �����.
                wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
                rb.velocity *= 0.999f;
            }
        }
        for (int i = 0; i < 2; i++)
        {
            // �չ����� ������ȯ�� �Ǿ���ϹǷ� for���� �չ����� �ش�ǵ��� �����Ѵ�.
            wheels[i].steerAngle = Input.GetAxis("Horizontal") * rot;
        }
        axiss = Input.GetAxis("Vertical");
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
        velocity = distance / Time.deltaTime;
        oldPosition = currentPosition;
    }
}