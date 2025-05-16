using UnityEngine;

public class CanDestory : MonoBehaviour
{
    public void End()
    {
        if (gameObject == null) return;
        Destroy(gameObject);
    }
}
