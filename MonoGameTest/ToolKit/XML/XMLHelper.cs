using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

	}
}
