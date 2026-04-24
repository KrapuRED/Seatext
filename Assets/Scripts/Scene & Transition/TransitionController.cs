using UnityEngine;

public class TransitionController : MonoBehaviour
{
    public static TransitionController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionScene(string transiitonName)
    {

    }
}
