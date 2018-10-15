using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelItemRecommedPackage : MonoBehaviour
{
    private ProductInfo_Package _productInfo;
    private ProductData data = null;


    public void Setup(ProductInfo_Package info)
    {
        _productInfo = info;

        string[] strOrgImgUrl;
        string strImgUrl = "";

        data = GData.Instance.GetProductData(info.productID);
        if(data != null)
        {

        }










    }





}
