public class ZombieSpeedDebuff : AbstractDebuff
{
	public ZombieSpeedDebuff(AbstractUnit unit, float value) : base(unit)
	{
		unit.Agent.speed -= value;
		unit.Animator.speed -= value;
	}
}
