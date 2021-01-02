using UnityEngine;
using Sirenix.OdinInspector;

public class Picker : MonoBehaviour
{
	[Required]
	public Transform anchor;
	[ReadOnly]
	public Parentable picked;
}
