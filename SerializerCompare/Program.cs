using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SerializerCompare
{
	public class Program
	{
		public static List<Results> c_results;
		public static Test c_test = new Test();


		static void Main(
			string[] args)
		{
			bool _stillWorking = true;
			char _menuOption = 'T';

			while (_stillWorking)
			{
				switch (_menuOption)
				{
					case 'T':
						RunTests();
						break;
					case 'E':
						_stillWorking = false;
						break;
					case 'D':
						Console.WriteLine("Type the name of the serializer to print:");
						string _serializationType = Console.ReadLine();

						PrintTestObject(_serializationType);
						break;
					case 'R':
						c_test.PrintResultTable(c_results);
						break;
					default:
						Console.WriteLine("Unknown input!");
						break;
				}

				if (_stillWorking)
				{
					PrintMenu();
					_menuOption = GetUserSelection();
				}
			}
		}


		static void PrintMenu()
		{
			Console.WriteLine("Options: (T)est, (R)esults, (D)eserializer output, (E)xit");
		}


		static char GetUserSelection()
		{
			char _menuOption = '~'; 

			string _inputString = Console.ReadLine();
			if (_inputString != null)
			{
				_inputString = _inputString.ToUpper().Replace(" ", string.Empty);

				if (_inputString.Length > 0)
					_menuOption = _inputString[0];
			}

			return _menuOption;
		}


		static void PrintTestObject(
			string serializationType)
		{
			Results resultsThisSer = c_results.Find(a => a.serName == serializationType);

			if (resultsThisSer != null)
			{
				Console.WriteLine(resultsThisSer.serializedFormObject);
			}
			else
			{
				Console.WriteLine("Incorrect serializer name!");
			}
		}


		static void RunTests()
		{
			// Pick an entity type
			var _originalObject = new SerializerCompare.Entities.InheritedEntity();
			_originalObject.FillDummyData();

			var testObject = new SerializerCompare.Entities.InheritedEntity();
			c_results = c_test.RunTests(_originalObject, testObject);

			c_test.PrintResultTable(c_results);
		}
	}
}