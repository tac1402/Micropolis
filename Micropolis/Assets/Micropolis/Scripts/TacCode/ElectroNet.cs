using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TAC.NetD;


public class ElectroNet : Net
{

	public void Start()
	{
	/*
		Material produces = new Material((int)Materials.Electro, 0, 100);
		Material consumes = new Material((int)Materials.Electro, 0, 1);

		CoalPowerPlant powerPlant = new CoalPowerPlant(1);
		powerPlant.AddOutCommingMaterial(produces);

		Processor resedential = new Processor(2);
		resedential.AddInCommingMaterial(consumes);

		AddProcessor(powerPlant);
		AddProcessor(resedential);

		AddConnect(100, 1, 101);
		AddConnect(101, 100, 102);
		AddConnect(102, 101, 2);

		Run();
		*/
	}




}

public enum MaterialType
{
	Electro = 1,
}

