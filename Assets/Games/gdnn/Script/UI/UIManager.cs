using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.GDNN
{
	public enum enSceneType
	{
		SCENE_NONE = 0,
		SCENE_LOGIN,
		SCENE_REG,
		SCENE_QUICK,
		SCENE_SERVER,
		SCENE_GAME,
		SCENE_TASK,
		SCENE_SHOP,
		SCENE_HELP,
		SCENE_PERSON,
		SCENE_MODIFY,
		SCENE_SINGLE,
		SCENE_VIP}

	;

	public class UIManager : MonoBehaviour
	{

		static UIManager _instance = null;
		GameObject o_game = null;
		public GameObject o_loading = null;

		enSceneType _SceneType = enSceneType.SCENE_NONE;

		public static UIManager Instance {
			get {
				if (_instance == null) {
					_instance = new GameObject ("UIManger").AddComponent<UIManager> ();
				}
				return _instance;
			}
		}

		public enSceneType SceneType {
			get {
				return _SceneType;
			}
		}

		void Awake ()
		{
			Debug.Log ("<color=red>Awake();</color>");
			if (_instance == null) {
				_instance = this;
			}
		}

		void OnDestroy ()
		{
			_instance = null;
		}

		void Start ()
		{
			o_game = GameObject.Find ("scene_game");
			GoUI (enSceneType.SCENE_SERVER, enSceneType.SCENE_GAME);
		}

		public void GoUI (enSceneType stFrom, enSceneType stTo)
		{
			HideAllUI ();
			_SceneType = enSceneType.SCENE_GAME;
			o_game.SetActive (true);
			if (o_loading != null) {
				o_loading.SetActive (true);
			}
			o_game.GetComponent<UIGame> ().Init ();
		}

		void HideAllUI ()
		{
			o_game.SetActive (false);
			if (o_loading != null) {
				o_loading.SetActive (false);
			}
		}

		//        public void GoUI(enSceneType stFrom, enSceneType stTo)
		//        {
		//            // HideAllUI();
		//            _SceneType = enSceneType.SCENE_GAME;
		//            if (o_game != null)
		//            {
		//                o_game.SetActive(true);
		//                o_game.GetComponent<UIGame>().Init();
		//            }
		//			if(o_loading != null){
		//				o_loading.SetActive(false);
		//			}
		//        }
	}
}
