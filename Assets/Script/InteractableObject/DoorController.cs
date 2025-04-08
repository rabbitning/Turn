using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
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
        _doorUp.GetComponent<Collider2D>().enabled = false;
        _doorDown.GetComponent<Collider2D>().enabled = false;
    }

    public void CloseDoor()
    {
        StartCoroutine(CCloseDoor());
    }

    IEnumerator CCloseDoor()
    {
        _doorUp.GetComponent<Collider2D>().enabled = true;
        _doorDown.GetComponent<Collider2D>().enabled = true;
        while (_doorUp.transform.position.y > _doorUpStartPos.y)
        {
            _doorUp.transform.Translate(Vector3.down * _openSpeed * Time.deltaTime);
            _doorDown.transform.Translate(Vector3.up * _openSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector2(1, _openSize * 2));
    }
}