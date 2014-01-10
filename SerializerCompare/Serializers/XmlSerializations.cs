using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;


namespace SerializerCompare.Serializers
{
	public class XmlDotNet : ITestSerializers
	{
		public string GetName()
		{
			return "Xml .NET";
		}


		public bool IsBinary()
		{
			return false;
		}


		public dynamic Serialize<T>(
			object thisObj)
		{
			var _memoryStream = new MemoryStream();
			var _xmlSerializer = new XmlSerializer(typeof(T));

			_xmlSerializer.Serialize(_memoryStream, thisObj);
			_memoryStream.Seek(0, SeekOrigin.Begin);

			var _streamReader = new StreamReader(_memoryStream, Encoding.UTF8);
			string _outputString = _streamReader.ReadToEnd();

			return _outputString;
		}


		public T Deserialize<T>(
			dynamic xml)
		{
			byte[] _byteArray = Encoding.UTF8.GetBytes((string)xml);

			var _memoryStream = new MemoryStream(_byteArray);
			_memoryStream.Seek(0, SeekOrigin.Begin);

			var _xmlSerializer = new XmlSerializer(typeof(T));

			return (T)_xmlSerializer.Deserialize(_memoryStream);
		}
	}
}