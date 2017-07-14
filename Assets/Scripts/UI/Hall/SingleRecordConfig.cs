using UnityEngine;
using System.Collections;

/// <summary>
/// 此配置脚本适用于大厅充值中心:充值记录,兑奖记录
/// </summary>
public class SingleRecordConfig : MonoBehaviour {

	/// <summary>
	/// 序号
	/// </summary>
	public UILabel index_label;

	/// <summary>
	/// 充值记录_申请单号
	/// </summary>
	public UILabel applyNum_lable;

	/// <summary>
	/// 充值记录_金额
	/// </summary>
	public UILabel money_label;

	/// <summary>
	/// 备注
	/// </summary>
	public UILabel remark_label;

	/// <summary>
	/// 状态
	/// </summary>
	public UILabel state_label;	

	/// <summary>
	/// 取消订单按钮
	/// </summary>
	public GameObject cancelBtn;
}
