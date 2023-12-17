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
    public GameObject loadingScreen;
    private Text m_Car_DashBoard;
    private Text m_CountDown;
    public Text m_CurrentTime;
    public Text m_LapTime;
    private GameObject m_myCar;
    private Rigidbody m_myCar_rigid;

    public float current_time;
    private float m_max_velocity;
    private int lobbycnt;
    [SerializeField] private float m_countdown = 7.1f;
    private bool selectedMode = false;
    private bool setting = true;
    private bool loading = false;
    private bool isStart = false;

    private Stopwatch stopwatch;
    private Button select_mode_btn;
    public RectTransform Needle;
    public PlayerController playerController;
    public StartLine startline;

    public Button single_btn, multi_btn, agent_btn, story_btn, garage_btn;

    // GameDirector에서 구현해야 할 사항들
    /// <summary>
    /// 1. 게임 모드 선택 시 처리 -> SelectGameMode()
    /// 2. 게임 시작 시 타이머 -> StartTimer(), EndTimer()
    /// 3. Start에 있는 것들 게임 시작 시로 변경 -> AnyGameStart()
    /// 4. needle의 범위는 (180~-90 각도)
    /// </summary>




    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        loadingScreen.SetActive(false);
        lobbycnt = 0;
    }

    private void Update()
    {
        if (isStart)
        {
            Needle.rotation = Quaternion.Euler(0, 0, 180 - (m_myCar.GetComponent<PlayerController>().mCarData.currentVelocity * 4) * (float)(0.675));
            if (m_CountDown.text == "0")
            {
                current_time += Time.deltaTime;
            }
        }

        if (SceneManager.GetActiveScene().name == "LobbyScene") 
        {
            isStart = false;
            if(lobbycnt != 0)
            {
                InitMode();
                lobbycnt = 0;
            }
        }
        else
        {
            lobbycnt++;
        }
    }

    void AnyGameStart()
    {
        m_Car_DashBoard = GameObject.Find("CurVelocity").GetComponent<Text>();
        m_CountDown = GameObject.Find("CountDown").GetComponent<Text>();
        m_CurrentTime = GameObject.Find("CurrentTime").GetComponent<Text>();
        loadingScreen = GameObject.Find("LoadingScreen");
        Needle = GameObject.Find("Needle").GetComponent<RectTransform>();
        m_myCar = GameObject.Find("MyCar");
        playerController = m_myCar.GetComponent<PlayerController>();
        startline = GameObject.Find("startline").GetComponent<StartLine>();
        m_LapTime = GameObject.Find("LapTime").GetComponent<Text>();

        m_myCar_rigid = m_myCar.GetComponent<Rigidbody>();
        m_max_velocity = 100.0f;

        StartCoroutine(DisplayDashBoard());
        StartCoroutine(DisplayLoadingScreen(loadingScreen));
        StartCoroutine(DisplayCountDown(m_countdown));

        isStart = true;
    }

    public void StartSingleMode()
    {
        StartCoroutine(loadScene("SingleMode"));
    }

    public void StartMultiMode()
    {
        SceneManager.LoadScene("TestMode");
        loadingScreen.SetActive(true);
        Invoke("AnyGameStart", 1.0f);
    }
    public void StartStoryMode()
    {
        StartCoroutine(loadScene("StoryMode"));
    }
    public void StartAgentMode()
    {
        SceneManager.LoadScene("TestMode2");
        loadingScreen.SetActive(true);
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

    IEnumerator DisplayDashBoard()
    {
        double vel = 0;
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.05f);

            if (m_myCar.GetComponent<PlayerController>().mCarData.currentVelocity == vel) continue;

            m_Car_DashBoard.text = ((int)(m_myCar.GetComponent<PlayerController>().mCarData.currentVelocity * 4)).ToString();

            current_time = Math.Abs(playerController.m_countdown);

            int hour, minute, second;
            string str_hour = "00", str_minute = "00", str_second = "00";

            hour = ((int)(current_time / 3600)); current_time %= 3600; minute = ((int)(current_time / 60)); second = (int)(current_time % 60);
            if (hour < 10) { str_hour = "0" + hour.ToString(); } else { str_hour = hour.ToString(); }
            if (minute < 10) { str_minute = "0" + minute.ToString(); } else { str_minute = minute.ToString(); }
            if (second < 10) { str_second = "0" + second.ToString(); } else { str_second = second.ToString(); }

            if (playerController.m_countdown < 0)
            {
                m_CurrentTime.text = str_hour + ":" + str_minute + ":" + str_second; /*current_time.ToString();*/
            }

        }
    }

    IEnumerator DisplayCountDown(float countdown)
    {
        while (countdown > 0)
        {
            yield return new WaitForSeconds(0.00001f);
            countdown -= Time.deltaTime;
            if (countdown < 3)
            {
                string cur_time = Mathf.Round(countdown).ToString();
                m_CountDown.text = cur_time;

                UnityEngine.Debug.Log(cur_time);

                if (m_CountDown.text == "0")
                {
                    m_CountDown.text = "";
                    //StartTimer();
                    break;
                }
            }

        }
    }

    IEnumerator DisplayLoadingScreen(GameObject loadingScreen)
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (loadingScreen != null)
                {
                    loadingScreen.SetActive(!loading);
                    yield return new WaitForSeconds(2);
                    loadingScreen.SetActive(loading);
                }
            }
        }
    }

    IEnumerator loadScene(string SceneName)
    {
        loadingScreen.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        loadingScreen.SetActive(false);
        SceneManager.LoadScene(SceneName);
        Invoke("AnyGameStart", 1.0f);
    }

    private void InitMode()
    {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("mode").Length; i++)
        {
            Modes[i] = GameObject.FindGameObjectsWithTag("mode")[i];
        }

        settings = GameObject.Find("Settings");

        single_btn = GameObject.Find("SingleMode").GetComponent<Button>();
        multi_btn = GameObject.Find("MultiMode").GetComponent<Button>();
        agent_btn = GameObject.Find("AgentMode").GetComponent<Button>();
        story_btn = GameObject.Find("StoryMode").GetComponent<Button>();
        garage_btn = GameObject.Find("Garage").GetComponent<Button>();

        single_btn.onClick.AddListener(StartSingleMode);
        multi_btn.onClick.AddListener(StartMultiMode);
        agent_btn.onClick.AddListener(StartAgentMode);
        story_btn.onClick.AddListener(StartStoryMode);
        garage_btn.onClick.AddListener(EnterGarage);
    }
}