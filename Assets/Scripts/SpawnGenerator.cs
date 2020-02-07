using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGenerator : MonoBehaviour
{
    public GameObject[] propPrefabs;    // 생성할 게임오브젝트

    private BoxCollider area;   // 생성할 구역

    public int count = 100; // 생성할 개수

    // 생성후에는 리스트로 관리
    private List<GameObject> props = new List<GameObject>();


    // 처음에만 박스콜라이더 가져오고 count만큼 Spawn() 함수 실행
    // 이후에 혹시 문제될까바 박스콜라이더는 비활성화 해 준다.
    private void Start()
    {
        area = GetComponent<BoxCollider>();

        for(int i = 0; i < count; i++)
        {
            Spawn();
        }

        area.enabled = false;
    }


    // 랜덤한 위치를 받아와서 Instantiate() 함수를 이용해
    // 프리팹을 생성 하고 리스트에 추가한다.
    private void Spawn()
    {
        int selection = Random.Range(0, propPrefabs.Length);

        GameObject selectedPrefab = propPrefabs[selection];

        Vector3 spawnPos = GetRandomPosition();

        GameObject instance =  Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        props.Add(instance);
    }


    // 박스 콜라이더 내의 랜덤한 위치 반환 함수
    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;
        Vector3 size = area.size;

        float xPos = basePosition.x + Random.Range(-size.x / 2, size.x / 2);
        float yPos = basePosition.y + Random.Range(-size.y / 2, size.y / 2);
        float zPos = basePosition.z + Random.Range(-size.z / 2, size.z / 2);

        Vector3 spawnPos = new Vector3(xPos, yPos, zPos);
        return spawnPos;
    }

    // 프랍들을 다 파괴하는게 아니고 비활성화 했다가 다시 활성화 하는 방식.
    // 그래서 Reset()을 하면 프랍들의 위치를 재설정하고
    // 꺼져있던 프랍들을 켜준다.
    public void Reset()
    {
        for(int i = 0; i < props.Count; i++)
        {
            props[i].transform.position = GetRandomPosition();
            props[i].SetActive(true);
        }
    }
}
