using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/GunData", fileName = "GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public ParticleSystem GunParticles;
    public ParticleSystem HitParticles;
    public float damage = 25;
    public float fireDistance = 50f;
    public float timeBetFire = 0.12f;
}