﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Majiyaba
{
	public class AdventureObject : MonoBehaviour
	{
		public string GetName()
		{
			return gameObject.name;
		}

		public int GetKey()
		{
			return gameObject.name.GetHashCode();
		}
	}
}