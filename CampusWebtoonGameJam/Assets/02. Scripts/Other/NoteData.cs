using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteType
{
    None,
    Up,     // 위 노트
    Down,   // 아래 노트
    Left,   // 왼쪽 노트
    Right   // 오른쪽 노트
}

[Serializable]
public class NoteData
{
    public NoteType type;   // 노트 종류
    public float time;      // 노트가 판정선에 도달할 때의 음악 재생 시간(s)
}
