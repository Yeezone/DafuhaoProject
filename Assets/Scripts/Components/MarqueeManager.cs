using System;
using System.Collections.Generic;

namespace com.QH.QPGame.Lobby
{
	public class MarqueeManager
	{
        public class MarqueeData
        {
            public UInt16   Type;							//消息类型
            public Int32    MsgNumberID;                    //消息编号
            public UInt16   ID;						        //消息ID(选择预设)
            public UInt64   StartTime;                      //开始播放时间
            public string   Message;                        //消息
            public UInt16   Weight;                         //排序优先级
            public float    MsgInterval;                    //每条消息播放时间
            public UInt16   MsgPlayTime;                    //播放时间                        
        };

	    public class MarqueeConfig
	    {
	        public UInt16 ID;
	        public string Template;
	    }

        //组装消息(1 系统消息 2 游戏消息)
        public enum TypeID
        {
            MarType_SYS= 1,
            MarType_Game,
        };

	    // 检测是否进入捕鱼游戏并且需要旋转屏幕
        public bool Reverse = false;

        public List<MarqueeData> MessageList = new List<MarqueeData>();
        private Dictionary<int, string> templates = new Dictionary<int, string>();

        public void Initialize()
	    {
            LoadTemplates();
	    }

	    public void Clear()
	    {
            MessageList.Clear();
            templates.Clear();
	    }

	    private void LoadTemplates()
	    {
            templates.Clear();

			var text = GameApp.ResMgr.LoadTextInResources(GlobalConst.Res.MarqueeDataFileName);
	        if (string.IsNullOrEmpty(text))
	        {
	            return;
	        }

			var configData = LitJson.JsonMapper.ToObject<MarqueeConfig[]>(text);
	        foreach (var marqueeConfig in configData)
	        {
	            templates.Add(marqueeConfig.ID, marqueeConfig.Template);
	        }
	    }

	    public void Sort()
	    {
	        MessageList.Sort(delegate(MarqueeData left, MarqueeData right)
	            {
	                if (left.StartTime == right.StartTime)
	                {
	                    return left.Weight - right.Weight;
	                }
	                return (int) (left.StartTime - right.StartTime);
	            });
	    }

	    /*private void OnDestroy()
        {
            GameApp.Account.MarqueeMessageEvent -= Instance_MarqueeMessageEvent;

            templates.Clear();
	        templates = null;

            MessageList.Clear();
	        MessageList = null;
	    }*/

	    private UInt16 CheckLoop(UInt16 PlayCount, float Interval, UInt64 StartTime)
        {
            UInt16 count = PlayCount;
            if (Interval > 0)
	        {
                TimeSpan dt = DateTime.Now - DateTime.Parse("1970-01-01 08:00:00");
                Int64 tempTime = (Int64)dt.TotalSeconds - (Int64)StartTime;
                Int16 tempCount = (Int16)(tempTime / (Int64)Interval);
                if (tempCount > 0 && tempCount < PlayCount)
                {
                    count -= (UInt16)tempCount;
                }
                else if (tempCount >= PlayCount)
                {
                    count = 0;
                } 
	        }
            else
            {
                count = 0;
            }
            return count;
	    }

        private void AddMessage(
            UInt16 mType,
            UInt16 MsgID, 
            string szMessage, 
            UInt64 MessageStartTime,
            float MsgInterval, 
            Int32 msgNumberID, 
            UInt16 MsgPlayTime)
	    {
            var tempMarqueeMsg= new MarqueeData();
            tempMarqueeMsg.Type = mType;
            tempMarqueeMsg.ID = MsgID;
            tempMarqueeMsg.StartTime = MessageStartTime;
            tempMarqueeMsg.MsgInterval = MsgInterval;
            tempMarqueeMsg.MsgNumberID = msgNumberID;
            tempMarqueeMsg.MsgPlayTime = MsgPlayTime;

            if (templates.ContainsKey(MsgID))
            {
                var param = szMessage.Split(',');
                var strTemplate = templates[MsgID];
                string str = string.Format(strTemplate, param);
                tempMarqueeMsg.Message = str;
            }
            else
            {
                tempMarqueeMsg.Message = szMessage;
            }

            if (mType == (UInt16)TypeID.MarType_SYS)
            {
                tempMarqueeMsg.Weight = 1;
            }
            else
            {
                tempMarqueeMsg.Weight = 0;
            }

            MessageList.Add(tempMarqueeMsg);

            Sort();
        }

        public void AddData(
            UInt16 mType, 
            UInt16 MsgID,
            UInt16 MsgPlayCount, 
            float MsgInterval, 
            string szMessage, 
            UInt64 MessageStartTime,
            Int32 msgNumberID, 
            UInt16 MsgPlayTime)
		{
            //将已经保存的系统消息删除(暂时不用)
            //if (mType == (UInt16)MessageID.MarMSG_SYS)
            //{
            //    for (int i = MessageList.Count - 1; i >= 0; i--)
            //    {
            //        if (MessageList[i].Type == (UInt16)MessageID.MarMSG_SYS)
            //        {
            //            MessageList.RemoveAt(i);
            //        }
            //    }
            //}
	        UInt16  PlayCount =  CheckLoop(MsgPlayCount, MsgInterval, MessageStartTime);
            for (int i = 0; i < PlayCount; i++)
            {
                UInt64 Interval = (UInt64)i * (UInt64)MsgInterval;
                AddMessage(mType,MsgID,szMessage,(MessageStartTime + Interval),MsgInterval, msgNumberID, MsgPlayTime);
            }
		}

	    public MarqueeData PopMessage()
	    {
            Sort();
            return null;
	    }

	    public MarqueeData PeekMessage()
	    {
            Sort();

	        if (MessageList.Count == 0)
	        {
	            return null;
	        }

	        return MessageList[0];
	    }

	    public void RemoveMessage( MarqueeData data)
	    {
	        if (MessageList.Count == 0)
	        {
	            return;
	        }

	        MessageList.Remove(data);
	    }

	    public void RemoveMessage(int msgID)
	    {
            //查找ID删除
            if (msgID == 0)
            {
                //删除
                for (int i = MessageList.Count - 1; i >= 0; i--)
                {
                    if (MessageList[i].MsgNumberID > 0)
                    {
                        MessageList.RemoveAt(i);
                    }
                }
            }
            else
            {
                //删除
                for (int i = MessageList.Count - 1; i >= 0; i--)
                {
                    if (MessageList[i].MsgNumberID == msgID)
                    {
                        MessageList.RemoveAt(i);
                    }
                }
            }
	    }
	}
}

