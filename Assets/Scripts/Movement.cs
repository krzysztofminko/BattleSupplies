using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class Movement : MonoBehaviour
{
	public enum Type { Walk, Run }

	[Min(0)]
	public float walkSpeed = 4;
	[Min(0)]
	public float runSpeed = 8;
	[Min(0)]
	public float turnSpeed = 600;

	private CharacterController characterController;
	private Animator animator;

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
	}

	public void Move(Vector3 direction, Type type = Type.Walk)
	{
		float speed = 0;
		switch (type)
		{
			case Type.Walk:
			{
				speed = walkSpeed;
			}
			break;
			case Type.Run:
			{
				speed = runSpeed;
			}
			break;
		}

		if (direction.sqrMagnitude > 0)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.main.transform.rotation * direction.normalized, Vector3.up)), turnSpeed * Time.deltaTime);
		}
		else
		{
			speed = 0;
		}

		characterController.SimpleMove(Camera.main.transform.rotation * direction * speed);
		animator.SetFloat("MoveSpeed", speed);
	}
}