using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private AudioSource gunAudioPlayer;
    private float timer = 0f;

    [SerializeField]
    private GunData gunData;
    public LineRenderer singleShotLineRender;
    public LineRenderer multiShotLineRender;
    public Transform gunBarrelEnd;
    public ParticleSystem gunParticles;
    public Light gunFlash;

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();

        singleShotLineRender.positionCount = 2;
        singleShotLineRender.enabled = false;

        multiShotLineRender.positionCount = 2;
        multiShotLineRender.enabled = false;
        multiShotLineRender.endWidth = 10f;

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
            SingleShot();
            timer = gunData.timeBetFire;
        }

        if (Input.GetMouseButton(1))
        {
            MultiShot();
            timer = gunData.timeBetFire * 15;
        }
    }

    private IEnumerator SingleShotEffect(Vector3 hitPosition)
    {
        singleShotLineRender.enabled = true;
        gunParticles.Play();
        gunFlash.enabled = true;

        gunAudioPlayer.PlayOneShot(gunData.shotClip);
        singleShotLineRender.SetPosition(0, gunBarrelEnd.position);
        singleShotLineRender.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.03f);
        singleShotLineRender.enabled = false;
        gunFlash.enabled = false;
    }

    private IEnumerator MultiShotEffect(Vector3 hitPosition)
    {
        multiShotLineRender.enabled = true;
        gunParticles.Play();
        gunFlash.enabled = true;

        gunAudioPlayer.PlayOneShot(gunData.shotClip);
        multiShotLineRender.SetPosition(0, gunBarrelEnd.position);
        multiShotLineRender.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.03f);
        multiShotLineRender.enabled = false;
        gunFlash.enabled = false;
    }

    private void SingleShot()
    {
        Vector3 hitPos;
        Ray ray = new(gunBarrelEnd.position, gunBarrelEnd.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, gunData.fireDistance))
        {
            hitPos = hit.point;
            var collider = hit.collider.GetComponent<CapsuleCollider>();
            Zom target = hit.collider.GetComponent<Zom>();
            if (collider != null && target != null)
            {
                target.OnDamage(gunData.damage, hitPos, hit.normal);
                Debug.Log(collider.name);
            }
        }
        else
        {
            hitPos = gunBarrelEnd.position + gunBarrelEnd.forward * gunData.fireDistance;
        }
        StartCoroutine(SingleShotEffect(hitPos));
    }

    private void MultiShot()
    {
        Debug.Log(multiShotLineRender.endWidth);

        Vector3 hitPos;
        Ray ray = new(gunBarrelEnd.position, gunBarrelEnd.forward);
        hitPos = gunBarrelEnd.position + gunBarrelEnd.forward * gunData.multiDistance;
        if (Physics.Raycast(ray, out RaycastHit hit, gunData.multiDistance))
        {
            var collider = hit.collider.GetComponent<CapsuleCollider>();
            Zom target = hit.collider.GetComponent<Zom>();
            if (collider != null && target != null)
            {
                target.OnDamage(gunData.damage, hitPos, hit.normal);
                Debug.Log(collider.name);
            }
        }
        StartCoroutine(MultiShotEffect(hitPos));
    }
}