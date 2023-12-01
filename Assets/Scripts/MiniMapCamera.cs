using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public GameObject myCar;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(myCar.transform.position.x, 20, myCar.transform.position.z);
        transform.rotation = Quaternion.Euler(90, myCar.transform.rotation.eulerAngles.y, myCar.transform.rotation.eulerAngles.z);
    }
}
