using MicropolisCore;
using MicropolisEngine;
using MicropolisGame;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class City : MonoBehaviour
{
	public Dictionary<int, GameObject> tiles = new Dictionary<int, GameObject>();
	public MicropolisUnityEngine engine;

	public bool IsDraw = false;

	public void PreLoadTiles()
	{
		for (var i = 0; i < (int)MapTileCharacters.TILE_COUNT; i++)
		{
			string p = "Assets/Resources/Exported Prefab/" + string.Format("tiles_{0}", i) + ".prefab";
			GameObject gObj = AssetDatabase.LoadAssetAtPath<GameObject>(p);
			tiles.Add(i, gObj);
		}
	}

	public void Draw()
	{
		if (IsDraw == false)
		{
			for (int x = 0; x < Micropolis.WORLD_W; x++)
			{
				for (int y = 0; y < Micropolis.WORLD_H; y++)
				{
					TileInfo tile = engine.map[new Vector3(x, 0, y)];
					tile.Tile = Instantiate(tiles[tile.Id], new Vector3(x, 0, y), Quaternion.Euler(0, 180, 0), gameObject.transform);
				}
			}
			IsDraw = true;
		}
		else
		{
			for (int x = 0; x < Micropolis.WORLD_W; x++)
			{
				for (int y = 0; y < Micropolis.WORLD_H; y++)
				{
					TileInfo tile = engine.map[new Vector3(x, 0, y)];

					if (tile.IsChanged == true)
					{
						// if the tile has no power and it's the center of the 
						// zone then display the lighting bolt tile instead
						if (tile.IsCenter == true && tile.IsPower == false)
						{
							tile.Id = (ushort)MapTileCharacters.LIGHTNINGBOLT;
						}

						MeshRenderer mrTile = tiles[tile.Id].GetComponentInChildren<MeshRenderer>();
						MeshRenderer mr = engine.map[new Vector3(x, 0, y)].Tile.GetComponentInChildren<MeshRenderer>();
						mr.material = mrTile.sharedMaterial;
						tile.IsChanged = false;
					}
				}
			}

		}
	}

}
