using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TilemapConvert))]
[CanEditMultipleObjects]
public class TilemapConvertEdit : Editor
{
	private static TilemapConvert[] CurrentTilemapConvert;

	public static TilemapConvert conv(object argObject)
	{
		return argObject as TilemapConvert;
	}
	public virtual void OnEnable()
	{
		CurrentTilemapConvert = Array.ConvertAll(targets, new Converter<object, TilemapConvert>(conv));
	}

	public override void OnInspectorGUI()
	{

		base.OnInspectorGUI();

		if (GUILayout.Button("Export TilePrefab"))
		{
			for (int i = 0; i < CurrentTilemapConvert.Length; i++)
			{
				CurrentTilemapConvert[i].ExportTilePrefab();
				UnityEditor.EditorUtility.SetDirty(CurrentTilemapConvert[i]);
			}
		}
	}

}

