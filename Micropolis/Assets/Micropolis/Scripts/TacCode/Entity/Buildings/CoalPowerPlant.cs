using System.Collections;
using System.Collections.Generic;
using TAC.NetD;
using UnityEngine;
using static Residential;

public class CoalPowerPlant : Building
{
	public CoalPowerPlantProcessor Processor = new CoalPowerPlantProcessor();

	public override void InitBuilding(int argId, Net argNet)
	{
		Processor.Id = argId;
		InitPorts(ConnectPortType.Out, Processor, argNet);
		argNet.AddProcessor(Processor);
	}

	public class CoalPowerPlantProcessor : Processor
	{
		public override void Processing()
		{
			Create((int)MaterialType.Electro, 100);
		}
	}
}
