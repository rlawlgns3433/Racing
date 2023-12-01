using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    public GameObject targetPosition;
    public GameObject MainCam;
    float span = 2, time;

    void Update()
    {
        transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition.transform.position, 0.01f);

        time += Time.deltaTime;
        if(time >= 4)
        {
            MainCam.SetActive(true);
            Destroy(gameObject);
        }
    }
}
