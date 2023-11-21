using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : SingletonBehaviour<GameDirector>
{
    [SerializeField] private GameObject[] Modes = new GameObject[4];
    [SerializeField] private GameObject settings;
    private Text m_Car_DashBoard;
    private Text m_CountDown;
    private GameObject m_myCar;
    private Rigidbody m_myCar_rigid;

    private float m_max_velocity;
    [SerializeField] private float m_countdown = 3f;
    private bool selectedMode = false;
    private bool setting = true;


    private Stopwatch stopwatch;
    private Button select_mode_btn;


    // GameDirector에서 구현해야 할 사항들
    /// <summary>
    /// 1. 게임 모드 선택 시 처리 -> SelectGameMode()
    /// 2. 게임 시작 시 타이머 -> StartTimer(), EndTimer()
    /// 3. Start에 있는 것들 게임 시작 시로 변경 -> AnyGameStart()
    /// </summary>




    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void AnyGameStart()
    {
        m_Car_DashBoard = GameObject.Find("CurVelocity").GetComponent<Text>();
        m_CountDown = GameObject.Find("CountDown").GetComponent<Text>();
        m_myCar = GameObject.Find("MyCar");
        m_myCar_rigid = m_myCar.GetComponent<Rigidbody>();
        m_max_velocity = 400.0f;

        StartCoroutine("DisplayDashBoard");
        StartCoroutine(DisplayCountDown(m_countdown));

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
            if (Modes[i] != null)
            {
                Modes[i].SetActive(!selectedMode);
            }
        }

        ColorBlock colorBlock = select_mode_btn.colors; // 모드 선택 버튼 색상


        selectedMode = !selectedMode;
    }

    public void StartSingleMode()
    {
        SceneManager.LoadScene("SingleMode");

        Invoke("AnyGameStart", 1.0f);
    }

    public void StartMultiMode()
    {
        SceneManager.LoadScene("TestMode");
        Invoke("AnyGameStart", 1.0f);
    }
    public void StartStoryMode()
    {
        SceneManager.LoadScene("StoryMode");
    }
    public void StartAgentMode()
    {
        SceneManager.LoadScene("TestMode2");
        Invoke("AnyGameStart", 1.0f);
    }
    public void EnterGarage()
    {
        SceneManager.LoadScene("Garage");
    }
    public void Settings()
    {
        if (settings != null)
        {
            settings.SetActive(!setting);
            settings.transform.GetChild(0).gameObject.SetActive(!setting);
            setting = !setting;

        }


        UnityEngine.Debug.Log(setting);

    }

    // 코루틴 부분 -> Thread
    // 타이머 로직 추가 필요
    IEnumerator DisplayDashBoard()
    {
        double vel = 0;
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.05f);

            if (m_myCar.GetComponent<PlayerController>().currentVelocity == vel) continue;

            m_Car_DashBoard.text = m_myCar.GetComponent<PlayerController>().currentVelocity * 4 + " / " + this.m_max_velocity + "\n" + m_myCar.GetComponent<PlayerController>().axiss;
        }
    }

    IEnumerator DisplayCountDown(float countdown)
    {
        while (countdown > 0)
        {
            yield return new WaitForSeconds(0.00001f);
            countdown -= Time.deltaTime;
            string cur_time = Mathf.Round(countdown).ToString();
            m_CountDown.text = cur_time;

            UnityEngine.Debug.Log(cur_time);

            if (m_CountDown.text == "0")
            {
                m_CountDown.text = "";
                break;
            }
        }
    }
}
