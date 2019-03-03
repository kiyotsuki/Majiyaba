using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Majiyaba
{
	public class DebugManager : ManagerBase
	{
		public override IEnumerator OnInitialize()
		{
			// Canvasをロード
			var req = Resources.LoadAsync<GameObject>("UI/debug_canvas");
			yield return new WaitUntil(() =>
			{
				return req.isDone;
			});
			var canvasPref = req.asset as GameObject;

			// インスタンス化と登録
			debugCanvas = GameObject.Instantiate(canvasPref);
			var canvasTrans = debugCanvas.transform;
			canvasTrans.SetParent(gameObject.transform);

			// デバッグメニュー取得
			debugMenu = debugCanvas.GetComponent<DebugMenu>();
			
			Ready = true;
			yield break;
		}

		/// <summary>
		/// デバッグメニュー取得
		/// </summary>
		/// <returns></returns>
		public DebugMenu GetMenu()
		{
			return debugMenu;
		}
		
		private GameObject debugCanvas = null;
		private DebugMenu debugMenu = null;
	}
}