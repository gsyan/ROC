using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPurchaser : MonoBehaviour, IBaseIAP
{
    public void Initialize() { }
    public void BuyProduct(string gameProductID, string marketProductID, string price)
    {
        PurchaserHelper.PurchaseResult sendResult = new PurchaserHelper.PurchaseResult();

        sendResult.orderNo = "";
        sendResult.purchaseData = "";
        sendResult.signature = "";
        sendResult.marketProductID = PurchaserHelper.Instance.MarketProductID;
        sendResult.gameProductID = PurchaserHelper.Instance.GameProductID;
        sendResult.price = PurchaserHelper.Instance.ProductPrice;
        sendResult.resultType = PurchaserHelper.PurchaseResult.ResultType.Success;

        Messenger<PurchaserHelper.PurchaseResult>.Broadcast(EventKey.CashProductPurchaseResult, sendResult);
    }
    public void ProcessRemainPurchase() { }
}
