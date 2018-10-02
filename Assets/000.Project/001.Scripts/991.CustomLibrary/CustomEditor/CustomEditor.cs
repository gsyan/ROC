using UnityEngine;


public class BitMaskAttribute : PropertyAttribute
{
    //LayeredPrefab 에서 사용
    //BitMask attribute( 예>[BitMask(typeof(SceneType))] ) 을 사용 하기 위해
    //로직은 EditorExtension / EnumBitMaskPropertyDrawer 을 참고

    public System.Type propType;
    public BitMaskAttribute(System.Type aType)
    {
        propType = aType;
    }
}

//Dlog 에서 특정 어트리븃을 사용하기 위해서 작성
namespace System.Runtime.CompilerServices
{
    //현재 unity에 포함된 .net 버전에는 없는 attribute 을 추가해 사용하는 꼼수, 향후 정식으로 생긴다면 삭제

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public sealed class CallerMemberNameAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public sealed class CallerFilePathAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public sealed class CallerLineNumberAttribute : Attribute
    {
    }
}