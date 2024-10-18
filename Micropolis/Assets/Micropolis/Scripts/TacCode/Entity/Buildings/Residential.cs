using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TAC.NetD;
using TMPro;
using UnityEngine;

public class Residential : Building
{
	public List<GameObject> SingleHouseModel = new List<GameObject>();
	private Dictionary<Vector3, GameObject> SingleHouse = new Dictionary<Vector3, GameObject>();

	public GameObject LevelRoot;
	public List<GameObject> LevelModel = new List<GameObject>();
	public GameObject CurrentLevel;

	public TMP_Text Info;
	public int PopulationDensity;

	public ResidentialProcessor Processor = new ResidentialProcessor();

	public override void InitBuilding(int argId, Vector3 argCenter, Net argNet)
	{
		base.InitBuilding(argId, argCenter, argNet);
		Processor.Id = argId;
		InitPorts(ConnectPortType.InOut, Processor, argNet);
		argNet.AddProcessor(Processor);
	}

	public void AddSingleHouse(int argType, Vector3 argOffset)
	{
		if (SingleHouse.ContainsKey(argOffset))
		{ 
			Destroy(SingleHouse[argOffset]);
			SingleHouse.Remove(argOffset);
		}

		GameObject house = Instantiate(SingleHouseModel[argType]);
		house.transform.SetParent(gameObject.transform, true);
		house.transform.localPosition = argOffset;

		SingleHouse.Add(argOffset, house);
	}

	public int GetSingleHouseCount()
	{ 
		return SingleHouse.Count;
	}

	public void RemoveSingleHouse()
	{
		int index = UnityEngine.Random.Range(0, SingleHouse.Count);
		List<Vector3> list = SingleHouse.Keys.ToList();
		Destroy(SingleHouse[list[index]]);
		SingleHouse.Remove(list[index]);
	}

	public void RemoveAllSingleHouse()
	{
		foreach (Vector3 key in SingleHouse.Keys.ToList())
		{
			Destroy(SingleHouse[key]);
		}
		SingleHouse.Clear();
	}

	public void ChangeLevel(int argType)
	{
		RemoveAllSingleHouse();
		Destroy(CurrentLevel);
		GameObject level = Instantiate(LevelModel[argType]);
		level.transform.SetParent(LevelRoot.transform, false);

		CurrentLevel = level;
	}


	protected override void Debug()
	{
		base.Debug();
		if (Processor.IsElectro == false)
		{
			StartElectroBlinking();
		}
		else
		{ 
			StopElectroBlinking();
		}

		if (Info != null)
		{
			if (PopulationDensity > 0)
			{
				Info.text = PopulationDensity.ToString();
				Info.gameObject.SetActive(true);
			}
			else
			{
				Info.gameObject.SetActive(false);
			}
		}

	}

	public class ResidentialProcessor : Processor
	{
		public bool IsElectro = false;
		public override void Consumption()
		{
			IsElectro = Delete((int)MaterialType.Electro, 9);
		}
	}
}
