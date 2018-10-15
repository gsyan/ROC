using UnityEngine;
using UnityEditor;

public static class EditorExtension
{
    public static int DrawBitMaskField(Rect aPosition, int aMask, System.Type aType, GUIContent aLabel)
    {
        string[] itemNames = System.Enum.GetNames(aType);
        int[] itemValues = System.Enum.GetValues(aType) as int[];

        int val = aMask;
        int maskVal = 0;//EditorGUI.MaskField() 에 넣어서 newMaskValue 를 받을 용도의 비트값(한 비트 크게 잡는다. 0 이면 1, 2 면 4)

        for ( int i=0;  i < itemValues.Length; ++i)
        {
            if( itemValues[i] != 0)
            {
                if( (val & itemValues[i]) == itemValues[i])//val에 itemValues[i] 가 포함되어 있는가?
                {
                    maskVal |= 1 << i;//maskVal 값은 itemValues[i]의 값이 아닌 1<<i 를 하므로서 한 비트 크게 잡힌다.
                }
            }
            else if(val == 0)
            {
                maskVal |= 1 << i;
            }
        }

        int newMaskValue = EditorGUI.MaskField(aPosition, aLabel, maskVal, itemNames);//변경이 있다면 현재의 value 값을 토해냄
        int changes = maskVal ^ newMaskValue;

        for(int i = 0; i < itemValues.Length; ++i)
        {
            if((changes & (1<< i)) != 0)//changes 는 1<<i 를 포함하는가?
            {
                if((newMaskValue & (1 << i)) != 0)
                {
                    if(itemValues[i] == 0)
                    {
                        val = 0;
                        break;
                    }
                    else
                    {
                        val |= itemValues[i];
                    }
                }
                else
                {
                    val &= ~itemValues[i];
                }
            }
        }
        
        return val;
    }


}


[CustomPropertyDrawer(typeof(BitMaskAttribute))]
public class EnumBitMaskPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        var typeAttr = attribute as BitMaskAttribute;
        label.text = label.text + " (" + property.intValue + ")";
        property.intValue = EditorExtension.DrawBitMaskField(position, property.intValue, typeAttr.propType, label);
    }
}