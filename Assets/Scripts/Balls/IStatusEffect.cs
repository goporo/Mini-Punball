

public interface IStatusEffect
{
    void Apply(Enemy enemy);
    void Duration(Enemy enemy, int rounds);
    bool IsFinished { get; }
}
