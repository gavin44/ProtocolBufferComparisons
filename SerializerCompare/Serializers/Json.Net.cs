using System;
using Newtonsoft.Json;


namespace SerializerCompare.Serializers
{
	public class JsonNET : ITestSerializers
	{
		public string GetName()
		{
			return "Json.NET";
		}


		public bool IsBinary()
		{
			return false;
		}


		public dynamic Serialize<T>(
			object thisObj)
		{
			return JsonConvert.SerializeObject(thisObj);
		}


		public T Deserialize<T>(
			dynamic json)
		{
			return JsonConvert.DeserializeObject<T>((string)json);
		}
	}
}