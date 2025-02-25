using System.Collections.Generic;
using UnityEngine;

public class MovePlateController : MonoBehaviour
{
    [SerializeField] GameObject _plate = null;
    Rigidbody2D _plateRb = null;

    [SerializeField] float _speed = 1f;
    [HideInInspector] public List<Transform> Points = new();
    int _currentPointIndex = 0;
    bool _moveBack = false;

    // Rigidbody2D _plateRb = null;

    void Start()
    {
        if (Points.Count > 0)
        {
            _plate.transform.position = Points[0].position;
        }
        else
        {
            _plate.transform.position = transform.position;
        }
        _plateRb = _plate.GetComponent<Rigidbody2D>();
    }

    // void Update()
    // {

    // }

    void FixedUpdate()
    {
        if (Points.Count == 0)
        {
            _plateRb.velocity = Vector2.zero;
            return;
        }

        var target = Points[_currentPointIndex];
        var direction = (target.position - _plate.transform.position).normalized;
        _plateRb.velocity = direction * _speed;

        if (Vector2.Distance(_plate.transform.position, target.position) < 0.1f)
        {
            if (_moveBack)
            {
                _currentPointIndex--;
                if (_currentPointIndex < 0)
                {
                    _currentPointIndex = 0;
                    _moveBack = false;
                }
            }
            else
            {
                _currentPointIndex++;
                if (_currentPointIndex >= Points.Count)
                {
                    _currentPointIndex = Points.Count - 1;
                    _moveBack = true;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (Points.Count == 0) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(Points[0].position, 0.1f);

        Gizmos.color = Color.gray;
        for (int i = 1; i < Points.Count; i++)
        {
            Gizmos.DrawSphere(Points[i].position, 0.1f);
        }

        Gizmos.color = Color.yellow;
        for (int i = 0; i < Points.Count - 1; i++)
        {
            Gizmos.DrawLine(Points[i].position, Points[i + 1].position);
        }
    }
}
