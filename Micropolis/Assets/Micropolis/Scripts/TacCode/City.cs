using MicropolisCore;
using MicropolisEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TAC.NetD;
using UnityEngine;

using NetD = TAC.NetD;

public class City : MonoBehaviour
{
	public List<int> TileModelsId = new List<int>();
	public List<GameObject> TileModels = new List<GameObject>();

	private Dictionary<int, GameObject> tiles = new Dictionary<int, GameObject>();
	public MicropolisUnityEngine engine;

	public bool IsDraw = false;


	public ElectroNet electroNet;

	public void Init()
	{
		electroNet = GetComponent<ElectroNet>();
		electroNet.Run();
	}

	public void PreLoadTiles()
	{
		for (var i = 0; i < (int)MapTileCharacters.TILE_COUNT; i++)
		{
			//string p = "Assets/Resources/Exported Prefab/" + string.Format("tiles_{0}", i) + ".prefab";
			//GameObject gObj = AssetDatabase.LoadAssetAtPath<GameObject>(p);

			GameObject gObj = Resources.Load<GameObject>("Exported Prefab/" + string.Format("tiles_{0}", i));

			if (gObj != null)
			{
				tiles.Add(i, gObj);
			}
		}
		for (int i = 0; i < TileModelsId.Count; i++)
		{
			tiles.Add(TileModelsId[i], TileModels[i]);
		}
	}

	public GameObject GetTile(int argId)
	{ 
		GameObject ret = null;
		if (tiles.ContainsKey(argId))
		{ 
			ret = tiles[argId];
		}
		return ret;
	}

	public void InitTile()
	{
		if (IsDraw == false)
		{
			for (int x = 0; x < Micropolis.WORLD_W; x++)
			{
				for (int y = 0; y < Micropolis.WORLD_H; y++)
				{
					TileInfo tile = engine.map[new Vector3(x, 0, y)];

					if (tile.Id >= 240 && tile.Id <= 826)
					{
						if (tile.IsCenter)
						{
							GameObject model = GetTile(tile.Id);
							if (model != null)
							{
								tile.Tile = Instantiate(model, new Vector3(x - 1, 0, y - 1), Quaternion.Euler(0, -180, 0));
								tile.Tile.transform.SetParent(gameObject.transform, false);
							}
							else
							{
								tile.Tile = Instantiate(tiles[tile.Id], new Vector3(x, 0, y), Quaternion.Euler(0, -180, 0));
								tile.Tile.transform.SetParent(gameObject.transform, false);
							}
						}
					}
					else if (tile.Id >= (ushort)MapTileCharacters.SMOKEBEGIN && tile.Id < (ushort)MapTileCharacters.TILE_COUNT)
					{
						tile.Tile = Instantiate(tiles[0], new Vector3(x, 0, y), Quaternion.Euler(0, -180, 0));
						tile.Tile.transform.SetParent(gameObject.transform, false);
					}
					else
					{
						tile.Tile = Instantiate(tiles[tile.Id], new Vector3(x, 0, y), Quaternion.Euler(0, -180, 0));
						tile.Tile.transform.SetParent(gameObject.transform, false);
					}
				}
			}
			IsDraw = true;
		}
	}


	public void Draw()
	{
		{
			for (int x = 0; x < Micropolis.WORLD_W; x++)
			{
				for (int y = 0; y < Micropolis.WORLD_H; y++)
				{
					TileInfo tile = engine.map[new Vector3(x, 0, y)];

					if (tile.IsChanged == true)
					{
						if (tile.Id >= 240 && tile.Id <= 826)
						{
							if (tile.IsCenter)
							{

								//Zone zone = Zone.Residential;
								/*
								Destroy(tile.Tile);

								tile.Tile = Instantiate(tiles[tile.Id], new Vector3(x - 1, 0, y - 1), Quaternion.Euler(0, -180, 0));
								tile.Tile.transform.SetParent(gameObject.transform, false);
								*/
								//InitZone(zone, tile);

								tile.IsChanged = false;

								tile.IsInit = true;

							}
						}
						else if (tile.Id >= (ushort)MapTileCharacters.SMOKEBEGIN && tile.Id < (ushort)MapTileCharacters.TILE_COUNT)
						{
							MeshRenderer mrTile = tiles[0].GetComponentInChildren<MeshRenderer>();
							if (engine.map[new Vector3(x, 0, y)].Tile != null)
							{
								MeshRenderer mr = engine.map[new Vector3(x, 0, y)].Tile.GetComponentInChildren<MeshRenderer>();
								mr.material = mrTile.sharedMaterial;
							}
							tile.IsChanged = false;
						}
						else
						{
							MeshRenderer mrTile = tiles[tile.Id].GetComponentInChildren<MeshRenderer>();
							if (engine.map[new Vector3(x, 0, y)].Tile != null)
							{
								MeshRenderer mr = engine.map[new Vector3(x, 0, y)].Tile.GetComponentInChildren<MeshRenderer>();
								mr.material = mrTile.sharedMaterial;
							}
							tile.IsChanged = false;
						}
					}
				}
			}

		}
	}


	public int ObjectIdCounter = 0;


	public void InitZone(Zone zoneType, TileInfo tile)
	{
		if (tile.IsInit == false)
		{
			ObjectIdCounter++;

			NetD.Material produces;
			NetD.Material consumes;
			switch (zoneType)
			{

				case Zone.CoalPowerPlant:
					produces = new NetD.Material((int)MaterialType.Electro, 0, Micropolis.COAL_POWER_STRENGTH);
					produces.Color = Color.red;

					CoalPowerPlant coalPowerPlant = tile.Tile.GetComponent<CoalPowerPlant>();
					coalPowerPlant.InitBuilding(ObjectIdCounter, electroNet);
					coalPowerPlant.Processor.AddOutCommingMaterial(produces);
					break;
				case Zone.NuclearPowerPlant:
					produces = new NetD.Material((int)MaterialType.Electro, 0, Micropolis.NUCLEAR_POWER_STRENGTH);
					produces.Color = Color.red;

					NuclearPowerPlant nuclearPowerPlant = tile.Tile.GetComponent<NuclearPowerPlant>();
					nuclearPowerPlant.InitBuilding(ObjectIdCounter, electroNet);
					nuclearPowerPlant.Processor.AddOutCommingMaterial(produces);
					break;
				case Zone.Residential:
					consumes = new NetD.Material((int)MaterialType.Electro, 0, 9);
					consumes.Color = Color.grey;

					Residential residential = tile.Tile.GetComponent<Residential>();
					residential.InitBuilding(ObjectIdCounter, electroNet);
					residential.Processor.AddInCommingMaterial(consumes);

					break;
				case Zone.Commercial:
					consumes = new NetD.Material((int)MaterialType.Electro, 0, 9);
					consumes.Color = Color.grey;

					Commercial commercial = tile.Tile.GetComponent<Commercial>();
					commercial.InitBuilding(ObjectIdCounter, electroNet);
					commercial.Processor.AddInCommingMaterial(consumes);

					break;
				case Zone.Industrial:
					consumes = new NetD.Material((int)MaterialType.Electro, 0, 9);
					consumes.Color = Color.grey;

					Industrial industrial = tile.Tile.GetComponent<Industrial>();
					industrial.InitBuilding(ObjectIdCounter, electroNet);
					industrial.Processor.AddInCommingMaterial(consumes);

					break;
				case Zone.PowerLines:
					PowerLines powerLines = tile.Tile.GetComponent<PowerLines>();
					powerLines.InitBuilding(ObjectIdCounter, electroNet);
					powerLines.Connect.CurrentPack.MaxCount = 100;

					foreach (Port port in powerLines.Connect.Ports)
					{
						port.Color = Color.red;
					}

					break;
			}

			tile.IsInit = true;
		}
	}
}



public enum Zone
{
	CoalPowerPlant,
	NuclearPowerPlant,

	Residential,
	Commercial,
	Industrial,

	PowerLines,

}
