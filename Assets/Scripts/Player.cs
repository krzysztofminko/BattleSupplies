using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, ITeam
{
	[SerializeField]
	private int _team;
	public int Team { get => _team; set => _team = value; }
}
