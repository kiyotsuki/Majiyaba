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
			_menu = menu;
			
			menu.AddButton("タイトル", () => changeScene("title"));
			menu.AddButton("ステージ01", () => changeScene("stage01"));
		}

		private void changeScene(string sceneName)
		{
			GameUtil.RequestChangeScene(sceneName);
			_menu.Close();
		}

		private DebugMenu _menu = null;
	}
}