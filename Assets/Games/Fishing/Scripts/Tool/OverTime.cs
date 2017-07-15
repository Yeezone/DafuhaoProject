using UnityEngine;
using com.QH.QPGame.Fishing;

public class OverTime : MonoBehaviour {

	public static OverTime Instance;

	public UILabel label;
	public	CanonCtrl	CanonCtrl_obj;
	public	Transform	RestTimeLabel;
	public	float		TimeLength = 60f;
	public	float		TimeCountDown = 10f;
	public	float		CurTime;
	private	bool		MouseButtonIsDown = false;

    // 您在此处,更新周期
	public	float		waitTime_temp = 15f;

	void Start () {
		Instance = this;
		CurTime = TimeLength;
		label = RestTimeLabel.GetComponent<UILabel>();
	}

	void OnDestroy()
	{
		Instance = null;
	}
	
	// Update is called once per frame
	void Update () {
		if(CanonCtrl_obj == null) return;
		CurTime -= Time.deltaTime;
//        if (!CanonCtrl_obj.autoFire && !Input.GetMouseButton(0) && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Escape) && !Input.GetKey(KeyCode.A))
//		{
//			CurTime -= Time.deltaTime;
//		}else{
//			CurTime = TimeLength;
//            transform.localScale = Vector2.zero;
//		}

		if(CurTime <= 0)
		{
			//刷新倒计时
			int tempTime = (int)(TimeCountDown + CurTime);
			if(label == null)
			{
				label = RestTimeLabel.GetComponent<UILabel>();
			}
			if(label != null)
			{
				label.text = "(" + tempTime.ToString() + ")";
			}
			transform.localScale = Vector2.one;
			//退出游戏
			if(tempTime <= 0)	CanonCtrl_obj.PressBackButton();
		}

        if (TimeLength - CurTime >= waitTime_temp)
        {
			if(CanonCtrl_obj.playerLogo != null){
				CanonCtrl_obj.playerLogo.SetActive (true);
			}
		}else{
            if (CanonCtrl_obj.playerLogo != null)
            {
                CanonCtrl_obj.playerLogo.SetActive(false);
            }
		}
	}
	public void RefreshWaitTime()
	{
		CurTime = TimeLength;
		transform.localScale = Vector2.zero;
	}
}
