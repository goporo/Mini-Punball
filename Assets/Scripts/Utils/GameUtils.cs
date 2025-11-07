public class GameUtils
{
  public static string FormatHealthText(int health)
  {
    if (health < 10000)
      return health.ToString();

    string[] suffixes = { "K", "M", "B", "T" };
    double value = health;
    int suffixIndex = -1;

    while (value >= 10000 && suffixIndex < suffixes.Length - 1)
    {
      value /= 1000;
      suffixIndex++;
    }

    if (suffixIndex == -1)
      return health.ToString();

    // Show one decimal if value < 100, else no decimal
    if (value < 100)
      return $"{value:0.0}{suffixes[suffixIndex]}";
    else
      return $"{value:0}{suffixes[suffixIndex]}";
  }
}