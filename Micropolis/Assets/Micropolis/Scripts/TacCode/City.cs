using MicropolisCore;
using MicropolisEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TAC;
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

								//GameObject go = Instantiate(GetTile(952), new Vector3(x - 1, 0, y - 1), Quaternion.Euler(0, -180, 0));
								//go.transform.SetParent(gameObject.transform, false);
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
					Vector3 position = new Vector3(x, 0, y);
					if (engine.map.ContainsKey(position))
					{
						TileInfo tile = engine.map[position];
						if (tile.IsChanged == true)
						{
							if (tile.Id >= 240 && tile.Id <= 826)
							{
								if (tile.IsCenter)
								{
									Zone zone = Zone.Unknown;
									Vector2Int size = new Vector2Int(1, 1);
									bool noCenter = false;
									Vector3 centerOffset = Vector3.zero;
									if (tile.Id >= (ushort)MapTileCharacters.RESBASE && tile.Id < (ushort)MapTileCharacters.HOSPITALBASE)
									{
										zone = Zone.Residential;
									}
									else if (tile.Id >= (ushort)MapTileCharacters.COMBASE && tile.Id < (ushort)MapTileCharacters.INDBASE)
									{
										zone = Zone.Commercial;
									}
									else if (tile.Id >= (ushort)MapTileCharacters.INDBASE && tile.Id < (ushort)MapTileCharacters.PORTBASE)
									{
										zone = Zone.Industrial;
									}
									else if (tile.Id == (ushort)MapTileCharacters.POLICESTATION)
									{
										zone = Zone.PoliceStation;
									}
									else if (tile.Id == (ushort)MapTileCharacters.FIRESTATION)
									{
										zone = Zone.FireStation;
									}
									else if (tile.Id == (ushort)MapTileCharacters.POWERPLANT)
									{
										zone = Zone.CoalPowerPlant;
										size = new Vector2Int(2, 2);
										noCenter = true;
									}
									else if (tile.Id == (ushort)MapTileCharacters.NUCLEAR)
									{
										zone = Zone.NuclearPowerPlant;
										size = new Vector2Int(2, 2);
										noCenter = true;
									}
									else if (tile.Id == (ushort)MapTileCharacters.PORT)
									{
										zone = Zone.Seaport;
										size = new Vector2Int(2, 2);
										noCenter = true;
									}
									else if (tile.Id == (ushort)MapTileCharacters.AIRPORT)
									{
										zone = Zone.Airport;
										size = new Vector2Int(3, 3);
										noCenter = true;
										centerOffset = new Vector3(1, 0, 1);
									}

									DeleteTile(position + centerOffset, size, noCenter);

									tile.Tile = Instantiate(tiles[tile.Id], new Vector3(x - 1, 0, y - 1), Quaternion.Euler(0, -180, 0));
									tile.Tile.transform.SetParent(gameObject.transform, false);

									InitZone(zone, tile, position);

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
	}


	public void DeleteTile(Vector3 argPosition, Vector2Int argSize, bool argNoCenter = false)
	{
		float dstep = 1.0f;
		DiscreteMap_<int> mask = null;
		if (argNoCenter == false)
		{
			mask = new DiscreteMap_<int>(argPosition, argSize, dstep);
		}
		else
		{
			mask = new DiscreteMap_<int>(argPosition, argSize, dstep, true);
		}

		for (int i = 0; i < mask.Count; i++)
		{
			Vector3 point = Discrete.Get2D(argPosition, dstep) + mask[i].To3();
			if (engine.map.ContainsKey(point))
			{
				TileInfo tile = engine.map[point];
				Destroy(tile.Tile);
				engine.map.Remove(point);
			}
		}
	}

	public int ObjectIdCounter = 0;


	public void InitZone(Zone zoneType, TileInfo tile, Vector3 argCenter)
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
					coalPowerPlant.InitBuilding(ObjectIdCounter, argCenter, electroNet);
					coalPowerPlant.Processor.AddOutCommingMaterial(produces);
					break;
				case Zone.NuclearPowerPlant:
					produces = new NetD.Material((int)MaterialType.Electro, 0, Micropolis.NUCLEAR_POWER_STRENGTH);
					produces.Color = Color.red;

					NuclearPowerPlant nuclearPowerPlant = tile.Tile.GetComponent<NuclearPowerPlant>();
					nuclearPowerPlant.InitBuilding(ObjectIdCounter, argCenter, electroNet);
					nuclearPowerPlant.Processor.AddOutCommingMaterial(produces);
					break;
				case Zone.Residential:
					consumes = new NetD.Material((int)MaterialType.Electro, 0, 9);
					consumes.Color = Color.grey;

					Residential residential = tile.Tile.GetComponent<Residential>();
					residential.InitBuilding(ObjectIdCounter, argCenter, electroNet);
					residential.Processor.AddInCommingMaterial(consumes);

					break;
				case Zone.Commercial:
					consumes = new NetD.Material((int)MaterialType.Electro, 0, 9);
					consumes.Color = Color.grey;

					Commercial commercial = tile.Tile.GetComponent<Commercial>();
					commercial.InitBuilding(ObjectIdCounter, argCenter, electroNet);
					commercial.Processor.AddInCommingMaterial(consumes);

					break;
				case Zone.Industrial:
					consumes = new NetD.Material((int)MaterialType.Electro, 0, 9);
					consumes.Color = Color.grey;

					Industrial industrial = tile.Tile.GetComponent<Industrial>();
					industrial.InitBuilding(ObjectIdCounter, argCenter, electroNet);
					industrial.Processor.AddInCommingMaterial(consumes);

					break;
				case Zone.PowerLines:
					PowerLines powerLines = tile.Tile.GetComponent<PowerLines>();
					powerLines.InitBuilding(ObjectIdCounter, argCenter, electroNet);
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
	Unknown = 0,
	Bulldozer = 1,
	Road = 2,
	Railroad = 3,
	PowerLines = 4,
	Wood = 5,

	Residential = 10,
	Commercial = 11,
	Industrial =12,


	PoliceStation = 31,
	FireStation = 32,
	Church = 33,
	Hospital = 34,
	CoalPowerPlant = 35,
	NuclearPowerPlant = 36,
	Seaport = 37,
	Airport = 38,
	Stadium = 39,
}
