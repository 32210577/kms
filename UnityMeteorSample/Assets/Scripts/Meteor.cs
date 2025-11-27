
using UnityEngine;

public interface IDamageable
{
    void ApplyDamage(float amount, Vector3 hitPoint);
}

public class Meteor : MonoBehaviour
{
    [Header("Damage")]
    public float damage = 30f;
    public float radius = 3.5f;
    public LayerMask hitMask; // Layers that should receive damage

    [Header("FX")]
    public GameObject impactVfx;
    public AudioClip impactSfx;

    [Header("Cleanup")]
    public float destroyAfterImpact = 3f;

    bool impacted = false;

    void OnCollisionEnter(Collision collision)
    {
        if (impacted) return;
        impacted = true;

        Vector3 point = collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position;

        // Area damage
        Collider[] cols = Physics.OverlapSphere(point, radius, hitMask, QueryTriggerInteraction.Ignore);
        foreach (var c in cols)
        {
            if (c.TryGetComponent<IDamageable>(out var dmg))
            {
                dmg.ApplyDamage(damage, point);
            }
        }

        // VFX/SFX
        if (impactVfx) Instantiate(impactVfx, point, Quaternion.identity);
        if (impactSfx) AudioSource.PlayClipAtPoint(impactSfx, point);

        // Optionally freeze meteor mesh
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        Destroy(gameObject, destroyAfterImpact);
    }
}
