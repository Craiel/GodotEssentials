namespace Craiel.Essentials.Extensions;

using System;
using System.IO;
using System.Reflection;
using Attributes;
using Godot;
using Utils;

public static class NodeExtensions
{
	public static T Instantiate<T>(this PackedScene prefab)
		where T: class
	{
		if (prefab == null)
		{
			throw new InvalidOperationException("Instantiate called with null prefab");
		}
		
		var instance = prefab.Instantiate();
		if (instance == null)
		{
			throw new InvalidOperationException("Instantiation failed");
		}
		
		var result = instance.GetRootNode<T>();
		if (result == null)
		{
			throw new InvalidDataException("Prefab " + prefab.ResourceName + " has no root node of type: " + typeof(T));
		}

		return result;
	}
	
	public static T GetRootNode<T>(this Node scene)
		where T: class
	{
		return scene.GetNode<T>(".");
	}
	
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

			if (field.FieldType == TypeDef<Color>.Value)
			{
				field.SetValue(node, meta.AsColor());
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
