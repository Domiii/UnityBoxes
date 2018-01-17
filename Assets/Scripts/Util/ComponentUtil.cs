
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ComponentUtil {
	/**
	 * Source: http://answers.unity3d.com/questions/458207/copy-a-component-at-runtime.html
	 */
	public static T CopyComponent<T>(T original, GameObject destination) where T : Component
	{
		System.Type type = original.GetType();
		Component copy = destination.AddComponent(type);
		System.Reflection.FieldInfo[] fields = type.GetFields();
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(copy, field.GetValue(original));
		}
		return copy as T;
	}

	public static void ForEachComponentInHierarchy<C>(this Transform target, System.Action<C> action, int depth = 0) 
		where C : Component
	{
		var c = target.GetComponent<C> ();
		if (c) {
			action (c);
		}

		// recurse
		foreach (var child in target) {
			ForEachComponentInHierarchy ((Transform)child, action, depth+1);
		}
	}
}
