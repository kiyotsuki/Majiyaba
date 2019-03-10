using UnityEngine;

namespace Majiyaba
{
	/// <summary>
	/// ゲーム内パラメータ
	/// ParamGeneratorによって自動出力
	/// </summary>
	public class ParamEffect
	{
		public enum ID
		{
			Invalid = -1,
			MoveTarget = 0,
		}
		
		public class Data
		{
			public Data(string Path)
			{
				this.Path = Path;
			}
			
			public string Path { get; }
		}
		
		private static readonly Data[] data = 
		{
			new Data("move_target"),
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
