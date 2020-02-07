using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public ParticleSystem explosionParticle;    // 파괴시 파티클

    public int score = 5;      // 파괴했을 때 점수
    public float hp = 10.0f;    // 체력

    
    public void TakeDamage(float damage) // 외부에서 이 함수를 통해 프롭에 데미지를 줄 것이다.
    {
        hp -= damage;

        if(hp <= 0)
        {
            // 파티클을 Instantiate로 생성 후 재생이 끝나면 파괴
            ParticleSystem instance = Instantiate(explosionParticle, transform.position, transform.rotation);
            instance.Play();

            // 파티클 오브젝트에 붙어있던 오디오도 가져와서 플레이
            AudioSource explosionAudio = instance.GetComponent<AudioSource>();
            explosionAudio.Play();

            Destroy(instance.gameObject, instance.main.duration);

            // 스테이지마다 프롭들을 생산 하고 파괴하고 하면 비용이 크니까 비활성화 했다가 다시 활성화 하는식으로 하자.
            gameObject.SetActive(false);
        }
    }
}
