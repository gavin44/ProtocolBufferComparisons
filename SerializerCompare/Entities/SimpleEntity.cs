using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Xml.Serialization;
using ProtoBuf;

namespace SerializerCompare.Entities
{
	[XmlInclude(typeof(SimpleEntity))]
	[Serializable]
	[DataContract]
	[ProtoContract]
	public class SimpleEntity
	{
		[DataMember(Order = 10)]
		[ProtoMember(1)]
		public string Message { get; set; }


		[DataMember(Order = 20)]
		[ProtoMember(2)]
		public string FunctionCall { get; set; }


		[DataMember(Order = 30)]
		[ProtoMember(3)]
		public string Parameters { get; set; }


		[DataMember(Order = 40)]
		[ProtoMember(4)]
		public string Name { get; set; }


		[DataMember(Order = 50)]
		[ProtoMember(5)]
		public int EmployeeId { get; set; }


		[DataMember(Order = 60)]
		[ProtoMember(6)]
		public float RaiseRate { get; set; }


		[DataMember(Order = 70)]
		[ProtoMember(7)]
		public string AddressLine1 { get; set; }


		[DataMember(Order = 80)]
		[ProtoMember(8)]
		public string AddressLine2 { get; set; }


		[DataMember(Order = 90)]
		[ProtoMember(9)]
		public byte[] Icon { get; set; }


		public void FillDummyData()
		{
			this.Message = "Hello World!";
			this.FunctionCall = "FunctionNameHere";
			this.Parameters = "x=1,y=2,z=3";
			this.Name = "SampleName";
			this.EmployeeId = 1;
			this.RaiseRate = 1.2F;
			this.AddressLine1 = "1 High Street";
			this.AddressLine2 = "Kerry";
			this.Icon = new byte[16];

			var _cryptoServiceProvider = new RNGCryptoServiceProvider();
			_cryptoServiceProvider.GetBytes(Icon);
		}
	}
}