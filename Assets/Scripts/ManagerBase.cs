using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Majiyaba
{
	public class ManagerBase : MonoBehaviour
	{
		public virtual IEnumerator OnInitialize()
		{
			yield break;
		}

		public virtual IEnumerator OnBeginScene()
		{
			yield break;
		}

		public virtual void OnUpdateScene()
		{

		}

		public virtual void OnTerminateScene()
		{
			
		}

		public bool Ready { get; protected set; }
	}
}