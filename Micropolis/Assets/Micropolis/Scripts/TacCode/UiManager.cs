using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
	public Zone SelectedZone;



	public void SelectBuilding(int argIndex)
	{
		SelectedZone = (Zone)argIndex;
	}


}

