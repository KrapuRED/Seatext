using UnityEngine;

public class Fish : TypeBox
{
    [Header("Fish Config")]
    [SerializeField] private FishOS fishData;

    private void Start()
    {
        Debug.Log($"[Fish - Start] Fish Name : {fishData.fishName}");
        setTypeBoxEvent.Raise(this);
        SetTextToType(fishData.fishName);
    }

    public override void CheckingText(string typing)
    {
        bool isCorrectLetter = IsCorrectLetter(typing);
        Debug.Log($"[Fish - CheckingText] Is Correct Letter : {isCorrectLetter}");

        if (isCorrectLetter)
        {
            // Remove the correctly typed letter from the current text
            RemoveText();

            if (IsTextComplete())
            {
                Debug.Log($"[Fish - CheckingText] Text Is Done : {currentTextToType}");
            }
        }
    }
}
