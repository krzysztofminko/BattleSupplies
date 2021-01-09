using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using System.Linq;

public class Container : MonoBehaviour
{
	[Serializable]
	public class Slot
	{
		[Required]
		public Transform anchor;
		[ReadOnly]
		public Transform obj;
	}

	[SerializeField, TableList(AlwaysExpanded = true)]
	private List<Slot> list = new List<Slot>(1);

	public bool IsEmpty => list.LastOrDefault(s => s.obj) == null;
	public bool HasEmptySlot => list.FirstOrDefault(s => !s.obj) != null;
	public Transform GetLastObject => list.LastOrDefault(s => s.obj)?.obj;

	public bool Load(Transform obj)
	{
		Slot slot = list.FirstOrDefault(s => !s.obj);
		if (slot != null)
		{
			slot.obj = obj;
			obj.parent = slot.anchor;
			obj.localPosition = Vector3.zero;
			obj.localRotation = Quaternion.identity;
			return true;
		}
		return false;
	}

	public Transform Unload(Transform obj = null)
	{
		Slot slot = obj ? list.FirstOrDefault(s => s.obj == obj) : list.LastOrDefault(s => s.obj);
		if (obj && slot == null)
		{
			Debug.LogError("Nothing to unload. Object not in container.", this);
			return null;
		}
		else if (slot != null)
		{
			obj = slot.obj;
			slot.obj = null;
			obj.SetParent(null);
			return obj;
		}
		return null;
	}
}
