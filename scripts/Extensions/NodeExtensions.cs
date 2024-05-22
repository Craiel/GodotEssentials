namespace Craiel.Essentials.Extensions;

using System;
using System.IO;
using Godot;

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
	
	public static T GetRootNode<T>(this Node node)
		where T: class
	{
		return node.GetNode<T>(".");
	}

	public static void Empty(this Node node, bool queuedFree = true)
	{
		if (node.GetChildCount() == 0)
		{
			return;
		}

		var childNodes = node.GetChildren();
		for (var i = 0; i < childNodes.Count; i++)
		{
			if (queuedFree)
			{
				childNodes[i].QueueFree();
			}
			else
			{
				childNodes[i].Free();
			}
		}
	}
}
