public interface IDamageable
{
    float MaxHp { get; }
    float Hp { get; set; }
    void Damage(float value);
}