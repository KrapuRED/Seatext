using UnityEngine;

public interface IPanelDisplayable
{
    string DisplayName { get; }
    string DisplayDescription { get; }
    string DisplayFlow { get; }
    Sprite DisplayIconSprite { get; }
    Sprite DisplayEnviSprite { get; }
}
