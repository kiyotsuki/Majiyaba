using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Majiyaba
{
	public class ActorManager : ManagerBase
	{
		public override IEnumerator OnInitialize()
		{
			Ready = true;
			yield break;
		}


		public void Update()
		{
			bool allGererated = true;
			foreach(var generator in generatorList)
			{
				var generated = generator.Update(this);

				if(generated == false)
				{
					allGererated = false;
				}
			}

			Ready = allGererated;
		}

		public void CreateActor(string name, System.Action<GameObject> callback = null)
		{
			var req = Resources.LoadAsync<GameObject>("Actor/" + name);
			var generator = new ActorGenerator(req, callback);
			generatorList.Add(generator);
		}

		public void DeleteActor(GameObject actor)
		{
			for (int i = 0; i < generatorList.Count; i++)
			{
				var generator = generatorList[i];
				if(generator.GetActor() == actor)
				{
					generatorList.RemoveAt(i);
					break;
				}
			}
			GameObject.Destroy(actor);
		}

		class ActorGenerator
		{
			public ActorGenerator(ResourceRequest request, System.Action<GameObject> callback = null)
			{
				this.request = request;
				this.callback = callback;
			}

			public bool Update(ActorManager manager)
			{
				if(actor != null)
				{
					return true;
				}

				if(request.isDone)
				{
					var pref = request.asset as GameObject;
					actor = GameObject.Instantiate(pref);
					actor.SetActive(false);

					actor.AddComponent<ActorMove>();
					
					actor.transform.SetParent(manager.gameObject.transform);
					if(callback != null)
					{
						callback(actor);
					}
				}
				return false;
			}

			public GameObject GetActor()
			{
				return actor;
			}

			private ResourceRequest request = null;
			private System.Action<GameObject> callback = null;
			private GameObject actor = null;
		}

		private List<ActorGenerator> generatorList = new List<ActorGenerator>();
	}
}