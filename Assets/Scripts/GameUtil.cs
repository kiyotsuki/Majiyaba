using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Majiyaba
{
	public class GameUtil
	{
		/// <summary>
		/// マネージャーを取得
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetManager<T>() where T : ManagerBase
		{
			var main = GameMain.Instance;
			if (main == null) return null;

			return main.GetManager<T>();
		}

		/// <summary>
		/// シーン変更リクエスト
		/// </summary>
		/// <param name="sceneName"></param>
		/// <returns></returns>
		public static bool RequestChangeScene(string sceneName)
		{
			var main = GameMain.Instance;
			if (main == null) return false;

			return main.RequestChangeScene(sceneName);
		}
	}
}