using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelRecommendPackage : UIPanelBase
{
    public Callback onClose = null;

    [SerializeField]
    protected UIWrapContentExtension _wrapContent = null;

    private List<ProductInfo_Package> _productInfoList = new List<ProductInfo_Package>();


    public override void Awake()
    {
        base.Awake();
        _wrapContent.onCreateItem = CreateItem;
        _wrapContent.onUpdateItem = UpdateItem;
    }
    private GameObject CreateItem()
    {
        return Utility.Instantiate("UI/PanelItem Recommend Package") as GameObject;
    }
    private void UpdateItem(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex < _productInfoList.Count)
        {
            go.SetActive(true);
            UIPanelItemRecommedPackage pirp = go.GetComponent<UIPanelItemRecommedPackage>();
            if (pirp != null)
            {
                //pirp.
            }
        }
        else
        {
            go.SetActive(false);
        }
    }

    public override void OnActive()
    {
        base.OnActive();
        Messenger<ProductInfo_Package>.AddListener(EventKey.UpdatePakage, DeleteInfo);
        Messenger<int>.AddListener(EventKey.BuyProduct, BuyProduct);
    }
    public override void OnDeactive()
    {
        base.OnDeactive();


    }
    private void DeleteInfo(ProductInfo_Package info)
    {
        if (_productInfoList.Remove(info))
        {
            UpdateData();
        }
    }
    private void BuyProduct(int productId)
    {
        for (int i = 0; i < _productInfoList.Count; i++)
        {
            if (_productInfoList[i].productID == productId)
            {
                _productInfoList[i].bPurchased = true;
                break;
            }
        }

        RefreshList();
    }
    void RefreshList()
    {

        _productInfoList = GInfo.recommentPackageList;
        _productInfoList.Sort(Sort);

        UpdateData();

    }


    private void UpdateData()
    {
        _wrapContent.UpdateItemCount(_productInfoList.Count);
    }


    private int Sort(ProductInfo_Package a, ProductInfo_Package b)
    {
        ProductData pda = GData.Instance.GetProductData(a.productID);
        ProductData pdb = GData.Instance.GetProductData(b.productID);

        if( pda == null || pdb == null)
        {
            return 0;
        }

        return pda.sortNo.CompareTo(pdb.sortNo);
    }

    public void OnClose()
    {
        UISystem.Instance.HidePanel(cachedTransform);
        if( onClose != null)
        {
            onClose();
        }
    }



    



}
