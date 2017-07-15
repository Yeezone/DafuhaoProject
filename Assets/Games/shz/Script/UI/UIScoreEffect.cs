using UnityEngine;
using System.Collections;
using com.QH.QPGame.GameUtils;

namespace com.QH.QPGame.SHZ
{
	public class UIScoreEffect : MonoBehaviour
    {

		public GameObject numLabel = null;
		public GameObject target = null;
		public string EndCallBack = "";

		public long	_score = 0;
		public long _targetScore = 0;
		public float _time = 5;
		public bool _enable = false;

		public AudioClip audioClip;
		public float volume = 1f;
		public float pitch = 1f;

		float _startTime = 0;
		float _lastTime = 0;
		long t = 0;
		double tmpScore = 0;

        void Start()
        {
        }

        void Update()
        {
			if (_enable == false) return;
			
			if (_startTime > 0)
			{
				uint nNow = (uint)System.Environment.TickCount;

				if(_targetScore < _score)
				{
					tmpScore -= ((_score - _targetScore)*(nNow-_lastTime) / ( _time*1000));
					if(tmpScore < _targetScore) tmpScore = _targetScore;

				}else
				{
					tmpScore += ((_targetScore - _score) *(nNow-_lastTime)/ ( _time*1000));
					if(tmpScore > _targetScore) tmpScore = _targetScore;
				}

				Show((long)tmpScore);

				_lastTime = nNow;
				_startTime -= Time.deltaTime; 
			}
			else
			{
				Show(_targetScore);
				_enable = false;
				_score=0;
				Invoke("OnCallback", 1.0f);
			}
        }

		void Show(long tmp)
		{
			if (numLabel != null){
//				string tmpScore = numLabel.GetComponent<UILabel>().text;
//				numLabel.GetComponent<UILabel>().text = formatScore(tmp,10);
				numLabel.GetComponent<UILabel>().text = tmp.ToString();
			}
		}

		public void OnCallback()
		{
			if (target != null && !string.IsNullOrEmpty(EndCallBack))
			{
				target.SendMessage(EndCallBack, gameObject, SendMessageOptions.DontRequireReceiver);
			}
			_enable = false;
			_startTime = 0;
//			gameObject.SetActive(false);
		}

		string formatScore(long money, int index)
		{
			string tempMoney = money.ToString();
			if(money<10000)	return tempMoney;
			
			long tempScore = 1;
			for(int i=0; i<index; i++)
				tempScore *= 10;
			if(money>=tempScore) 
			{
				tempMoney = (money / 10000).ToString()+"w";
			}
			return tempMoney;
		}

		public void Play()
		{
			_startTime = _time;
			_lastTime = (uint)System.Environment.TickCount;
			string tmp = numLabel.GetComponent<UILabel>().text;
//			_score = long.Parse(tmpScore);
			if(_score==0){
				long.TryParse(tmp,out _score);
			}
			tmpScore =  _score;
			_enable = true;
		}

		public void Over()
		{
			_score = _targetScore;
			tmpScore =  _score;
			_enable = false; 
			_score=0;
		}
    }
}