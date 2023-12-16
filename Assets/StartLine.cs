using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    public float m_time;
    public bool isArrived;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // ÇÑ ¹ÙÄû µµÂø
            isArrived = true;
            GameDirector.Instance.m_LapTime.text += GameDirector.Instance.m_CurrentTime.text + "\n";
            gameObject.GetComponent<BoxCollider>().enabled = false;
            m_time = 0;
        }
    }
    private void Start()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        if(m_time > 20)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
