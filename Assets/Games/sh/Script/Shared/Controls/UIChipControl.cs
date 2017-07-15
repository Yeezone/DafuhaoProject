using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.SH
{
    [ExecuteInEditMode]
    [AddComponentMenu("Custom/Controls/CardControl")]

    public class UIChipControl : MonoBehaviour
    {
//        public string ChipPrefabs = "SH_Prefabs/Chip";
		public GameObject ChipPrefabs;
        public int BaseDepth = 300;
        public float Duration = 0.3f;
        public GameObject TotalLabel = null;



        List<GameObject> _chiplist = new List<GameObject>();
        int _totalchip = 0;

        void Start()
        {

        }


        void Update()
        {

        }

        public void ClearChips()
        {
            foreach (GameObject chip in _chiplist)
            {
                Destroy(chip);
            }
            _chiplist.Clear();
            _totalchip = 0;

            if (TotalLabel != null)
            {
                if (_totalchip > 0)
                {

                    TotalLabel.SetActive(true);
                    TotalLabel.GetComponent<UILabel>().text = _totalchip.ToString();
                    TotalLabel.GetComponent<UILabel>().depth = BaseDepth + 100;
                }
                else
                {
                    TotalLabel.SetActive(true);
                    TotalLabel.GetComponent<UILabel>().text = "0";
                    TotalLabel.GetComponent<UILabel>().depth = BaseDepth + 100;
                }
            }
        }

        public void AddChips(byte bViewID, int nUserChip, int nCellChip)
        {


            //计算筹码个数
            int nChipQty = nUserChip / nCellChip;

            int nChip1000Qty = nChipQty / 1000;

            int nChip100Qty = (nChipQty % 1000) / 100;

            int nChip10Qty = ((nChipQty % 1000) % 100) / 10;

            int nChip1Qty = ((nChipQty % 1000) % 100) % 10;

            int nChipCount = _chiplist.Count;

            //1000
            for (int i = nChipCount; i < nChipCount + nChip1000Qty; i++)
            {
//                GameObject obj = (GameObject)Instantiate(Resources.Load(ChipPrefabs));
				GameObject obj = Instantiate(ChipPrefabs);
                obj.transform.parent = transform;
                float zValue = ((float)i) / 100 + 1;
                //obj.transform.localScale = new Vector3(42, 42, zValue);
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<UISprite>().width = 42;
                obj.GetComponent<UISprite>().height = 42;

                //扔出位置
                Vector3 OldPos = Vector3.zero;
                if (bViewID == 0)
                {
                    OldPos = new Vector3(0, -200, 0);
                }
                else if (bViewID == 1)
                {
                    OldPos = new Vector3(330, 0, 0);
                }
                else if (bViewID == 2)
                {
                    OldPos = new Vector3(330, 113, 0);
                }
                else if (bViewID == 3)
                {
                    OldPos = new Vector3(-330, 113, 0);
                }
                else if (bViewID == 4)
                {
                    OldPos = new Vector3(-330, 0, 0);
                }

                //目的位置
                int nDestX = UnityEngine.Random.Range(-60, 60);
                int nDestY = UnityEngine.Random.Range(10, 100);

                Vector3 NewPos = new Vector3(nDestX, nDestY, 0);
                obj.transform.localPosition = OldPos;
                TweenPosition.Begin(obj, Duration, NewPos);
				obj.name = "chip_" + i.ToString();
				//	金币调低,防止盖过卡牌.(使用循环避免产生太多图层)
				obj.GetComponent<UISprite>().depth = ((i+5)%5)+2;

//                obj.GetComponent<UISprite>().depth = BaseDepth + i;
               // obj.GetComponent<UISprite>().spriteName = "bean_1000";
                obj.GetComponent<UISprite>().spriteName = "game_gold01";
                _chiplist.Add(obj);
            }

            nChipCount = _chiplist.Count;

            //100
            for (int i = nChipCount; i < nChipCount + nChip100Qty; i++)
            {
//                GameObject obj = (GameObject)Instantiate(Resources.Load(ChipPrefabs));
				GameObject obj = Instantiate(ChipPrefabs);
                obj.transform.parent = transform;
                float zValue = ((float)i) / 100 + 1;
                //obj.transform.localScale = new Vector3(42, 42, zValue);
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<UISprite>().width = 42;
                obj.GetComponent<UISprite>().height = 42;

                //扔出位置
                Vector3 OldPos = Vector3.zero;
                if (bViewID == 0)
                {
                    OldPos = new Vector3(0, -200, 0);
                }
                else if (bViewID == 1)
                {
                    OldPos = new Vector3(330, 0, 0);
                }
                else if (bViewID == 2)
                {
                    OldPos = new Vector3(330, 113, 0);
                }
                else if (bViewID == 3)
                {
                    OldPos = new Vector3(-330, 113, 0);
                }
                else if (bViewID == 4)
                {
                    OldPos = new Vector3(-330, 0, 0);
                }

                //目的位置
                int nDestX = UnityEngine.Random.Range(-60, 60);
                int nDestY = UnityEngine.Random.Range(10, 100);

                Vector3 NewPos = new Vector3(nDestX, nDestY, 0);
                obj.transform.localPosition = OldPos;
                TweenPosition.Begin(obj, Duration, NewPos);
				obj.name = "chip_" + i.ToString();
				//	金币调低,防止盖过卡牌.(使用循环避免产生太多图层)
				obj.GetComponent<UISprite>().depth = ((i+5)%5)+2;

//                obj.GetComponent<UISprite>().depth = BaseDepth + i;
                //obj.GetComponent<UISprite>().spriteName = "bean_100";
                obj.GetComponent<UISprite>().spriteName = "game_gold01";
                _chiplist.Add(obj);
            }

            nChipCount = _chiplist.Count;

            //10
            for (int i = nChipCount; i < nChipCount + nChip10Qty; i++)
            {
//                GameObject obj = (GameObject)Instantiate(Resources.Load(ChipPrefabs));
				GameObject obj = Instantiate(ChipPrefabs);
                obj.transform.parent = transform;
                float zValue = ((float)i) / 100 + 1;
                //obj.transform.localScale = new Vector3(42, 42, zValue);
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<UISprite>().width = 42;
                obj.GetComponent<UISprite>().height = 42;

                //扔出位置
                Vector3 OldPos = Vector3.zero;
                if (bViewID == 0)
                {
                    OldPos = new Vector3(0, -200, 0);
                }
                else if (bViewID == 1)
                {
                    OldPos = new Vector3(330, 0, 0);
                }
                else if (bViewID == 2)
                {
                    OldPos = new Vector3(330, 113, 0);
                }
                else if (bViewID == 3)
                {
                    OldPos = new Vector3(-330, 113, 0);
                }
                else if (bViewID == 4)
                {
                    OldPos = new Vector3(-330, 0, 0);
                }

                //目的位置
                int nDestX = UnityEngine.Random.Range(-60, 60);
                int nDestY = UnityEngine.Random.Range(0, 70);
				nDestY -= 50;

                Vector3 NewPos = new Vector3(nDestX, nDestY, 0);
                obj.transform.localPosition = OldPos;
                TweenPosition.Begin(obj, Duration, NewPos);
				obj.name = "chip_" + i.ToString();
				//	金币调低,防止盖过卡牌.(使用循环避免产生太多图层)
				obj.GetComponent<UISprite>().depth = ((i+5)%5)+2;

//                obj.GetComponent<UISprite>().depth = BaseDepth + i;
                //obj.GetComponent<UISprite>().spriteName = "bean_10";
                obj.GetComponent<UISprite>().spriteName = "game_gold01";
                _chiplist.Add(obj);
            }

            nChipCount = _chiplist.Count;

            for (int i = nChipCount; i < nChipCount + nChip1Qty; i++)
            {
//                GameObject obj = (GameObject)Instantiate(Resources.Load(ChipPrefabs));
				GameObject obj = Instantiate(ChipPrefabs);
                obj.transform.parent = transform;
                float zValue = ((float)i) / 100 + 1;
                // obj.transform.localScale = new Vector3(42, 42, zValue);
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<UISprite>().width = 42;
                obj.GetComponent<UISprite>().height = 42;

                //扔出位置
                Vector3 OldPos = Vector3.zero;
                if (bViewID == 0)
                {
                    OldPos = new Vector3(0, -200, 0);
                }
                else if (bViewID == 1)
                {
                    OldPos = new Vector3(330, 0, 0);
                }
                else if (bViewID == 2)
                {
                    OldPos = new Vector3(330, 113, 0);
                }
                else if (bViewID == 3)
                {
                    OldPos = new Vector3(-330, 113, 0);
                }
                else if (bViewID == 4)
                {
                    OldPos = new Vector3(-330, 0, 0);
                }

                //目的位置
                int nDestX = UnityEngine.Random.Range(-60, 60);
                int nDestY = UnityEngine.Random.Range(10, 100);

                Vector3 NewPos = new Vector3(nDestX, nDestY, 0);
                obj.transform.localPosition = OldPos;
                TweenPosition.Begin(obj, Duration, NewPos);
                obj.name = "chip_" + i.ToString();
				//	金币调低,防止盖过卡牌.(使用循环避免产生太多图层)
				obj.GetComponent<UISprite>().depth = ((i+5)%5)+2;

//                obj.GetComponent<UISprite>().depth = BaseDepth + i;
                //obj.GetComponent<UISprite>().spriteName = "bean_1";
				obj.GetComponent<UISprite>().spriteName = "game_gold01";

                _chiplist.Add(obj);
            }

            //总注数字
            _totalchip += nUserChip;
            if (TotalLabel != null)
            {
                if (_totalchip > 0)
                {

                    TotalLabel.SetActive(true);
                    TotalLabel.GetComponent<UILabel>().text = _totalchip.ToString();
                    TotalLabel.GetComponent<UILabel>().depth = BaseDepth + 100;
                }
                else
                {

                    TotalLabel.SetActive(true);
                    TotalLabel.GetComponent<UILabel>().text = _totalchip.ToString();
                    TotalLabel.GetComponent<UILabel>().depth = BaseDepth + 100;
                }
            }

        }
        public void WinChips(byte bViewID)
        {
            foreach (GameObject chip in _chiplist)
            {
                //扔出位置
                Vector3 NewPos = Vector3.zero;

                Vector3 temp = transform.parent.FindChild("dlg_player_" + bViewID).FindChild("sp_chips").transform.localPosition;
                NewPos.x = temp.x + transform.parent.FindChild("dlg_player_" + bViewID).localPosition.x - transform.localPosition.x;
                NewPos.y = temp.y + transform.parent.FindChild("dlg_player_" + bViewID).localPosition.y - transform.localPosition.y;

//                if (bViewID == 0)
//                {
//                    NewPos = new Vector3(0, -250, 0);
//                }
//                else if (bViewID == 1)
//                {
//                    NewPos = new Vector3(330, 0, 0);
//                }
//                else if (bViewID == 2)
//                {
//                    NewPos = new Vector3(330, 113, 0);
//                }
//                else if (bViewID == 3)
//                {
//                    NewPos = new Vector3(-330, 113, 0);
//                }
//                else if (bViewID == 4)
//                {
//                    NewPos = new Vector3(-330, 0, 0);
//                }

				//	赢家收回金币的时候,金币图层设高.
				chip.GetComponent<UISprite>().depth = BaseDepth;

                TweenPosition.Begin(chip, 1f, NewPos);
				TweenScale.Begin (chip,1f,new Vector3(0.4f,0.4f,0.4f));
            }
        }
    }
}