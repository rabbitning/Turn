using UnityEngine;
using Cinemachine;
using System.Collections;

public class BossOpening : MonoBehaviour
{
    [SerializeField] BossController _boss = null;
    PlayerController _player = null;
    CinemachineVirtualCamera _camera = null;
    Vector2 _originalCameraDeadZone = Vector2.zero;

    void Start()
    {
        _camera = CMvcamController.Instance.GetComponent<CinemachineVirtualCamera>();
        StartCoroutine(StartOpening());
    }

    IEnumerator StartOpening()
    {
        CinemachineFramingTransposer transposer = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _originalCameraDeadZone = new Vector2(transposer.m_DeadZoneWidth, transposer.m_DeadZoneHeight);

        _player = PlayerController.Instance;
        GameManager.Instance.IsPaused = true;
        _player.CanMove = _boss.CanMove = false;

        yield return new WaitForSeconds(1f);

        transposer.m_DeadZoneWidth = transposer.m_DeadZoneHeight = 0;
        _camera.m_Follow = _boss.transform;

        yield return new WaitForSeconds(1f);

        _boss.Entry();

        yield return new WaitForSeconds(4f);

        transposer.m_DeadZoneWidth = _originalCameraDeadZone.x;
        transposer.m_DeadZoneHeight = _originalCameraDeadZone.y;
        _camera.m_Follow = _player.transform;

        GameManager.Instance.IsPaused = false;
        _player.CanMove = _boss.CanMove = true;
    }
}
