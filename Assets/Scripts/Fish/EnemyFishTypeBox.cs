using UnityEngine;

public class EnemyFishTypeBox : TypeBox
{
    [Header("EnemyFishTypeBox Config")]
    [SerializeField] private FishTextUI fishUI;

    public override void SetTextToType(string text)
    {
        base.SetTextToType(text);
        fishUI.SetFishWordText(currentTextToType);
    }

    public override bool CheckingText(string typing)
    {
        bool isCorrectLetter = IsCorrectLetter(typing);
        Debug.Log($"[Fish - CheckingText] Is Correct Letter : {isCorrectLetter}");

        if (isCorrectLetter)
        {
            // Remove the correctly typed letter from the current text
            _isStillMacthing = true;
            RemoveText();

            if (IsTextComplete())
            {
                Debug.Log($"[Fish - CheckingText] Text Is Done : {currentTextToType}");
                TypeBoxManager.instance.RemoveTypeBox(this);
                Destroy(gameObject);
            }

            // Update the UI with the remaining text
            fishUI.SetFishWordText(remainingTypedText);
        }
        else
        {
            Debug.Log($"[Fish - CheckingText] Wrong Letter! Typed : {typing}, Expected : {remainingTypedText[0]}");
            _isStillMacthing = false;
        }

        return isCorrectLetter;
    }

    public override void ResetTypeBox()
    {
        SetTextToType(currentTextToType);
    }
}
