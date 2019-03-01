using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptableObjectCreatorBase : MonoBehaviour {

    protected const string MenuItemPathBase = "Assets/Create/ScriptableObject/Master/";
    const string CreateMasterPathBase = "Assets/Project/Resources/ScriptableObjects/Masters/";

    protected static void CreateAsset<Type>(string masterName) where Type : ScriptableObject
    {
        Type item = ScriptableObject.CreateInstance<Type>();

        string dataPath = CreateMasterPathBase + masterName + "/";
        string[] path_array = Directory.GetFiles(dataPath, "*.asset");

        string dataNo = (path_array.Length + 1).ToString();
        string path = AssetDatabase.GenerateUniqueAssetPath(dataPath + "Data" + dataNo + ".asset");

        AssetDatabase.CreateAsset(item, path);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = item;
    }
}

