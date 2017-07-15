using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.CX
{

    [ExecuteInEditMode]
    [AddComponentMenu("Custom/Controls/CardControl")]

    public class UIChipControl : MonoBehaviour
    {
//		public string ChipPrefabs = "CX_Prefabs/Chip";
		public GameObject ChipPrefabs = null;
		public GameObject ChipPreObj = null;
        public int BaseDepth = 200;
        public float Duration = 0.3f;
        public GameObject TotalLabel = null;
        public GameObject TotalDesc = null;


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

            if (TotalLabel != null && TotalDesc != null)
            {
                if (_totalchip > 0)
                {
                    TotalDesc.SetActive(true);
                    TotalLabel.SetActive(true);
                    TotalLabel.GetComponent<UILabel>().text = _totalchip.ToString();
                    TotalLabel.GetComponent<UILabel>().depth = BaseDepth + 100;
                }
                else
                {
                    TotalDesc.SetActive(false);
                    TotalLabel.SetActive(false);
                }
            }
        }

        GameObject GetChipPre()
        {
            if (ChipPreObj == null)
            {
                //                ChipPreObj = Resources.Load("CX_Prefabs/Chip") as GameObject;
                ChipPreObj = GameObject.Instantiate(ChipPrefabs);
                ChipPreObj.SetActive(false);
            }
            return GameObject.Instantiate(ChipPrefabs);
        }

        public void AddChips(byte bViewID, int nUserChip, int nCellChip)
        {
            int nChipCount = _chiplist.Count;

            //计算筹码
            int nChipQty = nUserChip / nCellChip;

            if (nChipQty > 10)
            {
                nChipQty = 10;
            }
            if (nCellChip == 0 && nUserChip != 0)
            {
                nUserChip = 1;
            }
            if (nChipQty == 0)
            {
                return;
            }

            for (int i = nChipCount; i < nChipCount + nChipQty; i++)
            {
                GameObject obj = GetChipPre();
                obj.SetActive(true);
                obj.transform.SetParent(transform, false);
                obj.transform.parent = transform;
                float zValue = ((float)i) / 100 + 1;
                obj.transform.localScale = new Vector3(1, 1, zValue);

                //扔出位置
                Vector3 OldPos = Vector3.zero;

                OldPos = UIGame.o_player_chip[(bViewID)].transform.position; 


                //目的位置
                Vector3 NewPos = Vector3.zero;
                int nDestX = UnityEngine.Random.Range(-300, 300);
                int nDestY = UnityEngine.Random.Range(-100, 100);
                NewPos = new Vector3(nDestX, nDestY, 0);

                obj.transform.position = OldPos;
                TweenPosition.Begin(obj, Duration, NewPos);
                obj.name = "chip_" + i.ToString();

                obj.GetComponent<UISprite>().depth = BaseDepth + i;


                _chiplist.Add(obj);
            }

            _totalchip += nUserChip;
            if (TotalLabel != null && TotalDesc != null)
            {
                if (_totalchip > 0)
                {
                    TotalDesc.SetActive(true);
                    TotalLabel.SetActive(true);
                    TotalLabel.GetComponent<UILabel>().text = _totalchip.ToString();
                    TotalLabel.GetComponent<UILabel>().depth = BaseDepth + 100;
                }
                else
                {
                    TotalDesc.SetActive(false);
                    TotalLabel.SetActive(false);
                }
            }

        }
        public void WinChips(byte bViewID)
        {
            foreach (GameObject chip in _chiplist)
            {
                //扔出位置
                Vector3 NewPos = Vector3.zero;
                NewPos = UIGame.o_player_chip[(bViewID)].transform.position;

                TweenPosition.Begin(chip, 1f, NewPos);


            }
        }

        byte ViewToChair(byte ViewID)
        {
            //byte wChairID = (byte)((ViewID + GameEngine.Instance.MySelf.Self.ChairID) % GameLogic.GAME_PLAYER );
            byte wChairID = (byte)((ViewID + GameEngine.Instance.MySelf.DeskStation) % GameLogic.GAME_PLAYER);
            return wChairID;
        }
    }
}