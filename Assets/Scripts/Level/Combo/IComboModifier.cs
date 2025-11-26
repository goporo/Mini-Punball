public interface IComboModifier
{
  int ModifyAmount(BallBuffTarget target, int baseAmount);
}

public class SpecialBallModifier : IComboModifier
{
  private int multiplier = 5;

  public SpecialBallModifier(int multiplier)
  {
    this.multiplier = multiplier;
  }

  public int ModifyAmount(BallBuffTarget target, int baseAmount)
  {
    if (target != BallBuffTarget.Special)
    {
      return baseAmount;
    }
    return baseAmount * multiplier;
  }
}