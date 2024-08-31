using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgementUI : MonoBehaviour
{
    [SerializeField] private Slider feverGauge;
    [SerializeField] private GameObject[] HPObj;
    [SerializeField] private Image AccuracyStatusImg;
    [SerializeField] private Text ComboText; 

    [SerializeField] private Sprite [] AccuracyStatusSpr;

    Coroutine currentAccuracyCo;
    Coroutine currentFeverCo;
    private void OnEnable()
    {
        currentAccuracyCo = null;
    }
    public void ControlFeverGauge(float value, bool IsFeverTime)
    {
        // 올라가는거 feverTime구분 해야ㅐㄷ ㅁ
        //feverGauge.value = value;
        if(IsFeverTime)
            feverGauge.value = value;
        else
        {
            if (currentFeverCo != null) StopCoroutine(currentFeverCo); 
            currentFeverCo= StartCoroutine(SmoothControlGauge(value)); 
        }
    }
    IEnumerator SmoothControlGauge(float value)
    {
        while(feverGauge.value < value)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            feverGauge.value += 0.01f;
        }
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
    public void SetAccuracyStatusUI(AccuracyStatus status)
    {
        if (currentAccuracyCo != null) StopCoroutine(currentAccuracyCo); 
        currentAccuracyCo = StartCoroutine(ChangeAccuracyStatusUI(status));
    }
    IEnumerator ChangeAccuracyStatusUI(AccuracyStatus status)
    {
        Color co = AccuracyStatusImg.color;
        co.a = 0;
        AccuracyStatusImg.color = co;

        Color ComboCo = ComboText.color;
        ComboCo.a = 0;
        ComboText.color = ComboCo;
         
        switch (status)
        {
            case AccuracyStatus.perfect:
                AccuracyStatusImg.sprite = AccuracyStatusSpr[(int)AccuracyStatus.perfect];
                break;
            case AccuracyStatus.good:
                AccuracyStatusImg.sprite = AccuracyStatusSpr[(int)AccuracyStatus.good];
                break;
            case AccuracyStatus.bad:
                AccuracyStatusImg.sprite = AccuracyStatusSpr[(int)AccuracyStatus.bad];
                break;
            default:
                break;
        }

        while (AccuracyStatusImg.color.a<=1)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            co.a += 0.1f;
            AccuracyStatusImg.color = co; 

            ComboCo.a += 0.1f;
            ComboText.color = ComboCo;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        while (AccuracyStatusImg.color.a >= 0)
        {
            yield return new WaitForSecondsRealtime(0.02f);
            co.a -= 0.1f;
            AccuracyStatusImg.color = co;

            ComboCo.a -= 0.1f;
            ComboText.color = ComboCo;
        }
    }

}
