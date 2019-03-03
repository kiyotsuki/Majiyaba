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
			this.index = index;
			this.name = sourceName;
			this.type = ConvertType(sourceType);
			this.init = ConvertValue(sourceInit);
		}

		public int GetIndex()
		{
			return index;
		}

		public string GenerateConstructorDefine()
		{
			return $"{ type } { name }";
		}

		public string GenerateSetValueDefine()
		{
			return $"this.{ name } = { name };";
		}

		public string GeneratePropatyDefine()
		{
			return $"public { type } { name } {{ get; }}";
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
				return init;
			}

			switch (type)
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
					return type + "." + value;
			}
		}

		int index = 0;
		string name = null;
		string type = null;
		string init = null;
	}
}
