using UnityEngine;
using System.Collections.Generic;

namespace com.QH.QPGame.Fishing
{

	//[Serializable]
	//public class CoinType
	//{
	//	public List<Transform> coinType = new List<Transform>();
	//}

	public class CoinCtrl : MonoBehaviour 
	{
		public static CoinCtrl Instance;
		private Transform curTrans;
		// 两个金币的间距.
		public float coinsGap = 0.65f;
		// 两个金币之间弹跳的时间间隔.
		public float timeGap = 0.075f;
		public List<Transform> coinType = new List<Transform>();

		void Awake ()
		{
			Instance = this;
			curTrans = this.transform;
		}

	//	void Start()
	//	{
	//		Vector3 orgLocalScale = transform.localScale;
	//		transform.localScale = new Vector3(orgLocalScale.x * Utility.device2ServerRatio.x, orgLocalScale.y * Utility.device2ServerRatio.x, 1f);
	//	}

		// 创建一个金币.
		public void CreateCoins(int _canonID, int _coinIndex, int _num, Vector3 _middlePos, bool _playAudio)
		{
			if(CanonCtrl.Instance.singleCanonList[_canonID]==null)
			{
				Debug.Log("_canonID = " + _canonID + " does not exist createCoins.cs");
				return;
			}

			if(_coinIndex>=coinType.Count || coinType[_coinIndex]==null)
			{
				Debug.Log("_canonID = " + _canonID + " coinIndex = "+_coinIndex);
				return;
			}

			// 按照一排创建金币.
			Vector3 _pos = Vector3.zero;
			int _firstIndex = (int)(_num/2);
			int _dir = CanonCtrl.Instance.dirArray[_canonID];
			for(int i=0; i<_num; i++)
			{
				if(_dir==1)
				{
					_pos = _middlePos + new Vector3(0f, coinsGap*(i-_firstIndex), 0f);
				}
				else if(_dir==3)
				{
					_pos = _middlePos - new Vector3(0f, coinsGap*(i-_firstIndex), 0f);
				}
				else 
				{
					_pos = _middlePos + new Vector3(coinsGap*(i-_firstIndex), 0f, 0f);
				}

				// 创建金币.
				Transform _coin = Factory.Create(coinType[_coinIndex], _pos, Quaternion.identity);
				_coin.parent = curTrans;
				_coin.localScale = Vector3.zero;

				// 赋值.
				Vector3 _ownerPos = CanonCtrl.Instance.singleCanonList[_canonID].coinHomePos.position;
				_coin.GetComponent<SingleCoin>().Init(_canonID, _ownerPos, i*timeGap, _playAudio);
			}
		}

        void OnDestroy()
        {
            Instance = null;
        }
	}
}
