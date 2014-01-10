using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SerializerCompare.Serializers;
using SerializerCompare.Utils;
using ProtoBuf;


namespace SerializerCompare
{
	public class Test
	{
		private object c_originalObject;
		private object c_testObject;

		public List<Results> RunTests<T>(
			T originalObj, 
			T testObj)
		{
			var _resultTable = new List<Results>();
			this.c_originalObject = originalObj;
			this.c_testObject = testObj;

			int _loopLimit = 10000;

			// Json.NET test
			_resultTable.Add(this.TestSerializerInLoop<T>(new JsonNET(), _loopLimit));

			// .NET XML serializer
			_resultTable.Add(this.TestSerializerInLoop<T>(new XmlDotNet(), _loopLimit));

			// ProtoBuf test
			_resultTable.Add(this.TestSerializerInLoop<T>(new Serializers.ProtoBuf(), _loopLimit));

			return _resultTable;
		}


		public void PrintResultTable(
			List<Results> resultTable)
		{
			int _firstColumn = 5;
			int _columnN = 20;

			// Header - line 1
			string _formatString = String.Format("{{0,{0}}}", _firstColumn);
			string _toPrint = string.Empty;

			Console.Write(_formatString, _toPrint);

			for (int i = 0; i < resultTable.Count; i++)
			{
				_toPrint = String.Format("{0}", resultTable[i].serName);
				_formatString = String.Format("{{0,{0}}}", _columnN);

				Console.Write(_formatString, _toPrint);
			}

			Console.Write(Environment.NewLine);

			// Header - line 2
			_formatString = String.Format("{{0,{0}}}", _firstColumn);
			_toPrint = "Loop";

			Console.Write(_formatString, _toPrint);
			for (int i = 0; i < resultTable.Count; i++)
			{
				_toPrint = String.Format("Size:{0} bytes", resultTable[i].sizeBytes);
				_formatString = String.Format("{{0,{0}}}", _columnN);
				System.Console.Write(_formatString, _toPrint);
			}

			Console.Write(Environment.NewLine);

			// Rest of Table
			//Assumes: all colums have same length and rows line-up for same loop count
			for (int row = 0; row < resultTable[0].resultColumn.Count; row++)
			{
				_formatString = String.Format("{{0,{0}}}", _firstColumn);
				_toPrint = resultTable[0].resultColumn[row].iteration.ToString();

				Console.Write(_formatString, _toPrint);

				for (int i = 0; i < resultTable.Count; i++)
				{
					_formatString = String.Format("{{0,{0}}}", _columnN);
					_toPrint = String.Format("{0,4:n4} ms", resultTable[i].resultColumn[row].time.TotalMilliseconds);

					Console.Write(_formatString, _toPrint);
				}

				Console.Write(Environment.NewLine);
			}
		}


		private Results TestSerializerInLoop<T>(
			dynamic serializer, 
			int loopLimit)
		{
			int _sizeInBytes;
			bool _isSuccess;
			string _testObjJson;
			var _iterations = 1;
			var _result = new Results();
			var _warmUpObjects = new List<object>();

			_result.serName = serializer.GetName();
			_result.resultColumn = new List<ResultColumnEntry>();
			do
			{
				// test at this loop count
				ResultColumnEntry resultEntry = TestSerializer<T>(serializer, _iterations, out _sizeInBytes, out _isSuccess, out _testObjJson);
				_result.resultColumn.Add(resultEntry);
				_iterations = _iterations * 2;    // geometrically scale loop at x2
			} while (_iterations <= loopLimit);

			_result.sizeBytes = _sizeInBytes;
			_result.success = _isSuccess;
			_result.testObjectAsJson = _testObjJson; // for debug
			_result.serializedFormObject = PrintSerializedOutput<T>(serializer); // for debug

			Console.WriteLine("Serializer : {0}\nMessage Format :\n\t{1}\n\n",serializer, _result.serializedFormObject);
			if (_result.serName == "ProtoBuf")
			{
				var _protoFile = Serializer.GetProto<SerializerCompare.Entities.SimpleEntity>();
				Console.WriteLine("Proto File for Protocol Buffers :");
				Console.WriteLine("===============================\n{0}", _protoFile);
			}

			return _result;
		}


		private string PrintSerializedOutput<T>(
			dynamic serializer)
		{
			string _stringOutput;
			if (serializer.IsBinary())
			{
				byte[] _binaryOutput = serializer.Serialize<T>(this.c_originalObject);

				_stringOutput = BitConverter.ToString(_binaryOutput).Replace("-", " ");
			}
			else
			{
				_stringOutput = serializer.Serialize<T>(this.c_originalObject);
			}

			return _stringOutput;
		}


		private ResultColumnEntry TestSerializer<T>(
			dynamic serializer, 
			int iterations, 
			out int sizeInBytes, 
			out bool success, 
			out string testObjJson)
		{
			Stopwatch _stopWatch = new Stopwatch();

			if (serializer.IsBinary())
			{
				byte[] _binaryOutput;
				_stopWatch.Reset();
				_stopWatch.Start();

				for (int _index = 0; _index < iterations; _index++)
				{
					_binaryOutput = serializer.Serialize<T>(this.c_originalObject);

					this.c_testObject = serializer.Deserialize<T>(_binaryOutput);
				}

				_stopWatch.Stop();
				// Find size outside loop to avoid timing hits
				_binaryOutput = serializer.Serialize<T>(this.c_originalObject);
				sizeInBytes = _binaryOutput.Count();
			}
			else
			{
				string _stringOutput;
				_stopWatch.Reset();
				_stopWatch.Start();

				for (int index = 0; index < iterations; index++)
				{
					_stringOutput = serializer.Serialize<T>(this.c_originalObject);

					this.c_testObject = serializer.Deserialize<T>(_stringOutput);
				}
				_stopWatch.Stop();

				// Find size outside loop to avoid timing hits
				// Size as bytes for UTF-8 as it's most common on internet
				var _encoding = new UTF8Encoding();

				byte[] _stringInBytes = _encoding.GetBytes(serializer.Serialize<T>(this.c_originalObject));
				sizeInBytes = _stringInBytes.Count();
			}

			var _entry = new ResultColumnEntry();
			_entry.iteration = iterations;

			long _avgerageTicks = (_stopWatch.Elapsed.Ticks / iterations);
			//if (_avgerageTicks == 0)
			//{
			//	// sometime when running windows inside a VM this is 0! Possible vm issue?
			//	//Debugger.Break();
			//}

			_entry.time = new TimeSpan(_avgerageTicks);


			// Debug: To aid printing to screen, human debugging etc. Json used as best for console presentation
			JsonNET jsonSer = new JsonNET();

			string orignalObjectAsJson = JsonHelper.FormatJson(jsonSer.Serialize<T>(c_originalObject));

			testObjJson = JsonHelper.FormatJson(jsonSer.Serialize<T>(c_testObject));
			success = true;

			return _entry;
		}
	}
}