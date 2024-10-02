using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TAC.NetD;

public class Building : MonoBehaviour
{

	public int IdView;

	public virtual void InitBuilding(int argId, Net argNet)
	{
	}

	public void InitPorts(ConnectPortType argPortType, Processor argProcessor, Net argNet)
	{
		InitPorts(argPortType, argProcessor as Connect, argNet);
	}

	public void InitPorts(ConnectPortType argPortType, Connect argConnect, Net argNet)
	{
		IdView = argConnect.Id;
		argConnect.Ports = GetComponentsInChildren<Port>();
		foreach (Port p in argConnect.Ports)
		{
			p.ConnectType = argPortType;
			p.MainConnect = argConnect;
			p.CurrentNet = argNet;
		}
	}


}
