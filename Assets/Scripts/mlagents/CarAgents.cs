using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgents : Agent
{
    [SerializeField] private GameObject[] checkpoints = new GameObject[11];
    private int i = 0;
    private new Rigidbody rigidbody;
    private Transform transfrom;

    // PlayerController 스크립트
    private PlayerController playerController;


    #region mlagents
    public override void Initialize()
    {

        rigidbody = GetComponent<Rigidbody>();
        transfrom = GetComponent<Transform>();
        playerController = GetComponent<PlayerController>();

        checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
    }

    public override void OnEpisodeBegin()
    {
        // checkpoint 인덱스 초기화
        i = 0;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        transfrom.localPosition = new Vector3(0, 1.5f, -30);
        transfrom.localRotation = Quaternion.Euler(0, -90, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transfrom.localPosition);
        sensor.AddObservation(transfrom.localRotation);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // action1 = vertical
        // action2 = horizontal

        var continuousActions = actions.ContinuousActions;
        float action1 = continuousActions[0];
        float action2 = continuousActions[1];

        if (action1 != 0)
        {
            for (int i = 0; i < playerController.wheels.Length; i++)
            {
                // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
                playerController.wheels[i].motorTorque = action1 * playerController.power * Time.deltaTime;
                rigidbody.velocity *= 1.001f;
            }
            AddReward(0.01f);
        }
        else if (action1 == 0)
        {
            for (int i = 0; i < playerController.wheels.Length; i++)
            {
                // for문을 통해서 휠콜라이더 전체의 속도를 점점 낮춘다.
                playerController.wheels[i].motorTorque = action1 * playerController.power * Time.deltaTime;
                rigidbody.velocity *= 0.999f;
            }
            SetReward(-1.0f);
        }
        for (int i = 2; i < 4; i++)
        {
            // 앞바퀴만 각도전환이 되어야하므로 for문을 앞바퀴만 해당되도록 설정한다.
            playerController.wheels[i].steerAngle = action2 * playerController.rot;
        }
        playerController.axiss = action1;
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        playerController.MovingMachanism();
    }

    #endregion

    #region MonoBehaviour

    private void OnTriggerEnter(Collider other)
    {
        if(other.Equals(checkpoints[i++]) && i < 11)
        {
            AddReward(1);
        }
    }

    #endregion

    private void MovingMachanismForML(float action1, float action2)
    {
        if (action1 != 0)
        {
            for (int i = 0; i < playerController.wheels.Length; i++)
            {
                // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
                playerController.wheels[i].motorTorque = action1 * playerController.power * Time.deltaTime;
                rigidbody.velocity *= 1.001f;
            }
            AddReward(0.01f);
        }
        else if (action1 == 0)
        {
            for (int i = 0; i < playerController.wheels.Length; i++)
            {
                // for문을 통해서 휠콜라이더 전체의 속도를 점점 낮춘다.
                playerController.wheels[i].motorTorque = action1 * playerController.power * Time.deltaTime;
                rigidbody.velocity *= 0.999f;
            }
            SetReward(-1.0f);
        }
        for (int i = 2; i < 4; i++)
        {
            // 앞바퀴만 각도전환이 되어야하므로 for문을 앞바퀴만 해당되도록 설정한다.
            playerController.wheels[i].steerAngle = action2 * playerController.rot;
        }
        playerController.axiss = action1;
    }
}


