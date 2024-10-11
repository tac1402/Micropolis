using System.Collections;
using System.Collections.Generic;
using TAC.NetD;
using UnityEngine;

public class Industrial : Building
{
	public IndustrialProcessor Processor = new IndustrialProcessor();

	public GameObject IsPower;

	public override void InitBuilding(int argId, Vector3 argCenter, Net argNet)
	{
		base.InitBuilding(argId, argCenter, argNet);
		Processor.Id = argId;
		Processor.IsPower = IsPower;
		InitPorts(ConnectPortType.InOut, Processor, argNet);
		argNet.AddProcessor(Processor);
	}

	public class IndustrialProcessor : Processor
	{
		public GameObject IsPower;

		public bool IsElectro = false;
		public override void Consumption()
		{
			if (Id == 466)
			{
				int a = 1;
			}

			IsElectro = Delete((int)MaterialType.Electro, 9);

			if (IsPower != null)
			{
				if (IsElectro == true)
				{
					IsPower.SetActive(true);
				}
				else
				{
					IsPower.SetActive(false);
				}
			}
		}
	}
}
