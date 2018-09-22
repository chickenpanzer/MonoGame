using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ToolKit
{
	public static class XMLHelper
	{

		public static T ReadXMLToObject<T>(string xmlFileName)
		{
			var result = default(T);

			XmlSerializer serializer = new XmlSerializer(typeof(T));
			using (var sr = new FileStream(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFileName), FileMode.Open))
			{
				result = (T)serializer.Deserialize(sr);
			}

			return result;
		}

		public static void WriteObjectToXML<T>(string path, T objet)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));

			using (Stream fs = new FileStream(path, FileMode.Create))
			using (XmlWriter writer = new XmlTextWriter(fs, Encoding.Unicode))
			{
				serializer.Serialize(writer, objet);
				//writer.Close();
			}
		}

		public static T[] Redim<T>(this T[] arr, bool preserved)
		{
			int arrLength = 0;
			arrLength = (arr != null ? arr.Length : 0);

			T[] arrRedimed = new T[arrLength + 1];
			if (preserved)
			{
				for (int i = 0; i < arrLength; i++)
				{
					arrRedimed[i] = arr[i];
				}
			}
			return arrRedimed;
		}

	}
}
