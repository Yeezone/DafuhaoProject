using UnityEngine;
using System.Collections;
using LitJson;
using System;



public class RenwuManager : MonoBehaviour
{
    public UILabel userId;
    public UIButton notget_btn;
    public UIButton get_btn;
    public UIButton ok_btn;


     void OnEnable()
    {
        StartCoroutine(GetRenwu());
    }

    IEnumerator GetRenwu()
    {
        string userIdStr = userId.text;
        string url = "http://192.168.6.244:8899/dwc/sapi/checkFlstate.action?userid="+userIdStr;


        var www = new WWW(url);
        yield return www;
        var jsonData = JsonMapper.ToObject<RenWuJson>(www.text);

        if (jsonData.retcode == "1")
        {
            if (!get_btn.gameObject.activeSelf)
            {
                get_btn.gameObject.SetActive(true);               
            }
           else if (notget_btn.gameObject.activeSelf)
            {
                notget_btn.gameObject.SetActive(false);
            }
           else if (ok_btn.gameObject.activeSelf)
            {
                ok_btn.gameObject.SetActive(false);
            }
        }
       else if (jsonData.retcode == "0")
        {
            if (get_btn.gameObject.activeSelf)
            {
                get_btn.gameObject.SetActive(false);
            }
            else if (notget_btn.gameObject.activeSelf)
            {
                notget_btn.gameObject.SetActive(false);
            }
            else if (!ok_btn.gameObject.activeSelf)
            {
                ok_btn.gameObject.SetActive(true);
            }
        }
      else  if (jsonData.retcode == "-1")
        {
            if (get_btn.gameObject.activeSelf)
            {
                get_btn.gameObject.SetActive(false);
            }
            else if (!notget_btn.gameObject.activeSelf)
            {
                notget_btn.gameObject.SetActive(true);
            }
            else if (ok_btn.gameObject.activeSelf)
            {
                ok_btn.gameObject.SetActive(false);
            }
        }

    }

    public void OK_BTN_OnClick()
    {
        StartCoroutine(SendPost());
    }

    IEnumerator SendPost()
    {
        string userIdStr = userId.text;
        string url = "http://192.168.6.244:8899/dwc/sapi/getFl.action?userid=" + userIdStr;
        WWW postData = new WWW(url);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
        }
        else
        {
            Debug.Log(postData.text);
        }
    }
}




[Serializable]
public class RenWuJson
{
    public string message;
    public string retcode;
    public string result;
    public int score;
}
