using Sirenix.OdinInspector;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField]
	private float damage = 20;
	[SerializeField]
	private float damageLossDistance = 30;
	[SerializeField]
	private AnimationCurve damageLossCurve = new AnimationCurve(new Keyframe(0, 1, 0, -1), new Keyframe(1, 0, -1, 0));
	[SerializeField, Min(0), SuffixLabel("per second")]
	private float speed = 100;

	[SerializeField, ReadOnly]
	private float travelledDistance;
	[SerializeField, ReadOnly]
	private float actualDamage;

	[ReadOnly]
	public Destroyable target;

	private float height;

	private void Start()
	{
		height = transform.position.y;
	}

	private void Update()
	{
		if (target)
		{
			if ((target.transform.position + Vector3.up * height - transform.position).sqrMagnitude < speed * Time.deltaTime)
			{
				target.HP -= actualDamage;
				Destroy(gameObject);
			}
			transform.position += (target.transform.position + Vector3.up * height - transform.position).normalized * speed * Time.deltaTime;
			transform.forward = target.transform.position + Vector3.up * height - transform.position;
			travelledDistance += speed * Time.deltaTime;
			actualDamage = damage * damageLossCurve.Evaluate(travelledDistance / damageLossDistance);
		}
		else 
		{
			Destroy(gameObject);
		}
	}
}
