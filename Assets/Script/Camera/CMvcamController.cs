using UnityEngine;
using Cinemachine;

public class CMvcamController : EffectByViewChange
{
    public static CMvcamController Instance { get; private set; }
    CinemachineVirtualCamera _cinemachineVirtualCamera;
    CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    float _shakeDuration = 0;
    float _shakeTimer = 0;
    float _shakeStartIntensity = 0;

    void Awake()
    {
        Instance = this;
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    protected override void Start()
    {
        _cinemachineVirtualCamera.m_Follow = PlayerController.Instance.transform;
        base.Start();
    }

    void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_shakeStartIntensity, 0, 1 - (_shakeTimer / _shakeDuration));
        }
    }

    protected override void ViewChanged(bool isSS)
    {
        _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = isSS ? 0 : 0.15f;
    }

    public void CameraShake(float intesity, float duration)
    {
        _shakeStartIntensity = _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intesity;
        _shakeDuration = _shakeTimer = duration;
    }
}
