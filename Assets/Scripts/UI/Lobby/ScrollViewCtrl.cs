using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollViewCtrl : MonoBehaviour {

	public	ScrollViewType		ViewType;
	public	UIPanel				Panel;
	public	UIWrapContent		WrapContent;
	public	UIDragScrollView	DragScrollView;
	public	bool				AllwaysCenter = true;
	public	uint				OnceInterval = 1;
	public	float				SpeedValue;

	public GameObject[]			MoveBtns;

	private UICenterOnChild		centerOnChild;
	private	bool				action;
	private	int					itemCount;
	private float				itemWidth;
//	private	float				scrollviewOffsetX;
	private	float				targetOffsetX;
	private float				curOffsetX;
	private	bool				uiCenter = true;
	private	float				tempOffsetX;
	private	int					tempIndex;
	private	bool				tempBool;
	public enum ScrollViewType
	{
		Game,
		Room
	}
	// Use this for initialization
	void OnEnable () {
		tempBool = action = false;
		centerOnChild = WrapContent.GetComponent<UICenterOnChild>();
//		scrollviewOffsetX = Panel.transform.localPosition.x;
		Panel.GetComponent<UIScrollView>().ResetPosition();
//		Panel.transform.localPosition = new Vector3(scrollviewOffsetX,Panel.transform.localPosition.y,Panel.transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		if(tempBool)
		{
			if(tempIndex++ == 2)
			{
				if(tempOffsetX == Panel.clipOffset.x)
				{
					tempBool = false;
				}else{
					action = false;
				}
			}
			return;
		}
		if(action)
		{
			float tempVal = Panel.clipOffset.x;
			Panel.clipOffset = new Vector2(Panel.clipOffset.x+(targetOffsetX-Panel.clipOffset.x)*SpeedValue,Panel.clipOffset.y);
			Panel.transform.localPosition = new Vector2(Panel.transform.localPosition.x+(-targetOffsetX-Panel.transform.localPosition.x)*SpeedValue,
			                                            Panel.transform.localPosition.y);
			if(Mathf.Abs(Panel.clipOffset.x-targetOffsetX)<1f)
			{
				Panel.clipOffset = new Vector2(targetOffsetX,Panel.clipOffset.y);
				Panel.transform.localPosition = new Vector2(-targetOffsetX,Panel.transform.localPosition.y);
				centerOnChild.enabled = WrapContent.enabled = uiCenter;
				action = false;
			}
		}
	}

	public void ScrollViewInit()
	{
		if(ViewType == ScrollViewType.Room)
		{
			moveJudge();
			centerOnChild.enabled = WrapContent.enabled = false;
			if(itemCount == 1)
			{
				uiCenter = true;
				if(DragScrollView!=null) setDragEnabled(false);	//如果只有1个元素就不能拖
				if(MoveBtns!=null)
				{
					for(int i = 0; i < MoveBtns.Length; i++)
					{
						MoveBtns[i].SetActive(false);
					}
				}
//			}else if(itemCount == 2)
//			{
//				if(HallTransfer.Instance.uiConfig.curGameType == UIConfig.CurGameType.Ext8Game || HallTransfer.Instance.uiConfig.curGameType == UIConfig.CurGameType.Ext8Game)
//				{
//					targetOffsetX = (itemCount+0.5f)*itemWidth;
//					uiCenter = false;
//					if(DragScrollView!=null) setDragEnabled(false);	//如果只有2个元素不可以拖
//				}else{
//					uiCenter = true;
//					if(DragScrollView!=null) setDragEnabled(true);	//如果只有2个元素可以拖
//				}
			}else{
//				targetOffsetX = itemCount*itemWidth;
				uiCenter = true;
				if(DragScrollView!=null) setDragEnabled(true);	//否则可以拖
				if(MoveBtns!=null)
				{
					for(int i = 0; i < MoveBtns.Length; i++)
					{
						MoveBtns[i].SetActive(true);
					}
				}
			}
			action = true;
			Wrap();
//			Invoke("Wrap",0.1f);
		}else{
			uiCenter = true;
		}
	}

	void Wrap()
	{
		WrapContent.SortBasedOnScrollMovement();
		WrapContent.WrapContent();

	}

	public void ValueUp()
	{
		if(!moveJudge()) return;
		targetOffsetX = curOffsetX+itemWidth*(float)OnceInterval;
		centerOnChild.enabled = WrapContent.enabled = false;
		action = tempBool = true;
	}
	
	public void ValueDown()
	{
		if(!moveJudge()) return;
		targetOffsetX = curOffsetX-itemWidth*(float)OnceInterval;
		centerOnChild.enabled = WrapContent.enabled = false;
		action = tempBool = true;
	}

	bool moveJudge()
	{
		itemCount = (ViewType == ScrollViewType.Game)?CGameManager._instance.m_lstGameItemList.Count:CGameRoomManger._instance.m_lstRoomItemList.Count;
		if(action || itemCount < 2 ) return false;
		if(AllwaysCenter)
		{
			if((int)Panel.clipOffset.x%WrapContent.itemSize != 0) return false;
		}
		tempOffsetX = curOffsetX = Panel.clipOffset.x;
		tempIndex = 0;
		itemWidth = (float)WrapContent.itemSize;
		return true;
	}
	void setDragEnabled(bool dragEnabled)
	{
		DragScrollView.enabled = dragEnabled;
		if(ViewType == ScrollViewType.Game)
		{
			for(int i = 0; i < CGameManager._instance.m_lstGameItemList.Count; i++)
			{
				CGameManager._instance.m_lstGameItemList[i].GetComponent<UIDragScrollView>().enabled = dragEnabled;
			}
		}else{
			for(int i = 0; i < CGameRoomManger._instance.m_lstRoomItemList.Count; i++)
			{
				CGameRoomManger._instance.m_lstRoomItemList[i].GetComponent<UIDragScrollView>().enabled = dragEnabled;
			}
		}
	}
}
