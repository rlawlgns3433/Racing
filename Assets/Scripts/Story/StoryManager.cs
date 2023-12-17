using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryManager : SingletonBehaviour<StoryManager>
{
    string[] talks = { "���Ⱑ �� �����ͱ�?", "��...�ʴ� ����?", "���� ����ƺ����� �� ��. ���⸦ �����Ϸ� �Դ�!", "�츮��?.... �� �׷� ���� �ϴ°���?", "���Ⱑ ������ ��� �峭ġ�� ���� ���� �� ���� ũ��", "�ʰ� �׷� �����̶�� ���� ������ ���Ѿ� �� ������ ���� �� ����.", "�� ���� �� �ִٰ�? ��û�ϴ� �ΰ�", "��? �� �츮 �η��� ���� �ʸ� �� ��� ���ھ�" };
    string[] catchTalks = { "����,,, �����ٴ�. ���ϴ� �Ѥ���", "�̷� ���� ������� �� ���� ������� �ް���?", "�ΰ� ������ ���� ����?", "�� �� ����ü�� �� ���̴� ǫ ����ζ� ũ��������!" };
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
            // ���� ���� ����
            Astronaut.transform.position = AstronautPosition;
            Astronaut.transform.rotation = AstronautRotation;
            Chan.SetActive(true);
            MyCar.SetActive(false);
            storyCam.enabled = true;
            TalkPanel.SetActive(true);

            // astronaut ������ ����
            // ĳ���Ϳ� astronaut�� ��ȭ
            // ���丮 ��� Ŭ���� ȭ��
            // Ŭ���� ȭ�鿡�� ���� �޴��� �̵� ��ư����
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
