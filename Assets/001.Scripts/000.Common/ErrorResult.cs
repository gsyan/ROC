public enum ErrorResult : int
{
    OK = 0,                                         // 성공
    Fail,

    AlreadyPurchasedItem,                           // 이미 구입한 상품
    FailedEnterRoom,                                // 방 입장 실패
    MaximumValue,                                   // 최대 값에 도달 하였습니다.
    ServerInternalProblem,                          // 게임서버 내부로직 이상
    CantExcuteEpicQuest,                            // 에픽 퀘스트에 실행 불가 // 퀘스트포기, 퀘스트 즉시 완료
    ToShortLength,                                  // 길이가 짧음(닉네임 최소 한글 1글자 영문 2문자가 입력 되지 않았음)
    ToLongLength,                                   // 길이가 김(닉네임 최대 한글 8글자 영문 16문자를 초과 하였음)
    FindForbiddenWord,                              // 금지어가 검출 되었음
    RefreshData,                                    // 데이터가 변경 되었습니다.
    NewSocialAccount,                               // 새로운 유저 정보
    BattleFail,                                     // 전투 클리어 실패
    EquipedItem,                                    // 장착중인 아이템
    BlockedAccount,                                 // 블럭된 계정
    SessionClosed,                                  // 세션 끊김

    ExistUser = 100,                                // 중복 로그인 
    ExistItem,                                      // 이미 존재하는 아이템
    ExistRevive,                                    // 이미 부활을 사용 했음
    ExistsReward,                                   // 이미 보상을 받았음
    ExistsFriendRequest,                            // 받은 친구 요청이 존재함
    ExistsFriend,                                   // 이미 친구임
    ExistsFriendResponse,                           // 보낸 친구 요청이 존재함
    ExistsReceipt,                                  // 이미 지급된 아이템 영수증
    ExistsCoupon,                                   // 이미 사용한 쿠폰
    ExitstsCharacter,                               // 이미 캐릭터가 있음

    NotFoundServer = 200,                           // 서버를 찾을 수 없음
    NotFoundRoom,                                   // 방을 찾을 수 없습니다.
    NotFoundUser,                                   // 유저를 찾을 수 없음
    NotFoundData,                                   // 게임 데이터를 찾을 수 없습니다.
    NotFoundSession,                                // 유저 세션을 찾을 수 없습니다.
    NotFoundItem,                                   // 아이템을 찾을 수 없습니다.
    NotFoundCouponData,                             // 쿠폰 데이터를 찾을 수 없습니다.
    NotFoundFriendInfo,                             // 친구 정보를 찾을 수 없습니다
    NotFoundCharacter,                              // 캐릭터를 찾을 수 없습니다.

    NotEnoughItem = 300,                            // 아이템 부족
    NotEnoughLevel,                                 // 레벨이 충분하지 않습니다.
    NotEnoughGemLevel,                              // 보석 레벨이 충분하지 않습니다.
    NotEnoughDungeonStar,                           // 던전 클리어 등급이 충분하지 않습니다.
    NotEnoughCount,                                 // 남은 수량이 부족합니다. 
    NotEnoughTime,                                  // 시간이 부족합니다.
    NotEnoughFriendCount,                           // 친구 등록 공간이 부족합니다.
    NotEnoughFriendCount2,                          // 친구의 친구 등록 공간이 부족합니다.

    NotValidCharacterType = 400,                    // 유효하지 않은 캐릭터 타입
    NotValidWeaponType,                             // 유효하지 않은 무기 타입
    NotValidConsumableType,                         // 유효하지 않은 소모성 아이템 타입
    NotValidDungeonId,                              // 던전 아이디가 유효하지 않습니다.
    NotValidDungeonRoomId,                          // 던전룸 아이디가 유효하지 않습니다.
    NotValidIndex,                                  // Index 값이 유효하지 않습니다. 
    NotValidArgument,                               // 잘못된 인자 값입니다.
    NotValidUserID,                                 // 잘못된 유저 아이디
    NotValidUserName,                               // 잘못된 유저 이름
    NotValidDevice,                                 // 디바이스가 일치하지 않음
    NotValidCharacter,                              // 사용할수 없는 문자
    NotValidFacebookInfo,                           // Facebook 정보가 잘못됨
    NotValidProductID,                              // 제품 아이디가 일치하지 않음
    NotValidReceipt,                                // 영수증 정보가 일치하지 않음
    NotValidCouponID,                               // 쿠폰 번호가 잘못 되었음
    NotValidServerName,                             // 서버 Side 에러
    NotValidDecomposeItem,                          // 분해 불가능한 아이템
    NotValidItem,                                   // 잘못된 아이템

    ExistServerName = 50000,                        // 서버 이름 중복
    ExistServer,                                    // 서버 중복
    CurrencyCountUnderZero,                         // 재화 수량이 0보다 작음
    CannotUserCreate,			                    // 캐릭생성 불가
}
