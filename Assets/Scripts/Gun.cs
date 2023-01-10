using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private AudioSource gunAudioPlayer;
    private LineRenderer bulletLineRenderer;
    private float timer = 0f;

    [SerializeField]
    private GunData gunData;
    public Transform gunBarrelEnd;
    public ParticleSystem gunParticles;
    public Light gunFlash;

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;

        gunFlash.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPause)
            return;

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Shot();
            timer = gunData.timeBetFire;
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        bulletLineRenderer.enabled = true;
        gunParticles.Play();
        gunFlash.enabled = true;

        gunAudioPlayer.PlayOneShot(gunData.shotClip);
        bulletLineRenderer.SetPosition(0, gunBarrelEnd.position);
        bulletLineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;
        gunFlash.enabled = false;
    }

    private void Shot()
    {
        Vector3 hitPos;
        Ray ray = new(gunBarrelEnd.position, gunBarrelEnd.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, gunData.fireDistance))
        {
            hitPos = hit.point;
            var collider = hit.collider.GetComponent<CapsuleCollider>();
            Zom target = hit.collider.GetComponent<Zom>();
            if (collider != null)
            {
                target.OnDamage(gunData.damage, hitPos, hit.normal);
                Debug.Log(collider.name);
            }
        }
        else
        {
            hitPos = gunBarrelEnd.position + gunBarrelEnd.forward * gunData.fireDistance;
        }
        StartCoroutine(ShotEffect(hitPos));
    }
}