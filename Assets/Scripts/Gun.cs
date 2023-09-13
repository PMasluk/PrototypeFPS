using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float range = 100f;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private List<TargetMaterial> materialsToDamage = new List<TargetMaterial>();
    [SerializeField]
    private ParticleSystem shootFlash;
    [SerializeField]
    private AudioSource effectSource;
    [SerializeField]
    private AudioClip effect1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        shootFlash.Play();

        effectSource.PlayOneShot(effect1);

        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            if (!hit.transform.TryGetComponent<Target>(out var target))
            {
                return;
            }
            if (!materialsToDamage.Contains(target.Material))
            {
                return;
            }
                target.TakeDamage(damage);
        }
    }

}
