using ProtoBuf;
using System.IO;


namespace SerializerCompare.Serializers
{
	public class ProtoBuf : ITestSerializers
	{
		public string GetName()
		{
			return "ProtoBuf";
		}


		public bool IsBinary()
		{
			return true;
		}


		public dynamic Serialize<T>(
			object thisObj)
		{
			using (var _memoryStream = new MemoryStream())
			{
				Serializer.NonGeneric.Serialize(_memoryStream, thisObj);

				return _memoryStream.ToArray();
			}
		}


		public T Deserialize<T>(
			dynamic bytes)
		{
			using (var _memoryStream = new MemoryStream((byte[])bytes))
			{
				return Serializer.Deserialize<T>(_memoryStream);
			}
		}
	}
}