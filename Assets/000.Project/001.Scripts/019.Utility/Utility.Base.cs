using UnityEngine;
using System.Collections;

public partial class Utility
{
    public static float GetNextRefreshTime(int day = 1)
    {
        System.DateTime current = GInfo.serverTime;
        //.Date을 붙임으로서 리프레시 되는 타임을 00 시로(AM12:00) 세팅
        System.DateTime next = current.Date.AddDays(day);

        //다음 날 00시 까지 남은 초를 리턴한다.
        return (float)(next - current).TotalSeconds;
    }

    public static string GetLevelText(int level)
    {
        return "Lv" + level.ToString();
    }
    public static string GetEnhanceText(int enhance)
    {
        return "+" + enhance.ToString();
    }
    
    public static string GetWeaponTypeIcon(WeaponType type)
    {
        return "weapon_type_" + type.ToString().ToLower();
    }
    
    public static string GetWeaponTypeName(WeaponType type)
    {
        return Localization.Get("weapon_type_" + type.ToString().ToLower());
    }

    public static string GetCharacterIcon(int avatarID)
    {
        CharacterSkinData data = GData.Instance.GetCharacterSkinData(avatarID);
        if(data != null)
        {
            return data.iconName;
        }
        return "";
    }

    public static string GetCharacterName(CharacterType type)
    {
        return "chr_" + type.ToString().ToLower();
    }

    public static string GetSkillIcon(int id)
    {
        return "skill_" + id.ToString();
    }

    public static string GetSkillName(int id)
    {
        return Localization.Get("skill_name_" + id);
    }

    public static string GetSkillDesc(int id)
    {
        return Localization.Get("skill_desc_" + id);
    }

    public static string GetSkillDesc2(int id)
    {
        return Localization.Get("skill_desc1_" + id);
    }

    public static string GetSkillSlotName(SkillSlotType type)
    {
        return Localization.Get("slot_name_" + ((int)type + 1));
    }
    
    public static string GetSkillSlotDesc(SkillSlotType type, int grade)
    {
        return Localization.Get("slot_desc_" + ((int)type + 1) + "_lv" + grade);
    }

    public static string GetGemIcon(GemType type)
    {
        return type.ToString();
    }

    public static string GetProductIcon(ProductType type, int number)
    {
        switch (type)
        {
            case ProductType.Gold:
                return "cash_gold_" + number;

            case ProductType.Cube:
                return "cash_dia_" + number;
        }

        return "";
    }

    public static string GetPvPRankIcon(int grade)
    {
        return "pvp_icon_" + grade;
    }

    public static string GetChallengeRankIcon(int grade)
    {
        return "challenge_" + grade;
    }


    public static string GetFieldBattleSprtieName(int fieldId)
    {
        return "battle_field_" + (fieldId / 10000) % 10;
    }

    public static string GetFieldBackgroundSpriteName(int fieldId, FieldLevel level)
    {
        return "field_" + (fieldId / 10000) % 10 + "_" + level.ToString().ToLower();
    }
    
    public static string GetFieldName(int id)
    {
        return Localization.Get("field_name_" + id);
    }

    public static string GetFieldStarCount(int currentCound, int maxCount)
    {
        //[ffff00]는 뒤에 올 텍스트의 색, [-]은 그 앞까지의 서식을 무효화
        return string.Format("[ffff00]★[-] {0}/{1}", currentCound, maxCount);
    }
    
    public static string GetChatTypeString(ChatType chatType)
    {
        //case를 코드 외부로 빼는 작업을 해보자, GetCharacterIcon 처럼
        switch (chatType)
        {
            case ChatType.Normal:
                return Localization.Get("Chatting_sender_user");

            case ChatType.Notify:
                return Localization.Get("Chatting_sender_system");

            case ChatType.Whisper:
                return Localization.Get("Chatting_sender_whisper");

            case ChatType.Guild:
                return Localization.Get("길드");
        }

        return "";
    }

    public static string GetNpcIconFromID(int a_id)
    {
        NpcData npcData = null;

        npcData = GData.Instance.GetNpcDataFromID(a_id);

        if (npcData == null)
        {
            return null;
        }

        return npcData.icon;
    }

    public static string GetNpcIconFromKeyName(string a_name)
    {
        NpcData npcData = null;

        npcData = GData.Instance.GetNpcDataFromKeyName(a_name);

        if (npcData == null)
        {
            return null;
        }

        return npcData.icon;
    }

    public static string GetNpcNameKeyFromID(int a_id)
    {
        NpcData npcData = null;

        npcData = GData.Instance.GetNpcDataFromID(a_id);

        if (npcData == null)
        {
            return null;
        }

        return Localization.Get(npcData.textKey);
    }

    public static string GetNpcNameKeyFromKeyName(string a_name)
    {
        NpcData npcData = null;

        npcData = GData.Instance.GetNpcDataFromKeyName(a_name);

        if (npcData == null)
        {
            return null;
        }

        //왜 return Localization.Get(npcData.textKey); 가 아니지?
        return npcData.textKey;
    }

    public static string GetNpcObjectName(int id)
    {
        NpcData data = GData.Instance.GetNpcDataFromID(id);

        if (data == null)
        {
            return "NPC_NONE";
        }

        return "NPC_" + data.prefab;
    }

    public static CharacterType GetAvatartIdToCharacterType(int id)
    {
        //의도가 뭔지 파악일 못함
        return (CharacterType)((id % 30000) / 1000);
    }

    public static int GetAvatarIdToWeaponGrade(int id, int defaultGrade = 1)
    {
        CharacterSkinData data = GData.Instance.GetCharacterSkinData(id);
        if (data != null)
        {
            int transfromGrade;
            int.TryParse(data.prefab.Substring(data.prefab.Length - 4, 4), out transfromGrade);
            transfromGrade = (transfromGrade % 1000) / 10;
            if (transfromGrade > 0)
            {
                return transfromGrade;
            }
        }

        return defaultGrade;
    }

    public static CharacterStats GetWeaponStat(WeaponInfo info)
    {
        return GetWeaponStat(info.type, info.level, info.grade, info.enhance, info.gemLevels);
    }
    public static CharacterStats GetWeaponStat(WeaponType type, int level, int grade, int enhance, params int[] gemLevels)
    {//params 개수가 가변적인 인자를 넘길때 사용, 근데 진짜 이거 안붙이면 안되는지 테스트 필요함
        CharacterStats stat = null;

        EquipmentLevelData levelData = GData.Instance.GetWeaponLevelData(type, level);
        EquipmentGradeData gradeData = GData.Instance.GetWeaponGradeData(type, grade);
        EquipmentGradeData enhanceData = GData.Instance.GetWeaponGradeData(type, enhance);

        stat = (levelData.stats + gradeData.stats + enhanceData.stats);
        if( gemLevels != null)
        {
            for(int i=0; i< gemLevels.Length; ++i)
            {
                if(gemLevels[i] > 0)
                {
                    stat += GData.Instance.GetGemLevelData((GemType)i, gemLevels[i]).stats;
                }
            }
        }
        return stat;
    }

    public static CharacterStats GetArmorStat(ArmorInfo info)
    {
        return GetArmorStat(info.type, info.level, info.grade, info.enhance);
    }
    public static CharacterStats GetArmorStat(ArmorType type, int level, int grade, int enhance)
    {
        EquipmentLevelData levelData = GData.Instance.GetArmorLevelData(type, level);
        EquipmentGradeData gradeData = GData.Instance.GetArmorGradeData(type, grade);
        EquipmentEnhanceData enhanceData = GData.Instance.GetArmorEnhanceData(type, enhance);

        return levelData.stats + gradeData.stats + enhanceData.stats;
    }

    public static CharacterStats GetSkinStat(SkinInfo info)
    {
        EquipmentLevelData levelData = GData.Instance.GetSkinLevelData(info.level);
        EquipmentGradeData gradeData = GData.Instance.GetSkinGradeData(info.grade);
        EquipmentEnhanceData enhanceData = GData.Instance.GetSkinEnhanceData(info.enhance);

        return levelData.stats + gradeData.stats + enhanceData.stats;
    }
    public static CharacterStats GetSkinStat(int level, int grade, int enhance)
    {
        EquipmentLevelData levelData = GData.Instance.GetSkinLevelData(level);
        EquipmentGradeData gradeData = GData.Instance.GetSkinGradeData(grade);
        EquipmentEnhanceData enhanceData = GData.Instance.GetSkinEnhanceData(enhance);

        return levelData.stats + gradeData.stats + enhanceData.stats;
    }

    public static int GetBaseToLevelFieldId(int baseID, FieldLevel level)
    {
        //무엇에 쓰는거?
        return baseID + (((int)level - 1) * 100000000);
    }

    public static int GetDungeonIdToFieldId(int dungeonId)
    {
        //무엇에 쓰는거?
        return (dungeonId / 10000) * 10000;
    }

    public static void ShowDungeonEnterUI(DungeonStateInfo stateInfo, Callback onSuccess = null)
    {
        Callback onFail = delegate () {

            if( !stateInfo.isFieldOpen )
            {
                //이부분의 ui 구성을 위해 utility.ui 부분을 작성하자.
            }
            else if( !stateInfo.prevDungeonClear )
            {

            }
            else
            {

            }

        };
        ShowDungeonEnter(stateInfo, onFail, onSuccess);
    }


    public static void ShowDungeonEnter(DungeonStateInfo stateInfo, Callback onFail, Callback onSuccess)
    {
        if( stateInfo.state == DungeonState.Locked )
        {
            if(onFail != null)
            {
                onFail();
            }
        }
        else
        {
            if(onSuccess != null)
            {
                onSuccess();
            }
        }
    }


    //두 파일의 내용이 같은지 판단한다. (assetbundle의 매니페스트)
    public static bool CompareFileByte(string pathA, string pathB)
    {
        byte[] pathAByte = AssetBundleUtility.ComputeHash(pathA);
        byte[] pathBByte = AssetBundleUtility.ComputeHash(pathB);

        return ByteArrayCompare(pathAByte, pathBByte);
    }
    //두 byte 배열의 내용이 같은지 판단한다.
    public static bool ByteArrayCompare(byte[] ba1, byte[] ba2)
    {
        if (ba1.Length != ba2.Length)
        {
            return false;
        }

        int fCount = ba1.Length;
        for (int i=0; i< fCount; ++i)
        {
            if(ba1[i] != ba2[i])
            {
                return false;
            }
        }

        return true;
    }

    //string array 가 특정 string을 가지고 있는지 검사
    public static bool StringArrayContainString(string[] stringArray, string findingString)
    {
        int fCount = stringArray.Length;
        for(int i=0; i<fCount; ++i)
        {
            if( string.Compare(stringArray[i], findingString) == 0 )
            {
                return true;
            }
        }
        return false;
    }
    
    //게임 오브젝트 생성관련
    public static Object Instantiate(string path)
    {
        Object prefab = ResourceSystem.Load(path);
        if(prefab != null)
        {
            Object instance = Object.Instantiate(prefab);
            if (instance != null)
            {
                ResourceSystem.RegisterReference(path, instance);//weaklist에 등록됨
                return instance;
            }
            else
            {
                ResourceSystem.Unload(path);
            }
        }
        return null;
    }

    /// <summary>
    /// 자식 찾기, 재귀, 활성화, 비활성화 된 오브젝트 이름으로 찾는다.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform Search(Transform target, string name)
    {
        if(target.name == name)
        {
            return target;
        }

        int fCount = target.childCount;
        for ( int i = 0; i < fCount; ++i)
        {
            Transform tm = Search(target.GetChild(i), name);
            if(tm != null)
            {
                return tm;
            }
        }

        return null;
    }
    /// <summary>
    /// 자식 찾기, 활성화된 오브젝트만 찾는다.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform SearchActive(Transform target, string name)
    {
        //활성화 안된 애들 + 자식들 까지 패스
        if (!target.gameObject.activeInHierarchy)
        {
            return null;
        }

        if(target.name == name)
        {
            return target;
        }

        int fCount = target.childCount;
        for (int i = 0; i < fCount; ++i)
        {
            Transform tm = SearchActive(target.GetChild(i), name);
            if(tm != null)
            {
                return tm;
            }
        }

        return null;
    }




    //string 으로 layer 바꾸기
    public static void ChangeLayerRecursively(Transform target, string layerName)
    {
        ChangeLayerRecursively(target, LayerMask.NameToLayer(layerName));
    }
    //layer(int) 로 layer 바꾸기
    public static void ChangeLayerRecursively(Transform target, int layer)
    {
        target.gameObject.layer = layer;

        foreach( Transform child in target )
        {
            ChangeLayerRecursively(child, layer);
        }
    }





    //부모 설정, layer 까지 변경 할지 옵션
    public static void SetParent(Transform parent, Transform child, bool changeLayer)
    {
        if (parent != null)
        {
            child.parent = parent;
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
            child.localScale = Vector3.one;

            if( changeLayer )
            {
                child.gameObject.layer = parent.gameObject.layer;
            }
        }
    }
    public static void SetParentUI(Transform parent, Transform child, bool changeLayer)
    {
        if (parent != null)
        {
            child.parent = parent;
            child.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            child.localRotation = Quaternion.identity;
            child.localScale = Vector3.one;

            if (changeLayer)
            {
                child.gameObject.layer = parent.gameObject.layer;
            }
        }
    }





    public static string GetNationFromCode(string nationCode)
    {
        switch (nationCode)
        {
            case "KR":
                return "korean";
            case "CN":
                return "chinese";
            case "TW":
                return "taiwanese";
            case "JP":
                return "japanese";
            default:
                return "english";
        }
    }

    #region Make Object
    /// Make Plane with 3 points(triangle)
    public static Vector3[] GetQuadPointsFromTriangle(Transform p1, Transform p2, Transform p3)
    {   //p1 = bottom left / p2 = bottom right / p3 = top
        return GetQuadPointsFromTriangle(p1.position, p2.position, p3.position);
    }
    public static Vector3[] GetQuadPointsFromTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //p1 = bottom left / p2 = bottom right / p3 = top

        //삼각형 상태 점검, 이등변 삼각형을 만들어둔다
        Vector3 topToBL = (p1 - p3);
        Vector3 topToBR = (p2 - p3);
        if( topToBL.magnitude > topToBR.magnitude )
        {
            p2 = p3 + topToBR.normalized * topToBL.magnitude;
        }
        else if(topToBL.magnitude < topToBR.magnitude)
        {
            p1 = p3 + topToBL.normalized * topToBR.magnitude;
        }

        return Get4PointsFromIsoscelesTriangle(p1, p2, p3);
    }
    private static Vector3[] Get4PointsFromIsoscelesTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //p1 = bottom left / p2 = bottom right / p3 = top

        //bottom points 간 서로에 대한 normalized vector
        Vector3 v1To2Nor = (p2 - p1).normalized;
        Vector3 v2to1Nor = -v1To2Nor;//위에꺼 부호만 바꿔도 된다.

        //top point 에서 bottom points 로 향하는 벡터
        Vector3 v3to2 = p2 - p3;
        Vector3 v3to1 = p1 - p3;

        float lengthToptoTR = Vector3.Dot(v3to2, v1To2Nor);
        float lengthToptoTL = Vector3.Dot(v3to1, v2to1Nor);

        return new Vector3[]
        {
            p1,//BottomLeft point of Plane's four points
            p2,//BottomRight point of Plane's four points
            p3 + v1To2Nor * lengthToptoTR,//TopRight point of Plane's four points
            p3 + v2to1Nor * lengthToptoTL //TopLeft point of Plane's four points
        };
    }
    public static GameObject MakeQuad(Material planeMaterial, Vector3[] rectPoins)
    {
        return MakeQuad(planeMaterial, rectPoins[0], rectPoins[1], rectPoins[2], rectPoins[3]);
    }
    public static GameObject MakeQuad(Material planeMaterial, Vector3 rectPointBL, Vector3 rectPointBR, Vector3 rectPointTR, Vector3 rectPointTL)
    {
        GameObject plane = new GameObject("Plane");
        MeshFilter mf = plane.AddComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer mr = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        mr.material = planeMaterial;
        
        Mesh m = new Mesh();
        m.vertices = new Vector3[]
        {
            rectPointBL,
            rectPointBR,
            rectPointTR,
            rectPointTL
        };

        m.uv = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0)
        };

        m.triangles = new int[] { 0, 2, 1, 0, 3, 2 };

        mf.mesh = m;
        m.RecalculateBounds();
        m.RecalculateNormals();
        return plane;
    }

    public static GameObject InstantiateUnityPrimitive(PrimitiveType primitiveType)
    {
        //PrimitiveType.Plane
        //PrimitiveType.Cube
        //PrimitiveType.Sphere
        //PrimitiveType.Capsule
        //PrimitiveType.Cylinder
        return GameObject.CreatePrimitive(primitiveType);
    }
    
    #endregion Make Object

    #region Math, Vector

    public static int GetGCD(int a, int b)
    {//최대공약수, Greatest common divisor
        int mod = a % b;
        while (mod > 0)
        {
            a = b;
            b = mod;
            mod = a % b;
        }
        return b;
    }
    public static float GetGCD(float a, float b)
    {//최대공약수, Greatest common divisor
        float mod = a % b;
        while (mod > 0)
        {
            a = b;
            b = mod;
            mod = a % b;
        }
        return b;
    }
    public static int GetLCM(int a, int b)
    {//최소공배수, Least(lowest) common multiple
        return a * b / (GetGCD(a, b));
    }
    public static float GetLCM(float a, float b)
    {//최소공배수, Least(lowest) common multiple
        return a * b / (GetGCD(a, b));
    }

    public static void PositionLerp()
    {
        //float dist = Vector3.Distance(target.transform.position, transform.position);
        //transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * 1 / dist);
    }
    public static bool QuaternionLerp(ref Transform objTransform, Vector3 tarVec3, float time)
    {
        //update 문에서 작동
        //일정 시간동안 러프
        float theta = GetThetaOfTwoVector3D(objTransform.forward, tarVec3, false);
        Quaternion resRotation = Quaternion.identity;
        Quaternion targetRotation = Quaternion.LookRotation(tarVec3);
        if (theta > 0)
        {
            _rotationSec += Time.deltaTime;
            objTransform.rotation = Quaternion.Lerp(objTransform.rotation, targetRotation, _rotationSec / time);
            return true;
        }
        _rotationSec = 0.0f;
        return false;
    }
    private static float _rotationSec = 0.0f;


    public static float GetDistancePointToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        //선과 점의 거리, 선의 시작점과 점의 벡터를 선의 벡터의 외적하고 그 길이를 선의 길이로 나누면 된다.
        //bk: 아직은 왜인지는 모르겠다.
        Vector3 line = lineEnd - lineStart;
        Vector3 lineStartToPoint = point - lineStart;
        float distance = Vector3.Cross(line, lineStartToPoint).magnitude / line.magnitude;
        return distance;
    }
    public static Vector3 GetFootOfPerpendicularPointToLine(Vector3 p1, Vector3 p2, Vector3 point)
    {
        //선을 향해 점에서 내린 수선의 발 구하기
        Vector3 line = p2 - p1;
        float vectorShadowLength = GetLengthOfVectorShadow(p1, p2, point);//정사영(p1에서 p로의 벡터의 그림자)의 길이
        return p1 + line.normalized * vectorShadowLength;
    }
    public static float GetLengthOfVectorShadow(Vector3 p1, Vector3 p2, Vector3 p)
    {
        Vector3 line = p2 - p1;
        Vector3 v1toP = p - p1;
        //line 위에 그려지는 v1toP 의 그림자의 길이 리턴
        return Vector3.Dot(v1toP, line.normalized);
    }
    public static float CheckLeftOrRight(Vector3 characterForward, Vector3 toTarget, Vector3 up)
    {
        //케릭터 방향과 적으로의 방향 그리고 업 벡터를 사용해 상대의 위치(좌/ 우)를 판단
        Vector3 cross = Vector3.Cross(characterForward.normalized, toTarget.normalized).normalized;
        float dot = Vector3.Dot(cross, up);
        return dot;// +면 타겟이 케릭터의 오른쪽, - 면 타겟이 케릭터의 왼쪽
    }
    public static float GetRotateAngleWithUpAndForwardAxis(Transform baseObject, Transform checkObject)
    {
        Vector3 baseVec = baseObject.up;
        Vector3 checkVec = checkObject.up;
        float angle = Vector3.SignedAngle(baseVec, checkVec, baseObject.forward);//local z축이 마주보고 있다면.
        return angle;
    }
    public static float GetThetaOfTwoVector3D(Vector3 baseV, Vector3 targetV, bool angle)
    {
        //기본 제공되는 Vector3.Angle() 과 중복이지만 개념을 기억하는 차원에서 유지,
        //+ - 부호 필요한 경우 Vector3.SignedAngle()

        //기준 벡터 노말라이징
        Vector3 vec1 = Vector3.Normalize(baseV);
        //타겟 벡터 노말라이징
        Vector3 vec2 = Vector3.Normalize(targetV);
        //A dot B = |A| * |B| * cos(theta) 이용
        float res = Mathf.Acos(Vector3.Dot(vec1, vec2));
        if(angle)
        {
            res *= Mathf.Rad2Deg;
        }
        return res;
    }
    public static Vector3 GetCenterOfTriangle3D(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //세점의 중간 점을 찾아낸다.
        Vector3 p0Position = Vector3.zero;
        //밑변의 중간점을 찾고
        Vector3 p1ToP2 = p2 - p1;
        Vector3 centerP1ToP2 = p1 + p1ToP2 * 0.5f;
        //밑변의 중간점과 위 꼭지점을 잇는 벡터를 구해서 그 중간점을 결과값으로
        Vector3 v1 = centerP1ToP2 - p3;
        p0Position = p3 + v1 * 0.5f;

        return p0Position;
    }

    public static bool GetLineIntersection4Points2D(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 p0)
    {
        //활용식
        //x1 + t(x2 - x1) = x3 + s(x4 - x3)
        //y1 + t(y2 - y1) = y3 + s(y4 - y3)

        //1식을 t에 대해서 정리 하면서 남는 s는 2식을 s에 대해서 정리된 식으로 대체
        float t = ((p3.x - p1.x) * (p4.y - p3.y) + (p1.y - p3.y) * (p4.x - p3.x)) /
                  ((p2.x - p1.x) * (p4.y - p3.y) + (p1.y - p2.y) * (p4.x - p3.x));

        //float s = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) /
        //          ((p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x));

        if (t > 1.0f || t < 0.0f)
        {
            return false;//두 선의 교점은 존재하지 않음
        }

        float tParent = (p2.x - p1.x) * (p4.y - p3.y) + (p1.y - p2.y) * (p4.x - p3.x);
        if (tParent == 0)
        {
            return false;//두 선은 평행
        }

        float x = p1.x + t * (p2.x - p1.x);
        float y = p1.y + t * (p2.y - p1.y);

        Vector2 pointIntersection = new Vector2(x, y);

        p0 = pointIntersection;

        return true;
    }
    public static bool GetLineIntersection4Points3D(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, ref Vector3 p0)
    {


        return true;
    }
    public static bool GetLineIntersectionPointAndVector3D(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, ref Vector3 p0)
    {


        return true;
    }



    #endregion Math, Vector

    #region String Function
    public static void StringRemoveSpace(string str)
    {
        str.Trim();
        //대소문자
        //str.ToUpper();
        //str.ToLower();

        //문자열 추출
        //str.IndexOf();
        //str.LastIndexOf();
        //str.Substring(4,8);, str.Substring(4);
    }
    #endregion String Function

    #region Application System
    //SystemLanguage sl = Application.systemLanguage;//참고
    //RuntimePlatform rp = Application.platform;//참고
    //NativeBK.GAID
    //NativeBK.LocaleData.code

    #endregion Application System

    #region etc
    //public Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
    //public void loadSprite(string spriteBaseName)
    //{
    //    Sprite[] allSprites = Resources.LoadAll<Sprite>(spriteBaseName);
    //    if (allSprites == null || allSprites.Length <= 0)
    //    {
    //        Debug.LogError("The Provided Base-Atlas Sprite `" + spriteBaseName + "` does not exist!");
    //        return;
    //    }

    //    for (int i = 0; i < allSprites.Length; i++)
    //    {
    //        spriteDic.Add(allSprites[i].name, allSprites[i]);
    //        MakeFile(allSprites[i]);
    //    }
    //}
    //void MakeFile(Sprite sprite)
    //{
    //    try
    //    {
    //        Debug.Log(string.Format("{0} : {1}", sprite.name, sprite.rect.ToString()));
    //        Rect rect = sprite.rect; //분리할 스프라이트의 시작 좌표와 사이즈
    //        Texture2D mainTex = sprite.texture; //스프라이트의 메인 텍스쳐를 가져옴
    //        //새로 만들어질 텍스쳐, sprite.texture.format 이건 메인 텍스쳐의 포맷을 그대로 사용
    //        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, sprite.texture.format, false);
    //        //메인 텍스쳐에서 스프라이트의 영역 만큼 픽셀 값을 가져옴
    //        Color[] c = mainTex.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
    //        tex.SetPixels(c);// 새 텍스쳐에 픽셀값을 입힘
    //        tex.Apply(); // 적용
    //        var bytes = tex.EncodeToPNG(); // PNG byte로 형태로 만듬. JPG는 EncodeToJPG 사용 
    //        string savePath = string.Format("{0}/{1}.png", Application.persistentDataPath, sprite.name); //저장할 파일 위치
    //        Object.DestroyImmediate(tex, true); //새텍스쳐는 쓸일이 없으므로 삭제
    //        System.IO.File.WriteAllBytes(savePath, bytes); //파일로 쓰기
    //        Debug.Log("MakeFile : " + sprite.name);
    //    }
    //    catch (System.Exception ex)
    //    {

    //    }

    //}


    

    #endregion etc

}