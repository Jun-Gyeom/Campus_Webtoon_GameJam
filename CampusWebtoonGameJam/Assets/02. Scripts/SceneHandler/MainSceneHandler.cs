using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainSceneHandler : MonoBehaviour
{
    public GameObject howPanel; // 어떻게 하는거냥? 패널
    
    private float _animationDuration = 0.5f; // 애니메이션 지속 시간
    private Vector3 _initialScale = new Vector3(0.1f, 0.1f, 0.1f);
    private bool _isAnimating = false; // 애니메이션 진행 여부 확인
    
    void Start()
    {
        howPanel.transform.localScale = _initialScale;
        howPanel.SetActive(false);
    }
    public void GameStart()
    {
        // 게임 씬으로 이동
        SceneController.Instance.ChangeScene(SceneName.Game);
        
        // 이벤트에 채보 시작 등록
        SceneController.Instance.OnFadeComplate += () => NoteManager.Instance.StartChart(0);    // 현재 개발 목표는 한 곡이므로 정적인 식별자 사용
    }

    public void ShowHowPanel()
    {
        // 애니메이션이 진행 중이면 메서드를 종료
        if (_isAnimating)
            return;

        _isAnimating = true; // 애니메이션 시작 표시

        howPanel.SetActive(true);
        howPanel.transform.localScale = _initialScale; // 크기를 작은 상태로 초기화
        howPanel.transform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            _isAnimating = false; // 애니메이션 종료 표시
        });
    }

    public void HideHowPanel()
    {
        // 애니메이션이 진행 중이면 메서드를 종료
        if (_isAnimating)
            return;

        _isAnimating = true; // 애니메이션 시작 표시

        howPanel.transform.DOScale(_initialScale, _animationDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            howPanel.SetActive(false);
            _isAnimating = false; // 애니메이션 종료 표시
        });
    }

    public void QuitGame()
    {
        StartCoroutine(HandleQuit());
    }

    private IEnumerator HandleQuit()
    {
        // 페이드인
        SceneController.Instance.FadeIn();
        yield return new WaitForSeconds(SceneController.Instance.fadeInOutDuration);
        
        // 게임 종료
        Application.Quit();
    }
}
