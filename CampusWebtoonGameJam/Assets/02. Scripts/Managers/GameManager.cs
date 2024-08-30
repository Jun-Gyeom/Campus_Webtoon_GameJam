using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float Score { get; set; }        // 현재 게임의 점수
    public float HighScore { get; set; }    // 최고 점수
    public int Health { get; set; }         // 현재 HP
    public int MaxHealth { get; set; }      // 최대 HP

    private void OnEnable()
    {
        InputManager.OnUp += OnUpNoteInput;
        InputManager.OnDown += OnDownNoteInput;
        InputManager.OnLeft += OnLeftNoteInput;
        InputManager.OnRight += OnRightNoteInput;
    }

    private void OnDisable()
    {
        InputManager.OnUp -= OnUpNoteInput;
        InputManager.OnDown -= OnDownNoteInput;
        InputManager.OnLeft -= OnLeftNoteInput;
        InputManager.OnRight -= OnRightNoteInput;
    }

    // 위쪽 노트 입력 
    public void OnUpNoteInput()
    {
        Debug.Log("위!");
        NoteJudgement.Instance.JudgementRequest(NoteType.Up);
    }

    // 아래쪽 노트 입력 
    public void OnDownNoteInput()
    {
        Debug.Log("아래!");
        NoteJudgement.Instance.JudgementRequest(NoteType.Down);
    }

    // 왼쪽 노트 입력 
    public void OnLeftNoteInput()
    {
        Debug.Log("왼쪽!");
        NoteJudgement.Instance.JudgementRequest(NoteType.Left);
    }

    // 오른쪽 노트 입력 
    public void OnRightNoteInput()
    {
        Debug.Log("오른쪽!");
        NoteJudgement.Instance.JudgementRequest(NoteType.Right);
    }
}
