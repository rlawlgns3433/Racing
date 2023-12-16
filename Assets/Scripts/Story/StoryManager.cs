using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class StoryManager : SingletonBehaviour<StoryManager>
{
    string[] talks = { "여기가 내 놀이터군?", "너...너는 뭐야?", "나는 아쿠아봉에서 온 자. 여기를 접수하러 왔다!", "우리를?.... 왜 그런 짓을 하는거지?", "여기가 적당히 놀고 장난치기 좋은 곳인 거 같아 크킄", "너가 그런 목적이라면 나도 지구를 지켜야 할 이유가 생긴 거 같아.", "날 잡을 수 있다고? 멍청하다 인간", "뭐? 난 우리 인류를 위해 너를 꼭 잡고 말겠어" };
    int i;
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
    // Update is called once per frame
    public void Start()
    {
        int i = 0;
        StartCoroutine(Action());
    }

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.R) && MyCar.activeInHierarchy) || (MyCar.transform.position.y < 65))
        {
            ResetCarPosition();
        }

        if(isCatched)
        {
            // 게임 종료 로직
            Debug.Log("Catch!");
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
            yield return new WaitForSeconds(2);
        }
        Astronaut.GetComponent<RandomMovement>().enabled = true;
        Chan.SetActive(false);
        MyCar.SetActive(true);

        TalkPanel.SetActive(false);
        storyCam.enabled = false;
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
