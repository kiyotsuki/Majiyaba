using UnityEngine;

namespace Majiyaba
{
	/// <summary>
	/// ゲーム内パラメータ
	/// ParamGeneratorによって自動出力
	/// </summary>
	public class ParamChara
	{
		public enum ID
		{
			Invalid = -1,
			PlayerTest = 0,
			Player = 1,
			Hamach = 2,
		}
		
		public string PrefabName { get; protected set; } = null;
		public ParamCharaType.ID Type { get; protected set; } = ParamCharaType.ID.Invalid;
		
		public static ParamChara GetData(int id)
		{
			if(id < 0) return null;
			if(id == 0) return new ParamChara(){ PrefabName = "player_test", Type = ParamCharaType.ID.Object, };
			if(id == 1) return new ParamChara(){ PrefabName = "player", Type = ParamCharaType.ID.Human, };
			if(id == 2) return new ParamChara(){ PrefabName = "hamach", Type = ParamCharaType.ID.Fish, };
			return null;
		}
		
		public static ParamChara GetData(ID id)
		{
			return GetData((int)id);
		}
	}
}
