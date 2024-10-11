using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using TAC;
using static UnityEditor.Progress;
using MicropolisEngine;
using MicropolisCore;
using UnityEngine.Tilemaps;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine.UI;

public class ObjectPlacer : MonoBehaviour
{
	[Tooltip("Key to press to place a item in the scene")]
	public KeyCode placeKey = KeyCode.Mouse0;

	public UiManager UiManager;
	public MicropolisUnityEngine engine;

	public TMP_Text DebugInfo;


	private Camera camera;


	private void Start()
	{
		camera = GetComponentInChildren<Camera>();
	}


	void Update()
	{
		if (Helper.IsPointerOverUI() == false && Input.GetKeyDown(placeKey))
		{
			PlaceObject();
		}
	}

	public void PlaceObject()
	{
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);

		(Vector3 terrainPoint, Building building) = GetTerrainPoint(ray);

		if (building != null)
		{
		}
		else
		{ 
			terrainPoint += new Vector3(0.5f, 0, 0.5f);
			terrainPoint = GetDiscrete(terrainPoint, Vector3.one, XYZ.XYZ);
			terrainPoint.z = Micropolis.WORLD_H - terrainPoint.z;
		}

		DebugInfo.text = terrainPoint.x.ToString() + " - " + terrainPoint.z.ToString();

		if (building != null)
		{
			DebugInfo.text = "Id = " + building.IdView.ToString() + "; " + DebugInfo.text;
		}
		SetObject(terrainPoint);
	}


	private void SetObject(Vector3 argPoint)
	{
		TileInfo tile = null;
		if (engine.map.ContainsKey(argPoint))
		{ 
			tile = engine.map[argPoint];
			DebugInfo.text += "; Type = " + tile.Id.ToString();
		}

		switch (UiManager.SelectedZone)
		{
			case Zone.Wood:
				SetWood(tile);
				break;
			case Zone.Bulldozer:
				if (tile != null)
				{
					SetBuldozer(tile);
				}
				break;
			case Zone.Residential:
			case Zone.Commercial:
			case Zone.Industrial:
			case Zone.PoliceStation:
			case Zone.FireStation:
				if (AllowBuild(argPoint, new Vector2Int(1, 1)))
				{
					SetZone(tile, UiManager.SelectedZone);
				}
				break;
			case Zone.CoalPowerPlant:
			case Zone.NuclearPowerPlant:
			case Zone.Seaport:
				if (AllowBuild(argPoint, new Vector2Int(2, 2), true))
				{
					SetZone(tile, UiManager.SelectedZone);
				}
				break;
			case Zone.Airport:
				if (AllowBuild(argPoint + new Vector3(1, 0, 1), new Vector2Int(3, 3), true))
				{
					SetZone(tile, UiManager.SelectedZone);
				}
				break;
		}
	}


	private void SetWood(TileInfo currentTile)
	{
		if (currentTile != null && currentTile.Id.In(0, 1))
		{
			currentTile.Id = (int)MapTileCharacters.WOODS5;
			currentTile.IsChanged = true;
		}
	}
	private void SetBuldozer(TileInfo currentTile)
	{
		if (currentTile.Id >= (int)MapTileCharacters.TREEBASE && currentTile.Id <= (int)MapTileCharacters.WOODS5)
		{
			currentTile.Id = 0;
			currentTile.IsChanged = true;
		}
	}

	private void SetZone(TileInfo currentTile, Zone argZoneType)
	{
		switch (argZoneType)
		{
			case Zone.Residential:
				currentTile.Id = (int)MapTileCharacters.FREEZ;
				break;
			case Zone.Commercial:
				currentTile.Id = (int)MapTileCharacters.COMCLR;
				break;
			case Zone.Industrial:
				currentTile.Id = (int)MapTileCharacters.INDCLR;
				break;
			case Zone.PoliceStation:
				currentTile.Id = (int)MapTileCharacters.POLICESTATION;
				break;
			case Zone.FireStation:
				currentTile.Id = (int)MapTileCharacters.FIRESTATION;
				break;
			case Zone.CoalPowerPlant:
				currentTile.Id = (int)MapTileCharacters.POWERPLANT;
				break;
			case Zone.NuclearPowerPlant:
				currentTile.Id = (int)MapTileCharacters.NUCLEAR;
				break;
			case Zone.Seaport:
				currentTile.Id = (int)MapTileCharacters.PORT;
				break;
			case Zone.Airport:
				currentTile.Id = (int)MapTileCharacters.AIRPORT;
				break;
		}

		currentTile.IsCenter = true;
		currentTile.IsChanged = true;
	}

	private bool AllowBuild(Vector3 argPoint, Vector2Int argSize, bool argNoCenter = false)
	{
		float dstep = 1.0f;
		DiscreteMap_<int> mask = null;
		if (argNoCenter == false)
		{
			mask = new DiscreteMap_<int>(argPoint, argSize, dstep);
		}
		else
		{
			mask = new DiscreteMap_<int>(argPoint, argSize, dstep, true);
		}
		bool retAllow = true;
		for (int i = 0; i < mask.Count; i++)
		{
			Vector3 point = Discrete.Get2D(argPoint, dstep) + mask[i].To3();

			if (engine.map.ContainsKey(point))
			{
				TileInfo tile = engine.map[point];

				if (tile.Id.In(0, 1) ||
					(tile.Id >= (int)MapTileCharacters.TREEBASE && tile.Id <= (int)MapTileCharacters.WOODS5)
					)
				{

				}
				else
				{
					retAllow = false;
					break;
				}
			}
			else
			{
				retAllow = false;
				break;
			}
		}
		return retAllow;
	}


		private Vector3 GetDiscrete(Vector3 argValue, Vector3 argDiscreteStep, XYZ argXYZ)
	{
		Vector3 ret = argValue;

		if (XYZ_.IsX(argXYZ))
		{
			ret.x = ((int)(argValue.x / argDiscreteStep.x)) * argDiscreteStep.x;
		}
		if (XYZ_.IsY(argXYZ))
		{
			ret.y = ((int)(argValue.y / argDiscreteStep.y)) * argDiscreteStep.y;
		}
		if (XYZ_.IsZ(argXYZ))
		{
			ret.z = ((int)(argValue.z / argDiscreteStep.z)) * argDiscreteStep.z;
		}
		return ret;
	}


	private (Vector3, Building) GetTerrainPoint(Ray argRay, MapLayer argMapLayer = MapLayer.Terrain)
	{
		Vector3 ret = new Vector3(0, 0, 0);
		Building building = null;

		RaycastHit hit;
		if (Physics.Raycast(argRay, out hit, 1000, 1 << (int)argMapLayer))
		{
			ret = hit.point;
			building = FindBuilding(hit.collider.gameObject);
		}
		return (ret, building);
	}

	private Building FindBuilding(GameObject argObject)
	{
		Building retBuilding = null;
		retBuilding = argObject.GetComponent<Building>();
		if (retBuilding == null)
		{
			BuildPart part = argObject.GetComponent<BuildPart>();
			if (part != null)
			{
				retBuilding = part.Main;
			}
		}
		return retBuilding;
	}

}
