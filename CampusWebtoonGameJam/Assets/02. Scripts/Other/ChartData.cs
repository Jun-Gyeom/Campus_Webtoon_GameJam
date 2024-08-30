using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ChartData")]
public class ChartData : ScriptableObject
{
    [Header("채보 ID (식별자)")]
    public int chartID;             // 채보 식별자
    
    [Header("채보에 맞는 음악 이름")]
    public string music;            // 해당 채보의 음악 이름
    
    [Header("노트 생성 후, 판정선까지 이동하는 데에 걸리는 시간")]
    public float noteTravelTime;    // 노트 생성 후, 판정선까지 이동하는 데에 걸리는 시간
    
    [Header("노트 리스트")]
    public List<NoteData> notes;    // 노트 리스트
}
