using UnityEngine;
using UnityEngine.UI;

public class ScalingTypeBoxUI : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutTypeBoxUI;
    public void SetScalengTypeBoxUI(Vector2 preferredSize)
    {
        gridLayoutTypeBoxUI.cellSize = new Vector2(preferredSize.x , preferredSize.y / 2);
    }
}
