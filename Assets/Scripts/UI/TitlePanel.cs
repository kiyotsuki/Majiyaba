using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Majiyaba
{
	public class TitlePanel : MonoBehaviour
	{
		public void OnStartButton()
		{
			GameMain.Instance.RequestChangeScene(ParamScene.ID.Test);
		}
	}
}