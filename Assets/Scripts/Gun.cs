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


    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� �Ѿ� ������ �׸���
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // ���� �������� Ȱ��ȭ�Ͽ� �Ѿ� ������ �׸���
        bulletLineRenderer.enabled = true;
        gunData.GunParticles.Play();
        //if (hit)
        //gunData.HitParticles.Play();

        gunAudioPlayer.PlayOneShot(gunData.shotClip);
        bulletLineRenderer.SetPosition(0, gunBarrelEnd.position);
        bulletLineRenderer.SetPosition(1, hitPosition);

        // 0.03�� ���� ��� ó���� ���
        yield return new WaitForSeconds(0.03f);

        // ���� �������� ��Ȱ��ȭ�Ͽ� �Ѿ� ������ �����
        bulletLineRenderer.enabled = false;
    }
}