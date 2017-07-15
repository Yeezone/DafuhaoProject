using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.NN
{

    [ExecuteInEditMode]
    [AddComponentMenu("Custom/Controls/CardControl")]

    public class UIChipControl : MonoBehaviour
    {
//		public string ChipPrefabs = "NN_Prefabs/Chip";
		public GameObject ChipPrefabs;
        public int BaseDepth = 300;
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

        public void AddChips(byte bViewID, int nUserChip, int nCellChip)
        {
            int nChipCount = _chiplist.Count;

            //计算筹码
            int nChipQty = nUserChip / nCellChip;

            for (int i = nChipCount; i < nChipCount + nChipQty; i++)
            {
				GameObject obj = Instantiate(ChipPrefabs); //(GameObject)Instantiate(Resources.Load(ChipPrefabs));
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
                    OldPos = new Vector3(-330, -200, 0);
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
                Vector3 NewPos = Vector3.zero;

                if (bViewID == 0)
                {
                    int nDestX = UnityEngine.Random.Range(-10, 10);
                    int nDestY = UnityEngine.Random.Range(-50, -30);

                    NewPos = new Vector3(nDestX, nDestY, 0);
                }
                else if (bViewID == 1)
                {
                    int nDestX = UnityEngine.Random.Range(55, 65);
                    int nDestY = UnityEngine.Random.Range(35, 45);

                    NewPos = new Vector3(nDestX, nDestY, 0);
                }
                else if (bViewID == 2)
                {
                    int nDestX = UnityEngine.Random.Range(55, 65);
                    int nDestY = UnityEngine.Random.Range(105, 115);

                    NewPos = new Vector3(nDestX, nDestY, 0);
                }
                else if (bViewID == 3)
                {
                    int nDestX = UnityEngine.Random.Range(-65, -55);
                    int nDestY = UnityEngine.Random.Range(105, 115);

                    NewPos = new Vector3(nDestX, nDestY, 0);
                }
                else if (bViewID == 4)
                {
                    int nDestX = UnityEngine.Random.Range(-65, -55);
                    int nDestY = UnityEngine.Random.Range(35, 45);

                    NewPos = new Vector3(nDestX, nDestY, 0);
                }


                obj.transform.localPosition = OldPos;
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
                if (bViewID == 0)
                {
                    NewPos = new Vector3(-330, -200, 0);
                }
                else if (bViewID == 1)
                {
                    NewPos = new Vector3(330, 0, 0);
                }
                else if (bViewID == 2)
                {
                    NewPos = new Vector3(330, 113, 0);
                }
                else if (bViewID == 3)
                {
                    NewPos = new Vector3(-330, 113, 0);
                }
                else if (bViewID == 4)
                {
                    NewPos = new Vector3(-330, 0, 0);
                }

                TweenPosition.Begin(chip, 1f, NewPos);


            }
        }
    }
}