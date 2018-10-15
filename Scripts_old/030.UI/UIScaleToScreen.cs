using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ui 크기를 스크린에 맞추는 
public class UIScaleToScreen : MonoBehaviour
{
    public enum ScaleType
    {
        Both,
        Width,
        Height,
    }
    public ScaleType scaleType;

    /// <summary>
    /// 0.0f ~ 1.0f, 1.0f is full size of screen
    /// </summary>
    [Range(0.0f,2.0f)]
    public float scale = 1.0f;
    
    private int lastWidth = 0;
    private int lastHeight = 0;

    private void Update ()
    {
        if(lastWidth != Screen.width || lastHeight != Screen.height)
        {
            UIRoot root = NGUITools.FindInParents<UIRoot>(gameObject);
            if(root != null)
            {
                switch(scaleType)
                {
                    //////////////////////////////////////////////////////////////////////////////
                    case ScaleType.Both:
                        {
                            float scaleX = Screen.width * root.pixelSizeAdjustment * scale + 2.0f;
                            float scaleY = Screen.height * root.pixelSizeAdjustment * scale + 2.0f;

                            BoxCollider bc = GetComponent<BoxCollider>();
                            if(bc != null)
                            {
                                bc.size = new Vector3(scaleX, scaleY, 1.0f);
                            }

                            UIWidget w = GetComponent<UIWidget>();
                            if(w != null)
                            {
                                w.width = (int)scaleX;
                                w.height = (int)scaleY;
                            }
                        }
                        break;
                    //////////////////////////////////////////////////////////////////////////////
                    case ScaleType.Width:
                        {
                            float scaleX = Screen.width * root.pixelSizeAdjustment * scale + 2.0f;

                            BoxCollider bc = GetComponent<BoxCollider>();
                            if (bc != null)
                            {
                                bc.size = new Vector3(scaleX, bc.size.y, 1.0f);
                            }

                            UIWidget w = GetComponent<UIWidget>();
                            if (w != null)
                            {
                                w.width = (int)scaleX;
                            }
                        }
                        break;
                    //////////////////////////////////////////////////////////////////////////////
                    case ScaleType.Height:
                        {
                            float scaleY = Screen.height * root.pixelSizeAdjustment * scale + 2.0f;

                            BoxCollider bc = GetComponent<BoxCollider>();
                            if (bc != null)
                            {
                                bc.size = new Vector3(bc.size.x, scaleY, 1.0f);
                            }

                            UIWidget w = GetComponent<UIWidget>();
                            if (w != null)
                            {
                                w.height = (int)scaleY;
                            }
                        }
                        break;
                }
            }
            
            lastWidth = Screen.width;
            lastHeight = Screen.height;
        }


    }
}
