using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Collections.ObjectModel;

public class CSVImporterForScriptableObject : AssetPostprocessor
{

	/// <summary>
	/// CSVファイル管理ディレクトリのベースパス
	/// </summary>
    const string CSVBasePath = "Assets/Project/";

    public static readonly string ImportCSVBasePath = CSVBasePath + "CSV/Masters/";
    public static readonly string ExportAssetBasePath = CSVBasePath + "Resources/ScriptableObjects/Masters/";

    public static readonly ReadOnlyCollection<string> MasterThesauruses = new ReadOnlyCollection<string> (new string[] {
        "Card"
		// マスタテーブルごとに追加
    });

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        ImportMasterCSVs (importedAssets);
    }

    /// <summary>
    /// Imports the master CSV.
    /// </summary>
    /// <param name="importedAssets">Imported assets.</param>
    static void ImportMasterCSVs (string[] importedAssets)
    {
        Debug.Log("Start Importing Master CSV.");

        foreach (string importedAsset in importedAssets)
        {
            foreach (string masterThesaurus in MasterThesauruses)
            {
                string masterName = masterThesaurus + "Master";
                Debug.Log("Importing Master CSV: " + masterName);

                // チェック対象ファイルが見当たるか
                string importCSVFile = ImportCSVBasePath + masterName + ".csv";
                if (!importCSVFile.Equals (importedAsset)) continue;

                Type masterClassType = Type.GetType(masterName + ",Assembly-CSharp");

                // PrimaryKeysが取得できるか(マスタのAsset名に使用)
                string[] primaryKeys = MasterPrimaryKeys (masterClassType);
                if (primaryKeys.Length == 0) continue;

                using (StreamReader sr = new StreamReader(importCSVFile))
                {
                    // ヘッダーのカラム名からPrimaryKeyのindexを取得しておく
                    string headerLine = sr.ReadLine ();
                    string[] headerLineStrs = headerLine.Split(',');
                    List<int> primaryKeyIndexs = new List<int> ();
                    foreach (string primaryKey in primaryKeys)
                    {
                        int index = Array.IndexOf(primaryKeys, primaryKey);
                        primaryKeyIndexs.Add(index);
                    }

                    // 行ごとにAsse作成・更新
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] dataStrs = line.Split(',');

                        string exportFileId = "";
                        foreach (int primaryKeyIndex in primaryKeyIndexs)
                        {
                            if (string.IsNullOrEmpty(exportFileId))
                            {
                                exportFileId = dataStrs[primaryKeyIndex];
                            }
                            else
                            {
                                exportFileId += "_" + dataStrs[primaryKeyIndex];
                            }
                        }

                        // データ置き換え
                        string exportFile = ExportAssetBasePath + masterName + "/" + masterName + exportFileId + ".asset";
                        MethodInfo replaceDataMethod = masterClassType.GetMethod ("ReplaceData");
                        replaceDataMethod.Invoke(null, new object[] {exportFile, headerLineStrs, dataStrs});
                    }
                }
                AssetDatabase.SaveAssets();

                Debug.Log("Completed importing Master CSV. " + importCSVFile);
            }
        }
    }

    /// <summary>
    /// The primary keys of the master.
    /// マスタのPrimaryKeyを取得
    /// </summary>
    /// <returns>The primary keys.</returns>
    /// <param name="masterClassType">Master class type.</param>
    static string[] MasterPrimaryKeys (Type masterClassType)
    {
        FieldInfo primaryKeysField = masterClassType.GetField ("primaryKeys");
        ReadOnlyCollection<string> primaryKeys = (ReadOnlyCollection<string>)primaryKeysField.GetValue (null);
        string[] stringArray = new string[primaryKeys.Count];
        primaryKeys.CopyTo(stringArray, 0);
        return stringArray;
    }
}

