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
    private Text m_CurrentTime;
    private GameObject m_myCar;
    private Rigidbody m_myCar_rigid;

    public float current_time;
    private float m_max_velocity;
    [SerializeField] private float m_countdown = 20.1f;
    private bool selectedMode = false;
    private bool setting = true;
    private bool loading = false;
    private bool isStart = false;

    private Stopwatch stopwatch;
    private Button select_mode_btn;


    // GameDirector에서 구현해야 할 사항들
    /// <summary>
    /// 1. 게임 모드 선택 시 처리 -> SelectGameMode()
    /// 2. 게임 시작 시 타이머 -> StartTimer(), EndTimer()
    /// 3. Start에 있는 것들 게임 시작 시로 변경 -> AnyGameStart()
    /// </summary>




    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        loadingScreen.SetActive(false);
    }

    private void Update()
    {
        if(isStart && m_CountDown.text == "0")
        {
            current_time += Time.deltaTime;
        }
    }

    void AnyGameStart()
    {
        m_Car_DashBoard = GameObject.Find("CurVelocity").GetComponent<Text>();
        m_CountDown = GameObject.Find("CountDown").GetComponent<Text>();
        m_CurrentTime = GameObject.Find("CurrentTime").GetComponent<Text>();
        loadingScreen = GameObject.Find("LoadingScreen");   
        m_myCar = GameObject.Find("MyCar");
        m_myCar_rigid = m_myCar.GetComponent<Rigidbody>();
        m_max_velocity = 100.0f;

        StartCoroutine(DisplayDashBoard());
        StartCoroutine(DisplayLoadingScreen(loadingScreen));
        StartCoroutine(DisplayCountDown(m_countdown));

        isStart = true;
    }

    void StartTimer()
    {
        
    }

    double EndTimer()
    {
        TimeSpan elapsed = stopwatch.Elapsed;
        double totalSeconds = elapsed.TotalSeconds;

        double totals = stopwatch.Elapsed.TotalSeconds;

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
        SceneManager.LoadScene("StoryMode");
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
    public void QuitGamePopUp()
    {

    }

    IEnumerator DisplayDashBoard()
    {
        double vel = 0;
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.05f);

            if (m_myCar.GetComponent<PlayerController>().mCarData.currentVelocity == vel) continue;

            m_Car_DashBoard.text = ((int)(m_myCar.GetComponent<PlayerController>().mCarData.currentVelocity * 4)).ToString();

            int hour, minute, second;
            hour = ((int)(current_time / 3600)); current_time %= 3600; minute = ((int)(current_time / 60)); second = (int)(current_time % 60);
            m_CurrentTime.text = /*hour.ToString() + ":" + minute.ToString() + ":" + second.ToString();*/current_time.ToString();
        }
    }

    IEnumerator DisplayCountDown(float countdown)
    {
        while (countdown > 0)
        {
            yield return new WaitForSeconds(0.00001f);
            countdown -= Time.deltaTime;
            if(countdown < 3)
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
}
