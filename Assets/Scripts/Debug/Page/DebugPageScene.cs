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
			
			for(int i=0; i<ParamScene.Count; i++)
			{
				var sceneId = (ParamScene.ID)i;
				menu.AddButton(sceneId.ToString(), () => ChangeScene(sceneId));
			}
		}

		private void ChangeScene(ParamScene.ID id)
		{
			GameUtil.RequestChangeScene(id);
			menu.Close();
		}

		private DebugMenu menu = null;
	}
}