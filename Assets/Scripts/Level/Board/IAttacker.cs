using System.Collections;

public interface IAttacker
{
  IEnumerator DoAttack(BoardState board);
}