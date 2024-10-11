using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TAC.NetD;
using UnityEngine;

public class Residential : Building
{
	public ResidentialProcessor Processor = new ResidentialProcessor();

	public override void InitBuilding(int argId, Vector3 argCenter, Net argNet)
	{
		base.InitBuilding(argId, argCenter, argNet);
		Processor.Id = argId;
		InitPorts(ConnectPortType.InOut, Processor, argNet);
		argNet.AddProcessor(Processor);
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
