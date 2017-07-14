using UnityEngine;
using System.Collections.Generic;

public class marqueeEffect : MonoBehaviour {

	public		GameObject			textLabel;
	public		string				textValue;

	private		bool				running = false;				//跑马灯运行开关
	private		bool				moving = false;					//跑马灯移动状态
	private		List<string>		contents = new List<string>();	//跑马灯数据列表
	private		int					moveInterval = 2;				//跑马灯移动速度
	private		float				textPositionY = -12;			//跑马灯文字高度
	private		float				targetPositionX = 0;			//跑马灯文字目标横坐标
	private		float				waitInterval = 2.0f;			//跑马灯等待时间长度 单位:秒

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(moving)
		{
			if(targetPositionX < textLabel.GetComponent<UILabel>().leftAnchor.absolute)
			{
				textLabel.GetComponent<UILabel>().leftAnchor.absolute -= moveInterval;

			}else{
				moving = false;
				Invoke ("NextMarquee",waitInterval);
			}
		}
			
	}

	public void AddItem( string val )
	{
		contents.Add (val);
		if(!running) Show();
	}

	void Show ()
	{
		this.gameObject.SetActive(true);
		running = true;
		textLabel.GetComponent<UILabel>().text = contents[0];
		if(this.GetComponent<UIPanel>().width > textLabel.GetComponent<UILabel>().width)
		{
			targetPositionX = ( this.GetComponent<UIPanel>().width - textLabel.GetComponent<UILabel>().width ) * 0.5f;
		}else{
			targetPositionX = ( this.GetComponent<UIPanel>().width - textLabel.GetComponent<UILabel>().width - 20 );
		}
		textLabel.GetComponent<UILabel>().leftAnchor.absolute = (int)this.GetComponent<UIPanel>().width;
		moving = true;
	}

	void NextMarquee()
	{
		contents.RemoveAt(0);
		if(contents.Count > 0)
		{
			//显示下一条
			Show();
		}else{
			//结束
			textLabel.GetComponent<UILabel>().text = "";
			running = false;
			this.gameObject.SetActive(false);
		}
	}
}
