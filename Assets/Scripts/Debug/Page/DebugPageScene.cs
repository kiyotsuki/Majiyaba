using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Majiyaba
{
	public class DebugPageScene : DebugMenu.PageBase
	{
		/// <summary>
		/// 各ページのセットアップ処理
		/// ここでボタンを追加する
		/// </summary>
		public override void Setup(DebugMenu menu)
		{
			this.menu = menu;
			
			menu.AddButton("タイトル", () => ChangeScene("title"));
			menu.AddButton("ステージ01", () => ChangeScene("stage01"));
		}

		private void ChangeScene(string sceneName)
		{
			GameUtil.RequestChangeScene(sceneName);
			menu.Close();
		}

		private DebugMenu menu = null;
	}
}