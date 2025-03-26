using UnityEngine;

public interface IPlatformMovable
{
    void AttachToParent(Transform parent);
    void DetachFromParent();
}