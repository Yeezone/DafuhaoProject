using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.GameUtils;

namespace com.QH.QPGame.SHZ
{
	public class UIMaryManger : MonoBehaviour
	{
		public static UIMaryManger _instance = null;

	    public List<UIMaryTurItem> m_lTurnList = new List<UIMaryTurItem>();
	    private List<int> m_lRandTurnList = new List<int>();
	    public int m_iTargetIndex = 0;
	    public List<UIMaryRewadTimeItem> m_lTimeList = new List<UIMaryRewadTimeItem>();
		public AudioClip[] _Game3Sound = new AudioClip[6];
		public AudioClip[] _ResSound = new AudioClip[9];
		//限制圈数
	    public int m_iLimitCount = 2;
	    //旋转圈数
	    public int m_iTurnCount = 0;
	    //加速度
	    public float m_fAddSpeed = 0.02f;
	    //速度
	    public float m_fSpeed = 0.2f;
	    //开关
	    public bool m_bIsOpen = false;
	    //运行时间
	    public float m_fWorkTime = 0;
	    //跟随个数
	    public int m_iFollowCount = 1;
	    //跟随上限
	    public int m_iFollowLimitCount = 4;
	    //当前下表
	    public int m_iCurrentIndex = 0;
	    //是否是加速
	    public bool m_bIsAdd = false;

	    private int m_iAddCount = 0;

	    public delegate void MaryGameDelegate();
	    public MaryGameDelegate m_OnChange = null;

		public GameObject o_times;
		public GameObject o_allScore;
		public GameObject o_winScore;
		public GameObject o_betScore;
		public GameObject[] o_table3 = new GameObject[4];
		//结果音效索引
		byte[] m_lVoiceIndex = new byte[4];

		void Awake()
		{
			if (_instance == null){
				_instance = this;
			}
			Logger.UI.LogWarning("小玛丽");
		}

		void OnDestroy(){
			_instance = null;
		}

	    // Use this for initialization
	    void Start()
	    {
	    }

	    // Update is called once per frame
	    void Update()
	    {
	        TurnAnimation();
	    }


	    /// <summary>
	    /// 转盘
	    /// </summary>
	    private void TurnAnimation()
	    {
	        m_fWorkTime += Time.deltaTime;
	        if (m_bIsOpen && m_fWorkTime >= m_fSpeed)
	        {
	            m_iAddCount += 1;
	            m_iCurrentIndex -= 1;
				PlayVoice(0,_Game3Sound);
				if (m_iCurrentIndex < 0) m_iCurrentIndex += m_lTurnList.Count;

	            if (m_iCurrentIndex == m_iTargetIndex) m_iTurnCount += 1;
	            

	            if (m_iTurnCount >= m_iLimitCount) m_bIsAdd = false;
	            if (m_bIsAdd )
	            {
	                if (m_iFollowLimitCount > m_iFollowCount && m_iAddCount >= 5)
	                {
	                    m_iAddCount = 0;
	                    m_iFollowCount += 1;
	                }

	                m_fSpeed -= m_fAddSpeed;
	                if (m_fSpeed <= 0.03f) m_fSpeed = 0.03f;
	            }
	            if (m_bIsAdd == false )
	            {
	                if (m_iFollowCount > 1 && m_iAddCount >= 2)
	                {
	                    m_iAddCount = 0;
	                    m_iFollowCount -= 1;
	                }

	                m_fSpeed += 0.01f;
	            }
	            if (m_iTurnCount > m_iLimitCount && m_iCurrentIndex == m_iTargetIndex)
	            {
	                m_bIsOpen = false;

					if(m_lTurnList[m_iCurrentIndex].m_iID == 8)
					{
						Invoke("reCall",2.0f);
					}
					else if(m_lTurnList[m_iCurrentIndex].m_iID <8)
					{
						if(UIMaryResult.mList.Count>0){
							StartCoroutine(StartResult2());
						}else{
							Invoke("reCall",2.0f);
						}
					}
					refreshInfo();
	            };
	            SetCurrentIndex(m_iCurrentIndex);
	            m_fWorkTime = 0;
	        }
	    }

		void reCall()
		{
			if (m_OnChange != null) {
				m_OnChange();
			}
		}

	    //设置当前
	    public void SetCurrentIndex(int _iIndex)
	    {
	        for (int i = 0; i < m_lTurnList.Count; i++)
	        {
	            m_lTurnList[i].m_Sprite0.alpha = 0;
	        }
	        for (int i = 0; i < m_iFollowCount; i++)
	        {
	            int temp = i+_iIndex;
	            if(temp>= m_lTurnList.Count)temp -= m_lTurnList.Count;
	        
	            m_lTurnList[temp].m_Sprite0.alpha = 1.0f - i * 0.2f;
	        }
	    }
	    /// <summary>
	    /// 随机下标
	    /// </summary>
	    /// <param name="_iType">类型</param>
	    /// <returns></returns>
	    private int RandomIndex(int _iType)
	    {
	        m_lRandTurnList.Clear();
	        for (int i = 0; i < m_lTurnList.Count; i++)
	        {
	            if (m_lTurnList[i].m_iID == _iType)
	            {
	                m_lRandTurnList.Add(i);
	            }
	        }
	        int r = Random.Range(0, m_lRandTurnList.Count);
	        return m_lRandTurnList[r];
	    }

	    private void RectAnimation()
	    {
	        m_fWorkTime = 0;
	        m_bIsAdd = true;
	        m_iFollowCount = 1;
	        m_iTurnCount = 0;
	        m_fSpeed = 0.2f;
	    }

	    public void SetMaryGame(int _typeID, int[] _arry, MaryGameDelegate _OnChange)
	    {
			int[] tmpArr = new int[4];
			for(int k=0; k < _arry.Length; k++){
				tmpArr[k] = _arry[k];
			}

	        m_iTargetIndex = RandomIndex(_typeID);
	        RectAnimation();
	        m_OnChange = _OnChange;
	        
			UIMaryResult.CalcResult( tmpArr ,_typeID );

			StartAnimation();
			for(int i=0; i<4; i++){
				OnSetResult( i, _arry[i] );
				m_lVoiceIndex[i] =(byte) _arry[i];
			}
	    }

		public void StartAnimation()
		{
//			m_iLimitCount = times;
			m_bIsOpen = true;
			StartCoroutine( StartResult());
		}

		IEnumerator StartResult()
		{
//			yield return new WaitForSeconds(1.0f);
			int tempIndex = 0;
			do
			{
				showEffectStart(tempIndex,true);
				tempIndex++;
			}while(tempIndex<4);

			tempIndex = 0;
			yield return new WaitForSeconds(2f);
			
			while(tempIndex<4){
				showEffectStart(tempIndex,false);
				yield return new WaitForSeconds(0.05f);
				tempIndex++;
			}
		}

		IEnumerator StartResult2()
		{
			yield return new WaitForSeconds(0.5f);

			foreach( var index in UIMaryResult.mList)
			{
				var tmp = index;
				OnPlayResult(tmp,AniType.Light);
				PlayVoice(5,_Game3Sound);
			}

			yield return new WaitForSeconds(1.5f);

			foreach( var index in UIMaryResult.mList)
			{
				var tmp = index;
				OnPlayResult(tmp,AniType.Result);
				OnResAudio(tmp);
			}

			yield return new WaitForSeconds(3.0f);

			if (m_lTurnList[m_iCurrentIndex].m_iID <8){
				//闪图标
				if(UIMaryResult.mList.Count<3)
				{
					m_lTimeList[m_lTurnList[m_iCurrentIndex].m_iID].SetReward(true);
				}
			}
			refreshInfo();
			yield return new WaitForSeconds(3.0f);

			m_lTimeList[m_lTurnList[m_iCurrentIndex].m_iID].SetReward(false);

			if (m_OnChange != null) {
				m_OnChange();
			}
		}

		void refreshInfo()
		{
			o_times.GetComponent<UILabel>().text = UIGame.lBounsGames.ToString();
			o_allScore.GetComponent<UILabel>().text = UIGame.m_lUserWinScore.ToString("N0");
			o_winScore.GetComponent<UILabel>().text = UIGame.lBGScore.ToString("N0");;
			o_betScore.GetComponent<UILabel>().text = (UIGame.m_lCurrentJetton*UIGame.curChipLines).ToString("N0");;
		}

		/// <summary>
		/// 设置动画图标
		/// </summary>
		void OnSetResult( int index, int rtype)
		{
			GameObject result = o_table3[index].transform.FindChild("result").FindChild("result").gameObject;
			GameObject light = o_table3[index].transform.FindChild("result").FindChild("light").gameObject;
			GameObject img = o_table3[index].transform.FindChild("img").gameObject;
			result.GetComponent<UISprite>().atlas = UIGame.Instance.o_atlas[rtype];
			result.GetComponent<UISprite>().spriteName = "result_"+(int)rtype+"_3_0";
			result.GetComponent<UIAnimation>().m_strTextureName = "result_"+(int)rtype+"_3_";
			light.GetComponent<UISprite>().spriteName = "result_"+(int)rtype+"_2_0";
			light.GetComponent<UIAnimation>().m_strTextureName = "result_"+(int)rtype+"_2_";
			img.GetComponent<UISprite>().spriteName = "result_"+(int)rtype;
		}

		/// <summary>
		/// 播放结果动画
		/// </summary>
		void OnPlayResult( int index ,  AniType atype)
		{
			GameObject border = o_table3[index].transform.FindChild("frame").FindChild("border").gameObject;
			GameObject result = o_table3[index].transform.FindChild("result").FindChild("result").gameObject;
			GameObject light = o_table3[index].transform.FindChild("result").FindChild("light").gameObject;
			GameObject img = o_table3[index].transform.FindChild("img").gameObject;
			border.SetActive(true);
			
			if(img.activeSelf) img.SetActive(false);
			if(atype == AniType.Result)
			{
				border.GetComponent<UIAnimation>().m_bRepeatTimes = 10;
				border.GetComponent<UIAnimation>().m_fPerFrameTime = 0.22f;
				light.SetActive(false);
				result.SetActive(true);
				result.GetComponent<UIAnimation>().Play();
				
			}else if(atype == AniType.Light)
			{
				border.GetComponent<UIAnimation>().m_bRepeatTimes = 1;
				border.GetComponent<UIAnimation>().m_fPerFrameTime = 0.3f;
				result.SetActive(false);
				light.SetActive(true);
				light.GetComponent<UIAnimation>().Play();
			}
			border.GetComponent<UIAnimation>().Play();
		}

		/// <summary>
		/// 暂停滚动动画
		/// </summary>
		void OffResultScroll( int index )
		{
			GameObject img = o_table3[index].transform.FindChild("img").gameObject;
			GameObject img1 = o_table3[index].transform.FindChild("scroll").FindChild("scrollImg").gameObject;
			GameObject img2 = o_table3[index].transform.FindChild("scroll").FindChild("scrollImg2").gameObject;
			GameObject result = o_table3[index].transform.FindChild("result").FindChild("result").gameObject;
			GameObject light = o_table3[index].transform.FindChild("result").FindChild("light").gameObject;
			Vector3 _oldPos =  new Vector3(0, 25,0);
			Vector3 _newPos = new Vector3(_oldPos.x, _oldPos.y -50,_oldPos.z);
			
			img.transform.localPosition = _newPos;
			img.SetActive(true);
			
			float sec = 0;
			if(index%4== 0){
				sec = 0.26f;
			}else if(index%4 == 1)
			{
				sec = 0.3f;
			}else if(index%4 == 2)
			{
				sec = 0.34f;
			}else if(index%4 == 3)
			{
				sec = 0.38f;
			}

			TweenPosition.Begin(img, sec, _oldPos);

			img1.GetComponent<UITweenEnd>().Stop();
			img2.GetComponent<UITweenEnd>().Stop();
			img1.GetComponent<TweenPosition>().enabled = false;
			img2.GetComponent<TweenPosition>().enabled = false;		
			
			img1.SetActive(false);
			img2.SetActive(false);
		}
		
		/// <summary>
		/// 播放滚动动画
		/// </summary>
		void OnResultScroll( int index )
		{
			GameObject img = o_table3[index].transform.FindChild("img").gameObject;
			GameObject img1 = o_table3[index].transform.FindChild("scroll").FindChild("scrollImg").gameObject;
			GameObject img2 = o_table3[index].transform.FindChild("scroll").FindChild("scrollImg2").gameObject;
			img1.GetComponent<TweenPosition>().enabled = true;
			img2.GetComponent<TweenPosition>().enabled = true;
			img1.GetComponent<UITweenEnd>().Play();
			img2.GetComponent<UITweenEnd>().Play();
			
			img.SetActive(false);
			img1.SetActive(true);
			img2.SetActive(true);
		}
		
		void showEffectStart(int i ,bool start)
		{
			if(start){
				OnResultScroll(i);
			}else
			{
				OffResultScroll(i);
			}
		}

		/// <summary>
		/// 播放音效
		/// </summary>
		void PlayVoice(int sound , AudioClip[] _Audio)
		{
			float fvol = NGUITools.soundVolume;
			NGUITools.PlaySound(_Audio[sound], fvol, 1);
		}

		void OnResAudio(int inx)
		{
			byte tmpIndex = m_lVoiceIndex[inx];
			PlayVoice(tmpIndex,_ResSound);
		}

	}
}