using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astonaut : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StoryManager.Instance.isCatched = true;
        }
    }
}
