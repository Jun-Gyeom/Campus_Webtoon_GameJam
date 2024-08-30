using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeJudgement : MonoBehaviour
{
    Transform node;
    Transform judgementLine; // 판정 위치 

    public float perfectTiming = 0.05f;
    public float goodTiming = 0.1f;

    bool nodeInputData = false; // 현재 노드에 대한 입력 값이 있었는지

    // 얘네는 게임 매니저에 있어야 겟지?
    int comboCount = 0;
    int HP = 5;
    float score = 0;
    float scoreMultiple = 1;
    bool IsFeverTime = false;
    float feverAmount = 0;
    float maxFeverAmount = 100;

    // 1. 현재 판정선에 노트가 있는지. (ontriggerEnter로 넣고, 입력 확인 변수 초기화  0
    // 2-1. 있다면 입력이 들어왔는지.                                               0
    // 2-2. 없다면 miss 처리                                                       0
    // 3-1. 노트가 없다면 miss 처리                                                0
    // 3-2. 노트가 있다면 perfect, great 정확도 판단,                               0
    //
    // combo count                                                                0
    //
    // UI 
    // 판정 정확도 (perfect, great, miss)
    // life
    // combo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        node = collision.transform;
        nodeInputData = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!nodeInputData)
        {
            AccuracyProcessing(AccuracyStatus.bad);
        }
        node = null;
        nodeInputData = false;
    }
    /// <summary>
    /// input 들어오면 호출하는 판정 요청 함수
    /// </summary>
    
    public void JudgementRequest()
    {
        if (node != null)
        {
            nodeInputData = true;
            AccuracyProcessing(AccuracyCalculation());
        }
    }
    /// <summary>
    /// 정확도 계산
    /// </summary>
    /// <returns></returns>
    AccuracyStatus AccuracyCalculation()
    {
        float distance = Mathf.Abs(node.position.x - judgementLine.position.x);

        AccuracyStatus value =
            distance <= perfectTiming ? AccuracyStatus.perfect :
            distance <= goodTiming ? AccuracyStatus.good :
            AccuracyStatus.bad;
        return value;
    }
    /// <summary>
    /// 정확도 처리
    /// </summary>
    /// <param name="status"></param>
    private void AccuracyProcessing(AccuracyStatus status)
    {
        // appear UI (perfect, great, miss
        switch (status)
        {
            case AccuracyStatus.perfect:
                score += 500 * scoreMultiple;
                comboCount++;
                feverAmount += maxFeverAmount / 10f;
                break;
            case AccuracyStatus.good:
                score += 100 * scoreMultiple;
                comboCount++;
                feverAmount += maxFeverAmount / 20f;
                break;
            case AccuracyStatus.bad:
                comboCount = 0;
                if (--HP == 0)
                { Debug.Log("GAME OVER"); }
                break;
            default:
                break;
        }
        if (feverAmount >= maxFeverAmount && !IsFeverTime)
        {
            Debug.Log("fever time");
            // fever 함수.. 코루틴..이던..
            // IsFeverTime = true;
            // scoreMultiple = 1.25f; 
            // 시간 카운트하고
            //
            // 끝나면
            // feverAmount = 0;
            // scoreMultiple = 1f; 
            // IsFeverTime = false; 
        }
    }
}
public enum AccuracyStatus
{
    perfect, good, bad
}