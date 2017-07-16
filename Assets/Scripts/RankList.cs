using UnityEngine;
using System.Collections;
using LitJson;

public class RankList : MonoBehaviour {
    public GameObject[] Item;

    void Start()
    {
        StartCoroutine(GetRankList());
    }
      
   


     IEnumerator GetRankList()
    {
        string url = "http://192.168.6.244:8899/dwc/sapi/getTwentybang.action" ;
        var www = new WWW(url);
        yield return www;
        var jsonData = JsonMapper.ToObject<getTwentybang>(www.text);
        for (int i = 0; i < Item.Length; i++)
        {
            string face_name = "face_" + jsonData.result[i].faceId.ToString();
            Item[i].GetComponent<RankListNum>().head_Img.spriteName = face_name;
            string user_name = jsonData.result[i].nickName.ToString();
            Item[i].GetComponent<RankListNum>().user_name.text = user_name;
            string user_gold = (jsonData.result[i].score + jsonData.result[i].insureScore).ToString();
            Item[i].GetComponent<RankListNum>().user_gold.text = user_gold;
        }


    }
}
