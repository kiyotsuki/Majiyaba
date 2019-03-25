using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Majiyaba
{
	public class ScenarioManager : ManagerBase
	{
		public override IEnumerator OnInitialize()
		{
			var req = Resources.LoadAsync<GameObject>("UI/scenario_canvas");
			yield return new WaitUntil(() =>
			{
				return req.isDone;
			});
			var canvasPref = req.asset as GameObject;
			var canvas = GameObject.Instantiate(canvasPref);

			canvas.transform.SetParent(gameObject.transform);
			scenarioCanvas = canvas.GetComponent<ScenarioCanvas>();

			StartScenario("ScenarioTest", null);

			Ready = true;
			yield break;
		}


		public void StartScenario(string name, GameObject holder)
		{
			var s = StartCoroutine(name);

		}

		public IEnumerator Talk(string text)
		{
			var talkWindow = scenarioCanvas.GetTalkWindow();
			talkWindow.RequestText(text);
			return new WaitUntil(() =>
			{
				return talkWindow.IsActive() == false;
			});
		}

		private IEnumerator ScenarioTest()
		{
			yield return Talk("とりあえず\n思い付きで");
			yield return Talk("やってみようと思います");
			yield return Talk("一応\n三行まで\nいけるんですよ");
			yield break;
		}

		private ScenarioCanvas scenarioCanvas = null;
	}
}