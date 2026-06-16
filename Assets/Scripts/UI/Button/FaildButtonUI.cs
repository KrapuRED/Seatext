using UnityEngine;

public class FaildButtonUI : MonoBehaviour
{
    public void OnButtonClick()
    {
        GameEvents.OnButtonTypeBoxComplete.Invoke(ButtonTypeBoxContext.DoneExploreNode);

        //save Status Data
        GameEvents.OnSaveCurrentStatus.Invoke();
    }
}
