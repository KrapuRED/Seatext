using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(BaseRewardData), true)]
public class RewardDataDrawer : PropertyDrawer
{
    private readonly string[] rewardTypeOptions = { "None", "Currency", "Item" };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        if (property.managedReferenceValue != null)
        {
            var reward = property.managedReferenceValue as BaseRewardData;
            if (reward != null && IsSharedReference(property))
            {
                property.managedReferenceValue = reward.Clone();
            }
        }

        Rect dropdownRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        int currentIndex = GetCurrentIndex(property);
        int selectedIndex = EditorGUI.Popup(dropdownRect, label.text, currentIndex, rewardTypeOptions);

        if (selectedIndex != currentIndex)
        {
            property.managedReferenceValue = selectedIndex switch
            {
                1 => new CurrencyRewardData(),
                2 => new ItemRewardData(),
                _ => null
            };
        }

        if (property.managedReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            float yOffset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            foreach (SerializedProperty child in GetVisibleChildren(property))
            {
                Rect childRect = new Rect(position.x, position.y + yOffset, position.width, EditorGUI.GetPropertyHeight(child));
                EditorGUI.PropertyField(childRect, child, true);
                yOffset += EditorGUI.GetPropertyHeight(child) + EditorGUIUtility.standardVerticalSpacing;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;

        if (property.managedReferenceValue != null)
        {
            foreach (SerializedProperty child in GetVisibleChildren(property))
            {
                height += EditorGUI.GetPropertyHeight(child) + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        return height;
    }

    private int GetCurrentIndex(SerializedProperty property)
    {
        return property.managedReferenceValue switch
        {
            CurrencyRewardData => 1,
            ItemRewardData => 2,
            _ => 0
        };
    }

    private bool IsSharedReference(SerializedProperty property)
    {
        long id = property.managedReferenceId;

        string path = property.propertyPath;
        int arrayIndex = path.LastIndexOf(".Array");
        if (arrayIndex < 0) return false;

        string arrayPath = path.Substring(0, arrayIndex);
        var array = property.serializedObject.FindProperty(arrayPath);

        if (array == null || !array.isArray) return false;

        int count = 0;
        for (int i = 0; i < array.arraySize; i++)
        {
            var element = array.GetArrayElementAtIndex(i);
            var rewardProp = element.FindPropertyRelative("Reward");
            if (rewardProp != null && rewardProp.managedReferenceId == id)
                count++;
        }

        return count > 1;
    }

    private IEnumerable<SerializedProperty> GetVisibleChildren(SerializedProperty property)
    {
        var iterator = property.Copy();
        var end = property.GetEndProperty();

        if (!iterator.NextVisible(true)) yield break;

        while (!SerializedProperty.EqualContents(iterator, end))
        {
            yield return iterator.Copy();
            iterator.NextVisible(false);
        }
    }
}
#endif