using UnityEngine;
using System.Collections;
using Shared;

namespace com.QH.QPGame.BJL
{

    [AddComponentMenu("Custom/Controls/UICompCard")]


    public delegate void FinishCall();

    public class UICompCard : MonoBehaviour
    {
//        GameObject o_dlg = null;
//        GameObject o_user_nick_0 = null;
//        GameObject o_user_nick_1 = null;
//
//        GameObject o_user_card_0 = null;
//        GameObject o_user_card_1 = null;
//
//        GameObject o_user_win_0 = null;
//        GameObject o_user_win_1 = null;
//
//
//        //GameObject o_effect      = null;
//
//        int _EffectDelay = 0;
//        int _EffectTick = 0;
//        int _EffectFrame = 0;
//        bool _bShowEffect = false;
//        bool _bShowFlash = false;
//        bool _bResult = false;
//
//        string _strLoser = "";
//
//        FinishCall _FinishCall = null;
//
//        public FinishCall OnFinishCall
//        {
//            set
//            {
//                _FinishCall = value;
//            }
//            get
//            {
//                return _FinishCall;
//            }
//        }
//
//        void Awake()
//        {
////            o_dlg = GameObject.Find("scene_game/dlg_comp_effect");
////            o_user_nick_0 = GameObject.Find("scene_game/dlg_comp_effect/lbl_user_0");
////            o_user_nick_1 = GameObject.Find("scene_game/dlg_comp_effect/lbl_user_1");
////
////            o_user_card_0 = GameObject.Find("scene_game/dlg_comp_effect/ctr_cmp_cards_0");
////            o_user_card_1 = GameObject.Find("scene_game/dlg_comp_effect/ctr_cmp_cards_1");
////
////            o_user_win_0 = GameObject.Find("scene_game/dlg_comp_effect/sp_win_0");
////            o_user_win_1 = GameObject.Find("scene_game/dlg_comp_effect/sp_win_1");
//
//
//        }
//
//        void Start()
//        {
//
//        }
//
//
//        void FixedUpdate()
//        {
//            if (_bShowEffect)
//            {
//                //比牌特效
//                if ((System.Environment.TickCount - _EffectTick) > 100 && _bShowFlash == true)
//                {
//                    if (_EffectFrame > 3)
//                    {
//                        _EffectFrame = 0;
//                    }
//
//                    _EffectTick = System.Environment.TickCount;
//                    _EffectFrame++;
//                }
//
//                //灰牌效果
//                if ((System.Environment.TickCount - _EffectDelay) > 2000 && _bResult == true)
//                {
////                    Debug.Log("loseer=" + _strLoser + "usero=" + o_user_nick_0.GetComponent<UILabel>().text);
////                    if (_strLoser == o_user_nick_0.GetComponent<UILabel>().text)
////                    {
////                        o_user_win_0.SetActive(true);
////                        o_user_win_1.SetActive(true);
////                        o_user_card_0.GetComponent<UICardControl>().SetCardData(new byte[3] { 252, 252, 252 }, 3);
////                        o_user_win_0.GetComponent<UISprite>().spriteName = "lose";
////                        o_user_win_1.GetComponent<UISprite>().spriteName = "win";
////                    }
////                    else
////                    {
////                        o_user_win_0.SetActive(true);
////                        o_user_win_1.SetActive(true);
////                        o_user_card_1.GetComponent<UICardControl>().SetCardData(new byte[3] { 252, 252, 252 }, 3);
////                        o_user_win_0.GetComponent<UISprite>().spriteName = "win";
////                        o_user_win_1.GetComponent<UISprite>().spriteName = "lose";
////                    }
////
////                    _bResult = false;
////                    _bShowFlash = false;
////
////                }
////
////                //关闭
////                if ((System.Environment.TickCount - _EffectDelay) > 3000)
////                {
////                    o_dlg.SetActive(false);
////                    _bShowEffect = false;
////                    if (_FinishCall != null)
////                    {
////                        _FinishCall();
////                    }
////                }
//            }
//        }
//
//        void Show(string strCompUserNick0, string strCompUserNick1, string strLostUserNick)
//        {
////            o_dlg.SetActive(true);
////
////            o_user_nick_0.GetComponent<UILabel>().text = strCompUserNick0;
////            o_user_nick_1.GetComponent<UILabel>().text = strCompUserNick1;
////            o_user_card_0.GetComponent<UICardControl>().ClearCards();
////            o_user_card_0.GetComponent<UICardControl>().AppendCompCard(0, new byte[3] { 255, 255, 255 }, 3);
////            o_user_card_1.GetComponent<UICardControl>().ClearCards();
////            o_user_card_1.GetComponent<UICardControl>().AppendCompCard(1, new byte[3] { 255, 255, 255 }, 3);
////            o_user_win_0.SetActive(false);
////            o_user_win_1.SetActive(false);
////
////            _strLoser = strLostUserNick;
////            _bResult = true;
////            _bShowEffect = true;
////            _bShowFlash = true;
////            _EffectDelay = System.Environment.TickCount;
////            _EffectTick = System.Environment.TickCount;
////            _EffectFrame = 0;
////        }
    }
}