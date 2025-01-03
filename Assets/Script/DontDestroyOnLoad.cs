using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // private static DontDestroyOnLoad _instance;

    // private void Awake()
    // {
    //     if (_instance != null && _instance != this)
    //     {
    //         Destroy(gameObject); // 防止重複
    //         return;
    //     }

    //     _instance = this;
    //     DontDestroyOnLoad(gameObject);
    // }
}