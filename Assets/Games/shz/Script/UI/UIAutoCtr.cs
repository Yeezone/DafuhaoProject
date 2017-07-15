using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.SHZ
{    
	public delegate void AutoDelegate(string eventName);
	public delegate void AutoStartScene();
	public delegate void AutoGetScore();

	public enum AutoState
	{
		Null = 0,
		START,
		GET
	};

	public class UIAutoCtr : MonoBehaviour {
	
		public static UIAutoCtr _instance = null;

		private AutoState _state;
		public AutoState curState{
			set{
				_state = value;
			}
		}

		public AutoStartScene  OnStartScene;
		public AutoGetScore OnGetScore;
		public event AutoDelegate OnAutoHandler;

		// Use this for initialization
		void Start () {
			AddDelegate();
		}

		void Awake () {
			_instance = this;
		}

		void OnDestroy(){
			if(_instance != null) _instance=null;
		}
		
		// Update is called once per frame
		void Update () 
		{
			if(_state == AutoState.START)
			{
				Excute("START");
			}else if(_state == AutoState.GET)
			{
				Excute("GET");
			}
		}
			
		void Excute(string eventName)
		{
			_state = AutoState.Null;
			if(OnAutoHandler !=null)
				OnAutoHandler(eventName);
		}

		void AddDelegate()
		{
			OnAutoHandler += OnExcuteCtr;
		}

		void OnExcuteCtr(string eventName)
		{
			switch(eventName)
			{
			case "START":
				if(OnStartScene!=null) OnStartScene();
				break;
			case "GET":
				if(OnGetScore!=null) OnGetScore();
				break;
			}
		}

	}
}