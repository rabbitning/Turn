using System.Collections;
using UnityEngine;

public class DoorController : EffectByViewChange
{
    [SerializeField] GameObject _doorUp;
    [SerializeField] GameObject _doorDown;

    [Header("Door Setting")]

    [SerializeField] float _openSize = 1;
    Vector3 _doorUpStartPos;
    [SerializeField] float _openSpeed = 1;

    public void OpenDoor()
    {
        StartCoroutine(COpenDoor());
    }

    IEnumerator COpenDoor()
    {
        _doorUpStartPos = _doorUp.transform.position;
        while (_doorUp.transform.position.y < _doorUpStartPos.y + _openSize)
        {
            _doorUp.transform.Translate(Vector3.up * _openSpeed * Time.deltaTime);
            _doorDown.transform.Translate(Vector3.down * _openSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(_doorUp.GetComponent<Collider2D>());
        Destroy(_doorDown.GetComponent<Collider2D>());
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position - Vector3.up * 1.25f, new Vector2(1, _openSize * 2));
    }
}