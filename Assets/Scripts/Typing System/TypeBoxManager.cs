using UnityEngine;
using System.Collections.Generic;
using TreeEditor;

public enum TypeTypingBox
{
    none,
    UI,
    GamePlay
}

[System.Serializable]
public class TypingBoxByType
{
    public TypeTypingBox type;
    public List<TypingBox> typingBoxes;

    public TypingBoxByType(TypeTypingBox type)
    {
        this.type = type;
        typingBoxes = new List<TypingBox>();
    }
}

public class TypeBoxManager : MonoBehaviour
{
    public static TypeBoxManager instance;

    [SerializeField] private TypeTypingBox _currentTypingMode;
    [SerializeField] protected List<TypingBoxByType> _activeTypingBoxs = new List<TypingBoxByType>();

    [Header("Events")]
    [SerializeField] protected SetTypeBoxEventSO _setTypeBoxEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Initialize the dictionary with empty lists for each TypeTypingBox
        foreach (TypeTypingBox type in System.Enum.GetValues(typeof(TypeTypingBox)))
        {
            _activeTypingBoxs.Add(new TypingBoxByType(type));
        }
    }

    public void SetCurrentTypeMode(TypeTypingBox typeMod)
    {
        _currentTypingMode = typeMod;
    }

    public void CheckTyping(string typedText)
    {
        // This method can be overridden by derived classes to implement specific typing logic
        List<TypingBox> macthingTypeBox = new List<TypingBox>();

        if (_activeTypingBoxs.Count == 0)
        {
            return;
        }

        //set typebox active by type
        var activeTypeBox = _activeTypingBoxs.Find(x => x.type == _currentTypingMode);

        foreach (var box in new List<TypingBox>(activeTypeBox.typingBoxes))
        {
            if (box.CheckingText(typedText.ToLower()))
            {
                macthingTypeBox.Add(box);
            }
        }

        if (macthingTypeBox.Count == 0)
        {
            ResetAllTypeBox();
        }
    }

    private void SetActiveTypeBox(TypingBox activeTypeBox)
    {
        var type = activeTypeBox.typeTypingBox;
        var typeGroup = _activeTypingBoxs.Find(x => x.type == type);

        if (typeGroup == null)
        {
            typeGroup = new TypingBoxByType(type);
            _activeTypingBoxs.Add(typeGroup);
        }
        
        if (!typeGroup.typingBoxes.Contains(activeTypeBox))
        {
            typeGroup.typingBoxes.Add(activeTypeBox);
        }
    }

    public void ResetAllTypeBox()
    {
        var typeGroup = _activeTypingBoxs.Find(x => x.type == _currentTypingMode);

        foreach (var typeBox in typeGroup.typingBoxes)
        {
            typeBox.ResetTypeBox();
        }
    }

    public void RemoveTypeBox(TypingBox typeBox)
    {
        var typeGroup = _activeTypingBoxs.Find(x => x.type == _currentTypingMode);

        if (typeGroup == null)
        {
            Debug.LogWarning($"No type group found for type: {_currentTypingMode}");
            return;
        }
        
        typeGroup.typingBoxes.Remove(typeBox);

        ResetAllTypeBox();
    }

    private void OnEnable()
    {
        _setTypeBoxEvent.Register(SetActiveTypeBox);
    }

    private void OnDisable()
    {
        _setTypeBoxEvent.Unregister(SetActiveTypeBox);
    }
}
