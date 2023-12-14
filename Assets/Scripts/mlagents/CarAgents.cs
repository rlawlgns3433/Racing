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

//    // PlayerController 스크립트
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
//        // checkpoint 인덱스 초기화
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
//                    // 가속 없음
//                    for (int i = 0; i < playerController.wheels.Length; i++)
//                    {
//                        // for문을 통해서 휠콜라이더 전체의 속도를 점점 낮춘다.
//                        playerController.wheels[i].motorTorque = action1 * playerController.mCarData.power * Time.deltaTime;
//                        rigidbody.velocity *= 0.999f;
//                    }
//                    AddReward(-0.01f);
//                    break;
//                }
//            case 1:
//                {
//                    // 전진
//                    for (int i = 0; i < playerController.wheels.Length; i++)
//                    {
//                        // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
//                        playerController.wheels[i].motorTorque = 1.0f * playerController.mCarData.power * Time.deltaTime;
//                        rigidbody.velocity *= 1.001f;
//                    }
//                    AddReward(0.01f);
//                    break;
//                }
//            case 2:
//                {
//                    // 후진
//                    for (int i = 0; i < playerController.wheels.Length; i++)
//                    {
//                        // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
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
//        //        // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
//        //        playerController.wheels[i].motorTorque = action1 * playerController.power * Time.deltaTime;
//        //        rigidbody.velocity *= 1.001f;
//        //    }
//        //    AddReward(0.01f);
//        //}
//        //else if (action1 == 0)
//        //{
//        //    for (int i = 0; i < playerController.wheels.Length; i++)
//        //    {
//        //        // for문을 통해서 휠콜라이더 전체의 속도를 점점 낮춘다.
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
//                        // 앞바퀴만 각도전환이 되어야하므로 for문을 앞바퀴만 해당되도록 설정한다.
//                        playerController.wheels[i].steerAngle = action2 * playerController.mCarData.rot;
//                    }
//                    break;
//                }

//            case 1:
//                {
//                    for (int i = 0; i < 2; i++)
//                    {
//                        // 앞바퀴만 각도전환이 되어야하므로 for문을 앞바퀴만 해당되도록 설정한다.
//                        playerController.wheels[i].steerAngle = -1.0f * playerController.mCarData.rot;
//                    }
//                    break;
//                }

//            case 2:
//                {
//                    for (int i = 0; i < 2; i++)
//                    {
//                        // 앞바퀴만 각도전환이 되어야하므로 for문을 앞바퀴만 해당되도록 설정한다.
//                        playerController.wheels[i].steerAngle = 1.0f * playerController.mCarData.rot;
//                    }
//                    break;
//                }
//        }

//        //for (int i = 2; i < 4; i++)
//        //{
//        //    // 앞바퀴만 각도전환이 되어야하므로 for문을 앞바퀴만 해당되도록 설정한다.
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
//                // for문을 통해서 휠콜라이더 전체를 Vertical 입력에 따라서 power만큼의 힘으로 움직이게한다.
//                playerController.wheels[i].motorTorque = action1 * playerController.mCarData.power * Time.deltaTime;
//                rigidbody.velocity *= 1.001f;
//            }
//            AddReward(0.01f);
//        }
//        else if (action1 == 0)
//        {
//            for (int i = 0; i < playerController.wheels.Length; i++)
//            {
//                // for문을 통해서 휠콜라이더 전체의 속도를 점점 낮춘다.
//                playerController.wheels[i].motorTorque = action1 * playerController.mCarData.power * Time.deltaTime;
//                rigidbody.velocity *= 0.999f;
//            }
//            SetReward(-1.0f);
//        }
//        for (int i = 2; i < 4; i++)
//        {
//            // 앞바퀴만 각도전환이 되어야하므로 for문을 앞바퀴만 해당되도록 설정한다.
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
    public bool isClosed = false;
    public float oldDistance;
    public float curDistance;
    public int cnt = 1;
    public RaycastHit hit_forward;
    public BoxCollider[] checkpoints = new BoxCollider[19];
    public PlayerController playerController;
    Rigidbody carRb;
    public float speed = 10f;

    private void Awake()
    {
        
    }
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerController.WheelPosAndAni();
    }

    public override void OnEpisodeBegin()
    {
        Init();
        cnt = 1;
        transform.localPosition = new Vector3(0, 1.5f, -30);
        transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Collect observations from the environment
        // For example, car's current velocity, position, rotation, etc.
        sensor.AddObservation(carRb.velocity);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);

        sensor.AddObservation(isClosed);
        sensor.AddObservation(checkpoints[cnt - 1].transform.position);
        sensor.AddObservation(Vector3.Distance(checkpoints[cnt - 1].transform.position, gameObject.transform.position));
        sensor.AddObservation(oldDistance - curDistance);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Apply actions to the agent
        float moveAction = actions.ContinuousActions[0];
        float turnAction = actions.ContinuousActions[1];

        //playerController.MovingMachanism(moveAction, turnAction);

        Vector3 move = transform.forward * moveAction * speed * Time.fixedDeltaTime;
        carRb.MovePosition(carRb.position + move);

        float turn = turnAction * Time.fixedDeltaTime * 200f;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        carRb.MoveRotation(carRb.rotation * turnRotation);


        // Reward the agent based on the desired behavior
        // Example of reward shaping:
        float currentSpeed = playerController.rb.velocity.magnitude;
        float desiredSpeed = 5f; // Desired minimum speed
        float speedReward = currentSpeed > desiredSpeed ? 0.01f * (currentSpeed - desiredSpeed) : 0f;
        AddReward(speedReward);

        // Check if the car is off the track and apply negative reward
        if (IsOffTrack())
        {
            AddReward(-0.1f); // Negative reward for going off-track
            //EndEpisode(); // End the episode when off-track
        }

        CloseToTarget();

        if (Physics.Raycast(transform.position, transform.forward, out hit_forward))
        {
            Debug.DrawRay(transform.position, transform.forward * hit_forward.distance * 2, Color.red);
        }
        if (hit_forward.collider.gameObject.CompareTag("CheckPoint"))
        {
            AddReward(0.01f);
        }
        else
        {
            AddReward(-0.001f);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "CheckPoint"+cnt)
        {
            checkpoints[cnt].enabled = true;
            checkpoints[cnt - 1].enabled = false;
            cnt++;
        }
    }

    void Init()
    {
        GameObject CHK_Points = GameObject.Find("CheckPoints");
        BoxCollider[] children = CHK_Points.GetComponentsInChildren<BoxCollider>();
        checkpoints = new BoxCollider[children.Length];

        for (int i = 0; i < children.Length; i++)
        {
            checkpoints[i] = children[i];
            checkpoints[i].enabled = false;
        }
        checkpoints[0].enabled = true;
    }

    void CloseToTarget()
    {
        oldDistance = Vector3.Distance(checkpoints[cnt - 1].transform.position, transform.position);

        Invoke("GetDist", 20);

        if(oldDistance > curDistance)
        {
            AddReward((oldDistance - curDistance) * 0.03f);
            isClosed = true;
        }
        else if(oldDistance <= curDistance)
        {
            AddReward((oldDistance - curDistance) * 0.03f);
            isClosed = false;
        }
    }

    void GetDist()
    {
        curDistance = Vector3.Distance(checkpoints[cnt - 1].transform.position, transform.position);

        Debug.Log("CurDIST : " + curDistance + "\nOldDIST : " + oldDistance);
    }

}
