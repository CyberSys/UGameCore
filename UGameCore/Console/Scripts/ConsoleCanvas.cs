﻿using UnityEngine;

namespace UGameCore.Menu
{
	/// <summary>
	/// Used only for detection.
	/// </summary>
	public class ConsoleCanvas : MonoBehaviour
	{
		public	static	ConsoleCanvas	Instance { get ; private set ; }

		void Awake() {
			Instance = this;
		}

	}
}

