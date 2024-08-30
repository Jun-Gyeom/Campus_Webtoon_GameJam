using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeJudgement : MonoBehaviour
{
    [SerializeField] private JudgementUI judgementUI;

    Transform note;
    [SerializeField] private Transform judgementLine; // ���� ��ġ 

    public float perfectTiming = 0.05f;
    public float goodTiming = 0.1f;

    bool nodeInputData = false; // ���� ��忡 ���� �Է� ���� �־�����

    // ��״� ���� �Ŵ����� �־�� ����?
    int comboCount = 0;
    int HP = 5;
    float score = 0;
    float scoreMultiple = 1;
    bool IsFeverTime = false;
    float feverAmount = 0;
    float maxFeverAmount = 100;

    // 1. ���� �������� ��Ʈ�� �ִ���. (ontriggerEnter�� �ְ�, �Է� Ȯ�� ���� �ʱ�ȭ  0
    // 2-1. �ִٸ� �Է��� ���Դ���.                                               0
    // 2-2. ���ٸ� miss ó��                                                       0
    // 3-1. ��Ʈ�� ���ٸ� miss ó��                                                0
    // 3-2. ��Ʈ�� �ִٸ� perfect, great ��Ȯ�� �Ǵ�,                               0
    //
    // combo count                                                                0
    //
    // UI 
    // ���� ��Ȯ�� (perfect, great, miss)
    // life
    // combo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("��Ŀ���");
        note = collision.transform;
        nodeInputData = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("����");
        if (!nodeInputData)
        {
            AccuracyProcessing(AccuracyStatus.bad);
        }
        note = null;
        nodeInputData = false;
    }
    /// <summary>
    /// input ������ ȣ���ϴ� ���� ��û �Լ�
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
    /// ��Ȯ�� ���
    /// </summary>
    /// <returns></returns>
    AccuracyStatus AccuracyCalculation()
    {
        float distance = Mathf.Abs(note.position.x - judgementLine.position.x);

        AccuracyStatus value =
            distance <= perfectTiming ? AccuracyStatus.perfect :
            distance <= goodTiming ? AccuracyStatus.good :
            AccuracyStatus.bad;
        return value;
    }
    /// <summary>
    /// ��Ȯ�� ó��
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
        while(feverTime>0)
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