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
			foreach(var generator in _generatorList)
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
			_generatorList.Add(generator);
		}

		class ActorGenerator
		{
			public ActorGenerator(ResourceRequest request, System.Action<GameObject> callback = null)
			{
				_request = request;
				_callback = callback;
			}

			public bool Update(ActorManager manager)
			{
				if(_actor != null)
				{
					return true;
				}

				if(_request.isDone)
				{
					var pref = _request.asset as GameObject;
					_actor = GameObject.Instantiate(pref);
					_actor.SetActive(false);

					_actor.AddComponent<ActorMove>();
					
					_actor.transform.SetParent(manager.gameObject.transform);
					if(_callback != null)
					{
						_callback(_actor);
					}
				}
				return false;
			}

			private ResourceRequest _request = null;
			private System.Action<GameObject> _callback = null;
			private GameObject _actor = null;
		}

		private List<ActorGenerator> _generatorList = new List<ActorGenerator>();
	}
}