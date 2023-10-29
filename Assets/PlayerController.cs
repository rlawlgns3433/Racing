using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    // ���ݶ��̴� 4��
    public WheelCollider[] wheels = new WheelCollider[4];
    // ���� ���� ���� �κ� 4��
    GameObject[] wheelMesh = new GameObject[4];

    public float power = 100f; // ������ ȸ����ų ��
    public float rot = -45f; // ������ ȸ�� ����
    Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // ���� �߽��� y�� �Ʒ��������� �����.
        rb.centerOfMass = new Vector3(0, -1, 0);


        // ���� ���� �±׸� ���ؼ� ã�ƿ´�.(������ ����Ǵ��� �ڵ����� ã�����ؼ�)
        wheelMesh = GameObject.FindGameObjectsWithTag("WheelMesh");

        for (int i = 0; i < wheelMesh.Length; i++)
        {	// ���ݶ��̴��� ��ġ�� �����޽��� ��ġ�� ���� �̵���Ų��.
            wheels[i].transform.position = wheelMesh[i].transform.position;
        }
    }

    private void FixedUpdate()
    {
        WheelPosAndAni();

        for (int i = 0; i < wheels.Length; i++)
        {
            // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
            wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
        }
        for (int i = 2; i < 4; i++)
        {
            // �չ����� ������ȯ�� �Ǿ���ϹǷ� for���� �չ����� �ش�ǵ��� �����Ѵ�.
            wheels[i].steerAngle = Input.GetAxis("Horizontal") * rot;
        }
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
}