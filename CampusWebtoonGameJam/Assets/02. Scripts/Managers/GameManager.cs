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
    
    public bool IsGameOver { get; set; }    // 게임오버 여부 
    public SceneName CurrentScene { get; set; } // 현재 씬 

    [Header("Character")] public Animator characterAnimator;    // 캐릭터 애니메이터 (아빠, 냐미) 
    public bool isDad = false;      // 아빠인지 여부 
    
    [Header("Input Effect")]
    public GameObject inputEffectPrefab;    // 입력 이펙트 프리팹을 할당할 변수
    public Transform spawnPoint;            // 이펙트가 생성될 위치

    private void Update()
    {
        print(IsGameOver);
    }

    private void OnEnable()
    {
        MaxHealth = 5;
        Health = MaxHealth;

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
        if (CurrentScene != SceneName.Game)
        {
            return;
        }

        if (IsGameOver)
        {
            return;
        }
        
        Debug.Log("위!");
        NoteJudgement.Instance.JudgementRequest(NoteType.Up);
        SpawnEffect();

        DanceAnimationPlay(NoteType.Up);
    }

    // 아래쪽 노트 입력 
    public void OnDownNoteInput()
    {
        if (CurrentScene != SceneName.Game)
        {
            return;
        }

        if (IsGameOver)
        {
            return;
        }


        
        Debug.Log("아래!");
        NoteJudgement.Instance.JudgementRequest(NoteType.Down);
        SpawnEffect();
        
        DanceAnimationPlay(NoteType.Down);
    }

    // 왼쪽 노트 입력 
    public void OnLeftNoteInput()
    {
        if (CurrentScene != SceneName.Game)
        {
            return;
        }

        if (IsGameOver)
        {
            return;
        }
        
        Debug.Log("왼쪽!");
        NoteJudgement.Instance.JudgementRequest(NoteType.Left);
        SpawnEffect();
        
        DanceAnimationPlay(NoteType.Left);
    }

    // 오른쪽 노트 입력 
    public void OnRightNoteInput()
    {
        if (CurrentScene != SceneName.Game)
        {
            return;
        }

        if (IsGameOver)
        {
            return;
        }
        
        Debug.Log("오른쪽!");
        NoteJudgement.Instance.JudgementRequest(NoteType.Right);
        SpawnEffect();
        
        DanceAnimationPlay(NoteType.Right);
    }

    private void DanceAnimationPlay(NoteType type)
    {
        if (characterAnimator == null)
        {
            characterAnimator = GameObject.Find("CH").GetComponent<Animator>();
        }
        
        if (isDad)
        {
            switch (type)
            {
                case NoteType.Up:
                    characterAnimator.Play("Dad_Up");
                    return; 
                case NoteType.Down:
                    characterAnimator.Play("Dad_Down");
                    return; 
                case NoteType.Left:
                    characterAnimator.Play("Dad_Left");
                    return; 
                case NoteType.Right:
                    characterAnimator.Play("Dad_Right");
                    return; 
            }
        }
        else
        {
            switch (type)
            {
                case NoteType.Up:
                    characterAnimator.Play("NM_Up");
                    return; 
                case NoteType.Down:
                    characterAnimator.Play("NM_Down");
                    return; 
                case NoteType.Left:
                    characterAnimator.Play("NM_Left");
                    return; 
                case NoteType.Right:
                    characterAnimator.Play("NM_Right");
                    return; 
            }
        }
        
        Invoke("IdleAnimationPlay", 1f);
    }

    private void IdleAnimationPlay()
    {
        if (characterAnimator == null)
        {
            characterAnimator = GameObject.Find("CH").GetComponent<Animator>();
        }
        
        if (isDad)
        {
            characterAnimator.Play("Dad_Idle");
        }
        else
        {
            characterAnimator.Play("NM_Idle");
        }
    }
    
    // 게임 클리어
    public void GameClear()
    {
        // 음악 끄기
        AudioManager.Instance.StopBGM();
        
        // 스코어 표시
        
        // 효과음 재생
        AudioManager.Instance.PlaySFX("Sounds_SFX_Clear");
        
        // 게임 클리어 패널 띄우기
        SceneController.Instance.ShowClearPanel();
    }

    // 게임 오버
    public void GameOver()
    {
        // 이미 게임오버라면 메서드를 종료 
        if (IsGameOver)
        {
            return;
        }
        
        // 게임오버 처리
        Debug.Log("게임 오버");
        IsGameOver = true;
        
        // 음악 끄기
        AudioManager.Instance.StopBGM();
        
        // 효과음 재생 
        AudioManager.Instance.PlaySFX("Sounds_SFX_GameOver");

        // 게임오버 패널 띄우기 
        SceneController.Instance.ShowGameOverPanel();
    }

    // 입력 이펙트 생성 
    private void SpawnEffect()
    {
        // 이펙트 프리팹을 현재 위치에 생성
        GameObject newEffect = Instantiate(inputEffectPrefab, spawnPoint.position, Quaternion.identity);

        // 애니메이션 길이만큼 대기 후 이펙트 객체 삭제
        Animator animator = newEffect.GetComponent<Animator>();
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(newEffect, animationLength);
    }
}
