using System;
using System.Collections.Generic;
using ProtoBuf;

namespace SerializerCompare
{
	[ProtoContract]
	public class Results
	{
		[ProtoMember(1)]
		public string serName { get; set; }
		[ProtoMember(2)]
		public int sizeBytes { get; set; }
		[ProtoMember(3)]
		public bool success { get; set; }
		[ProtoMember(4)]
		public List<ResultColumnEntry> resultColumn { get; set; }

		// debug
		[ProtoMember(5)]
		public string serializedFormObject { get; set; }
		[ProtoMember(6)]
		public string orignalObjectAsJson { get; set; } // stored as JSON for human visualization
		//This is the original object => serialized => deserialized. Should again match original object if all went well!
		[ProtoMember(7)]
		public string testObjectAsJson { get; set; } // stored as JSON for human visualization
	}


	[ProtoContract]
	public class ResultColumnEntry
	{
		[ProtoMember(1)]
		public TimeSpan time { get; set; }
		[ProtoMember(2)]
		public int iteration { get; set; }
	}
}
