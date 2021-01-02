using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using System.Linq;

public class Storage : MonoBehaviour, IPickable
{
	[Serializable]
	public class Slot
	{
		[Required]
		public Transform anchor;
		[ReadOnly]
		public Cargo cargo;
	}

	[SerializeField, TableList(AlwaysExpanded = true)]
	private List<Slot> list = new List<Slot>(1);

	public bool HasEmptySlot => list.FirstOrDefault(s => !s.cargo) != null;

	public bool Put(Cargo cargo)
	{
		Slot slot = list.FirstOrDefault(s => !s.cargo);
		if (slot != null)
		{
			slot.cargo = cargo;
			cargo.SetParent(slot.anchor);
			return true;
		}
		return false;
	}

	public Cargo Pick(Transform parentTo)
	{
		Slot slot = list.LastOrDefault(s => s.cargo);
		if (slot == null)
		{
			return null;
		}
		else
		{
			Cargo cargo = slot.cargo;
			slot.cargo = null;
			cargo.SetParent(parentTo);
			return cargo;
		}
	}
}
