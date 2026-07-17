using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishSkillUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup fishSkillUI_CG;
    [SerializeField] private Image iconSkill;
    
    [Header("Cool Down Effect")]
    [SerializeField] private CanvasGroup coolDownCG;
    [SerializeField] private Image coolDownIcon;
    [SerializeField] private TMP_Text coolDownText;
    [SerializeField] private GameObject buttonTypeBox;
    
    private float _totalCoolDown;

    public void InitialazeSkill(Sprite iconSkillSprite)
    {
        if (iconSkillSprite == null)
        {
            Debug.LogError("IconSkillSprite is null");
            return;
        }
        
        fishSkillUI_CG.alpha = 1;
        iconSkill.sprite = iconSkillSprite;
    }
    
    public void StartCoolDown(float coolDownDuration)
    {
        _totalCoolDown = coolDownDuration;
        coolDownCG.alpha = 1;
        coolDownIcon.fillAmount = 1;
        buttonTypeBox.SetActive(false);
    }
    
    public void UpdateCoolDownUI(float remainingCoolDown)
    {
        if (remainingCoolDown <= 0)
        {
            coolDownCG.alpha = 0;
            coolDownIcon.fillAmount = 0;
            coolDownText.text = string.Empty;
            buttonTypeBox.SetActive(true);
            return;
        }

        coolDownIcon.fillAmount = _totalCoolDown > 0 
            ? remainingCoolDown / _totalCoolDown 
            : 0f;

        coolDownText.text = Mathf.CeilToInt(remainingCoolDown).ToString();
    }
}
