using UnityEngine;
using UnityEditor;
using System.Collections.ObjectModel;

public class CardMaster : MasterBaseScriptableObject
{
    const string MasterName = "CardMaster";

    public static readonly ReadOnlyCollection<string> primaryKeys = new ReadOnlyCollection<string>(new string[] {
        "cardId",
    });

    public int cardId; // カードID
    public int sortId; // ソートID
    public int cardType; // カード種別（1:ユニット, 2:マジック）
    public int masterId; // 種別に応じた参照先マスタのID
    public string periodStartAt; // 公開開始日時
    public string periodFinishAt; // 公開終了日時

    /// <summary>
    /// Replaces the data.
    /// ScriptableObjectデータを更新
    /// </summary>
    /// <param name="exportFile">Export file.</param>
    /// <param name="columnKeys">Column keys.</param>
    /// <param name="columnValues">Column values.</param>
    public static void ReplaceData(string exportFile, string[] columnKeys, string[] columnValues)
    {
        CardMaster data = AssetDatabase.LoadAssetAtPath<CardMaster>(exportFile);
        if (data == null)
        {
            data = ScriptableObject.CreateInstance<CardMaster>();
            AssetDatabase.CreateAsset((ScriptableObject)data, exportFile);
        }
        ReplaceColumnValues(data, columnKeys, columnValues);
        EditorUtility.SetDirty(data);
    }

    public static CardMaster GetById (int cardId) // TODO 共通化して複合主キーで取得できるように
    {
        return Resources.Load<CardMaster>("ScriptableObjects/Masters/" + MasterName + "/" + MasterName + cardId);
    }

}

