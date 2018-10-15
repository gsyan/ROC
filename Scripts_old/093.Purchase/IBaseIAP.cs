
public interface IBaseIAP
{
    void Initialize();
    void BuyProduct(string gameProductID, string marketProductID, string price);
    void ProcessRemainPurchase();
}
