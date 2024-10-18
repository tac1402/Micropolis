using System.Collections;
using System.Collections.Generic;
using TAC.NetD;
using TMPro;
using UnityEngine;

public class Commercial : Building
{
	public GameObject LevelRoot;
	public List<GameObject> LevelModel = new List<GameObject>();
	public GameObject CurrentLevel;

	public TMP_Text Info;
	public int PopulationDensity;

	public CommercialProcessor Processor = new CommercialProcessor();

	public override void InitBuilding(int argId, Vector3 argCenter, Net argNet)
	{
		base.InitBuilding(argId, argCenter, argNet);
		Processor.Id = argId;
		InitPorts(ConnectPortType.InOut, Processor, argNet);
		argNet.AddProcessor(Processor);
	}

	public void ChangeLevel(int argType)
	{
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

	public class CommercialProcessor : Processor
	{
		public bool IsElectro = false;
		public override void Consumption()
		{
			IsElectro = Delete((int)MaterialType.Electro, 9);
		}
	}

}
