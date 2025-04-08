using UnityEngine;
using UnityEngine.Events;

public class LeverController : MonoBehaviour
{
    public UnityEvent OnLeverPulled;
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) PullLever();
    }

    void PullLever()
    {
        _animator.SetTrigger("PullLever");

        OnLeverPulled?.Invoke();
        Destroy(gameObject.GetComponent<Collider2D>());
    }

    // override protected void ViewChanged(bool isSS)
    // {
    //     _animator.SetBool("IsSS", isSS);
    //     base.ViewChanged(isSS);
    // }
}