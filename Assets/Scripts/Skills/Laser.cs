using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : RegisterableEffect
{
    private LineRenderer lr;
    private float lifeTime = .25f;
    private float timer;
    public Vector3 Startpoint;
    public Vector3 Endpoint;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        RegisterEffect();
    }

    public void Init(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        timer = lifeTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        UnregisterEffect();
    }
}
