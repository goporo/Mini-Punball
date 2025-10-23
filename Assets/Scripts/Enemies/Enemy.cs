using System.Collections;
using UnityEngine;

public class Enemy : BoardObject
{
    [SerializeField] private EnemySO data;
    [SerializeField] private EnemyUI enemyUI;

    private int currentHealth;
    private MoveBehavior MoveBehavior => data.moveBehavior;


    public event System.Action<Enemy> OnDeath;

    private void Awake()
    {
        currentHealth = data.baseHealth;
        enemyUI?.Init(data.baseHealth);
    }

    public IEnumerator DoMove(BoardState board)
    {
        var target = MoveBehavior.GetTargetCell(this, board);
        Debug.Log($"Enemy {name} moving from {CurrentCell} to {target}");

        if (board.TryMove(this, target))
        {
            yield return MoveBehavior.AnimateMove(this, board);
        }

        yield break;
    }

    public override void SetCell(Vector2Int cell, BoardState board)
    {
        CurrentCell = cell;
        transform.position = board.GetWorldPosition(cell.x, cell.y);
    }

    public void TakeDamage(DamageContext context)
    {
        currentHealth -= context.amount;
        enemyUI?.OnTakeDamage(currentHealth, data.baseHealth);

        if (currentHealth <= 0)
        {
            data.deathBehavior?.OnDeath(this, board: null);
            OnDeath?.Invoke(this);
        }
    }

}
