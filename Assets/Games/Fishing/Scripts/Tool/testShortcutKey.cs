using UnityEngine;
using com.QH.QPGame.Fishing;

public class testShortcutKey : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.UpArrow)){
			//方向上->加炮
			CanonCtrl.Instance.singleCanonList[ CanonCtrl.Instance.realCanonID ].C_S_GunPowerUp();
		}else if(Input.GetKeyUp(KeyCode.DownArrow)){
			//方向下->减炮
			CanonCtrl.Instance.singleCanonList[ CanonCtrl.Instance.realCanonID ].C_S_GunPowerDown();
		}else if(Input.GetKeyUp(KeyCode.LeftArrow)){
			//方向左->下分
			BuyScoreAndAccount.Instance.C_S_Account_NoPanel();
		}else if(Input.GetKeyUp(KeyCode.RightArrow)){
			//方向右->上分
			BuyScoreAndAccount.Instance.C_S_BuyScore_NoPanel();
		}else if(Input.GetKeyUp(KeyCode.S)){
			//S->锁定
			CanonCtrl.Instance.PressLockButton();
			if(!CanonCtrl.Instance.inLockMode) CanonCtrl.Instance.PressLockButton();
		}else if(Input.GetKeyUp(KeyCode.Q)){
			//Q->取消锁定
            CanonCtrl.Instance.PressKeyQ();
            //if (CanonCtrl.Instance.inLockMode) CanonCtrl.Instance.PressLockButton(); 
		}else if(Input.GetKeyUp(KeyCode.D)){
			//D->加速发炮
			CanonCtrl.Instance.FireSpeedUp();
        }else if(Input.GetKeyUp(KeyCode.A)){
            //A->自动发炮
            CanonCtrl.Instance.SetAutoFire(!CanonCtrl.Instance.autoFire);
        }
	}
}
