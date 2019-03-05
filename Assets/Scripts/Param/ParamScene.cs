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
		
		public class Data
		{
			public Data(string SceneName)
			{
				this.SceneName = SceneName;
			}
			
			public string SceneName { get; }
		}
		
		private static readonly Data[] data = 
		{
			new Data("title"),
			new Data("stage01"),
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
