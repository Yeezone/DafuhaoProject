using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	/// <summary>
	/// high score 总控制器.
	/// </summary>

	public class HighScoreCtrl : MonoBehaviour 
	{
		public static HighScoreCtrl Instance;
		// high score 的父节点.
		public Transform highScoreCache;
		// 每一个玩家上一次的 high score 预设.
		private Transform [] highScoreRecord;

		void Awake () 
		{
			Instance = this;
			highScoreRecord = new Transform[10];
		}

		// 给某一个玩家创建一个high score.
		public void Create(int _canonID, Transform _item, Transform _pos, int _value, int _dir)
		{
			if(_item==null || _pos==null)
			{
				Debug.LogError("high score _item is empty ! _item = "+ _item +" / _cache = "+_pos);
				return;
			}

			// 1. 判断是否有 high score 在播放着，如果有，就把这个 high score 给回收, 然后再创建一个新的.
			if(highScoreRecord[_canonID]!=null)
			{
				if(highScoreRecord[_canonID].gameObject.activeInHierarchy || highScoreRecord[_canonID].gameObject.activeSelf) 
				{
					Factory.Recycle(highScoreRecord[_canonID]);
					highScoreRecord[_canonID] = null;
				}
			}

			// 2. 新建一个新的high score.
			Quaternion _rot;
			// 如果符合旋转条件,就对high score进行旋转生成.
			if(CanonCtrl.Instance.turn_screen == true && CanonCtrl.Instance.turn_screen_on_of){
				_rot = new Quaternion(0.0f,0.0f,1.0f,0.0f);
			}else{
				_rot = new Quaternion();
			}
			Transform _temp = Factory.Create(_item, _pos.position,_rot);
			_temp.parent = transform;
            _temp.localScale = Vector3.one;
			_temp.GetComponentInChildren<HighScoreNumCtrl>().ApplyValue(_value, 0);
			// 检测当前有无关闭音效,如果关闭,则高分板的音效关闭.
			_temp.GetComponent<AudioSource>().enabled = AudioCtrl.Instance.m_bIsOpen;

			// 3. 记录当前创建的 high score.
			highScoreRecord[_canonID] = _temp;
		}

        void OnDestroy()
        {
            Instance = null;
        }
	}
}