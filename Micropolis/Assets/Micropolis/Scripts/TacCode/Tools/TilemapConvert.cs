using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

public class TilemapConvert : MonoBehaviour
{
	public GameObject TilePrefab;


	public void ExportTilePrefab()
	{
		var dirPath = "Assets/Resources/Exported Prefab/";
		if (!Directory.Exists(dirPath))
		{
			Directory.CreateDirectory(dirPath);
		}

		for (int i = 0; i < 960; i++)
		{
			string p = "Assets/Resources/Exported Materials/" + string.Format("m_{0}", i) + ".mat";
			Material m = AssetDatabase.LoadAssetAtPath<Material>(p);

			GameObject gObj = Instantiate(TilePrefab);
			MeshRenderer mr = gObj.GetComponentInChildren<MeshRenderer>();
			mr.material = m;

			PrefabUtility.SaveAsPrefabAsset(gObj, dirPath + string.Format("tiles_{0}", i) + ".prefab");

			DestroyImmediate(gObj);
		}
	}

	public void ExportAsPng(Texture2D tex, string name)
	{

		/*
		//toPng.ExportAsPng(toPng.TextureFromSprite(asset.sprite), string.Format("tiles_{0}", i));

		*/


		byte[] bytes =  tex.EncodeToPNG();
		var dirPath = Application.dataPath + "/Exported Tilemaps/";
		if (!Directory.Exists(dirPath))
		{
			Directory.CreateDirectory(dirPath);
		}
		File.WriteAllBytes(dirPath + name + ".png", bytes);
		//AssetDatabase.Refresh();
	}

	public void ExportAsMaterial(Material m, string name)
	{

		/*
		Material m = new Material(Shader.Find("Standard"));
		string p = "Assets/Resources/Exported Tilemaps/" + string.Format("tiles_{0}", i) + ".png";
		Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath(p, typeof(Texture2D));
		m.mainTexture =  t;
		toPng.ExportAsMaterial(m, string.Format("m_{0}", i));
		*/

		var dirPath = "Assets/Resources/Exported Materials/";
		if (!Directory.Exists(dirPath))
		{
			Directory.CreateDirectory(dirPath);
		}

		AssetDatabase.CreateAsset(m,  dirPath + name + ".mat");
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	public Texture2D TextureFromSprite(Sprite sprite)
	{
		if (sprite.rect.width != sprite.texture.width)
		{
			Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
			Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
														 (int)sprite.textureRect.y,
														 (int)sprite.textureRect.width,
														 (int)sprite.textureRect.height);
			newText.SetPixels(newColors);
			newText.Apply();
			return newText;
		}
		else
			return sprite.texture;
	}

}

#endif
