using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            // SingletonBehaviour�� �ʱ�ȭ �Ǳ� ���̶��
            if (_instance == null)
            {
                // �ش� ������Ʈ�� ã�� �Ҵ��Ѵ�.
                _instance = FindObjectOfType<T>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
        // ������ �� ������ 2������ �û��ϰ� �ȴ�.
        if (_instance != null)
        {
            // (1) �ٸ� ���� ������Ʈ�� �ִٸ�
            if (_instance != this)
            {
                // �ϳ��� ���� ������Ʈ�� ������ �����Ѵ�.
                Destroy(gameObject);
            }


            // (2) Awake() ȣ�� �� �Ҵ�� �ν��Ͻ��� �ڱ� �ڽ��̶��
            // �ƹ��͵� ���� �ʴ´�.


            return;
        }

        // �� �Ʒ��� ���� SingletonBahaviour�� ���� ����
        // Instance ���� �� Awake()�� ����Ǵ� ����̴�.
        _instance = GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }
}