using UnityEditor;
using UnityEngine;

namespace movementsystem.Scripts.Editor.Menu
{
    public class CreatePlayer : MonoBehaviour
    {
        [MenuItem("GameObject/Movement System/Player", false, 10)]
        private static void CreateCustomPrefab()
        {
            string prefabPath = "Packages/com.emirhan.movementsystem/movementsystem/Prefabs/Player/PlayerRoot.prefab";

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab != null)
            {
                PrefabUtility.InstantiatePrefab(prefab);
            }
            else
            {
                Debug.LogError("Prefab bulunamadı! Yolun doğru olduğundan emin olun: " + prefabPath);
            }
        }
    }
}