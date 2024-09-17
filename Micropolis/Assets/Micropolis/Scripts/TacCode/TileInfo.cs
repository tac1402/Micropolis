using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo 
{
	public GameObject Tile;

	public int Id;

    public bool IsPower; // ������ ������� �������������
	public bool CanConduct; // ����� ��������� ������������� 
	public bool CanLit; // bit 13, tile can be lit.
	public bool IsBulldozable; // bit 12, tile is bulldozable.
	public bool IsCenter; // ����� ������

	public TileInfo() { }
	public TileInfo(int argId) 
	{ 
		Id = argId;
	}

}
