using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Majiyaba
{
	public class EffectManager : ManagerBase
	{
		public override IEnumerator OnInitialize()
		{
			Ready = false;
			var requestList = new List<ResourceRequest>();
			for (int i = 0; i < ParamEffect.Count; i++)
			{
				var data = ParamEffect.Get(i);
				var req = Resources.LoadAsync("Effects/" + data.Path);
				requestList.Add(req);
			}

			yield return new WaitUntil(() =>
			{
				bool allDone = true;
				for(int i = 0; i < requestList.Count; i++)
				{
					var req = requestList[i];
					if(req == null)
					{
						continue;
					}
					if(req.isDone)
					{
						effectSources.Add((ParamEffect.ID)i, req.asset as GameObject);
						requestList[i] = null;
						continue;
					}
					allDone = false;
				}
				return allDone;
			});

			Ready = true;
			yield break;
		}

		public GameObject GenerateEffect(ParamEffect.ID id)
		{
			if(effectSources.ContainsKey(id) == false)
			{
				return null;
			}
			var source = effectSources[id];
			return Instantiate(source);
		}

		private Dictionary<ParamEffect.ID, GameObject> effectSources = new Dictionary<ParamEffect.ID, GameObject>();
	}
}