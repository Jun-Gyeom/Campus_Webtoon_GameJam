using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : Singleton<NoteManager>
{
    public List<ChartData> charts;                      // 채보 리스트
    public Transform createNotePosition;                // 노트 생성 위치 
    public Transform judgmentLinePosition;              // 판정선 위치 
    
    public GameObject upNotePrefab;                     // 위쪽 노트 프리팹
    public GameObject downNotePrefab;                   // 아래쪽 노트 프리팹
    public GameObject leftNotePrefab;                   // 왼쪽 노트 프리팹
    public GameObject rightNotePrefab;                  // 오른쪽 노트 프리팹
    public float MusicStartTime { get; private set; }   // 음악이 시작한 시간 

    private float _distanceToJudgeLine;
    private float _noteSpeed;
    private float _noteTravelTime;
    private void Start()
    {
        // 노트가 이동할 거리 계산
        _distanceToJudgeLine = Vector3.Distance(createNotePosition.position, judgmentLinePosition.position);
    }

    // 채보 시작 메서드 
    public void StartChart(int chartID)
    {
        // 채보 찾기
        ChartData chart = charts.Find(chart => chart.chartID == chartID);
        if (chart == null)
        {
            Debug.LogError($"해당 ID의 채보가 존재하지 않습니다. : {chartID}");
            return;
        }
        
        // 게임오버 여부 초기화
        GameManager.Instance.IsGameOver = false;

        // 음악 재생
        AudioManager.Instance.PlayBGM(chart.music);
        
        // 음악 시작 시간 설정
        MusicStartTime = Time.time;
        
        // 노트 판정선까지 이동 시간 입력
        _noteTravelTime = chart.noteTravelTime;
        
        // 노트 속도 계산 
        _noteSpeed = _distanceToJudgeLine / chart.noteTravelTime;
        
        // 노트 생성 시작
        StartCoroutine(GenerateNotes(chart.notes));
    }

    private IEnumerator GenerateNotes(List<NoteData> notes)
    {
        foreach (var note in notes)
        {
            // 대기 시간 계산 (note.time에서 noteTravelTime을 뺀 값을 기준으로)
            float spawnTime = note.time - _noteTravelTime;
            float delay = spawnTime - (Time.time - MusicStartTime);
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }
            
            // 노트 생성
            SpawnNote(note.type);
        }
    }

    private void SpawnNote(NoteType type)
    {
        GameObject obj;
        // 노트 타입에 맞는 노트 생성 
        switch (type)
        {
            case NoteType.Up:
                obj = Instantiate(upNotePrefab, createNotePosition.position, Quaternion.identity);
                obj.GetComponent<Note>().type = NoteType.Up;
                                
                // 노트 이동 속도 전달 
                obj.GetComponent<Note>().noteSpeed = _noteSpeed;
                Debug.Log("위쪽 노트 생성");
                return;
            
            case NoteType.Down:
                obj = Instantiate(downNotePrefab, createNotePosition.position, Quaternion.identity);
                obj.GetComponent<Note>().type = NoteType.Down; 
                                
                // 노트 이동 속도 전달 
                obj.GetComponent<Note>().noteSpeed = _noteSpeed;
                Debug.Log("아래쪽 노트 생성");
                return;
            
            case NoteType.Left:
                obj = Instantiate(leftNotePrefab, createNotePosition.position, Quaternion.identity);
                obj.GetComponent<Note>().type = NoteType.Left; 
                                
                // 노트 이동 속도 전달 
                obj.GetComponent<Note>().noteSpeed = _noteSpeed;
                Debug.Log("왼쪽 노트 생성");
                return;
            
            case NoteType.Right:
                obj = Instantiate(rightNotePrefab, createNotePosition.position, Quaternion.identity);
                obj.GetComponent<Note>().type = NoteType.Right; 
                
                // 노트 이동 속도 전달 
                obj.GetComponent<Note>().noteSpeed = _noteSpeed;
                Debug.Log("오른쪽 노트 생성");
                return;
            
            default:
                Debug.LogError("노트 타입을 식별할 수 없어 노트를 생성할 수 없습니다.");
                return;
        }
    }
}
