using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    string[] talks = { "���Ⱑ �� �����ͱ�?", "��...�ʴ� ����?", "���� ����ƺ����� �� ��. ���⸦ �����Ϸ� �Դ�!", "�츮��?.... �� �׷� ���� �ϴ°���?" };
    string[] character = { "Chan", "???" };
    int i;
    public Text talkText;
    public Text talkCharacter_Chan;
    public Text talkCharacter_A;

    // Update is called once per frame
    public void Start()
    {
        int i = 0;
        StartCoroutine(Action());
    }
    IEnumerator Action()
    {
        while(true && i < talks.Length)
        {
            if(i%2==1)
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
    }
}
