using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CMvcamController : MonoBehaviour
{
    public static CMvcamController cMvcamController { get; private set; }
    CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    float shakeDuration = 0;
    float shakeTimer = 0;
    float shakeStartIntensity = 0;

    void Awake()
    {
        cMvcamController = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    void Start()
    {
        GameManager.gameManager.OnViewChanged.AddListener(ViewChanged);
        ViewChanged(GameManager.gameManager.GetView());
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(shakeStartIntensity, 0, 1 - (shakeTimer / shakeDuration));
        }
    }

    void ViewChanged(bool currentView)
    {
        if (currentView)
        {
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0;
        }
        else
        {
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0.09f;
        }
    }

    public void CameraShake(float intesity, float duration)
    {
        shakeStartIntensity = cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intesity;
        shakeDuration = shakeTimer = duration;
    }
}
