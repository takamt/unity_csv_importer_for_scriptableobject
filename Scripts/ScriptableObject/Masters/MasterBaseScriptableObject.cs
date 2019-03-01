using System;
using System.Reflection;
using UnityEngine;

public class MasterBaseScriptableObject : ScriptableObject {

    /// <summary>
    /// Replaces the column values.
    /// カラム値を置き換え
    /// </summary>
    /// <param name="data">Data.</param>
    /// <param name="columnKeys">Column keys.</param>
    /// <param name="columnValues">Column values.</param>
    public static void ReplaceColumnValues (object data, string[] columnKeys, string[] columnValues)
    {
        for (int i = 0; i < columnKeys.Length; i++)
        {
            string columnKey = columnKeys[i];
            string columnValue = columnValues[i];

            Type masterClassType = data.GetType();
            FieldInfo columnKeyField = masterClassType.GetField(columnKey);
            columnKeyField.SetValue(
                data,
                System.Convert.ChangeType(columnValue, columnKeyField.FieldType)
            );
        }
    }	
}

