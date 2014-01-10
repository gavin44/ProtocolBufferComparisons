using System;


namespace SerializerCompare.Serializers
{
	public interface ITestSerializers
	{
		string GetName();


		bool IsBinary();


		dynamic Serialize<T>(object thisObj); 


		T Deserialize<T>(dynamic serInput);
	}
}