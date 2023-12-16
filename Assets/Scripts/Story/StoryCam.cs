using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryCam : MonoBehaviour
{
    public GameObject targetPosition;

    void Update()
    {
        transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition.transform.position, 0.01f);
    }
}
