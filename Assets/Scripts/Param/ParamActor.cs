using UnityEngine;

namespace Majiyaba
{
	/// <summary>
	/// ゲーム内パラメータ
	/// ParamGeneratorによって自動出力
	/// </summary>
	public class ParamActor
	{
		public enum ID
		{
			Invalid = -1,
			PlayerTest = 0,
			Player = 1,
		}
		
		public class Data
		{
			public Data(string PrefabName, ParamActorType.ID Type)
			{
				this.PrefabName = PrefabName;
				this.Type = Type;
			}
			
			public string PrefabName { get; }
			public ParamActorType.ID Type { get; }
		}
		
		private static readonly Data[] data = 
		{
			new Data("player_test", ParamActorType.ID.Object),
			new Data("player", ParamActorType.ID.Human),
		};
		
		public static Data Get(int id)
		{
			if( id < 0 || data.Length <= id ) return null;
			return data[id];
		}
		
		public static Data Get(ID id)
		{
			return Get((int)id);
		}
		
		public static int Count
		{
			get { return data.Length; }
		}
	}
}
