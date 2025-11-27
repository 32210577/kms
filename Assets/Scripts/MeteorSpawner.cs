
using UnityEngine;
using System.Collections;

public class MeteorSpawner : MonoBehaviour
{
    [Header("References")]
    public Camera targetCamera;               // Usually Camera.main
    public GameObject meteorPrefab;           // Meteor prefab
    public LayerMask groundMask;              // Ground layer

    [Header("Spawn Timing")]
    public float spawnInterval = 2.0f;        // Interval in seconds
    public Vector2 intervalJitter = new Vector2(0f, 0.5f); // Optional jitter

    [Header("Spawn Area (Viewport)")]
    [Range(0f, 1f)] public float viewportMargin = 0.08f;   // Avoid edges

    [Header("Spawn Height")]
    public float spawnHeight = 20f;           // Height above ground to spawn
    public float minDropSpeed = 10f;          // Optional initial downward speed
    public float maxDropSpeed = 18f;

    [Header("Safety")]
    public int maxRaycastTries = 6;           // Retry if raycast fails

    [Header("Toggle")]
    public bool autoStart = true;             // Auto start spawns
    private Coroutine loopRoutine;

    void Awake()
    {
        if (targetCamera == null) targetCamera = Camera.main;
    }

    void OnEnable()
    {
        if (autoStart) StartSpawning();
    }

    void OnDisable()
    {
        StopSpawning();
    }

    public void StartSpawning()
    {
        if (loopRoutine == null)
            loopRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
            loopRoutine = null;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            TrySpawnMeteorInView();
            float t = spawnInterval + Random.Range(intervalJitter.x, intervalJitter.y);
            if (t < 0.05f) t = 0.05f;
            yield return new WaitForSeconds(t);
        }
    }

    void TrySpawnMeteorInView()
    {
        if (targetCamera == null || meteorPrefab == null) return;

        float min = viewportMargin;
        float max = 1f - viewportMargin;

        for (int i = 0; i < maxRaycastTries; i++)
        {
            Vector2 vp = new Vector2(Random.Range(min, max), Random.Range(min, max));
            Ray ray = targetCamera.ViewportPointToRay(new Vector3(vp.x, vp.y, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask, QueryTriggerInteraction.Ignore))
            {
                Vector3 groundPoint = hit.point;
                Vector3 spawnPoint = groundPoint + Vector3.up * spawnHeight;

                GameObject m = Instantiate(meteorPrefab, spawnPoint, Quaternion.identity);

                if (m.TryGetComponent<Rigidbody>(out var rb))
                {
                    float speed = Random.Range(minDropSpeed, maxDropSpeed);
                    rb.velocity = Vector3.down * speed;
                }
                return;
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (targetCamera == null) return;
        var mid = targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 5f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(mid, Vector3.forward * 0.5f);
        Gizmos.DrawRay(mid, Vector3.right * 0.5f);
        Gizmos.DrawRay(mid, Vector3.up * 0.5f);
    }
#endif
}
