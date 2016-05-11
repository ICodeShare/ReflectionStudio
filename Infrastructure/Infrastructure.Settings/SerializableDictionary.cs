using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
namespace Infrastructure.Settings
{
	[XmlRoot("dictionary")]
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
	{
		public SerializableDictionary()
		{
		}
		protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		public XmlSchema GetSchema()
		{
			return null;
		}
		public void ReadXml(XmlReader reader)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
			bool isEmptyElement = reader.IsEmptyElement;
			reader.Read();
			if (isEmptyElement)
			{
				return;
			}
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.ReadStartElement("item");
				reader.ReadStartElement("key");
				TKey key = (TKey)((object)xmlSerializer.Deserialize(reader));
				reader.ReadEndElement();
				reader.ReadStartElement("value");
				TValue value = (TValue)((object)xmlSerializer2.Deserialize(reader));
				reader.ReadEndElement();
				base.Add(key, value);
				reader.ReadEndElement();
				reader.MoveToContent();
			}
			reader.ReadEndElement();
		}
		public void WriteXml(XmlWriter writer)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
			foreach (TKey current in base.Keys)
			{
				writer.WriteStartElement("item");
				writer.WriteStartElement("key");
				xmlSerializer.Serialize(writer, current);
				writer.WriteEndElement();
				writer.WriteStartElement("value");
				TValue tValue = base[current];
				xmlSerializer2.Serialize(writer, tValue);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}
	}
}
