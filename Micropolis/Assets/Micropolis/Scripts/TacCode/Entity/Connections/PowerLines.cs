using System.Collections;
using System.Collections.Generic;
using TAC.NetD;
using UnityEngine;

public class PowerLines : Building
{
	public PowerLinesConnect Connect = new PowerLinesConnect();

	public override void InitBuilding(int argId, Net argNet)
	{
		Connect.Id = argId;
		InitPorts(ConnectPortType.InOut, Connect, argNet);
		argNet.AddConnect(Connect);
	}

	public class PowerLinesConnect : Connect
	{
	}
}
