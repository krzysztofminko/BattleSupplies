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

	public Parentable Remove(Parentable obj = null)
	{
		Debug.Log("Remove " + obj, this);
		Slot slot = obj? list.FirstOrDefault(s => s.obj == obj) : list.LastOrDefault(s => s.obj);
		if (slot == null)
		{
			Debug.LogError("Nothing to remove.", this);
			return null;
		}
		else
		{
			slot.obj.Parent = null;
			Parentable tmp = slot.obj;
			slot.obj = null;
			return tmp;
		}
	}
}
