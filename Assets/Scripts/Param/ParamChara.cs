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
		
		public class Data
		{
			public Data(string PrefabName, ParamCharaType.ID Type)
			{
				this.PrefabName = PrefabName;
				this.Type = Type;
			}
			
			public string PrefabName { get; }
			public ParamCharaType.ID Type { get; }
		}
		
		private static readonly Data[] data = 
		{
			new Data("player_test", ParamCharaType.ID.Object),
			new Data("player", ParamCharaType.ID.Human),
			new Data("hamach", ParamCharaType.ID.Fish),
		};
		
		public static Data GetData(int id)
		{
			if( id < 0 || data.Length <= id ) return null;
			return data[id];
		}
		
		public static Data GetData(ID id)
		{
			return GetData((int)id);
		}
		
		public static int GetCount()
		{
			return data.Length;
		}
	}
}
