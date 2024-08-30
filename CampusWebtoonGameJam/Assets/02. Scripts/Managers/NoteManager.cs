using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : Singleton<NoteManager>
{
    public List<ChartData> charts;                      // 채보 리스트
    public Transform createNotePosition;                // 노트 생성 위치 
    
    public GameObject upNotePrefab;                     // 위쪽 노트 프리팹
    public GameObject downNotePrefab;                   // 아래쪽 노트 프리팹
    public GameObject leftNotePrefab;                   // 왼쪽 노트 프리팹
    public GameObject rightNotePrefab;                  // 오른쪽 노트 프리팹
    public float MusicStartTime { get; private set; }   // 음악이 시작한 시간 

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

        // 음악 재생
        AudioManager.Instance.PlayBGM(chart.music);
        
        // 음악 시작 시간 설정
        MusicStartTime = Time.time;
        
        // 노트 생성 시작
        StartCoroutine(GenerateNotes(chart.notes));
    }

    private IEnumerator GenerateNotes(List<NoteData> notes)
    {
        foreach (var note in notes)
        {
            // 대기 시간 계산 
            float delay = note.time - (Time.time - MusicStartTime);
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
        // 노트 타입에 맞는 노트 생성 
        switch (type)
        {
            case NoteType.Up:
                Instantiate(upNotePrefab, createNotePosition.position, Quaternion.identity);
                Debug.Log("위쪽 노트 생성");
                return;
            
            case NoteType.Down:
                Instantiate(downNotePrefab, createNotePosition.position, Quaternion.identity);
                Debug.Log("아래쪽 노트 생성");
                return;
            
            case NoteType.Left:
                Instantiate(leftNotePrefab, createNotePosition.position, Quaternion.identity);
                Debug.Log("왼쪽 노트 생성");
                return;
            
            case NoteType.Right:
                Instantiate(rightNotePrefab, createNotePosition.position, Quaternion.identity);
                Debug.Log("오른쪽 노트 생성");
                return;
            
            default:
                Debug.LogError("노트 타입을 식별할 수 없어 노트를 생성할 수 없습니다.");
                return;
        }
    }
}
