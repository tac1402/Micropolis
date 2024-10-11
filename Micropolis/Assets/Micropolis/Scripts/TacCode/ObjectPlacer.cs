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

		terrainPoint += new Vector3(0.5f, 0, 0.5f);

		terrainPoint = GetDiscrete(terrainPoint, Vector3.one, XYZ.XYZ);

		terrainPoint.z = Micropolis.WORLD_H - terrainPoint.z;

		DebugInfo.text = terrainPoint.x.ToString() + " - " + terrainPoint.z.ToString();

		if (building != null)
		{
			DebugInfo.text = "Id = " + building.IdView.ToString() + "; " + DebugInfo.text;
		}
		SetObject(terrainPoint);
	}


	private void SetObject(Vector3 argPoint)
	{
		TileInfo tile = engine.map[new Vector3(argPoint.x, 0, argPoint.z)];

		DebugInfo.text += "; Type = " + tile.Id.ToString();

		switch (UiManager.SelectedZone)
		{
			case Zone.Wood:
				SetWood(tile);
				break;
			case Zone.Bulldozer:
				SetBuldozer(tile);
				break;
		}
	}


	private void SetWood(TileInfo currentTile)
	{
		if (currentTile.Id.In(0, 1))
		{
			currentTile.Id = 43;
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
