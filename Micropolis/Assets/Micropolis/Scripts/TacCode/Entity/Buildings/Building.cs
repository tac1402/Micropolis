using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TAC.NetD;
using static Industrial;
using System.Diagnostics;

public class Building : MonoBehaviour
{

	public int IdView;
	public Vector3 Center;

	public int DebugMaterialId;
	public float CountIn;
	public float CountOut;
	public float CountStore;
	public float CountInConnect;


	private Processor processor;
	private Connect connect;

	public virtual void InitBuilding(int argId, Vector3 argCenter, Net argNet)
	{
		IdView = argId;
		Center = argCenter;
		StartCoroutine(DebugTick());
	}

	public void InitPorts(ConnectPortType argPortType, Processor argProcessor, Net argNet)
	{
		processor = argProcessor;
		InitPorts(argPortType, argProcessor as Connect, argNet);
		argProcessor.InitLine(argProcessor.Ports);
	}

	public void InitPorts(ConnectPortType argPortType, Connect argConnect, Net argNet)
	{
		connect = argConnect;
		argConnect.Ports = GetComponentsInChildren<Port>();
		int index = 0;
		foreach (Port p in argConnect.Ports)
		{
			index++;
			p.PortIndex = index;
			p.ConnectType = argPortType;
			p.MainConnect = argConnect;
			p.CurrentNet = argNet;
		}
	}


	protected IEnumerator DebugTick()
	{
		while (true)
		{
			Debug();
			yield return new WaitForSeconds(1.0f);
		}
	}

	protected virtual void Debug() 
	{
		if (DebugMaterialId > 0)
		{
			if (processor != null)
			{
				CountIn = processor.GetIn(DebugMaterialId);
				CountOut = processor.GetOut(DebugMaterialId);
				CountStore = processor.GetStore(DebugMaterialId);
			}
			if (connect != null)
			{
				CountInConnect = connect.GetInConnect(DebugMaterialId);
			}
		}
	}
}
