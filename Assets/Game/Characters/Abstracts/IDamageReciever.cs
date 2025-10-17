using System;

public interface IDamageReciever
{
    public void SetDamage(int damage);
    public Type GetDamageRecieverType();
}
