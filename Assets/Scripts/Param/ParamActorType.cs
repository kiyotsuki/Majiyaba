using UnityEngine;

namespace Majiyaba
{
	/// <summary>
	/// ゲーム内パラメータ
	/// ParamGeneratorによって自動出力
	/// </summary>
	public class ParamActorType
	{
		public enum ID
		{
			Invalid = -1,
			Object = 0,
			Human = 1,
			Animal = 2,
		}
	}
}
