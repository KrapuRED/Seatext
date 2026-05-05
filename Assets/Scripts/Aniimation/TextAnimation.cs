using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    
    [Header("Word Animation")]
    [SerializeField] private float _speedAnimation;
    [SerializeField] private float _defaultScaleAnimation;
    [SerializeField] private float _maxScaleAnimation;
    
    private DOTweenTMPAnimator _tweener;

    public void RefreshText(string nexText)
    {
        _text.text = nexText;
        _tweener = new DOTweenTMPAnimator(_text);
    }
    
    public void OnPlayChangeTextAnimation(int indexChar)
    {
        if (indexChar < 0 || indexChar >= _tweener.textInfo.characterCount) return;
        
        Sequence seq = DOTween.Sequence();
        
        seq.Append(_tweener.DOScaleChar(indexChar, _maxScaleAnimation, _speedAnimation)
            .SetEase(Ease.OutBack));
        seq.Join(_tweener.DOColorChar(indexChar, Color.white, _speedAnimation)
        .SetEase(Ease.OutBack));
        
        seq.Append(_tweener.DOScaleChar(indexChar, _defaultScaleAnimation, _speedAnimation)
            .SetEase(Ease.OutBack)); 
    }
    
    public void OnWrongKey()
    {
        
    }
}
