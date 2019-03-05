using UnityEngine;
using UnityEditor;

namespace Majiyaba
{
	/// <summary>
	/// パラメータ変数定義
	/// 各パラメータの変数一つについての定義
	/// CSVのヘッダ部分から生成される
	/// </summary>
	public class ParamValueDefine
	{
		public ParamValueDefine(int index, string sourceName, string sourceType, string sourceInit)
		{
			Index = index;
			Name = sourceName;
			Type = ConvertType(sourceType);
			Init = ConvertValue(sourceInit);
		}
		
		private string ConvertType(string type)
		{
			switch (type)
			{
				case "int":
				case "float":
				case "double":
				case "bool":
				case "string":
					return type;

				default:
					return "Param" + type + ".ID";
			}
		}

		public string ConvertValue(string value)
		{
			if (value == "")
			{
				return Init;
			}

			switch (Type)
			{
				case "int":
				case "float":
				case "double":
					return value;

				case "bool":
					return value.ToLower();

				case "string":
					if (value == "null") return "null";
					return '"' + value + '"';

				default:
					return Type + "." + value;
			}
		}

		public int Index { get; }
		public string Name { get; }
		public string Type { get; }
		public string Init { get; }
	}
}
