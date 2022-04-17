
public class BulletParameters
{
    private readonly Enemy _targetedEnemy;
    private readonly float _damage;
    private readonly float _speed;

    public BulletParameters(Enemy targetedEnemy, float damage, float speed)
    {
        _targetedEnemy = targetedEnemy;
        _damage = damage;
        _speed = speed;
    }

    public Enemy TargetedEnemy => _targetedEnemy;
    public float Damage => _damage;
    public float Speed => _speed;
}