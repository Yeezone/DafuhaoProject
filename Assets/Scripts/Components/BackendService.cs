using System;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.Utility;
using com.QH.QPGame.Services.Data;
using LitJson;
using UnityEngine;

namespace com.QH.QPGame.Lobby
{
    public class BackendService : MonoBehaviour
    {
        private const float TIMEOUT = 10.0f;
        public class BS_MoneyRequest
        {
            public string 						Url;
            public uint 						UserID;
            public int 							Amount;
            public string 						Remark;
			public string						passWord;
			public string						Key;
			public int 							OrderType;///操作类型  充值200 兑奖201
			public Action<BS_MoneyResult> 		Result;
        }

		public class BS_MoneyResult
		{
			public int      Code;
			public string   Msg;
		}

        public class BS_MoneyRecordRequest
        {
            public string           			Url;
            public uint             			UserID;
            public uint             			PageIndex;
            public uint             			PageSize;
			public DateTime         			OrderTime;
			public string						Key;
			public int 							OrderType;///操作类型 充值200 兑奖201
            public Action<BS_MoneyRecord> 		Result;
        }

        public class BS_MoneyRecord
        {
            public int 						Code;
			public string 					Msg;
			public uint 					AllCount;
            public List<BS_MoneyRecordItem> Data;
        }

		public class BS_NoticeRequest
		{
			public string           			Url;
			public Action<BS_NoticeResult> 		Result;
		}
		
		public class BS_NoticeResult
		{
			public int      Code;
			public string   Msg;
		}

		public class BS_ChangeMoneyCancelRequest
		{
			public string           						Url;
			public uint             						UserID;
			public string             						CancelReason;
			public string             						ApplyNumber;
			public string									Key;
			public int 										OrderType;///操作类型 充值200 兑奖201
			public Action<BS_ChangeMoneyCancelResult> 		Result;
		}

		public class BS_ChangeMoneyCancelResult
		{
			public int      Code;
			public string   Msg;
		}

		public void Recharge(uint uid, Int64 amount, string remark,string key, Action<BS_MoneyResult> result)
        {
            //string url = GameApp.GameData.BackStorgeUrl + "/ProxyHandler/User.ashx?action=recharge";
		    string url = GameApp.GameData.BackStorgeUrl + GlobalConst.URL.Recharge;
            var req = new BS_MoneyRequest();
            req.Url = url;
            req.UserID = uid;
            req.Amount = (int)amount;
            req.Remark = remark;
            req.Result = result;
			req.OrderType = 200;
			req.passWord = "";
			req.Key = key;
            StartCoroutine("RechargeOrExchange", req);
        }

		public void Exchange(uint uid, Int64 amount, string remark,string password, string key,Action<BS_MoneyResult> result)
        {
            //string url = GameApp.GameData.BackStorgeUrl + "/ProxyHandler/User.ashx?action=recharge";
		    string url = GameApp.GameData.BackStorgeUrl + GlobalConst.URL.Exchange;
            var req = new BS_MoneyRequest();
            req.Url = url;
            req.UserID = uid;
            req.Amount = (int)amount;
            req.Remark = remark;
            req.Result = result;
			req.OrderType = 201;
			req.passWord = password;
			req.Key = key;
            StartCoroutine("RechargeOrExchange", req);
        }

        public IEnumerator RechargeOrExchange(BS_MoneyRequest req)
        {
            var data = new WWWForm();
            data.AddField("UserID", req.UserID.ToString());
			data.AddField("payAmount", req.Amount.ToString());
			data.AddField("submitRemark", req.Remark);
			data.AddField("OrderType",req.OrderType.ToString());
			data.AddField("password",req.passWord);
			data.AddField("token",req.Key);

            float elapsedTime = 0.0f;
            var www = new WWW(req.Url, data);
            yield return www;

            while (!www.isDone)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= TIMEOUT) break;
                yield return new WaitForFixedUpdate();
            }

            if (!string.IsNullOrEmpty(www.error) || string.IsNullOrEmpty(www.text))
            {
                req.Result(null);
                yield break;
            }

			var jsonData = JsonMapper.ToObject<BS_MoneyResult>(www.text);
			req.Result(jsonData);
            yield break;
        }

		public void GetRechargeRecord(uint uid, uint pageIndex, uint pageSize, DateTime dt,string key, Action<BS_MoneyRecord> result) 
        {
            //string url = GameApp.GameData.BackStorgeUrl + "/ProxyHandler/User.ashx?action=getrecharge";
		    string url = GameApp.GameData.BackStorgeUrl + GlobalConst.URL.GetRecord;
            var req = new BS_MoneyRecordRequest();
			req.Url = url;
			req.OrderTime = dt;
            req.PageSize = pageSize;
            req.PageIndex = pageIndex;
            req.UserID = uid;
            req.Result = result;
			req.OrderType = 200;
			req.Key = key;
            StartCoroutine("GetRechargeOrExchangeRecord", req);
        }

		public void GetExchangeRecord(uint uid, uint pageIndex, uint pageSize, DateTime dt,string key, Action<BS_MoneyRecord> result)
        {
            //string url = GameApp.GameData.BackStorgeUrl + "/ProxyHandler/User.ashx?action=getrecharge";
            string url = GameApp.GameData.BackStorgeUrl + GlobalConst.URL.GetRecord;
            var req = new BS_MoneyRecordRequest();
			req.Url = url;
			req.OrderTime = dt;
            req.PageSize = pageSize;
            req.PageIndex = pageIndex;
            req.UserID = uid;
            req.Result = result;
			req.OrderType = 201;
			req.Key = key;
            StartCoroutine("GetRechargeOrExchangeRecord", req);
        }

        public IEnumerator GetRechargeOrExchangeRecord(BS_MoneyRecordRequest req)
        {
            var data = new WWWForm();
            data.AddField("UserID", req.UserID.ToString());
			data.AddField("currentPageIndex", req.PageIndex.ToString());
			data.AddField("pageSize", req.PageSize.ToString());
			data.AddField("date", req.OrderTime.ToString("yyyyMMdd"));
			data.AddField("OrderType", req.OrderType.ToString());
			data.AddField("token",req.Key);
            float elapsedTime = 0.0f;
            var www = new WWW(req.Url, data);
            yield return www;

            while (!www.isDone)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= TIMEOUT) break;
                yield return new WaitForFixedUpdate();
            }

            if (!string.IsNullOrEmpty(www.error) || string.IsNullOrEmpty(www.text))
            {
                req.Result(null);
                yield break;
            }

            var records = JsonMapper.ToObject<BS_MoneyRecord>(www.text);
            req.Result(records);
            yield break;
        }

		public  void GetNotice(  Action<BS_NoticeResult> result )
		{
            //string url = GameApp.GameData.BackStorgeUrl + "/ProxyHandler/User.ashx?action=getNotice";
		    string url = GameApp.GameData.BackStorgeUrl + GlobalConst.URL.GetNotice;
			var req = new BS_NoticeRequest();
			req.Url = url;
			req.Result = result;
			StartCoroutine("GetPublication",req);
		}

		public IEnumerator GetPublication( BS_NoticeRequest req )
        {
			var data = new WWWForm();
			System.Random rad = new System.Random();
			data.AddField("rad",rad.Next(0, 10).ToString());
			float elapsedTime = 0.0f;
			var www = new WWW(req.Url, data);
			yield return www;
			
			while (!www.isDone)
			{
				elapsedTime += Time.deltaTime;
				if (elapsedTime >= TIMEOUT) break;
                yield return new WaitForFixedUpdate();
			}
			
			if (!string.IsNullOrEmpty(www.error) || string.IsNullOrEmpty(www.text))
			{
				req.Result(null);
				yield break;
			}

			var jsonData = JsonMapper.ToObject<BS_NoticeResult>(www.text);
			req.Result(jsonData);
            yield return null;
        }

		public void RechangeCancel( uint uid, string dwCancelReason, string dwApplyNumber,string key, Action<BS_ChangeMoneyCancelResult> result)
		{
            //string url = GameApp.GameData.BackStorgeUrl + "/ProxyHandler/User.ashx?action=RechangeCancel";
		    string url = GameApp.GameData.BackStorgeUrl + GlobalConst.URL.CancelRecharge;
			var req = new BS_ChangeMoneyCancelRequest();
			req.Url = url;
			req.UserID = uid;
			req.OrderType = 200;
			req.CancelReason = dwCancelReason;
			req.ApplyNumber = dwApplyNumber;
			req.Result = result;
			req.Key = key;
			StartCoroutine("ChangeCancel", req);

		}

		public void ExchangeCancel( uint uid, string dwCancelReason, string dwApplyNumber,string key, Action<BS_ChangeMoneyCancelResult> result)
		{
            //string url = GameApp.GameData.BackStorgeUrl + "/ProxyHandler/User.ashx?action=ExchangeCancel";
		    string url = GameApp.GameData.BackStorgeUrl + GlobalConst.URL.CancelExchange;
			var req = new BS_ChangeMoneyCancelRequest();
			req.Url = url;
			req.UserID = uid;
			req.OrderType = 201;
			req.CancelReason = dwCancelReason;
			req.ApplyNumber = dwApplyNumber;
			req.Result = result;
			req.Key = key;
			StartCoroutine("ChangeCancel", req);
		}

		public IEnumerator ChangeCancel( BS_ChangeMoneyCancelRequest req )
		{
			var data = new WWWForm();
			data.AddField("UserID", req.UserID.ToString());
			data.AddField("CancelReason", req.CancelReason);
			data.AddField("ApplyNumber", req.ApplyNumber);
			data.AddField("OrderType", req.OrderType.ToString());
			data.AddField("token",req.Key);
			float elapsedTime = 0.0f;
			var www = new WWW(req.Url, data);
			yield return www;
			
			while (!www.isDone)
			{
				elapsedTime += Time.deltaTime;
				if (elapsedTime >= TIMEOUT) break;
                yield return new WaitForFixedUpdate();
			}
			
			if (!string.IsNullOrEmpty(www.error) || string.IsNullOrEmpty(www.text))
			{
				req.Result(null);
				yield break;
			}
			
			var jsonData = JsonMapper.ToObject<BS_ChangeMoneyCancelResult>(www.text);
			req.Result(jsonData);
			yield return null;
		}

    }
}
