public interface IDamagable
{
    void TakeDamage(float damage, string rocket_tag);
}
public interface IShooting
{
    event RocketFire Fire;
}