using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaserHelper : SingletonGameObject<PurchaserHelper>
{
    private bool _isRequest = false;
    private bool _isInit = false;

    public class PurchaseResult
    {
        public enum ResultType
        {
            Success = 0,
            UserCancel,
            Fail,
        }

        public string gameProductID = "";
        public string marketProductID = "";
        public string price = "";
        public string orderNo = "";
        public string purchaseData = "";
        public string signature = "";
        public ResultType resultType;

        public PurchaseResult() { }

        public PurchaseResult(string jsonString)
        {
            PurchaseResult res = JsonUtility.FromJson<PurchaseResult>(jsonString);
            if (res == null) { return; }

            gameProductID = res.gameProductID;
            marketProductID = res.marketProductID;
            price = res.price;
            orderNo = res.orderNo;
            purchaseData = res.purchaseData;
            signature = res.signature;

            if (gameProductID == null) { gameProductID = ""; }
            if (marketProductID == null) { marketProductID = ""; }
            if (price == null) { price = ""; }
            if (orderNo == null) { orderNo = ""; }
            if (purchaseData == null) { purchaseData = ""; }
            if (signature == null) { signature = ""; }
        }

        public string GetToJsonString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    private string _productKey;
    private IBaseIAP _purchaser = null;

    public MarketType marketType
    {
        get
        {
#if BILLING_UNITY
    #if UNITY_IOS
			return MarketType.AppleStore;
    #elif UNITY_ANDROID
            return MarketType.GoogleStore;
    #endif
#endif //BILLING_UNITY

#if BILLING_NSTORE
			return MarketType.NStore;
#endif //BILLING_NSTORE

#if BILLING_ONESTORE
			return MarketType.OneStore;
#endif //BILLING_ONESTORE
        }
    }

    public string GameProductID { get;set; }
    public string MarketProductID { get; set; }
    public string ProductPrice { get; set; }
    
    //<게임에서 관리되는 product ID, 스토어에서 관리되는 product ID>
    private Dictionary<string, string> _productIDList = new Dictionary<string, string>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        Messenger<PurchaseResult>.AddListener(EventKey.CashProductPurchaseResult, ProductPerchaseResult);
    }
    private void OnDisable()
    {
        Messenger<PurchaseResult>.RemoveListener(EventKey.CashProductPurchaseResult, ProductPerchaseResult);
    }

    public void Init()
    {
        if(_isInit) { return; }
        _isInit = true;

#if UNITY_EDITOR
        _purchaser = gameObject.AddComponent<VirtualPurchaser>();
#elif BILLING_UNITY
        //_purchaser = gameObject.AddComponent<UnityPurchaser>();
        _purchaser = gameObject.AddComponent<VirtualPurchaser>();
#elif BILLING_NSTORE
        _purchaser = gameObject.AddComponent<NIAPPurchaser>();
#elif BILLING_ONESTORE
        _purchaser = gameObject.AddComponent<OneStoreIapManager>();
#endif

        List<ProductData> productList = GData.Instance.GetProductDatas();










    }




    private void ProductPerchaseResult(PurchaseResult result)
    {

    }


    public Dictionary<string, string> GetProductIDList()
    {
        return _productIDList;
    }


}
