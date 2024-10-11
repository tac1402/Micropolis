// Author: Sergej Jakovlev <tac1402@gmail.com>
// Copyright (C) 2024 Sergej Jakovlev
// You can use this code for educational purposes only;
// this code or its modifications cannot be used for commercial purposes
// or in proprietary libraries without permission from the author

namespace TAC
{
	public static class XYZ_
	{
		public static bool IsX(XYZ argXYZ)
		{
			bool ret = false;
			switch (argXYZ)
			{
				case XYZ.X:
				case XYZ.XY:
				case XYZ.XZ:
				case XYZ.XYZ:
					ret = true;
					break;
			}
			return ret;
		}
		public static bool IsY(XYZ argXYZ)
		{
			bool ret = false;
			switch (argXYZ)
			{
				case XYZ.Y:
				case XYZ.XY:
				case XYZ.YZ:
				case XYZ.XYZ:
					ret = true;
					break;
			}
			return ret;
		}
		public static bool IsZ(XYZ argXYZ)
		{
			bool ret = false;
			switch (argXYZ)
			{
				case XYZ.Z:
				case XYZ.YZ:
				case XYZ.XZ:
				case XYZ.XYZ:
					ret = true;
					break;
			}
			return ret;
		}

		public static XYZ GetXYZ(IgnoreNormal argIgnoreNormal)
		{
			XYZ xyz = XYZ.None;
			if (argIgnoreNormal.X == false) { xyz = XYZ.X; }
			else if (argIgnoreNormal.Z == false) { xyz = XYZ.Z; }
			return xyz;
		}

	}
	public enum XYZ
	{
		None,
		X,
		Y,
		Z,
		XZ,
		YZ,
		XY,
		XYZ
	}
}
