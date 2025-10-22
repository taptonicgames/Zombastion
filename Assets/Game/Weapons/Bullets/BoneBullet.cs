using UnityEngine;

public class BoneBullet : Bullet
{
	[SerializeField]
    private Transform tr;

	private const float ROTATION_SPEED = 30f;

	protected override void Update()
    {
        base.Update();
        tr.Rotate(-Vector3.up * ROTATION_SPEED * Time.deltaTime * 50, Space.Self);
    }
}
