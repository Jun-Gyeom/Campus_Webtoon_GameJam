using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoteJudgement : MonoBehaviour
{
    [SerializeField] private JudgementUI judgementUI;

    Transform note;
    [SerializeField] private Transform judgementLine; // 판정 위치 
    [SerializeField] private Transform perfectTimingLine; // 판정 위치 

    float perfectScope; // perfect 범위
    float goodScope; // good 범위

    bool noteInputData = false; // 현재 노드에 대한 입력 값이 있었는지

    // 얘네는 게임 매니저에 있어야 겟지?
    int comboCount = 0;
    int HP = 5;
    float score = 0;
    float scoreMultiple = 1;
    bool IsFeverTime = false;
    float feverAmount = 0;
    float maxFeverAmount = 100;

    private void Awake()
    {
        perfectScope = perfectTimingLine.GetComponent<BoxCollider2D>().size.x / 2;
        goodScope = judgementLine.GetComponent<BoxCollider2D>().size.x / 2;
        Debug.Log(perfectScope);
    }
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
        Debug.Log("얘ㅔ에에");
        note = collision.transform;
        noteInputData = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("워어");
        if (!noteInputData)
        {
            AccuracyProcessing(AccuracyStatus.bad);
        }
        note = null;
        noteInputData = false;
    }
    /// <summary>
    /// input 들어오면 호출하는 판정 요청 함수
    /// </summary>
    public void JudgementRequest(NoteType noteType)
    {
        if (note != null)
        {
            /*if (noteType == note.GetComponent<Note>().noteType)
            {
                nodeInputData = true;
                AccuracyProcessing(AccuracyCalculation());

            }
            else
            {

                Destroy(note.gameObject);
                AccuracyProcessing(AccuracyStatus.bad);
            }*/
        }
    }
    /// <summary>
    /// 정확도 계산
    /// </summary>
    /// <returns></returns>
    AccuracyStatus AccuracyCalculation()
    {
        float distance = Mathf.Abs(note.position.x - judgementLine.position.x);

        AccuracyStatus value =
            distance <= perfectScope ? AccuracyStatus.perfect :
            distance <= goodScope ? AccuracyStatus.good :
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
                SetComboCount(1);
                if (HP < 5) HP++;

                judgementUI.SetAccuracyStatusText(AccuracyStatus.perfect);
                judgementUI.SetHPObj(HP);
                feverAmount += maxFeverAmount / 10f;
                break;
            case AccuracyStatus.good:

                score += 100 * scoreMultiple;
                SetComboCount(1);

                judgementUI.SetAccuracyStatusText(AccuracyStatus.good);
                feverAmount += maxFeverAmount / 20f;
                break;
            case AccuracyStatus.bad:

                SetComboCount(-comboCount);
                if (--HP == 0)
                { Debug.Log("GAME OVER"); }

                judgementUI.SetAccuracyStatusText(AccuracyStatus.bad);
                judgementUI.SetHPObj(HP);
                break;
            default:
                break;
        }
        if (feverAmount >= maxFeverAmount && !IsFeverTime)
        {
            Debug.Log("fever time");
            StartCoroutine(FeverTime());
        }
    }

    void SetComboCount(int changevalue)
    {
        comboCount += changevalue;
        judgementUI.SetComboText(comboCount);
    }
    IEnumerator FeverTime()
    {

        IsFeverTime = true;
        scoreMultiple = 1.25f;

        float maxFeverTime = 5f;
        float feverTime = maxFeverTime;
        while (feverTime > 0)
        {
            yield return new WaitForSecondsRealtime(0.02f);
            feverTime -= 0.02f;
            judgementUI.ControlFeverGauge(feverTime / maxFeverTime);
        }

        feverAmount = 0;
        scoreMultiple = 1f;
        IsFeverTime = false;

    }
}
public enum AccuracyStatus
{
    perfect, good, bad
}