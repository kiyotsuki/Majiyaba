using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Majiyaba
{
	public class DebugPageTop : DebugMenu.PageBase
	{
		/// <summary>
		/// 各ページのセットアップ処理
		/// ここでボタンを追加する
		/// </summary>
		public override void Setup(DebugMenu menu)
		{
			menu.AddButton("シーン", () => menu.ToPage(DebugMenu.Page.Scene));
			menu.AddButton("クエスト", () => menu.ToPage(DebugMenu.Page.Quest));
			menu.AddButton("シナリオ", () => menu.ToPage(DebugMenu.Page.Scenario));
		}
	}
}