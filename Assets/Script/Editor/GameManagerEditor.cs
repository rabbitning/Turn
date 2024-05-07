using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameManager.gameManager = (GameManager)target;
        base.OnInspectorGUI();
    }
}