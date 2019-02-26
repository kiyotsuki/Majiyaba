using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Majiyaba
{
	public class AdventureManager : ManagerBase
	{
		public override IEnumerator OnInitialize()
		{
			Ready = true;
			yield break;
		}

		public override IEnumerator OnBeginScene()
		{
			Ready = false;

			var adventureObjects = GameObject.FindObjectsOfType<AdventureObject>();
			foreach(var obj in adventureObjects)
			{
				int key = obj.GetKey();
				if (_adventureObjects.ContainsKey(key))
				{
					Debug.LogAssertion("同じ名前のオブジェクトを登録しようとしました " + obj.GetName() + " : " + _adventureObjects[key].GetName());
					continue;
				}
				_adventureObjects.Add(key, obj);
			}

			if (_playerActor == null)
			{
				var actorManager = GameUtil.GetManager<ActorManager>();
				actorManager.CreateActor("player", setupPlayer);
			}
			else
			{
				setupPlayer(_playerActor);
			}
			yield break;
		}

		private void setupPlayer(GameObject player)
		{
			_playerActor = player;
			_playerActor.SetActive(true);

			var entryPoint = GetAdventureObject("player_entry");
			if (entryPoint != null)
			{
				_playerActor.transform.position = entryPoint.transform.position;
				_playerActor.transform.rotation = entryPoint.transform.rotation;
			}

			Ready = true;
		}


		public override void OnUpdateScene()
		{
			if(_playerActor == null)
			{
				return;
			}
			
			// 左クリックしたときに、
			if (Input.GetMouseButtonDown(0))
			{
				// マウスの位置からRayを発射して、
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100f, 1 << 9))
				{
					var move = _playerActor.GetComponent<ActorMove>();
					move.ReqestMove(hit.point);
				}
			}

			var pos = _playerActor.transform.position;
			pos += Camera.main.transform.forward * -2;
			pos.y += 0.5f;
			Camera.main.transform.position = pos;
		}


		public override void OnTerminateScene()
		{
			_adventureObjects.Clear();
			if (_playerActor != null)
			{
				_playerActor.SetActive(false);
			}
		}
		
		public GameObject GetAdventureObject(string name)
		{
			var key = name.GetHashCode();
			if (_adventureObjects.ContainsKey(key) == false)
			{
				Debug.LogAssertion("未登録のオブジェクトを取得しようとしました " + name);
				return null;
			}
			return _adventureObjects[key].gameObject;
		}

		private GameObject _playerActor = null;

		private Dictionary<int, AdventureObject> _adventureObjects = new Dictionary<int, AdventureObject>();
	}
}