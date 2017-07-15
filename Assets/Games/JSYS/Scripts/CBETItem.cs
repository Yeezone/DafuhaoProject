using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace com.QH.QPGame.JSYS
{
public class CBETItem : MonoBehaviour 
{
    /// <summary>
    /// 下注ID
    /// </summary>
    public int m_iBetID;

    public UIButton m_cButton;

    /// <summary>
    /// 闪动
    /// </summary>
    public UISprite m_BgSprite;
    /// <summary>
    /// 玩家自己的下注
    /// </summary>
    public CLabelNum_JSYS m_cMyBet;
    private long _lMyBet;
    public long m_lMyBet
    {
        get { return _lMyBet; }
        set
        {
            _lMyBet = value;
            m_cMyBet.m_iNum = _lMyBet;

        }
    }
    public CLabelNum_JSYS m_cMulitTimes;
 
    /// <summary>
    /// 总下注
    /// </summary>
    public CLabelNum_JSYS m_cAllBet;
    private long _lAllBet;
    public long m_lAllBet
    {
        get { return _lAllBet; }
        set
        {
            _lAllBet = value;
            m_cAllBet.m_iNum = _lAllBet;
        }
    }

    public void SetDisable(bool _bIsDisable)
    {
        //m_cButton.isEnabled = _bIsDisable;
        m_cButton.GetComponent<BoxCollider>().enabled = _bIsDisable;
    }
    /// <summary>
    /// 设置无效颜色
    /// </summary>
    public void SetDisableColor()
    {
        m_cButton.state = UIButtonColor.State.Disabled;
        this.GetComponent<CMouseStateColor>().SetButtonColor(this.GetComponent<CMouseStateColor>().m_ButtonColor.m_cDisableColor);
        this.GetComponent<CMouseStateColor>().SetButtonColor2(this.GetComponent<CMouseStateColor>().m_ButtonColor2.m_cDisableColor);
    }
    /// <summary>
    ///设置正常颜色
    /// </summary>
    public void SetNormalColor()
    {
        m_cButton.state = UIButtonColor.State.Normal;
        this.GetComponent<CMouseStateColor>().SetButtonColor(this.GetComponent<CMouseStateColor>().m_ButtonColor.m_cNormalColor);
        this.GetComponent<CMouseStateColor>().SetButtonColor2(this.GetComponent<CMouseStateColor>().m_ButtonColor2.m_cNormalColor);
    }

    /// <summary>
    /// 设置中奖闪动
    /// </summary>
    /// <param name="_bIsReward"></param>
    public void SetRewardAnimal(bool _bIsReward)
    {
        m_BgSprite.GetComponent<TweenAlpha>().duration = 0.4f;
        m_BgSprite.GetComponent<TweenAlpha>().ResetToBeginning();
        m_BgSprite.alpha = 0;
        m_BgSprite.GetComponent<TweenAlpha>().enabled = _bIsReward;
        
    }
  
}
}
