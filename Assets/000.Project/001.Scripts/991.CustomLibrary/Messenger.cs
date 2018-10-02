// Messenger.cs v1.0 by Magnus Wolffelt, magnus.wolffelt@gmail.com
//
// Inspired by and based on Rod Hyde's Messenger:
// http://www.unifycommunity.com/wiki/index.php?title=CSharpMessenger
//
// This is a C# messenger (notification center). It uses delegates
// and generics to provide type-checked messaging between event producers and
// event consumers, without the need for producers or consumers to be aware of
// each other. The major improvement from Hyde's implementation is that
// there is more extensive error detection, preventing silent bugs.
//
// Usage example:
// Messenger<float>.AddListener("myEvent", MyEventHandler);
// ...
// Messenger<float>.Broadcast("myEvent", 1.0f);

using System;
using System.Collections.Generic;

public enum MessengerMode
{
    DONT_REQUIRE_LISTENER,
    REQUIRE_LISTENER,
}

static public class MessengerInternal
{
    static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
    static public readonly MessengerMode DEFAULT_MODE = MessengerMode.REQUIRE_LISTENER;

    static public void OnListenerAdding(string eventKey, Delegate listenerBeingAdded)
    {
        if( !eventTable.ContainsKey(eventKey) )
        {
            eventTable.Add(eventKey, null);
        }

        Delegate d = eventTable[eventKey];
        if (d != null && d.GetType() != listenerBeingAdded.GetType())
        {
            throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventKey, d.GetType().Name, listenerBeingAdded.GetType().Name));
        }
    }
    static public void OnListenerRemoving(string eventKey, Delegate listenerBeingRemoved)
    {
        if (eventTable.ContainsKey(eventKey))
        {
            Delegate d = eventTable[eventKey];

            if (d == null)
            {
                throw new ListenerException(string.Format("Attempting to remove listener with for event type {0} but current listener is null.", eventKey));
            }
            else if (d.GetType() != listenerBeingRemoved.GetType())
            {
                throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event key {0}. Current listeners have type {1} and listener being removed has type {2}", eventKey, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
        }
        else
        {
            throw new ListenerException(string.Format("Attempting to remove listener for type {0} but Messenger doesn't know about this event type.", eventKey));
        }
    }
    static public void OnListenerRemoved(string eventKey)
    {
        if (eventTable[eventKey] == null)
        {
            eventTable.Remove(eventKey);
        }
    }
    static public void OnBroadcasting(string eventKey, MessengerMode mode)
    {
        if (mode == MessengerMode.REQUIRE_LISTENER && !eventTable.ContainsKey(eventKey))
        {
            throw new MessengerInternal.BroadcastException(string.Format("Broadcasting message {0} but no listener found.", eventKey));
        }
    }

    static public BroadcastException CreateBroadcastSignatureException(string eventKey)
    {
        return new BroadcastException(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", eventKey));
    }
    public class BroadcastException : Exception
    {
        public BroadcastException(string msg)
            : base(msg)
        {
        }
    }
    public class ListenerException : Exception
    {
        public ListenerException(string msg)
            : base(msg)
        {
        }
    }
}

static public class Messenger
{
    private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

    static public void AddListener(string eventKey, Callback handler)
    {
        MessengerInternal.OnListenerAdding(eventKey, handler);//key 값과 null을 eventTable 에 추가한다.
        eventTable[eventKey] = (Callback)eventTable[eventKey] + handler;
    }
    static public void RemoveListener(string eventKey, Callback handler)
    {
        MessengerInternal.OnListenerRemoving(eventKey, handler);
        eventTable[eventKey] = (Callback)eventTable[eventKey] - handler;
        MessengerInternal.OnListenerRemoved(eventKey);
    }
    static public void Broadcast(string eventKey)
    {
        Broadcast(eventKey, MessengerInternal.DEFAULT_MODE);
    }
    static public void Broadcast(string eventKey, MessengerMode mode)
    {
        MessengerInternal.OnBroadcasting(eventKey, mode);
        Delegate d;
        if(eventTable.TryGetValue(eventKey, out d))
        {
            Callback callback = d as Callback;
            if( callback != null)
            {
                callback();
            }
            else
            {
                throw MessengerInternal.CreateBroadcastSignatureException(eventKey);
            }
        }
    }
}

static public class Messenger<T>
{
    private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

    static public void AddListener(string eventKey, Callback<T> handler)
    {
        MessengerInternal.OnListenerAdding(eventKey, handler);
        eventTable[eventKey] = (Callback<T>)eventTable[eventKey] + handler;
    }
    static public void RemoveListener(string eventKey, Callback<T> handler)
    {
        MessengerInternal.OnListenerRemoving(eventKey, handler);
        eventTable[eventKey] = (Callback<T>)eventTable[eventKey] - handler;
        MessengerInternal.OnListenerRemoved(eventKey);
    }
    static public void Broadcast(string eventKey, T arg1)
    {
        Broadcast(eventKey, arg1, MessengerInternal.DEFAULT_MODE);
    }
    static public void Broadcast(string eventKey, T arg1, MessengerMode mode)
    {
        MessengerInternal.OnBroadcasting(eventKey, mode);
        Delegate d;
        if (eventTable.TryGetValue(eventKey, out d))
        {
            Callback<T> callback = d as Callback<T>;
            if (callback != null)
            {
                callback(arg1);
            }
            else
            {
                throw MessengerInternal.CreateBroadcastSignatureException(eventKey);
            }
        }
    }

}

static public class Messenger<T, U>
{
    private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

    static public void AddListener(string eventKey, Callback<T, U> handler)
    {
        MessengerInternal.OnListenerAdding(eventKey, handler);
        eventTable[eventKey] = (Callback<T, U>)eventTable[eventKey] + handler;
    }
    static public void RemoveListener(string eventKey, Callback<T, U> handler)
    {
        MessengerInternal.OnListenerRemoving(eventKey, handler);
        eventTable[eventKey] = (Callback<T, U>)eventTable[eventKey] - handler;
        MessengerInternal.OnListenerRemoved(eventKey);
    }
    static public void Broadcast(string eventKey, T arg1, U arg2)
    {
        Broadcast(eventKey, arg1, arg2, MessengerInternal.DEFAULT_MODE);
    }
    static public void Broadcast(string eventKey, T arg1, U arg2, MessengerMode mode)
    {
        MessengerInternal.OnBroadcasting(eventKey, mode);
        Delegate d;
        if (eventTable.TryGetValue(eventKey, out d))
        {
            Callback<T, U> callback = d as Callback<T, U>;
            if (callback != null)
            {
                callback(arg1, arg2);
            }
            else
            {
                throw MessengerInternal.CreateBroadcastSignatureException(eventKey);
            }
        }
    }
}

static public class Messenger<T, U, V>
{
    private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

    static public void AddListener(string eventKey, Callback<T, U, V> handler)
    {
        MessengerInternal.OnListenerAdding(eventKey, handler);
        eventTable[eventKey] = (Callback<T, U, V>)eventTable[eventKey] + handler;
    }
    static public void RemoveListener(string eventKey, Callback<T, U, V> handler)
    {
        MessengerInternal.OnListenerRemoving(eventKey, handler);
        eventTable[eventKey] = (Callback<T, U, V>)eventTable[eventKey] - handler;
        MessengerInternal.OnListenerRemoved(eventKey);
    }
    static public void Broadcast(string eventKey, T arg1, U arg2, V arg3)
    {
        Broadcast(eventKey, arg1, arg2, arg3, MessengerInternal.DEFAULT_MODE);
    }
    static public void Broadcast(string eventKey, T arg1, U arg2, V arg3, MessengerMode mode)
    {
        MessengerInternal.OnBroadcasting(eventKey, mode);
        Delegate d;
        if (eventTable.TryGetValue(eventKey, out d))
        {
            Callback<T, U, V> callback = d as Callback<T, U, V>;
            if (callback != null)
            {
                callback(arg1, arg2, arg3);
            }
            else
            {
                throw MessengerInternal.CreateBroadcastSignatureException(eventKey);
            }
        }
    }
}

public static class EventKey
{
    public const string UpdateNotice = "Notice";

    public const string UpdateAssetInfo = "UpdateAssetInfo";//cube, gold, energy
    public const string UpdateItem = "UpdateSkillInfo";//점검 필요 뜻이 아예 다른데?
    public const string UpdateExp = "UpdateExp";

    public const string EnterNextPortal = "EnterNextPortal";
    public const string EnterExitPortal = "EnterExitPortal";
    public const string SkipAppearance = "SkipAppearance";

    public const string OnLocalize = "OnLocalize";

    public const string TutorialCheck = "StartTutorial";
    public const string TutorialSequenceActive = "TutorialSequenceActive";

    public const string RequestReserveSkill = "RequestReserveSkill";
    public const string ToggleAutoActivity = "ToggleAutoActivity";
    public const string StopAutoMoving = "StopAutoMoving";

    public const string SetupStatusUI = "SetupStatusUI";

    public const string ColorChange = "ColorChange";
    public const string WeaponChange = "WeaponChange";

    public const string ChangeCameraSetting = "ChangeCameraSetting";
    public const string ChangeCameraSettingImmediate = "ChangeCameraSettingImmediate";
    public const string PlayCameraBlur = "PlayCameraBlur";

    //public const string EnablePortal = "EnablePortal";
    public const string ShowPause = "ShowPause";

    public const string UpdatePopup = "UpdatePopup";

    public const string ShowMissionClear = "ShowMissionClear";
    public const string ShowQuestClear = "ShowQuestClear";
    public const string UpdateMissionPanel = "UpdateMissionPanel";

    public const string ShowStatusInfo = "ShowStatusInfo";
    public const string CharacterStatusModelCheck = "CharacterStatusModelCheck";
    public const string CharacterChangeAction = "CharacterChangeAction";

    public const string ShowInteractionUI = "ShowInteractionUI";
    public const string HideInteractionUI = "HideInteractionUI";

    public const string UpdateStatusInfo = "UpdateStatusInfo";
    public const string UpdateStatusSlot = "UpdateStatusSlot";
    public const string RequestRevival = "RequestRevival";
    public const string UpdateDailyDungeon = "UpdateDailyDungeon";
    public const string UpdateTreasureItem = "UpdateTreasureItem";
    public const string UpdateTreasureCount = "UpdateTreasureCount";
    

    public const string StartAutoContinueBattle = "StartAutoContinueBattle";
    public const string StopAutoContinueBattle = "StopAutoContinueBattle";
    public const string UpdateContinueBattle = "UpdateContinueBattle";

    public const string NotifyNewQuest = "NotifyNewQuest";

    public const string ShowAttendEvent = "ShowAttendEvent";

    public const string AddGameFriend = "AddGameFriend";
    public const string RemoveGameFriend = "RemoveGameFriend";
    public const string RemoveRecommendFriend = "RemoveRecommendFriend";
    public const string RemoveResponseFriend = "RemoveResponseFriend";
    public const string RequestedFacebookFriend = "RequestedFacebookFriend";
    public const string UpdateNewFriend = "UpdateNewFriend";
    public const string UpdateSendQuest = "UpdateSendQuest";

    public const string UpdatePvpModeStatus = "UpdatePvpModeStatus";
    public const string UpdateChallengeModeStatus = "UpdateChallengeModeStatus";
    public const string UpdateDayReward = "UpdateDayReward";
    public const string GetSeasonReward = "GetSeasonReward";
    public const string CompletePvpMatch = "CompletePvpMatch";

    public const string AddQuestInfo = "AddQuestInfo";
    public const string DeleteQuestInfo = "DeleteQuestInfo";
    public const string UpdateQuestPanel = "UpdateQuestPanel";
    public const string UpdateQuestInfo = "UpdateQuestInfo";
    public const string OnShortcutsNPC = "OnShortcutsNPC";

    public const string AddActor = "AddActor";
    public const string RemoveActor = "RemoveActor";

    public const string PurchaseProduct = "PurchaseProduct";
    public const string BuyProduct = "BuyProduct";
    public const string UpdatePakage = "UpdatePakage";
    public const string UpdateRegular = "UpdateRegular";

    public const string TagMatch = "TagMatch";
    public const string TagMatchButtonHide = "TagMatchButtonHide";
    public const string TagMatchButtonCheck = "TagMatchButtonCheck";
    public const string TagMatchComplete = "TagMatchComplete";

    public const string ClosePanelCharacterStatus = "ClosePanelCharacterStatus";

    public const string EnableWeaponAvatarButton = "EnableWeaponAvatarButton";
    public const string DisableWeaponAvatarButton = "DisableWeaponAvatarButton";

    // 캐쉬 제품 처리.
    public const string CashProductPurchaseResult = "CashProductPurchaseResult";

    public const string PatchDownalodProgress = "PatchDownalodProgress";

    public const string NextStep = "NextStep";

    // 채팅
    public const string UpdateChatMessage = "UpdateChatMessage";
    public const string NewChatMessage = "NewChatMessage";
    public const string OnWhisper = "OnWhisper";

    public const string StartTownProcessor = "StartTownProcessor";

    //로그인 시 서버 선택
    public const string SelectServer = "SelectServer";

    // 보스레이드 추가 2017.06.30->zin
    public const string UpdateBossRaid = "UpdateBossRaid";

    public const string QuickPosition = "QuickPosition";


}


