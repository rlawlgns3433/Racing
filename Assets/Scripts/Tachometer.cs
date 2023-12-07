using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tachometer : MonoBehaviour
{
    public GameObject Needle;
    public Text CurrentVelocity;
    public Text Gear;

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator UpdateNeedle()
    {
        yield return new WaitForEndOfFrame();


    }
}
