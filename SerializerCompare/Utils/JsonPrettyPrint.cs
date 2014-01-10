using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializerCompare.Utils
{
	class JsonHelper
	{
		private const string INDENT_STRING = "    ";


		public static string FormatJson(
			string jsonString)
		{
			var _indent = 0;
			var _quoted = false;
			var _stringBuilder = new StringBuilder();

			for (var index = 0; index < jsonString.Length; index++)
			{
				var _char = jsonString[index];

				switch (_char)
				{
					case '{':
					case '[':
						_stringBuilder.Append(_char);

						if (!_quoted)
						{
							_stringBuilder.AppendLine();
							Enumerable.Range(0, ++_indent).ForEach(item => _stringBuilder.Append(INDENT_STRING));
						}
						break;
					case '}':
					case ']':
						if (!_quoted)
						{
							_stringBuilder.AppendLine();
							Enumerable.Range(0, --_indent).ForEach(item => _stringBuilder.Append(INDENT_STRING));
						}
						_stringBuilder.Append(_char);
						break;
					case '"':
						_stringBuilder.Append(_char);

						bool _escaped = false;
						var _index = index;
						while (_index > 0 && jsonString[--_index] == '\\')
							_escaped = !_escaped;

						if (!_escaped)
							_quoted = !_quoted;
						break;
					case ',':
						_stringBuilder.Append(_char);

						if (!_quoted)
						{
							_stringBuilder.AppendLine();
							Enumerable.Range(0, _indent).ForEach(item => _stringBuilder.Append(INDENT_STRING));
						}
						break;
					case ':':
						_stringBuilder.Append(_char);
						if (!_quoted)
							_stringBuilder.Append(" ");
						break;
					default:
						_stringBuilder.Append(_char);
						break;
				}
			}

			return _stringBuilder.ToString();
		}
	}


	static class Extensions
	{
		public static void ForEach<T>(
			this IEnumerable<T> rangeIterator, 
			Action<T> action)
		{
			foreach (var _index in rangeIterator)
			{
				action(_index);
			}
		}
	}
}