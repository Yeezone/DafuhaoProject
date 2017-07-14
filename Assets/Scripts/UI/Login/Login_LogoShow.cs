using UnityEngine;
using System.Collections;

public class Login_LogoShow : MonoBehaviour {

	public	float			WaitTime = 0;
	public	float			OutTime = 0;
	public	Sprite[]		LogoList;
	public	Transform		LogoBack;

	private	float			CurTime = 0f;
	private	int				CurImageIndex = 0;
	private	int				LogoState = 0;
	private	float			AlphaValue = 0f;


	void Start()
	{
		this.GetComponent<UI2DSprite>().sprite2D = LogoList[CurImageIndex];
	}
	
	// Update is called once per frame
	void Update () {
		CurTime += Time.deltaTime;
		if(LogoState == 0)
		{
			if(CurTime >= WaitTime)
			{
				CurTime = 0;
				LogoState = 1;
			}
		}else{
			if(CurTime >= OutTime)
			{
				if(++CurImageIndex == LogoList.Length)
				{
					Destroy( this.transform.parent.gameObject );
				}else{
					LogoState = 0;
					CurTime = 0f;
					this.GetComponent<UI2DSprite>().sprite2D = LogoList[CurImageIndex];
					this.GetComponent<UI2DSprite>().alpha = 1f;
				}
			}else{
				AlphaValue = (1f - CurTime / OutTime);
				this.GetComponent<UI2DSprite>().alpha = AlphaValue;
				if(CurImageIndex == LogoList.Length-1) LogoBack.GetComponent<UI2DSprite>().alpha = AlphaValue;
			}
		}
	}
}
