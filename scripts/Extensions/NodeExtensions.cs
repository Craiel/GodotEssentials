namespace Craiel.Essentials.Extensions;

using System;
using System.Reflection;
using Attributes;
using Godot;
using Utils;

public static class NodeExtensions
{
	public static void DeserializeNodeMeta(this Node node)
	{
		var nodeType = node.GetType();
		FieldInfo[] fields = nodeType.GetFields();
		for (var i = 0; i < fields.Length; i++)
		{
			FieldInfo field = fields[i];
			if (field.GetCustomAttribute<NodeSerializedMetaAttribute>() == null)
			{
				continue;
			}
			
			Variant meta = node.GetMeta(field.Name);

			if (field.FieldType == TypeDef<double>.Value)
			{
				field.SetValue(node, meta.AsDouble());
				continue;
			}

			if (field.FieldType.IsSubclassOf(TypeDef<Resource>.Value))
			{
				field.SetValue(node, meta.Obj);
				continue;
			}

			if (field.FieldType.IsSubclassOf(TypeDef<Control>.Value))
			{
				var nodePath = meta.AsNodePath();
				field.SetValue(node, node.GetNode(nodePath));
				continue;
			}
			
			throw new NotSupportedException("Unhandled Field Type: " + field.FieldType);
		}
	}
}
