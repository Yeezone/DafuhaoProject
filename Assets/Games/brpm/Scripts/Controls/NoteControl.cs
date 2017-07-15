using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.BRPM
{
    public class NoteControl : MonoBehaviour
    {
        public GameObject uiGrid;
        public GameObject itemdata;

        public List<GameObject> gameData = new List<GameObject>();

        //初始化记录
        public void DestroyChild()
        {
            gameData.Clear();
            foreach (Transform child in uiGrid.transform)
            {
                Destroy(child.gameObject);
            }
        }

        //写入记录信息
        public void ShowNoteData(byte[] Ranking, byte[] multiple)
        {
            for (int i = 0; i < Ranking.Length; i++)
            {
                if (multiple[i] > 0)
                {
                    DazzlingHorse(Ranking[i], multiple[i]);
                }               
            }
        }
        //生成单个信息
        public void AloneNoteData(byte head1, byte head2, byte multiple)
        {
            if (multiple == 0) return;
            SetHorseHead(head1, head2, multiple);
        }

        //判断生成信息
        void DazzlingHorse(byte horseindex, byte multiple)
        {
            switch (horseindex)
            {
                case 0: SetHorseHead(1, 6, multiple); break;
                case 1: SetHorseHead(1, 5, multiple); break;
                case 2: SetHorseHead(1, 4, multiple); break;
                case 3: SetHorseHead(1, 3, multiple); break;
                case 4: SetHorseHead(1, 2, multiple); break;
                case 5: SetHorseHead(2, 6, multiple); break;
                case 6: SetHorseHead(2, 5, multiple); break;
                case 7: SetHorseHead(2, 4, multiple); break;
                case 8: SetHorseHead(2, 3, multiple); break;
                case 9: SetHorseHead(3, 6, multiple); break;
                case 10: SetHorseHead(3, 5, multiple); break;
                case 11: SetHorseHead(3, 4, multiple); break;
                case 12: SetHorseHead(4, 6, multiple); break;
                case 13: SetHorseHead(4, 5, multiple); break;
                case 14: SetHorseHead(5, 6, multiple); break;
            }
        }

        //生成记录信息
        void SetHorseHead(byte head1, byte head2, byte multiple)
        {
            GameObject child = null;
            child = (GameObject)Instantiate(itemdata, Vector3.zero, Quaternion.identity);

            child.transform.parent = uiGrid.transform;
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;          

            child.transform.Find("horse_head1_1").GetComponent<UISprite>().spriteName = "horseHead_" + head1.ToString();
            child.transform.Find("horse_head1_2").GetComponent<UISprite>().spriteName = "horseHead_" + head2.ToString();
            child.transform.Find("head_value1").GetComponent<label_number>().m_iNum = head1;
            child.transform.Find("head_value2").GetComponent<label_number>().m_iNum = head2;
            child.transform.Find("multiple").GetComponent<label_number>().m_iNum = multiple; 
            uiGrid.GetComponent<UIGrid>().enabled = true;
            child.gameObject.name = gameData.Count.ToString();
            gameData.Add(child.gameObject);
	        if ( gameData.Count > 7 )
            {
                Destroy(gameData[0]);
                gameData.RemoveAt(0);                
            }
        }
    }
}

