using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TilemapToPng : MonoBehaviour
{
	public void ExportAsPng(Texture2D tex, string name)
	{
		byte[] bytes =  tex.EncodeToPNG();
		var dirPath = Application.dataPath + "/Exported Tilemaps/";
		if (!Directory.Exists(dirPath))
		{
			Directory.CreateDirectory(dirPath);
		}
		File.WriteAllBytes(dirPath + name + ".png", bytes);
		AssetDatabase.Refresh();
	}

	public void ExportAsMaterial(Material m, string name)
	{
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