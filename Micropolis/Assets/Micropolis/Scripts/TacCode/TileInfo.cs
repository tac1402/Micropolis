using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo 
{
	public GameObject Tile;

	private int id;
	public int Id
	{
		get { return id; }
		set
		{
			if (id != value)
			{
				IsChanged = true;
			}
			id = value;
		}
	}


    public bool IsPower; // здание требует электричество
	public bool CanConduct; // может проводить электричество 
	public bool CanLit; // bit 13, tile can be lit.
	public bool IsBulldozable; // bit 12, tile is bulldozable.
	public bool IsCenter; // центр здания

	public bool IsChanged;

	public TileInfo() { }
	public TileInfo(int argId) 
	{ 
		Id = argId;
	}

}
