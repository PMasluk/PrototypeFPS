using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    [SerializeField]
    private float durability = 50f;
    [SerializeField]
    private TargetMaterial material;
    [SerializeField]
    private MeshRenderer mesh;
    [SerializeField]
    private MeshCollider collider;
    [SerializeField]
    private Rigidbody rigidbody;
    [SerializeField]
    private ParticleSystem particle;

    public UnityEvent OnObjectDestroied;

    public TargetMaterial Material => material;

    public void TakeDamage(float amount)
    {
        durability -= amount;

        if (durability <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        OnObjectDestroied.Invoke();

        if (particle == null)
        {
            Destroy(gameObject);
            return;
        }
        
        mesh.enabled = false;
        collider.enabled = false;
        rigidbody.useGravity = false;
        particle.Play();
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(particle.main.duration);
        Destroy(gameObject);
    }
}
