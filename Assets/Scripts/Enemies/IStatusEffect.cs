public interface IStatusEffect
{
    StatusEffectType EffectType { get; }
    void OnApply(Enemy enemy);
    void OnRound(Enemy enemy); // called at start of round
    void OnExpire(Enemy enemy);

    bool IsExpired { get; }
}

