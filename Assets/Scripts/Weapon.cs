using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
	[SerializeField]
	private int _id;
	public int ID => _id;

	[SerializeField]
	private Transform _model;
	public Transform Model => _model;
}
