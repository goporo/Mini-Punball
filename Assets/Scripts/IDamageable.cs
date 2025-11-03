public interface IDamageable
{
  /// <summary>
  /// Applies damage to the object.
  /// </summary>
  /// <param name="context">The damage context containing the damage information.</param>
  /// <returns>True if the target died, false otherwise.</returns>
  public bool TakeDamage(DamageContext context);
}
