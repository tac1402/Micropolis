using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAC.NetD
{
	public class PortView : Port
	{

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			Gizmos.color = Color;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 1, 1));
		}
#endif

	}

}