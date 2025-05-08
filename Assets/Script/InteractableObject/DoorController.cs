using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class DoorController : MonoBehaviour
{
    [SerializeField] bool _defaultClose = false;
    [SerializeField] Transform _doorMask = null;
    [SerializeField] SpriteRenderer _doorUp = null;
    BoxCollider2D _doorUpCol = null;
    [SerializeField] SpriteRenderer _doorDown = null;
    BoxCollider2D _doorDownCol = null;

    [Header("Door Setting")]

    [SerializeField] int _openSize = 1;
    [SerializeField] float _openSpeed = 1;
    Vector3 _originalPosition = Vector3.zero;

#if UNITY_EDITOR
    void OnValidate()
    {
        EditorApplication.delayCall += () =>
        {
            if (this == null) return; // 確保物件未被刪除
            OnEnable();
        };
    }
#endif

    void OnEnable()
    {
        _originalPosition = _doorMask.localPosition;
        _doorMask.localScale = new Vector3(_doorMask.localScale.x, _openSize * 2, _doorMask.localScale.z);
        _doorUp.size = new Vector2(_doorUp.size.x, _openSize);
        _doorDown.size = new Vector2(_doorDown.size.x, _openSize);

        _doorUpCol = _doorUp.GetComponent<BoxCollider2D>();
        _doorUpCol.size = new Vector2(_doorUpCol.size.x, _openSize);
        _doorUpCol.offset = new Vector2(_doorUpCol.offset.x, _openSize / 2);

        _doorDownCol = _doorDown.GetComponent<BoxCollider2D>();
        _doorDownCol.size = new Vector2(_doorDownCol.size.x, _openSize);
        _doorDownCol.offset = new Vector2(_doorDownCol.offset.x, -_openSize / 2);

        if (_defaultClose)
        {
            _doorUp.transform.localPosition = _doorDown.transform.localPosition = _originalPosition;
            _doorUpCol.enabled = _doorDownCol.enabled = true;
        }
        else
        {
            _doorUp.transform.localPosition = _originalPosition + Vector3.up * _openSize;
            _doorDown.transform.localPosition = _originalPosition + Vector3.down * _openSize;
            _doorUpCol.enabled = _doorDownCol.enabled = false;
        }
    }

    public void OpenDoor()
    {
        StartCoroutine(COpenDoor());
    }

    IEnumerator COpenDoor()
    {
        while (_doorUp.transform.localPosition.y < _originalPosition.y + _openSize)
        {
            _doorUp.transform.Translate(_openSpeed * Time.deltaTime * Vector3.up);
            _doorDown.transform.Translate(_openSpeed * Time.deltaTime * Vector3.down);
            yield return null;
        }
        _doorUp.transform.localPosition = _originalPosition + Vector3.up * _openSize;
        _doorDown.transform.localPosition = _originalPosition + Vector3.down * _openSize;

        _doorUpCol.enabled = false;
        _doorDownCol.enabled = false;
    }

    public void CloseDoor()
    {
        StartCoroutine(CCloseDoor());
    }

    IEnumerator CCloseDoor()
    {
        _doorUpCol.enabled = true;
        _doorDownCol.enabled = true;
        while (_doorUp.transform.localPosition.y > _originalPosition.y)
        {
            _doorUp.transform.Translate(_openSpeed * Time.deltaTime * Vector3.down);
            _doorDown.transform.Translate(_openSpeed * Time.deltaTime * Vector3.up);
            yield return null;
        }
        _doorUp.transform.localPosition = _originalPosition;
        _doorDown.transform.localPosition = _originalPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.rotation * new Vector2(1, _openSize * 2));
    }
}