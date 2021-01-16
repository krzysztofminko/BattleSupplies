using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using VehiclePhysics;

public class ExtendedVehicle : Vehicle, ITeam
{
	[Serializable]
	public class Seat
	{
		[Required]
		public Transform transform;
		[Required]
		public Transform exit;
		[ReadOnly]
		public Transform user;
	}
	
	[Header("Extensions")]
	[TableList(AlwaysExpanded = true, ShowIndexLabels = true)]
	public List<Seat> seats;

	[SerializeField]
	private int _team = -1;
	public int Team { get => _team; set => _team =  value; }
}
