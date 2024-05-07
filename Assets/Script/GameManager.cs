using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager = null;
    [HideInInspector] public UnityEvent<bool> OnViewChanged = null;
    [SerializeField] bool isSideView = true;
    public bool GetView() { return isSideView; }
    public void SetView(bool nextView)
    {
        if (changeViewTimer >= changeViewCd && isSideView != nextView) // 切換視角時執行一次
        {
            CMvcamController.cMvcamController.CameraShake(viewChangedShakeIntensity, viewChangedShakeDuration);
            StartCoroutine(ISetView(nextView));
        }
    }
    IEnumerator ISetView(bool nextView)
    {
        yield return new WaitForSeconds(changeViewDelay);
        isSideView = nextView;
        OnViewChanged?.Invoke(isSideView);
    }
    [SerializeField] float changeViewDelay = 0;
    public float changeViewCd = 0;
    float changeViewTimer = 0;

    [Space(10)]
    [Header("View Change Camera Shake Setting")]

    [SerializeField] float viewChangedShakeIntensity = 0;
    [SerializeField] float viewChangedShakeDuration = 0;

    [Space(10)]

    [SerializeField] Vector2 gravity = Vector2.zero;

    [Space(10)]

    [SerializeField] bool unFocusedAutoPause = true;
    [SerializeField] GameObject pausePanel = null;
    bool isPause = false;

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);

        gameManager.OnViewChanged.AddListener(ViewChanged);

        changeViewTimer = changeViewCd;

        ViewChanged(GetView());
    }

    void Update()
    {
        if (!Application.isFocused && !isPause && unFocusedAutoPause)
        {
            TogglePause(true);
        }

        if (changeViewTimer <= changeViewCd)
        {
            changeViewTimer += Time.deltaTime;
        }
    }

    void ViewChanged(bool currentView)
    {
        if (currentView)
        {
            Physics2D.gravity = gravity;
        }
        else
        {
            Physics2D.gravity = Vector2.zero;
        }
        changeViewTimer = 0;
    }

    public void TogglePause(bool set = false)
    {
        isPause = !isPause || set;
        pausePanel.SetActive(isPause || set);
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
