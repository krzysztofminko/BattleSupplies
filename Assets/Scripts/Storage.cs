using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using System.Linq;

public class Storage : MonoBehaviour
{
	[Serializable]
	public class Slot
	{
		[Required]
		public Transform anchor;
		[ReadOnly]
		public Parentable obj;
	}

	[SerializeField, TableList(AlwaysExpanded = true)]
	private List<Slot> list = new List<Slot>(1);

	public bool HasEmptySlot => list.FirstOrDefault(s => !s.obj) != null;

	public void Add(Parentable obj)
	{
		Slot slot = list.FirstOrDefault(s => !s.obj);
		if (slot != null)
		{
			slot.obj = obj;
			obj.Parent = slot.anchor;
		}
	}

	public Parentable Remove()
	{
		Slot slot = list.LastOrDefault(s => s.obj);
		if (slot == null)
		{
			Debug.LogError("Can't Remove antyhing from empty Storage.", this);
			return null;
		}
		else
		{
			Parentable obj = slot.obj;
			slot.obj = null;
			obj.Parent = null;
			return obj;
		}
	}
}
