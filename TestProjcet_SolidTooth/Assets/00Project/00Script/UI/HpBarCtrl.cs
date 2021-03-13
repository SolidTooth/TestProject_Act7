using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HpBarCtrl : MonoBehaviour
{
    public HpCtrl targetHp;
    [SerializeField]
    private RectTransform barRect;
    [SerializeField]
    private Slider sliderBG;
    [SerializeField]
    private Slider sliderFT;

    private float bgDelay;
    private bool isBGSlide;

    private void Awake()
    {
        barRect = GetComponent<RectTransform>();
    }

    public void setTarget(HpCtrl newTargetHp)
    {
        targetHp = newTargetHp;
        resetBar();
        updatePivot();
    }
    public void removeTarget()
    {
        targetHp = null;
        HpBarManager.instance.pushHpBar(this);
    }
    public void setBarValue(float value)
    {
        sliderFT.value = value;
        bgDelay = 0.2f;
        if (isBGSlide == false)
        {
            isBGSlide = true;
            StartCoroutine(cor_SliderBg());
        }
    }
    public void resetBar()
    {
        isBGSlide = false;
        sliderBG.value = sliderFT.value = 1f;
    }
    private IEnumerator cor_SliderBg()
    {
        while (sliderFT.value < sliderBG.value && isBGSlide)
        {
            if (bgDelay > 0)
            {
                bgDelay -= Time.deltaTime;
            }
            else
            {
                sliderBG.value -= Time.deltaTime * 0.5f;
            }
            yield return null;
        }
        sliderBG.value = sliderFT.value;
        isBGSlide = false;
    }
    private void Update()
    {
        if (targetHp != null)
        {
            updatePivot();
        }
    }
    private void updatePivot()
    {
        //구글링하면 Camera.main.WorldToScreenPoint를 사용하는 방법이 있지만 Scene에서 직관적으로 다루는 방식을 사용;
        barRect.position = targetHp.HpBarPivot.position;
    }

}
