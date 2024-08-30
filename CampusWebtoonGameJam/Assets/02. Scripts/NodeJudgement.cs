using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeJudgement : MonoBehaviour
{
    Transform node;
    Transform judgementLine; // ���� ��ġ 

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
    /// input ������ ȣ���ϴ� ���� ��û �Լ�
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
    /// ��Ȯ�� ���
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
            // fever �Լ�.. �ڷ�ƾ..�̴�..
            // IsFeverTime = true;
            // scoreMultiple = 1.25f; 
            // �ð� ī��Ʈ�ϰ�
            //
            // ������
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