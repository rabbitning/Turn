using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : EffectByViewChange
{
    public static GameManager Instance { get; private set; }
    public PlayerController Player { get; private set; }
    [HideInInspector] public UnityEvent<bool> OnViewChanged = new();
    [SerializeField] bool _isSideScroll = true;
    public bool GetView() => _isSideScroll;
    public void SetView(bool nextView)
    {
        if (_viewChangeCooldownTimer >= ViewChangeCooldown && _isSideScroll != nextView) // 切換視角時執行一次
        {
            CMvcamController.Instance.CameraShake(_viewChangedShakeIntensity, _viewChangedShakeDuration);
            StartCoroutine(CSetView(nextView));
        }
    }
    IEnumerator CSetView(bool nextView)
    {
        yield return new WaitForSecondsRealtime(_viewChangeDelay);
        _isSideScroll = nextView;
        OnViewChanged?.Invoke(_isSideScroll);
    }
    [SerializeField] float _viewChangeDelay = 0;
    public float ViewChangeCooldown = 0;
    float _viewChangeCooldownTimer = 0;

    [Space(10)]
    [Header("View Change Camera Shake Setting")]

    [SerializeField] float _viewChangedShakeIntensity = 0;
    [SerializeField] float _viewChangedShakeDuration = 0;

    [Space(10)]

    [SerializeField] Vector2 _gravity = Vector2.zero;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);

        Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    protected override void Start()
    {
        Time.timeScale = 1;

        _viewChangeCooldownTimer = ViewChangeCooldown;

        base.Start();
    }

    void Update()
    {
        HandleInput();

        _viewChangeCooldownTimer += Time.deltaTime;
    }

    protected override void ViewChanged(bool isSS)
    {
        base.ViewChanged(isSS);
        if (isSS)
        {
            Physics2D.gravity = _gravity;
        }
        else
        {
            Physics2D.gravity = Vector2.zero;
        }
        _viewChangeCooldownTimer = 0;
    }

    void HandleInput()
    {
        if (Time.timeScale == 0) return;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetView(!_isSideScroll);
        }
    }

    public void Win()
    {
        StartCoroutine(CLoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void GameOver()
    {
        StartCoroutine(CLoadScene(SceneManager.GetActiveScene().buildIndex));
        // Player.Respawn();
    }

    IEnumerator CLoadScene(int SceneIndex, float delay = 0f)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(SceneIndex);
        Time.timeScale = 1;
    }
}