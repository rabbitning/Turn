using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerBodyController))]
public class PlayerBodyEditor : Editor
{
    PlayerBodyController m_Target = null;
    public override void OnInspectorGUI()
    {
        m_Target = (PlayerBodyController)target;
        m_Target.col = m_Target.GetComponent<BoxCollider2D>();
        base.OnInspectorGUI();
    }
}
