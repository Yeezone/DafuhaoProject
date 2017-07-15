using UnityEngine;

namespace com.QH.QPGame.SHZ
{
	public class UIShow : MonoBehaviour {

		public	GameObject []	show_Window;
		public	GameObject []	hide_Window;

		public	GameObject []	rePositonObject;
		public	Vector3 []		objectPostion;

		public	GameObject []	txt_lable;
		public	string []		lable_txt;

		public	GameObject []	txt_input;
		public	string []		input_txt;

		public	GameObject 		selecte_input;
		
		public	GameObject []	spriteTarget;
		public	UIAtlas []		spriteAtlas;
		public	string []		spriteName;

		public	GameObject []	toggleBtn;
		public	bool []			toggleValue;


		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnClick() 
		{
//			if( true )
//			{
				for(var i = 0; i < show_Window.Length; i++)
				{
					if(show_Window[i] != null) show_Window[i].SetActive(true);//显示窗口
				}
				for(var i = 0; i < hide_Window.Length; i++)
				{
					if(hide_Window[i] != null) hide_Window[i].SetActive(false);//隐藏窗口
				}
				for(var i = 0; i < rePositonObject.Length; i++)
				{
					if(rePositonObject[i] != null && objectPostion[i] != null) rePositonObject[i].transform.localPosition = objectPostion[i];//位移
				}
				for(var i = 0; i < txt_lable.Length; i++)
				{
					if(txt_lable[i] != null && lable_txt[i] != null) txt_lable[i].GetComponent<UILabel>().text = lable_txt[i];//标签填值
				}
				for(var i = 0; i < txt_input.Length; i++)
				{
					if(txt_input[i] != null && input_txt[i] != null) txt_input[i].GetComponent<UIInput>().value = input_txt[i];//输入框填值
				}
				if(selecte_input != null) selecte_input.GetComponent<UIInput>().isSelected = true;
				for(var i = 0; i < toggleBtn.Length; i++)
				{
					if(toggleBtn[i] != null) toggleBtn[i].GetComponent<UIToggle>().value = toggleValue[i];//toggle按钮值
				}
//			}
		}
	}
}
