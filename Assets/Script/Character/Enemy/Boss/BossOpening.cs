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

        _player = GameManager.Instance.Player;
        GameManager.Instance.IsPaused = true;
        _player.CanMove = _boss.CanMove = false;

        yield return new WaitForSeconds(1.5f);

        _camera.m_Follow = _boss.transform;
        transposer.m_DeadZoneWidth = transposer.m_DeadZoneHeight = 0;

        yield return new WaitForSeconds(3f);

        _camera.m_Follow = _player.transform;
        transposer.m_DeadZoneWidth = _originalCameraDeadZone.x;
        transposer.m_DeadZoneHeight = _originalCameraDeadZone.y;

        GameManager.Instance.IsPaused = false;
        _player.CanMove = _boss.CanMove = true;
    }
}
