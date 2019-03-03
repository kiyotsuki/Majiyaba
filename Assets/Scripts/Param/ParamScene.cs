using UnityEngine;

namespace Majiyaba
{
	/// <summary>
	/// ゲーム内パラメータ
	/// ParamGeneratorによって自動出力
	/// </summary>
	public class ParamScene
	{
		public enum ID
		{
			Invalid = -1,
			Title = 0,
			Stage01 = 1,
		}
		
		public string SceneName { get; protected set; } = null;
		public bool Playable { get; protected set; } = false;
		public float Scale { get; protected set; } = 1;
		public ParamChara.ID Player { get; protected set; } = ParamChara.ID.Invalid;
		
		public static ParamScene GetData(int id)
		{
			if(id < 0) return null;
			if(id == 0) return new ParamScene(){ SceneName = "title", };
			if(id == 1) return new ParamScene(){ SceneName = "stage01", Player = ParamChara.ID.Hamach, };
			return null;
		}
		
		public static ParamScene GetData(ID id)
		{
			return GetData((int)id);
		}
	}
}
