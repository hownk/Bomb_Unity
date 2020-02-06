using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private ParticleSystem explosionParticle;   // 폭발 파티클
    private AudioSource explosionAudio;          // 폭발 효과음

    public float maxDamage = 100.0f;    // 데미지 (체력에 줄)
    public float explosionForce = 1000.0f;  // 폭발 힘
    public float explosionRadius = 20.0f;   // 폭발 반경

    public float lifeTime = 10.0f;  // 라이프 타임
    

    private void Start()
    {
        explosionParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        explosionAudio = transform.GetChild(0).GetComponent<AudioSource>();
        // 볼이 트리거에 닿지 않은 경우 lifeTime 뒤에 알아서 파괴 시켜줘야 한다.
        Destroy(gameObject, lifeTime);  
    }

    private void OnTriggerEnter(Collider other)
    {
        // 볼을 바로 Destroy 해버리면 자식도 같이 파괴되기 때문에
        // 자식으로 있는 파티클 시스템을 자식에서 미리 빼준다.
        explosionParticle.transform.parent = null;

        explosionParticle.Play();   // 파티클, 효과음 플레이
        explosionAudio.Play();

        // 파티클도 재생이 끝나면 파괴시킨다
        Destroy(explosionParticle.gameObject, explosionParticle.main.duration);
        Destroy(gameObject);    // 마지막으로 볼을 파괴
    }
}
