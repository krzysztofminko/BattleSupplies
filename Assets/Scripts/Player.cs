using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, ITeam
{
	[SerializeField]
	private int _team;
	public int Team { get => _team; set => _team = value; }

	private void Update()
	{
		if (Input.GetButtonDown("ShowInfo"))
			ProgressBar.EnableAll();

		if (Input.GetButtonUp("ShowInfo"))
			ProgressBar.DisableAll();
	}
}
