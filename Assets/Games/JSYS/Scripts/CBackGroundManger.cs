using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace com.QH.QPGame.JSYS
{
    public class CBackGroundManger : MonoBehaviour
    {
        public static CBackGroundManger _instance;
        /// <summary>
        /// 背景
        /// </summary>
        public List<GameObject> m_gBgList = new List<GameObject>();

        public bool m_bIsGameStatus = false;
        public CWindowShowHide_JSYS m_cExit;

        void Awake()
        {
            _instance = this;
        }

        void OnDestroy()
        {
            _instance = null;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
            {
                CloseWindow();
            }
        }
        /// <summary>
        /// 设置背景
        /// </summary>
        /// <param name="_iIndex">当前背景下标“0是海洋背景”“1是走兽”“2飞禽”</param>
        public void SetBackGround(int _iIndex)
        {
            for (int i = 0; i < m_gBgList.Count; i++)
            {
                m_gBgList[i].SetActive(false);
            }
            m_gBgList[_iIndex].SetActive(true);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void CloseWindow()
        {
            if (m_bIsGameStatus)
            {
                m_cExit.ShowWindow();
                StartCoroutine(WaitTimeHide());
            }
            else
            {
                 CGameEngine.Instance.Quit();
            }
        }
        public void MinWindow_OnClick()
        {

        }
        IEnumerator WaitTimeHide()
        {
            yield return new WaitForSeconds(3.0f);
            m_cExit.HideWindow();
        }

    }
}
