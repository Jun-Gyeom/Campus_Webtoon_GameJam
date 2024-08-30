using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgementUI : MonoBehaviour
{
    [SerializeField] private Slider feverGauge;
    [SerializeField] private GameObject[] HPObj;
    [SerializeField] private Text AccuracyStatusText;
    [SerializeField] private Text ComboText;

    public void ControlFeverGauge(float value)
    {
        feverGauge.value = value;
    }

    public void SetHPObj(int HP)
    {
        switch (HP)
        {
            case 5:
                HPObj[4].SetActive(true);
                break;
            case 4:
                HPObj[3].SetActive(true);
                HPObj[4].SetActive(false);
                break;
            case 3:
                HPObj[2].SetActive(true);
                HPObj[3].SetActive(false);
                break;
            case 2:
                HPObj[1].SetActive(true);
                HPObj[2].SetActive(false);
                break;
            case 1:
                HPObj[0].SetActive(true);
                HPObj[1].SetActive(false);
                break;
            case 0:
                HPObj[0].SetActive(false);
                break;
            default:
                break;

        }
    }

    public void SetComboText(int count)
    {
        ComboText.text = count.ToString();
    }
    public void SetAccuracyStatusText(AccuracyStatus status)
    {
        switch (status)
        {
            case AccuracyStatus.perfect:
                AccuracyStatusText.text = "perfect";
                break;
            case AccuracyStatus.good:
                AccuracyStatusText.text = "good";
                break;
            case AccuracyStatus.bad:
                AccuracyStatusText.text = "bad";
                break;
            default:
                break;
        }

    }
}
