using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using com.QH.QPGame.ZJH;


[ExecuteInEditMode]
[AddComponentMenu("Custom/Controls/CardControl")]

public class UIChipControl : MonoBehaviour
{


//    public string     ChipPrefabs     = "Prefabs/Chip";
    public GameObject     ChipPrefabs     = null;
    public int        BaseDepth       = 300;
    public float      Duration        = 0.3f;
    public GameObject TotalLabel      = null;
    public GameObject TotalDesc       = null;

    public GameObject CurrentLabel      = null;
    public GameObject CurrentDesc       = null;

	//筹码信息框
	public Transform[] showChip =new Transform[4];
	public Transform[] showCount = new Transform[4]; //倍数

	public int currShow;//倍数按钮倍数
	public int FollowsCurrShow;//跟注按钮倍数


    public int _lCurrentScore = 0;
    public int _lCellScore = 0;

    List<GameObject>  _chiplist       = new List<GameObject>();
    int               _totalchip      = 0;


	void Awake()
	{
		for(int i=0;i<4;i++)
		{
			showChip[i]=this.transform.Find("showChips_"+i);
			if(showChip[i]!=null){
				showChip[i].gameObject.SetActive(false);
			}
		}
	}

	//移动鼠标到按钮上时显示筹码值
	public void Follow()
	{
		showChip[0].gameObject.SetActive(true);
	}
	public void FollowOut(){
		showChip[0].gameObject.SetActive(false);
	}

	public void Two()
	{
		showChip[1].gameObject.SetActive(true);

	}
	public void TwoOut(){
		showChip[1].gameObject.SetActive(false);
	}
	public void Five()
	{
		showChip[2].gameObject.SetActive(true);

	}
	public void FiveOut(){
		showChip[2].gameObject.SetActive(false);
	}

	public void Ten()
	{
		showChip[3].gameObject.SetActive(true);

	}
	public void TenOut(){
		showChip[3].gameObject.SetActive(false);
	}



    public void ClearChips()
    {
         foreach(GameObject chip in _chiplist)
         {
             Destroy(chip);
         }
         _chiplist.Clear();
         _totalchip = 0;

        if(TotalLabel!=null && TotalDesc!=null)
        {
            if(_totalchip>0)
            {
                TotalDesc.SetActive(true);
                TotalLabel.SetActive(true);
                TotalLabel.GetComponent<UILabel>().text  = _totalchip.ToString();
                TotalLabel.GetComponent<UILabel>().depth = BaseDepth+100;
            }
            else
            {
                TotalDesc.SetActive(false);
                TotalLabel.SetActive(false);
            }
        }

        if(CurrentLabel!=null && CurrentDesc!=null)
        {
            CurrentDesc.SetActive(false);
            CurrentLabel.SetActive(false);
        }
    }

    public void AddChips(byte bViewID, int nUserChip,int nCellChip,int nCurrTimes,bool kanpai)
    {
        int nChipCount = _chiplist.Count;

        //计算筹码
        int nChipQty = nUserChip / nCellChip;
        _lCellScore = nCellChip;
        _lCurrentScore = nCurrTimes;
		if(kanpai==true){
            currShow = 2;
		}else{
            currShow = 1;
		}
        showCount[1].gameObject.GetComponent<UILabel>().text = (currShow * (2 * _lCellScore + _lCurrentScore)).ToString();
        showCount[2].gameObject.GetComponent<UILabel>().text = (currShow * (5 * _lCellScore + _lCurrentScore)).ToString();
        showCount[3].gameObject.GetComponent<UILabel>().text = (currShow * (10 * _lCellScore + _lCurrentScore)).ToString();


        for(int i=nChipCount;i<nChipCount+nChipQty;i++)
        {
             GameObject obj = (GameObject)Instantiate(ChipPrefabs);
             obj.transform.parent =  transform;
             float zValue = ((float)i)/100+1;
             obj.transform.localScale = new Vector3(1,1,zValue);

             //扔出位置
             Vector3 OldPos = Vector3.zero;

		
			if(UIManager.Instance.curGamePlatform==GamePlatform.ZJH_ForPC)
			{
				if(bViewID == 0)
				{
					OldPos = new Vector3(-446,-365,0);
				}
				else if(bViewID == 1)
				{
					OldPos = new Vector3(427,-137,0);
				}
				else if(bViewID == 2)
				{
					OldPos = new Vector3(429,100,0);
				}
				else if(bViewID == 3)
				{
					OldPos = new Vector3(-444,104,0);
				}
				else if(bViewID == 4)
				{
					OldPos = new Vector3(-446,-95,0);
				}

			}
			if(UIManager.Instance.curGamePlatform==GamePlatform.ZJH_ForMobile)
			{
				if(bViewID == 0)
				{
					OldPos = new Vector3(-500,-266,0);
				}
				else if(bViewID == 1)
				{
					OldPos = new Vector3(511,-21,0);
				}
				else if(bViewID == 2)
				{
					OldPos = new Vector3(318,127,0);
				}
				else if(bViewID == 3)
				{
					OldPos = new Vector3(-298,127,0);
				}
				else if(bViewID == 4)
				{
					OldPos = new Vector3(-487,-33,0);
				}
			}
             


             //目的位置
             int nDestX = UnityEngine.Random.Range(-60,60);
             int nDestY = UnityEngine.Random.Range(10,60);

             Vector3 NewPos = new Vector3(nDestX,nDestY,0);
             obj.transform.localPosition = OldPos;
             TweenPosition.Begin(obj,Duration,NewPos);
             obj.name = "chip_"+i.ToString();
             obj.GetComponent<UISprite>().depth = BaseDepth+i;
             _chiplist.Add(obj);
        }

        _totalchip += nUserChip;
        if(TotalLabel!=null && TotalDesc!=null)
        {
            if(_totalchip>0)
            {
                TotalDesc.SetActive(true);
                TotalLabel.SetActive(true);
                TotalLabel.GetComponent<UILabel>().text  = _totalchip.ToString();
                TotalLabel.GetComponent<UILabel>().depth = BaseDepth+100;
            }
            else
            {
                TotalDesc.SetActive(false);
                TotalLabel.SetActive(false);
            }
        }

		//当前注计算
        if(CurrentLabel!=null && CurrentDesc!=null)
        {
			//需要改
            int nCurrChips = nCurrTimes;
            CurrentDesc.SetActive(true);
            CurrentLabel.SetActive(true);
            
            CurrentLabel.GetComponent<UILabel>().text  = nCurrChips.ToString();
			if(kanpai==true){
				FollowsCurrShow=nCurrChips*2;
			}else{
				FollowsCurrShow=nCurrChips;
			}
			showCount[0].gameObject.GetComponent<UILabel>().text=FollowsCurrShow.ToString();
            CurrentLabel.GetComponent<UILabel>().depth = BaseDepth+100;
        }

    }
    public void WinChips(byte bViewID)
    {
         foreach(GameObject chip in _chiplist)
         {
             //扔出位置
             
             Vector3 NewPos = Vector3.zero;
             Vector3 temp = transform.parent.FindChild("dlg_info_" + bViewID).FindChild("desc_score").transform.localPosition;
             NewPos.x = temp.x + transform.parent.FindChild("dlg_info_" + bViewID).localPosition.x - transform.localPosition.x;
             NewPos.y = temp.y + transform.parent.FindChild("dlg_info_" + bViewID).localPosition.y - transform.localPosition.y;

             Vector3 NewScale = new Vector3(0.305f,0.305f,0.305f);
             TweenPosition.Begin(chip,1f,NewPos);
             TweenScale.Begin(chip, 1f, NewScale);

         }
    }
}
