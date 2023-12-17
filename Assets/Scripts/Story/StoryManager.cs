using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryManager : SingletonBehaviour<StoryManager>
{
    string[] talks = { "여기가 내 놀이터군?", "너...너는 뭐야?", "나는 아쿠아봉에서 온 자. 여기를 접수하러 왔다!", "우리를?.... 왜 그런 짓을 하는거지?", "여기가 적당히 놀고 장난치기 좋은 곳인 거 같아 크킄", "너가 그런 목적이라면 나도 지구를 지켜야 할 이유가 생긴 거 같아.", "날 잡을 수 있다고? 멍청하다 인간", "뭐? 난 우리 인류를 위해 너를 꼭 잡고 말겠어" };
    string[] catchTalks = { "으읏,,, 잡히다니. 분하다 ㅡㅅㅡ", "이런 놈을 잡았으니 난 많은 현상금을 받겠지?", "인간 따위가 감히 나를?", "넌 곧 실험체가 될 몸이니 푹 쉬어두라구 크하하하하!" };
    public int i;
    public bool isCatched;
    public GameObject maincam;
    public GameObject Astronaut;
    public GameObject Chan;
    public GameObject MyCar;
    public GameObject TalkPanel;
    public Text talkText;
    public Text talkCharacter_Chan;
    public Text talkCharacter_A;
    public StoryCam storyCam;
    public GameObject ClearScreen;

    private Vector3 AstronautPosition;
    private Quaternion AstronautRotation;
    // Update is called once per frame
    public void Start()
    {
        int i = 0;
        AstronautPosition = Astronaut.transform.position;
        AstronautRotation = Astronaut.transform.rotation;
        StartCoroutine(Action());
        StartCoroutine(CatchTalk());
    }

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.R) && MyCar.activeInHierarchy) || (MyCar.transform.position.y < 65))
        {
            ResetCarPosition();
        }

        if(isCatched)
        {
            Astronaut.GetComponent<RandomMovement>().animator.Play("Happy");
            Astronaut.GetComponent<RandomMovement>().enabled = false;
            // 게임 종료 로직
            Astronaut.transform.position = AstronautPosition;
            Astronaut.transform.rotation = AstronautRotation;
            Chan.SetActive(true);
            MyCar.SetActive(false);
            storyCam.enabled = true;
            TalkPanel.SetActive(true);

            // astronaut 움직임 멈춤
            // 캐릭터와 astronaut의 대화
            // 스토리 모드 클리어 화면
            // 클리어 화면에는 메인 메뉴로 이동 버튼생성
            StopCoroutine(Action());
        }

        if(ClearScreen.activeInHierarchy)
        {
            if(Input.anyKeyDown)
            {
                StopCoroutine(Action());
                StopCoroutine(CatchTalk());
                Destroy(gameObject);
                SceneManager.LoadScene("LobbyScene");
            }
        }
    }
    IEnumerator Action()
    {
        yield return new WaitForSeconds(6.0f);

        TalkPanel.SetActive(true);

        while (true && i < talks.Length)
        {
            if (i % 2 == 1)
            {
                talkCharacter_A.enabled = false;
                talkCharacter_Chan.enabled = true;
            }
            else
            {
                talkCharacter_A.enabled = true;
                talkCharacter_Chan.enabled = false;
            }
            talkText.text = talks[i++];
            yield return new WaitForSeconds(2.5f);
        }
        Astronaut.GetComponent<RandomMovement>().enabled = true;
        Chan.SetActive(false);
        MyCar.SetActive(true);

        TalkPanel.SetActive(false);
        storyCam.enabled = false;
        talkText.text = "";
        i = 0;
    }

    IEnumerator CatchTalk()
    {
        Debug.Log("CatchTalk");
        Debug.Log(catchTalks.Length);
        yield return new WaitForSeconds(1.0f);
        while (true)
        {
            if(i < catchTalks.Length && isCatched)
            {
                Debug.Log(catchTalks.Length);
                if (i % 2 == 1)
                {
                    talkCharacter_A.enabled = false;
                    talkCharacter_Chan.enabled = true;
                }
                else
                {
                    talkCharacter_A.enabled = true;
                    talkCharacter_Chan.enabled = false;
                }
                talkText.text = catchTalks[i++];
                yield return new WaitForSeconds(2.5f);
            }
            else if(i >= catchTalks.Length && isCatched)
            {
                ClearScreen.SetActive(true);
                StopCoroutine(CatchTalk());
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ResetCarPosition()
    {
        MyCar.transform.position = new Vector3(-324.24f, 70.8f, 51.33f);
        MyCar.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

        PlayerController playerController = MyCar.GetComponent<PlayerController>();
        for (int i = 0; i < MyCar.GetComponent<PlayerController>().wheels.Length; i++)
        {
            playerController.wheels[i].motorTorque = 0;
        }

    }
}
