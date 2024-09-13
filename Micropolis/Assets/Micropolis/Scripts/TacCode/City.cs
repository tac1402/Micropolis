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

	private Dictionary<Vector3, GameObject> map = new Dictionary<Vector3, GameObject>();

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
					var tile = engine.map[x, y];
					var tileId = tile & (ushort)MapTileBits.LOMASK;

					// if the tile has no power and it's the center of the 
					// zone then display the lighting bolt tile instead
					if ((tile & (ushort)MapTileBits.ZONEBIT) == (ushort)MapTileBits.ZONEBIT &&
						(tile & (ushort)MapTileBits.PWRBIT) == (ushort)MapTileBits.PWRBIT)
					{
						tileId = (ushort)MapTileCharacters.LIGHTNINGBOLT;
					}

					// map is defined from top to bottom but Tilemap works from bottom to top so invert 
					// the y value here and offset by 1 so we start at 0, -1 instead of 0, 0 in the grid
					var offset = y * -1 - 1;
					//_mapLayer.SetTile(new Vector3Int(x, offset, 0), _tileEngine.GetTile(tileId));

					GameObject t = Instantiate(tiles[tileId], new Vector3(x, 0, y), Quaternion.Euler(0, 180, 0), gameObject.transform);

					map.Add(new Vector3(x, 0, y), t);
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
					var tile = engine.map[x, y];
					var tileId = tile & (ushort)MapTileBits.LOMASK;

					// if the tile has no power and it's the center of the 
					// zone then display the lighting bolt tile instead
					if ((tile & (ushort)MapTileBits.ZONEBIT) == (ushort)MapTileBits.ZONEBIT &&
						(tile & (ushort)MapTileBits.PWRBIT) == (ushort)MapTileBits.PWRBIT)
					{
						tileId = (ushort)MapTileCharacters.LIGHTNINGBOLT;
					}

					// map is defined from top to bottom but Tilemap works from bottom to top so invert 
					// the y value here and offset by 1 so we start at 0, -1 instead of 0, 0 in the grid
					var offset = y * -1 - 1;
					//_mapLayer.SetTile(new Vector3Int(x, offset, 0), _tileEngine.GetTile(tileId));

					MeshRenderer mrTile = tiles[tileId].GetComponentInChildren<MeshRenderer>();
					MeshRenderer mr = map[new Vector3(x, 0, y)].GetComponentInChildren<MeshRenderer>();
					mr.material = mrTile.sharedMaterial;
				}
			}

		}
	}

}
