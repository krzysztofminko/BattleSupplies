using UnityEngine;
using System.Collections;
using VehiclePhysics;

public class Player : MonoBehaviour, ITeam
{
	[SerializeField]
	private int _team;
	public int Team { get => _team; set => _team = value; }

	private void Awake()
	{
		ProgressBar.DisableAll();
	}

	private void Update()
	{
		if (Input.GetButtonDown("ShowInfo"))
			ProgressBar.EnableAll();

		if (Input.GetButtonUp("ShowInfo"))
			ProgressBar.DisableAll();
	}
}
