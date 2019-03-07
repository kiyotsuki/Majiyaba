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

		public virtual IEnumerator OnBeginScene(ParamScene.Data scene)
		{
			yield break;
		}

		public virtual void OnUpdateScene(ParamScene.Data scene)
		{

		}

		public virtual void OnEndScene(ParamScene.Data next)
		{

		}

		public bool Ready { get; protected set; }
	}
}