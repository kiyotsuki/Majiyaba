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
			Player = 0,
		}
		
		public class Data
		{
			public Data(string Path, ParamActorType.ID Type)
			{
				this.Path = Path;
				this.Type = Type;
			}
			
			public string Path { get; }
			public ParamActorType.ID Type { get; }
		}
		
		private static readonly Data[] data = 
		{
			new Data("Misaki/misaki", ParamActorType.ID.Human),
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
