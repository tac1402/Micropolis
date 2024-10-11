// Author: Sergej Jakovlev <tac1402@gmail.com>
// Copyright (C) 2024 Sergej Jakovlev
// You can use this code for educational purposes only;
// this code or its modifications cannot be used for commercial purposes
// or in proprietary libraries without permission from the author

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;


namespace TAC
{
	public static class Helper
	{
		public static bool In<T>(this T t, params T[] args)
		{
			return args.Contains(t);
		}

		public static bool Range(this int x, int min, int max)
		{
			 return ((x - max) * (x - min) <= 0);
		}
		public static bool Range(this float x, float min, float max)
		{
			return ((x - max) * (x - min) <= 0);
		}

		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
			}
		}

        public static string Repeat(this string text, int n)
        {
            return new StringBuilder(text.Length * n)
              .Insert(0, text, n)
              .ToString();
        }

		public static bool IsPointerOverUI()
		{
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			return results.Count > 0;
		}

    }

	public static class EnumExt
	{
        public static T GetValue<T>(string argValueName) where T : Enum
        {
			T ret = default(T);
            foreach (var item in Enum.GetValues(typeof(T)))
            {
				if (item.ToString() == argValueName)
				{ 
					ret = (T) item;
					break;
				}
            }
			return ret;
        }
    }

}

