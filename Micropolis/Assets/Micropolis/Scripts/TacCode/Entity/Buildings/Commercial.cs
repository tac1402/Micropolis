using System.Collections;
using System.Collections.Generic;
using TAC.NetD;
using UnityEngine;

public class Commercial : Building
{
	public CommercialProcessor Processor = new CommercialProcessor();

	public override void InitBuilding(int argId, Vector3 argCenter, Net argNet)
	{
		base.InitBuilding(argId, argCenter, argNet);
		Processor.Id = argId;
		InitPorts(ConnectPortType.InOut, Processor, argNet);
		argNet.AddProcessor(Processor);
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
