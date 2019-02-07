using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCondition
{
    public string min_app_version = "";
    public int patch_number;
    public int min_patch_number;
    
    public bool is_opened;
    public bool is_regular;
    public int close_hour;
    public int close_minute;
    public int open_hour;
    public int open_minute;

    public string playstore_download_url = "";
    public string nstore_download_url = "";
    public string onestore_download_url = "";
    public string application_download_url = "";

    public string tester_app_version = "";
    public string[] tester = new string[2];

}