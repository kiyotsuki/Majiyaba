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

			var objects = GameObject.FindObjectsOfType<AdventureObject>();
			foreach(var obj in objects)
			{
				int key = obj.GetKey();
				if (adventureObjects.ContainsKey(key))
				{
					Debug.LogAssertion("同じ名前のオブジェクトを登録しようとしました " + obj.GetName() + " : " + adventureObjects[key].GetName());
					continue;
				}
				adventureObjects.Add(key, obj);
			}

			if (playerActor == null)
			{
				var actorManager = GameUtil.GetManager<ActorManager>();
				actorManager.CreateActor("player", setupPlayer);
			}
			else
			{
				setupPlayer(playerActor);
			}
			yield break;
		}

		private void setupPlayer(GameObject player)
		{
			playerActor = player;
			playerActor.SetActive(true);

			var entryPoint = GetAdventureObject("playerentry");
			if (entryPoint != null)
			{
				playerActor.transform.position = entryPoint.transform.position;
				playerActor.transform.rotation = entryPoint.transform.rotation;
			}

			Ready = true;
		}


		public override void OnUpdateScene()
		{
			if(playerActor == null)
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
					var move = playerActor.GetComponent<ActorMove>();
					move.ReqestMove(hit.point);
				}
			}

			var pos = playerActor.transform.position;
			pos += Camera.main.transform.forward * -2;
			pos.y += 0.5f;
			Camera.main.transform.position = pos;
		}


		public override void OnTerminateScene()
		{
			adventureObjects.Clear();
			if (playerActor != null)
			{
				playerActor.SetActive(false);
			}
		}
		
		public GameObject GetAdventureObject(string name)
		{
			var key = name.GetHashCode();
			if (adventureObjects.ContainsKey(key) == false)
			{
				Debug.LogAssertion("未登録のオブジェクトを取得しようとしました " + name);
				return null;
			}
			return adventureObjects[key].gameObject;
		}

		private GameObject playerActor = null;

		private Dictionary<int, AdventureObject> adventureObjects = new Dictionary<int, AdventureObject>();
	}
}