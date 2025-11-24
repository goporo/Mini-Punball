using UnityEngine;

public class GameUtils
{
  public static string FormatHealthText(int health)
  {
    if (health < 10000)
      return health.ToString();

    if (health < 1_000_000)
    {
      int thousands = health / 1000;
      int decimalPart = (health % 1000) / 100;

      return decimalPart > 0
          ? $"{thousands}.{decimalPart}K"
          : $"{thousands}K";
    }

    if (health < 1_000_000_000)
    {
      int millions = health / 1_000_000;
      int decimalPart = (health % 1_000_000) / 100_000;

      return decimalPart > 0
          ? $"{millions}.{decimalPart}M"
          : $"{millions}M";
    }

    int billions = health / 1_000_000_000;
    int decimalB = (health % 1_000_000_000) / 100_000_000;

    return decimalB > 0
        ? $"{billions}.{decimalB}B"
        : $"{billions}B";
  }


}