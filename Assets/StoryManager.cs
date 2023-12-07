using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    string[] talks = { "여기가 내 놀이터군?", "너...너는 뭐야?", "나는 아쿠아봉에서 온 자. 여기를 접수하러 왔다!", "우리를?.... 왜 그런 짓을 하는거지?" };
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
