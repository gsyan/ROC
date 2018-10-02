using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconImage : MonoBehaviour
{
    private static Dictionary<long, Texture2D> _imageDic = new Dictionary<long, Texture2D>();

    [SerializeField]
    protected UIDownloadTexture _downloadTexture = null;

    [SerializeField]
    protected Image _sprite = null;

    public void SetNPC(string npcName)
    {
        UseDownloadTexture(false);
        //_sprite.sprite = Utility.GetNpcIconFromKeyName(npcName);
    }

    public void SetFriend(FriendInfo info)
    {
        //_sprite.sprite = Utility.GetCharacterIcon(info.avataID);

        Texture2D tex;
        if(_imageDic.TryGetValue(info.usn, out tex))
        {
            UseDownloadTexture(true);
            _downloadTexture.UpdateTexture(tex);
        }
        else
        {
            UseDownloadTexture(false);
            _downloadTexture.UpdateTexture(info.socialThumnailURL, delegate (Texture2D tex2D) {
                if( tex2D != null)
                {
                    _imageDic.Add(info.usn, tex2D);
                }
                UseDownloadTexture(tex2D != null);
            });
        }
    }



    private void UseDownloadTexture(bool isTexture)
    {
        _downloadTexture.enabled = isTexture;
        _sprite.enabled = !isTexture;
    }
}
