using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovePlateController))]
public class MovePlateControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MovePlateController controller = (MovePlateController)target;
        
        GUILayout.Space(10);

        EditorGUILayout.LabelField("當前節點數量", controller.Points.Count.ToString());

        if (GUILayout.Button("新增節點"))
        {
            // 創建一個新的空物件
            GameObject newPoint = new("Point " + (controller.Points.Count + 1));
            newPoint.transform.position = controller.transform.position; // 設置新節點的位置

            // 將新節點添加到 MovePlateController 的節點列表中
            controller.Points.Add(newPoint.transform);

            // 將新節點設置為 MovePlateController 的子物件
            newPoint.transform.parent = controller.transform;
            newPoint.layer = controller.gameObject.layer;

            newPoint.SetActive(false); // 隱藏新節點物件

            // 標記場景已更改
            EditorUtility.SetDirty(controller);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("刪除節點"))
        {
            if (controller.Points.Count > 0)
            {
                // 獲取最後一個節點
                Transform lastPoint = controller.Points[controller.Points.Count - 1];

                // 從列表中移除最後一個節點
                controller.Points.RemoveAt(controller.Points.Count - 1);

                // 刪除節點物件
                DestroyImmediate(lastPoint.gameObject);

                // 標記場景已更改
                EditorUtility.SetDirty(controller);
            }
        }

        GUILayout.Space(50);

        if (GUILayout.Button("重置節點"))
        {
            // 遍歷並刪除所有節點
            foreach (Transform point in controller.Points)
            {
                DestroyImmediate(point.gameObject);
            }

            // 清空節點列表
            controller.Points.Clear();

            // 標記場景已更改
            EditorUtility.SetDirty(controller);
        }
    }
}