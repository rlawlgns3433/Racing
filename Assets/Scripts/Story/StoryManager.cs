using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class StoryManager : SingletonBehaviour<StoryManager>
{
    string[] talks = { "���Ⱑ �� �����ͱ�?", "��...�ʴ� ����?", "���� ����ƺ����� �� ��. ���⸦ �����Ϸ� �Դ�!", "�츮��?.... �� �׷� ���� �ϴ°���?", "���Ⱑ ������ ��� �峭ġ�� ���� ���� �� ���� ũ��", "�ʰ� �׷� �����̶�� ���� ������ ���Ѿ� �� ������ ���� �� ����.", "�� ���� �� �ִٰ�? ��û�ϴ� �ΰ�", "��? �� �츮 �η��� ���� �ʸ� �� ��� ���ھ�" };
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
            // ���� ���� ����
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
