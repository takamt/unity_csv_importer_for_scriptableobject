using UnityEngine;
using UnityEditor;
using System.IO;

public class CardMasterCreator : ScriptableObjectCreatorBase {

    const string MasterName = "CardMaster";

    [MenuItem(MenuItemPathBase + MasterName)]
	public static void CreateAsset()
	{
        CreateAsset<CardMaster>(MasterName);
	}
}

