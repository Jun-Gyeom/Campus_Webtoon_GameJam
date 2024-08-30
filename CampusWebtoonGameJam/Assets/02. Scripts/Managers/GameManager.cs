using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float Score { get; set; }        // 현재 게임의 점수
    public float HighScore { get; set; }    // 최고 점수
    public int Health { get; set; }         // 현재 HP
    public int MaxHealth { get; set; }      // 최대 HP

    // 위쪽 노트 입력 
    public void OnUpNoteInput()
    {
        
    }

    // 아래쪽 노트 입력 
    public void OnDownNoteInput()
    {
        
    }

    // 왼쪽 노트 입력 
    public void OnLeftNoteInput()
    {
        
    }

    // 오른쪽 노트 입력 
    public void OnRightNoteInput()
    {
        
    }
}
