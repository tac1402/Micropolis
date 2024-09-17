using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo 
{
	public GameObject Tile;

	public int Id;

    public bool IsPower; // здание требует электричество
	public bool CanConduct; // может проводить электричество 
	public bool CanLit; // bit 13, tile can be lit.
	public bool IsBulldozable; // bit 12, tile is bulldozable.
	public bool IsCenter; // центр здания

	public TileInfo() { }
	public TileInfo(int argId) 
	{ 
		Id = argId;
	}

}
