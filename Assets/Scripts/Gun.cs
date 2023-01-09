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

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            StartCoroutine(ShotEffect(Vector3.forward * gunData.fireDistance));
            timer = gunData.timeBetFire;
        }
    }


    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;
        gunData.GunParticles.Play();
        //if (hit)
        //gunData.HitParticles.Play();

        gunAudioPlayer.PlayOneShot(gunData.shotClip);
        bulletLineRenderer.SetPosition(0, gunBarrelEnd.position);
        bulletLineRenderer.SetPosition(1, hitPosition);

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }
}