using UnityEngine;
using System.Collections;

public class guanggaoAutoPlay : MonoBehaviour {

	public	uint				imgNum;			//广告数量
	public	Transform			imgTransform;	//广告图片Transform
	public	float				intervalTime;	//间隔时间

	private	bool				changeSwitch = true;	//切换开关
	private	uint				curImgIndex = 0;		//当前广告INDEX
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(changeSwitch)
		{
			Invoke("ChangeImg",intervalTime);
			changeSwitch = false;
		}
	}

	void ChangeImg()
	{
		if(curImgIndex == imgNum) curImgIndex = 0;
//		imgTransform.GetComponent<UISprite>().spriteName = "guanggao" + curImgIndex++;
//		imgTransform.GetComponent<UI2DSprite>().sprite2D = HallTransfer.Instance.uiConfig.guanggaoImgs[curImgIndex++].GetComponent<UI2DSprite>().sprite2D;
		changeSwitch = true;
	}

}
