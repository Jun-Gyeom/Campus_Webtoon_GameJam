using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneName
{
    Main,
    Game
}
public class SceneController : Singleton<SceneController>
{
    public bool IsSceneChanging { get; private set; }            // 씬 전환 중인지 여부 
    public event Action OnFadeComplate;               // 씬 전환 페이드아웃 종료시 실행되는 이벤트  
    
    [Header("Settings")]
    public float fadeInOutDuration;             // 페이드인 페이드아웃에 걸리는 시간 
    public float fadingDuration;                // 페이드인 된 채로 대기하는 시간 
    
    [Header("Object")]
    public Image fadeImage;                     // 페이드인 페이드아웃에 사용할 이미지
    public GameObject fadeImageGameObject;
    public GameObject clearPanelGameObject;     // 클리어 패널 게임오브젝트
    public GameObject gameOverPanelGameObject;  // 게임오버 패널 게임오브젝트
    
    private WaitForSeconds _waitForFadeInOut;
    private WaitForSeconds _waitForFading;
    
    private float _animationDuration = 0.5f; // 애니메이션 지속 시간
    private Vector3 _initialScale = new Vector3(0.1f, 0.1f, 0.1f);
    private bool _isAnimating = false; // 애니메이션 진행 여부 확인
    private bool _isClearPanelAnimating;    // 클리어 패널 애니메이션 진행 여부 
    private bool _isQuiting = false;    // 종료 중인지 여부 확인
    
    private void Start()
    {
        _waitForFadeInOut = new WaitForSeconds(fadeInOutDuration);
        _waitForFading = new WaitForSeconds(fadingDuration);
    }
    
    public void ChangeScene(SceneName sceneName)
    {
        if (IsSceneChanging) return;
        
        // 배경음악 중지
        AudioManager.Instance.StopBGM();

        StartCoroutine(HandleSceneChange(sceneName));
    }
    
    // 씬 전환 코루틴 
    private IEnumerator HandleSceneChange(SceneName sceneName)
    {
        IsSceneChanging = true;

        // 페이드인
        FadeIn();
        yield return _waitForFadeInOut;

        // 씬 비동기 로딩 
        yield return StartCoroutine(LoadSceneAsync(sceneName));
        
        // 현재 씬 변경 입력
        GameManager.Instance.CurrentScene = sceneName;
        
        // 대기
        yield return _waitForFading;

        // 페이드아웃
        FadeOut();
        yield return _waitForFadeInOut;
        
        // 콜백 이벤트 실행 
        OnFadeComplate?.Invoke();
        OnFadeComplate = null;
        
        IsSceneChanging = false;
    }
    

    private IEnumerator LoadSceneAsync(SceneName sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneName);
        if (asyncOperation == null)
        {
            Debug.LogError($"존재하지 않는 씬 번호입니다. : {(int)sceneName}");
        }
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }

    public void FadeIn()
    {
        // 알파 값을 1으로 서서히 변경
        fadeImageGameObject.SetActive(true);
        fadeImage.DOFade(1f, fadeInOutDuration); 
    }

    public void FadeOut()
    {
        // 알파 값을 0로 서서히 변경
        fadeImage.DOFade(0f, fadeInOutDuration).OnComplete(() =>
        {
            fadeImageGameObject.SetActive(false);
        });  
    }

    public void ShowClearPanel()
    {
        // 애니메이션이 진행 중이면 메서드를 종료
        if (_isClearPanelAnimating)
            return;
        
        _isClearPanelAnimating = true; // 애니메이션 시작 표시

        clearPanelGameObject.SetActive(true);
        clearPanelGameObject.transform.localScale = _initialScale; // 크기를 작은 상태로 초기화
        clearPanelGameObject.transform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            _isClearPanelAnimating = false; // 애니메이션 종료 표시
        });
    }

    public void HideClearPanel()
    {
        // 애니메이션이 진행 중이면 메서드를 종료
        if (_isClearPanelAnimating)
            return;
        
        // 버튼 효과음 재생
        AudioManager.Instance.PlaySFX("Sounds_SFX_Button");

        _isClearPanelAnimating = true; // 애니메이션 시작 표시

        clearPanelGameObject.transform.DOScale(_initialScale, _animationDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            clearPanelGameObject.SetActive(false);
            _isClearPanelAnimating = false; // 애니메이션 종료 표시
        });
    }

    public void ShowGameOverPanel()
    {
        // 애니메이션이 진행 중이면 메서드를 종료
        if (_isAnimating)
            return;

        _isAnimating = true; // 애니메이션 시작 표시

        gameOverPanelGameObject.SetActive(true);
        gameOverPanelGameObject.transform.localScale = _initialScale; // 크기를 작은 상태로 초기화
        gameOverPanelGameObject.transform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            _isAnimating = false; // 애니메이션 종료 표시
        });
    }

    public void HideGameOverPanel()
    {
        // 애니메이션이 진행 중이면 메서드를 종료
        if (_isAnimating)
            return;
        
        // 버튼 효과음 재생
        AudioManager.Instance.PlaySFX("Sounds_SFX_Button");

        _isAnimating = true; // 애니메이션 시작 표시

        gameOverPanelGameObject.transform.DOScale(_initialScale, _animationDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameOverPanelGameObject.SetActive(false);
            _isAnimating = false; // 애니메이션 종료 표시
        });
    }

    // 메인으로 이동
    public void ToMain()
    {
        // 게임 클리어 패널 숨기기
        HideClearPanel();
        
        // 메인 씬으로 이동 
        ChangeScene(SceneName.Main);
    }
    
    // 다시 하기
    public void ReTry()
    {
        // 게임오버 패널 숨기기 
        HideGameOverPanel();
        
        // 게임 클리어 패널 숨기기
        HideClearPanel();
        
        NoteJudgement.Instance.Init();
        // 게임 재시작
        ChangeScene(SceneName.Game);
        OnFadeComplate += () => NoteManager.Instance.StartChart(0);
    }
}
