using UnityEngine;
using System.Collections;

public class showUserBankMoney : MonoBehaviour {

	public static showUserBankMoney _instance;

	/// <summary>
	/// 是否启用隐藏银行金币效果
	/// </summary>
	public bool m_ShowEffect = false;

	// 大厅显示用户银行金币的开关
	private bool m_IsOpen = false;
	// 大厅用户银行label
	public UILabel m_labUserBank;
	// 缓存金币
	public long m_lBankMoney;

	//
	public GameObject m_sprShow;
	public GameObject m_sprHide;

	void Awake()
	{
		_instance = this;
	}
	void OnDestroy()
	{
		_instance = null;
	}

	void Start () {
	
	}

	void Update () {
	
	}

	public void ShowUserBank()
	{
		if(m_labUserBank != null)
		{
			if(m_IsOpen)
			{
				m_labUserBank.text = "******";
				m_IsOpen = false;
				m_sprShow.SetActive(false);
			}else{
				m_labUserBank.text = m_lBankMoney.ToString("N0");
				m_IsOpen = true;
				m_sprShow.SetActive(true);
			}
		}
	}

}
