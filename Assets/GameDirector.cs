using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    private Text m_Car_DashBoard;
    private GameObject m_myCar;
    private Rigidbody m_myCar_rigid;
    private float  m_max_velocity;
    // Start is called before the first frame update
    void Start()
    {
        m_Car_DashBoard = GameObject.Find("CurVelocity").GetComponent<Text>();

        m_myCar = GameObject.Find("MyCar");
        m_myCar_rigid = m_myCar.GetComponent<Rigidbody>();
        m_max_velocity = 400.0f;

        StartCoroutine("DisplayDashBoard");
    }

    private void LateUpdate()
    {
      
    }

    IEnumerator DisplayDashBoard()
    {
        double vel = 0;
        while(true)
        {
            yield return new WaitForSecondsRealtime(0.05f);

            if (m_myCar.GetComponent<PlayerController>().velocity == vel) continue;

            string formattedVelocity = m_myCar.GetComponent<PlayerController>().velocity.ToString("F2");
            m_Car_DashBoard.text = formattedVelocity + " / " + this.m_max_velocity + "\n" + m_myCar.GetComponent<PlayerController>().axiss;


        }

    }
}
