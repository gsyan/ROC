using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCondition
{
    public string app_version = "";
    public int patch_number;
    public string min_app_version = "";
    public int min_patch_number;
    
    public bool is_opened;
    public bool is_regular;
    public int close_hour;
    public int close_minute;
    public int open_hour;
    public int open_minute;

    public ServerType server_type;
    public int server_group;

    public string playstore_download_url = "";
    public string nstore_download_url = "";
    public string onestore_download_url = "";
    public string application_download_url = "";

    public string tester_app_version = "";
    public string[] tester = new string[2];

}

//[System.Serializable]
//public class ServerAddress
//{
//	[System.Serializable]
//	public class Element
//	{
//		public string version;
//		public string address;
//		public int port;
//	}

//	public string default_address;
//	public int default_port;
//	public List<Element> address_list;

//	public bool GetServerInfo(string version, out string address, out int port)
//	{
//		for (int i = 0; i < address_list.Count; i++)
//		{
//			if (address_list[i].version == version)
//			{
//				address = address_list[i].address;
//				port = address_list[i].port;

//				return true;
//			}
//		}

//		address = default_address;
//		port = default_port;

//		return false;
//	}
//}