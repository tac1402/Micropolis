using System.Collections;
using System.Collections.Generic;
using TAC.NetD;
using UnityEngine;

public class Industrial : Building
{
	public IndustrialProcessor Processor = new IndustrialProcessor();

	public override void InitBuilding(int argId, Vector3 argCenter, Net argNet)
	{
		base.InitBuilding(argId, argCenter, argNet);
		Processor.Id = argId;
		InitPorts(ConnectPortType.InOut, Processor, argNet);
		argNet.AddProcessor(Processor);
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
	}

	public class IndustrialProcessor : Processor
	{

		public bool IsElectro = false;
		public override void Consumption()
		{

			IsElectro = Delete((int)MaterialType.Electro, 9);

		}
	}
}
