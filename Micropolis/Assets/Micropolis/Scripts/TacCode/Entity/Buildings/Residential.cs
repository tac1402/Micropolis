using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TAC.NetD;
using UnityEngine;

public class Residential : Building
{
	public List<GameObject> SingleHouseModel = new List<GameObject>();
	private Dictionary<Vector3, GameObject> SingleHouse = new Dictionary<Vector3, GameObject>();

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

	public class ResidentialProcessor : Processor
	{
		public bool IsElectro = false;
		public override void Consumption()
		{
			IsElectro = Delete((int)MaterialType.Electro, 9);
		}
	}
}
