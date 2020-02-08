using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public LayerMask whatIsProp;

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
        // Layer가 'Prop'인 콜라이더를 배열로 다 가져온다.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, whatIsProp);

        for(int i = 0; i < colliders.Length; i++)
        {
            // Rigidbody 컴포넌트 가져와서 AddExplosionforce()함수로 물리충격 가하기
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            // Prop 스크립트 가져와서 데미지 계산후 TakeDamage()호출
            Prop targetProp = colliders[i].GetComponent<Prop>();
            float damage = CalculateDamage(colliders[i].transform.position);
            targetProp.TakeDamage(damage);
        }

        // 볼을 바로 Destroy 해버리면 자식도 같이 파괴되기 때문에
        // 자식으로 있는 파티클 시스템을 자식에서 미리 빼준다.
        explosionParticle.transform.parent = null;

        explosionParticle.Play();   // 파티클, 효과음 플레이
        explosionAudio.Play();

        GameManager.instance.OnBallDestroy();   // 게임매니저에 볼 파괴됬다고 알려주기

        // 파티클도 재생이 끝나면 파괴시킨다
        Destroy(explosionParticle.gameObject, explosionParticle.main.duration);
        Destroy(gameObject);    // 마지막으로 볼을 파괴
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // 폭탄으로부터 프랍까지의 거리
        float distance = (targetPosition - transform.position).magnitude;

        // 폭탄과 프랍의 거리가 같으면 1 가장자리이면 0
        float percentage = (explosionRadius - distance) / explosionRadius;

        // 폭탄으로부터의 거리비에 따라 데미지 변환
        float damage =  maxDamage * percentage;

        // 혹시 가상의 구 밖에있는데 걸린 프랍은 음수값이 들어가지 않도록 0으로 만들어 주기.
        damage = Mathf.Max(damage, 0);

        return damage;
    }
}
