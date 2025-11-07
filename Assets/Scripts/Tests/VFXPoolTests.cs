using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class VFXPoolTests
{
  private GameObject prefab;
  private Transform parent;

  [UnitySetUp]
  public IEnumerator SetUp()
  {
    // Create a dummy prefab with ExampleVFX component
    prefab = new GameObject("VFXPrefab");
    prefab.AddComponent<LaserVFX>(); // Replace with your actual VFX type
    parent = new GameObject("PoolParent").transform;
    yield return null;
  }

  [UnityTearDown]
  public IEnumerator TearDown()
  {
    Object.Destroy(prefab);
    Object.Destroy(parent.gameObject);
    yield return null;
  }

  [UnityTest]
  public IEnumerator Pool_Reuses_And_Instantiates_As_Expected()
  {
    int initialSize = 5;
    var pool = new VFXPool<LaserVFX, LaserVFXParams>(prefab, parent, initialSize);

    // Get all pre-instantiated objects
    var instances = new LaserVFX[initialSize];
    for (int i = 0; i < initialSize; i++)
    {
      instances[i] = pool.GetTyped();
      Assert.IsNotNull(instances[i]);
    }

    // Pool should now be empty, next call should instantiate a new one
    var extra = pool.GetTyped();
    Assert.IsNotNull(extra);
    Assert.IsFalse(System.Array.Exists(instances, x => x == extra), "Extra instance should be a new object");

    // Return all to pool
    foreach (var inst in instances)
      pool.Return(inst);
    pool.Return(extra);

    // Now pool should have initialSize+1 objects
    int count = 0;
    while (true)
    {
      var obj = pool.GetTyped();
      if (obj == null) break;
      count++;
      if (count > initialSize + 1) break; // Prevent infinite loop
    }
    Assert.AreEqual(initialSize + 1, count);
    yield return null;
  }
}

// Dummy classes for test compilation; replace with your actual types
public class LaserVFX : BaseVFX<LaserVFXParams> { }

public class BaseVFX<TParams> : PooledVFX, IBaseVFX<TParams> where TParams : IVFXSpawnParams
{
}
public class LaserVFXParams : IVFXSpawnParams { }
public interface IVFXSpawnParams { }
public abstract class PooledVFX : MonoBehaviour { }
public interface IBaseVFX<T> { }

public class VFXPool<T, TParams> where T : BaseVFX<TParams> where TParams : IVFXSpawnParams
{
  public VFXPool(GameObject prefab, Transform parent, int initialSize) { }
  public T GetTyped() { return null; }
  public void Return(T vfx) { }
}