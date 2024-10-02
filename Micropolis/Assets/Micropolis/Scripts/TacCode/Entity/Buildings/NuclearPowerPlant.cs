using System.Collections;
using System.Collections.Generic;
using TAC.NetD;
using UnityEngine;
using static CoalPowerPlant;

public class NuclearPowerPlant : Building
{
	public NuclearPowerPlantProcessor Processor = new NuclearPowerPlantProcessor();

	public override void InitBuilding(int argId, Net argNet)
	{
		Processor.Id = argId;
		InitPorts(ConnectPortType.Out, Processor, argNet);
		argNet.AddProcessor(Processor);
	}

	public class NuclearPowerPlantProcessor : Processor
	{
		public override void Processing()
		{
			Create((int)MaterialType.Electro, 100);
		}
	}
}
