using UnityEngine;

public class CanMoveByMovePlatform : MonoBehaviour
{
    public void AttachToParent(Transform parent)
    {
        transform.SetParent(parent);
    }
    public void DetachFromParent()
    {
        transform.SetParent(null);
    }
}