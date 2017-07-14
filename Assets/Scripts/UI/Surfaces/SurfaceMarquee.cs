using UnityEngine;
using System;

namespace com.QH.QPGame.Lobby.Surfaces
{
	public class SurfaceMarquee : Surface 
    {
        //坐标
        private Vector2 bg_height;

        //游戏场景
        private bool IsBegin = false;

        //当前执行的游戏场景数据
        private MarqueeManager.MarqueeData currentMessage = null;

        //播放时间
        private float tempPlayTime = 0f;

	    private float messagePlayTime = 0f;

        void Start()
        {
            IsBegin = true;
        }

        void OnDestroy()
        {
            IsBegin = false;
        }

	    //		 Update is called once per frame
        void Update()
        {
            if (IsBegin)
            {
                int msgCount = GameApp.MarqueeMgr.MessageList.Count;
				if (msgCount <= 0)
				{
					return;
				}
				
                if (currentMessage == null)
                {
                    //使用DateTime.MinValue替换
                    TimeSpan dt = DateTime.Now - DateTime.Parse("1970-01-01 08:00:00");
                    if (GameApp.MarqueeMgr.MessageList[0].StartTime < dt.TotalSeconds)
                    {
                        currentMessage = GameApp.MarqueeMgr.PeekMessage();
                        messagePlayTime = currentMessage.MsgPlayTime;
                        tempPlayTime = Time.time + messagePlayTime;
                        //显示
                        showMsg(currentMessage.ID, currentMessage.Message);
                    }
                }
                else
                {
                    if (Time.time > tempPlayTime)
                    {
                        //一条消息结束,从List中删除
                        GameApp.MarqueeMgr.RemoveMessage(currentMessage);
                        currentMessage = null;
                        messagePlayTime = 0f;
                    }
                    else if ((tempPlayTime - Time.time) < ( messagePlayTime / 2))
                    {
						Text_lbl.gameObject.SetActive(false);
						Bg_spr.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void showMsg(UInt16 ID, string Msg)
        {
            // 如果进入捕鱼游戏并且需要旋转,跑马灯也会跟着旋转
            if (GameApp.MarqueeMgr.Reverse)
            {
                transform.rotation = new Quaternion(0.0f, 0.0f, 1.0f, 0.0f);
            }
            else
            {
                transform.rotation = new Quaternion();
            }

            Show(Msg);
        }



        //TODO 暂留，日后清理
        public UILabel	Text_lbl;
        public UISprite	Bg_spr;
		public void	Show(string _message)
		{
			Text_lbl.text = _message;
			TweenPosition tempTweenPos = Text_lbl.GetComponent<TweenPosition>();
			if(tempTweenPos!=null)
			{
				float tempPanelSize = Text_lbl.GetComponentInParent<UIPanel>().GetViewSize().x;
				float tempFrom = (float)Text_lbl.width/2+tempPanelSize/2;
				float tempTo = -(float)Text_lbl.width/2+tempPanelSize/2;
				if(tempTo>0) tempTo = 0;
				tempTweenPos.from = new Vector3(tempFrom,0f,0f);
				tempTweenPos.to = new Vector3(tempTo,0f,0f);
				tempTweenPos.duration = currentMessage.MsgInterval/2;
				tempTweenPos.ResetToBeginning();
				tempTweenPos.Play();
			}
		    Text_lbl.gameObject.SetActive(true);
			Bg_spr.gameObject.SetActive(true);
		}
	}
	
}

