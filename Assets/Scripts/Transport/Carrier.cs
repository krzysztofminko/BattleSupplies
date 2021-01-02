using UnityEngine;
using Sirenix.OdinInspector;

public class Carrier : MonoBehaviour
{
	[Required]
	public Transform anchor;
	[ReadOnly]
	public Cargo cargo;
}
