//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Unity.MLAgents;
//using Unity.MLAgents.Actuators;
//using Unity.MLAgents.Sensors;

//public class CarAgents : Agent
//{
//    [SerializeField] private GameObject[] checkpoints = new GameObject[11];
//    private int i = 0;
//    private new Rigidbody rigidbody;
//    private Transform transfrom;

//    // PlayerController ��ũ��Ʈ
//    private PlayerController playerController;


//    #region mlagents
//    public override void Initialize()
//    {

//        rigidbody = GetComponent<Rigidbody>();
//        transfrom = GetComponent<Transform>();
//        playerController = GetComponent<PlayerController>();

//        checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
//    }

//    public override void OnEpisodeBegin()
//    {
//        // checkpoint �ε��� �ʱ�ȭ
//        i = 0;
//        rigidbody.velocity = Vector3.zero;
//        rigidbody.angularVelocity = Vector3.zero;
//        transfrom.localPosition = new Vector3(0, 1.5f, -30);
//        transfrom.localRotation = Quaternion.Euler(0, -90, 0);
//    }

//    public override void CollectObservations(VectorSensor sensor)
//    {
//        sensor.AddObservation(transfrom.localPosition);
//        sensor.AddObservation(transfrom.localRotation);
//    }

//    public override void OnActionReceived(ActionBuffers actions)
//    {
//        // action1 = vertical
//        // action2 = horizontal

//        var discreteActions = actions.DiscreteActions;
//        int action1 = discreteActions[0];
//        int action2 = discreteActions[1];

//        switch(action1)
//        {
//            case 0:
//                {
//                    // ���� ����
//                    for (int i = 0; i < playerController.wheels.Length; i++)
//                    {
//                        // for���� ���ؼ� ���ݶ��̴� ��ü�� �ӵ��� ���� �����.
//                        playerController.wheels[i].motorTorque = action1 * playerController.mCarData.power * Time.deltaTime;
//                        rigidbody.velocity *= 0.999f;
//                    }
//                    AddReward(-0.01f);
//                    break;
//                }
//            case 1:
//                {
//                    // ����
//                    for (int i = 0; i < playerController.wheels.Length; i++)
//                    {
//                        // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
//                        playerController.wheels[i].motorTorque = 1.0f * playerController.mCarData.power * Time.deltaTime;
//                        rigidbody.velocity *= 1.001f;
//                    }
//                    AddReward(0.01f);
//                    break;
//                }
//            case 2:
//                {
//                    // ����
//                    for (int i = 0; i < playerController.wheels.Length; i++)
//                    {
//                        // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
//                        playerController.wheels[i].motorTorque = -1.0f * playerController.mCarData.power * Time.deltaTime;
//                        rigidbody.velocity *= 1.001f;
//                    }
//                    AddReward(0.01f);
//                    break;
//                }
//        }

//        //if (action1 != 0)
//        //{
//        //    for (int i = 0; i < playerController.wheels.Length; i++)
//        //    {
//        //        // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
//        //        playerController.wheels[i].motorTorque = action1 * playerController.power * Time.deltaTime;
//        //        rigidbody.velocity *= 1.001f;
//        //    }
//        //    AddReward(0.01f);
//        //}
//        //else if (action1 == 0)
//        //{
//        //    for (int i = 0; i < playerController.wheels.Length; i++)
//        //    {
//        //        // for���� ���ؼ� ���ݶ��̴� ��ü�� �ӵ��� ���� �����.
//        //        playerController.wheels[i].motorTorque = action1 * playerController.power * Time.deltaTime;
//        //        rigidbody.velocity *= 0.999f;
//        //    }
//        //    SetReward(-1.0f);
//        //}

//        switch (action2)
//        {
//            case 0:
//                {
//                    for (int i = 0; i < 2; i++)
//                    {
//                        // �չ����� ������ȯ�� �Ǿ���ϹǷ� for���� �չ����� �ش�ǵ��� �����Ѵ�.
//                        playerController.wheels[i].steerAngle = action2 * playerController.mCarData.rot;
//                    }
//                    break;
//                }

//            case 1:
//                {
//                    for (int i = 0; i < 2; i++)
//                    {
//                        // �չ����� ������ȯ�� �Ǿ���ϹǷ� for���� �չ����� �ش�ǵ��� �����Ѵ�.
//                        playerController.wheels[i].steerAngle = -1.0f * playerController.mCarData.rot;
//                    }
//                    break;
//                }

//            case 2:
//                {
//                    for (int i = 0; i < 2; i++)
//                    {
//                        // �չ����� ������ȯ�� �Ǿ���ϹǷ� for���� �չ����� �ش�ǵ��� �����Ѵ�.
//                        playerController.wheels[i].steerAngle = 1.0f * playerController.mCarData.rot;
//                    }
//                    break;
//                }
//        }

//        //for (int i = 2; i < 4; i++)
//        //{
//        //    // �չ����� ������ȯ�� �Ǿ���ϹǷ� for���� �չ����� �ش�ǵ��� �����Ѵ�.
//        //    playerController.wheels[i].steerAngle = action2 * playerController.rot;
//        //}

//        playerController.mCarData.axiss = action1;
//    }


//    public override void Heuristic(in ActionBuffers actionsOut)
//    {
//        //playerController.MovingMachanism();
//    }

//    #endregion

//    #region MonoBehaviour

//    private void OnTriggerEnter(Collider other)
//    {
//        if(other.Equals(checkpoints[i++]) && i < 11)
//        {
//            AddReward(1);
//        }
//    }

//    #endregion

//    private void MovingMachanismForML(float action1, float action2)
//    {
//        if (action1 != 0)
//        {
//            for (int i = 0; i < playerController.wheels.Length; i++)
//            {
//                // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
//                playerController.wheels[i].motorTorque = action1 * playerController.mCarData.power * Time.deltaTime;
//                rigidbody.velocity *= 1.001f;
//            }
//            AddReward(0.01f);
//        }
//        else if (action1 == 0)
//        {
//            for (int i = 0; i < playerController.wheels.Length; i++)
//            {
//                // for���� ���ؼ� ���ݶ��̴� ��ü�� �ӵ��� ���� �����.
//                playerController.wheels[i].motorTorque = action1 * playerController.mCarData.power * Time.deltaTime;
//                rigidbody.velocity *= 0.999f;
//            }
//            SetReward(-1.0f);
//        }
//        for (int i = 2; i < 4; i++)
//        {
//            // �չ����� ������ȯ�� �Ǿ���ϹǷ� for���� �չ����� �ش�ǵ��� �����Ѵ�.
//            playerController.wheels[i].steerAngle = action2 * playerController.mCarData.rot;
//        }
//        playerController.mCarData.axiss = action1;
//    }
//}


using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgents : Agent
{
    public PlayerController playerController;
    Rigidbody carRb;
    public float speed = 10f;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset car's position and other necessary variables
        // For example, reset the car's position to the starting point of the track
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Collect observations from the environment
        // For example, car's current velocity, position, rotation, etc.
        sensor.AddObservation(carRb.velocity);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Apply actions to the agent
        float moveAction = actions.ContinuousActions[0];
        float turnAction = actions.ContinuousActions[1];

        Vector3 move = transform.forward * moveAction * speed * Time.fixedDeltaTime;
        carRb.MovePosition(carRb.position + move);

        float turn = turnAction * Time.fixedDeltaTime * 200f;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        carRb.MoveRotation(carRb.rotation * turnRotation);

        // Reward the agent based on the desired behavior
        // Example of reward shaping:
        float currentSpeed = carRb.velocity.magnitude;
        float desiredSpeed = 10f; // Desired minimum speed
        float speedReward = currentSpeed > desiredSpeed ? 0.1f * (currentSpeed - desiredSpeed) : 0f;
        AddReward(speedReward);

        // Check if the car is off the track and apply negative reward
        if (IsOffTrack())
        {
            AddReward(-0.1f); // Negative reward for going off-track
            EndEpisode(); // End the episode when off-track
        }
    }

    bool IsOffTrack()
    {
        if(playerController.GetObjectFromCar().CompareTag("OutOfTrack"))
        {
            return true;
        }
        // Implement logic to check if the car is off the track
        // For example, raycasting or collider checks
        // Return true if the car is off-track, false otherwise
        return false; // Placeholder, replace with actual logic
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // This method is used for testing and debugging, allowing manual control of the agent
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }
}
