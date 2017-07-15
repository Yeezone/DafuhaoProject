using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.BRPM 
{
    public class ResultController : MonoBehaviour
    {
        public GameObject scrollBar;
        public GameObject userResultData;
        public GameObject emptyData;
        public GameObject PlayerWinSocre;
        public GameObject PlayerLoseSocre;
        public GameObject PlayerAddBetScore;
        public GameObject PlayerBetScore;

        private GameObject o_HorseHead1 = null;
        private GameObject o_HorseHead2 = null;
        private GameObject RankingUser = null;
        private GameObject PlayerRanking = null;

        void Awake()
        {
            o_HorseHead1 = transform.Find("horse_head1").gameObject;
            o_HorseHead2 = transform.Find("horse_head2").gameObject;
			RankingUser = transform.Find("sotr_Panel/Scroll View/UIGrid").gameObject;
            PlayerRanking = transform.Find("ranKing_label/ranKing_value").gameObject;
        }

        public void SetResultData(int[] areamultiple, long[] playerTableScore,byte head1, byte head2, long score, long betscore, byte ranking)
        {
            o_HorseHead1.transform.GetComponent<UISprite>().spriteName = "fj_" + (head1+1).ToString();
            o_HorseHead2.transform.GetComponent<UISprite>().spriteName = "fj_" + (head2+1).ToString();
            //PlayerAddBetScore.GetComponent<label_number>().m_iNum = score + betscore;
            PlayerBetScore.GetComponent<label_number>().m_iNum = betscore;

            int areaid = 0;
            for (int i = 0; i < GameXY.HORSES_ALL - 1; i++)
            {
                for (int k = 0; k < GameXY.HORSES_ALL - i - 1; k++)
                {
                    if ((head1 == i && head2 == GameXY.HORSES_ALL - k - 1)
                        || (head2 == i && head1 == GameXY.HORSES_ALL - k - 1))
                    {
                        PlayerAddBetScore.GetComponent<label_number>().m_iNum = areamultiple[areaid] * playerTableScore[areaid];
                    }
                    areaid++;
                }
            }

            if (score > 0)
            {
                PlayerLoseSocre.SetActive(false);
                PlayerWinSocre.SetActive(true);
                PlayerWinSocre.transform.GetComponent<label_number>().m_iNum = score;
                PlayerRanking.transform.parent.gameObject.SetActive(true);
                PlayerRanking.transform.GetComponent<label_number>().m_iNum = ranking;
            }
            else
            {
                if (score == 0)
                {
                    PlayerLoseSocre.SetActive(false);
                    PlayerWinSocre.SetActive(true);
                    PlayerWinSocre.transform.GetComponent<label_number>().m_iNum = score;
                }
                else
                {
                    PlayerWinSocre.SetActive(false);
                    PlayerLoseSocre.SetActive(true);
                    PlayerLoseSocre.transform.GetComponent<label_number>().m_iNum = score;
                }
                PlayerRanking.transform.parent.gameObject.SetActive(false);
            }
        }

        public void SetRankingList(string[] ranking, long[] score)
        {
            int datacount = 0;

            foreach (Transform child in RankingUser.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < ranking.Length; i++)
            {
                if (score[i] <= 0) continue;
                datacount ++;
                GameObject child = null;
                child = (GameObject)Instantiate(userResultData, Vector3.zero, Quaternion.identity);

                child.name = i.ToString();
                child.transform.parent = RankingUser.transform;
                child.transform.localPosition = Vector3.zero;
                child.transform.localScale = Vector3.one;                

                if (i < 3)
                {
                    child.transform.Find("item_value").gameObject.SetActive(false);
                    child.transform.Find("item_head_0").gameObject.SetActive(true);
                    child.transform.Find("item_head_0").GetComponent<UISprite>().spriteName = "sort_" + (i+1).ToString();
                    child.transform.Find("item_name_0").GetComponent<UILabel>().text = ranking[i];
                    child.transform.Find("item_score_0").GetComponent<UILabel>().text = score[i].ToString();
                }
                else
                {
                    child.transform.Find("item_value").gameObject.SetActive(true);
                    child.transform.Find("item_value").GetComponent<label_number>().m_iNum = i + 1;
                    child.transform.Find("item_head_0").gameObject.SetActive(false);
                    child.transform.Find("item_name_0").GetComponent<UILabel>().text = ranking[i];
                    child.transform.Find("item_score_0").GetComponent<UILabel>().text = score[i].ToString();
                }
            }

            if (datacount < 6 && datacount != 0)
            {
                for (int i = 0; i < 6 - datacount; i++)
                {
                    GameObject nullobj = null;
                    nullobj = (GameObject)Instantiate(emptyData, Vector3.zero, Quaternion.identity);

                    nullobj.name = (datacount + i).ToString();
                    nullobj.transform.parent = RankingUser.transform;
                }
            }

            RankingUser.GetComponent<UIGrid>().enabled = true;
            RankingUser.GetComponent<UIGrid>().Reposition();
            RankingUser.transform.parent.GetComponent<UIScrollView>().ResetPosition();
        }
    }
}

