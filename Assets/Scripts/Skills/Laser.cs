using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lr;
    public float lifeTime = 5f; // how long it stays visible
    private float timer;
    public Vector3 Startpoint;
    public Vector3 Endpoint;
    private Color laserColor = new Color(1f, 0.45f, 0f); // light yellow

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        Init(Startpoint, Endpoint);
    }

    public void Init(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startColor = lr.endColor = laserColor;
        timer = lifeTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) Destroy(gameObject);
    }
}
