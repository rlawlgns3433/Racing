using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : SingletonBehaviour<GameDirector>
{
    private Text m_Car_DashBoard;
    private GameObject m_myCar;
    private Rigidbody m_myCar_rigid;
    private float  m_max_velocity;
    private bool selectedMode = false;
    Stopwatch stopwatch;
    Button select_mode_btn;

    [SerializeField] private GameObject[] Modes = new GameObject[4];
    // GameDirector���� �����ؾ� �� ���׵�
    /// <summary>
    /// 1. ���� ��� ���� �� ó�� -> SelectGameMode()
    /// 2. ���� ���� �� Ÿ�̸� -> StartTimer(), EndTimer()
    /// 3. Start�� �ִ� �͵� ���� ���� �÷� ���� -> AnyGameStart()
    /// </summary>




    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
      
    }



    void AnyGameStart()
    {
        m_Car_DashBoard = GameObject.Find("CurVelocity").GetComponent<Text>();

        m_myCar = GameObject.Find("MyCar");
        m_myCar_rigid = m_myCar.GetComponent<Rigidbody>();
        m_max_velocity = 400.0f;

        StartCoroutine("DisplayDashBoard");
    }

    void StartTimer()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    double EndTimer()
    {
        TimeSpan elapsed = stopwatch.Elapsed;
        double totalSeconds = elapsed.TotalSeconds;

        stopwatch.Reset();
        return totalSeconds;
    }

    public void SelectGameMode()
    {
        select_mode_btn = GameObject.Find("select_mode").GetComponent<Button>();
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("mode").Length; i++)
        {
            Modes[i] = GameObject.FindGameObjectsWithTag("mode")[i];
        }

        for (int i = 0; i < 4; i++)
        {
            if(Modes[i] != null)
            {
                Modes[i].SetActive(!selectedMode);
            }
        }

        ColorBlock colorBlock = select_mode_btn.colors; // ��� ���� ��ư ����

        
        selectedMode = !selectedMode;
    }

    public void StartSingleMode()
    {
        SceneManager.LoadScene("SingleMode");

        Invoke("AnyGameStart", 1.0f);
    }

    public void StartMultiMode()
    {
        SceneManager.LoadScene("MultiMode");
    }
    public void StartStoryMode()
    {
        SceneManager.LoadScene("StoryMode");
    }
    public void StartAgentMode()
    {
        SceneManager.LoadScene("AgentMode");
    }


    // �ڷ�ƾ �κ� -> Thread
    // Ÿ�̸� ���� �߰� �ʿ�
    IEnumerator DisplayDashBoard()
    {
        double vel = 0; 
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.05f);

            if (m_myCar.GetComponent<PlayerController>().velocity == vel) continue;

            string formattedVelocity = m_myCar.GetComponent<PlayerController>().velocity.ToString("F2");
            m_Car_DashBoard.text = formattedVelocity + " / " + this.m_max_velocity + "\n" + m_myCar.GetComponent<PlayerController>().axiss;
        }
    }
}
