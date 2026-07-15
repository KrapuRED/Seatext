using System;
using UnityEngine;
using System.Collections.Generic;

public enum TypeTypingBox
{
    None,
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
    [SerializeField] protected List<TypingBoxByType> activeTypingBoxs = new List<TypingBoxByType>();

    [Header("Events")]
    [SerializeField] private SetTypeBoxEventSO setTypeBoxEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Initialize the dictionary with empty lists for each TypeTypingBox
        foreach (TypeTypingBox type in System.Enum.GetValues(typeof(TypeTypingBox)))
        {
            activeTypingBoxs.Add(new TypingBoxByType(type));
        }
    }
    
    private void OnEnable()
    {
        setTypeBoxEvent.Register(SetActiveTypeBox);
    }

    private void OnDisable()
    {
        UnRegisterEvent();
    }

    private void OnDestroy()
    {
        UnRegisterEvent();
    }

    private void UnRegisterEvent()
    {
        setTypeBoxEvent.Unregister(SetActiveTypeBox);
        
    }

    public void SetCurrentTypeMode(TypeTypingBox typeMod)
    {
        if (instance == null)
        {
            Debug.LogWarning($"[TypeBoxManager] TypeBoxManager instance is null");
            return;
        }
        
        _currentTypingMode = typeMod;
    }

    public void CheckTyping(string typedText)
    {
        // This method can be overridden by derived classes to implement specific typing logic
        List<TypingBox> macthingTypeBox = new List<TypingBox>();

        if (activeTypingBoxs.Count == 0)
        {
            return;
        }

        //set typebox active by type
        var activeTypeBox = activeTypingBoxs.Find(x => x.type == _currentTypingMode);

        foreach (var box in new List<TypingBox>(activeTypeBox.typingBoxes))
        {
            if (box.CheckingText(typedText.ToLower()))
            {
                macthingTypeBox.Add(box);
            }
        }
        
        if (macthingTypeBox.Count == 1)
        {
            // Notify whoever cares about the single match
            GameEvents.OnSingleTypeBoxMatch?.Invoke(macthingTypeBox[0]);
        }
        else if (macthingTypeBox.Count == 0)
        {
            ResetAllTypeBox();
        }
    }

    private void SetActiveTypeBox(TypingBox activeTypeBox)
    {
        var type = activeTypeBox.TypeTypingBox;
        var typeGroup = activeTypingBoxs.Find(x => x.type == type);

        if (typeGroup == null)
        {
            typeGroup = new TypingBoxByType(type);
            activeTypingBoxs.Add(typeGroup);
        }
        
        if (!typeGroup.typingBoxes.Contains(activeTypeBox))
        {
            typeGroup.typingBoxes.Add(activeTypeBox);
        }
    }

    public void ResetAllTypeBox()
    {
        var typeGroup = activeTypingBoxs.Find(x => x.type == _currentTypingMode);

        foreach (var typeBox in typeGroup.typingBoxes)
        {
            typeBox.ResetTypeBox();
        }
    }

    public void RemoveTypeBox(TypingBox typeBox)
    {
        var typeGroup = activeTypingBoxs.Find(x => x.type == _currentTypingMode);

        if (typeGroup == null)
        {
            Debug.LogWarning($"No type group found for type: {_currentTypingMode}");
            return;
        }
        
        typeGroup.typingBoxes.Remove(typeBox);

        //ResetAllTypeBox();
    }
}
