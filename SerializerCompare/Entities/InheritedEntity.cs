using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Xml.Serialization;
using ProtoBuf;


namespace SerializerCompare.Entities
{
	[XmlInclude(typeof(InheritedEntity))]
	[Serializable]
	[DataContract]
	[ProtoContract]
	public class InheritedEntity : SerializerCompare.Entities.SimpleEntity
	{
		[DataMember(Order = 100)]
		[ProtoMember(1)]
		public byte[] LargeIcon { get; set; }


		public new void FillDummyData()
		{
			base.FillDummyData();
			LargeIcon = new byte[32];

			var _cryptoServiceProvider = new RNGCryptoServiceProvider();
			_cryptoServiceProvider.GetBytes(LargeIcon);
		}
	}
}