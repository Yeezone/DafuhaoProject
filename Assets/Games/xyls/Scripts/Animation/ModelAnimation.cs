using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.xyls
{
    public class ModelAnimation : MonoBehaviour
    {

        public static ModelAnimation _instance;

        // 指针
        public GameObject zhizhen;
        // 组件
        private TweenRotation tr_zhizehn;
        // 目标
        [HideInInspector]
        public float target_zhizhen;

        // 动物
        public GameObject m_gAnimalParent;
        // 当前开奖动物
        //[HideInInspector]
        //public GameObject m_gPrizeAnimal;
        // 缓存开奖动物的坐标
        private Vector3 m_vAnimalPosTemp;
        // 缓存开奖动物的旋转
        private Quaternion m_qAnimalRotTemp = Quaternion.identity;
        // 组件
        private TweenRotation tr_dongwu;
        // 目标
        [HideInInspector]
        public float target_dongwu;

        // 庄闲和
        public GameObject enjoyGameType;
        // 组件
        private TweenPosition tp_enjoyGameType;
        // 目标
        private float target_enjoyGameType;
        // 缓存Rect
        private Rect temp_rect = new Rect(0, 0, 1, 1);
        // 庄闲和的UITexture组件
        private UITexture m_tEnjoyGameType;
        // 缓存上局庄闲和开奖的位置
        [HideInInspector]
        public float target_enjoyGameType_temp=0;

        // 动物父节点
        //public Transform m_tAnimalParent;
        // 特殊开奖_动物父节点
        public GameObject m_gSpecialPrize_AllAnimal;
        // 动物特效
        public GameObject m_gSpecialEffect_Animal;
        // 特殊开奖_颜色父节点
        public GameObject m_gSpecialPrize_AllColor;
        // 颜色特效
        public GameObject m_gSpecialEffect_Color;
        
        // 相机
        public GameObject m_gCamera;
        // 动物模型带的面板
        private GameObject[] animalPlane = new GameObject[2];

        void Start()
        {
            _instance = this;

            tr_zhizehn = zhizhen.GetComponent<TweenRotation>();
            tr_dongwu = m_gAnimalParent.GetComponent<TweenRotation>();
            tp_enjoyGameType = enjoyGameType.GetComponent<TweenPosition>();
            m_tEnjoyGameType = enjoyGameType.GetComponent<UITexture>();
        }

        void Update()
        {
            temp_rect.y = enjoyGameType.transform.localPosition.z * 0.1f;
            m_tEnjoyGameType.uvRect = temp_rect;
        }

        void OnDestroy()
        {
            _instance = null;
        }

        /// <summary>
        /// 旋转动画
        /// </summary>
        public void RotModel_zhizhen(int animal, int[] prizeIndex, int enjoyGame)
        {
            // 获取到开奖结果的位置.
            target_zhizhen = (prizeIndex[0] * 15) - 90;
            if (animal == 0)
            {
                target_dongwu = (prizeIndex[0] * 15) - 0;
            }
            else if (animal == 1)
            {
                target_dongwu = (prizeIndex[0] * 15) - 15;
            }
            else if (animal == 2)
            {
                target_dongwu = (prizeIndex[0] * 15) - 30;
            }
            else if (animal == 3)
            {
                target_dongwu = (prizeIndex[0] * 15) - 45;
            }

            target_zhizhen -= 1080;
            tr_zhizehn.from = zhizhen.transform.localRotation.eulerAngles;
            tr_zhizehn.to = new Vector3(0, target_zhizhen, 0);
            tr_zhizehn.enabled = true;
            tr_zhizehn.ResetToBeginning();

            target_dongwu += 1440;
            tr_dongwu.from = m_gAnimalParent.transform.localRotation.eulerAngles;
            tr_dongwu.to = new Vector3(0, target_dongwu, 0);
            tr_dongwu.enabled = true;
            tr_dongwu.ResetToBeginning();

            if (enjoyGame == 0)
            {
                tp_enjoyGameType.from = new Vector3(0, enjoyGameType.transform.localPosition.y, target_enjoyGameType_temp);
                tp_enjoyGameType.to = new Vector3(0, enjoyGameType.transform.localPosition.y, -60f);
                tp_enjoyGameType.enabled = true;
                tp_enjoyGameType.ResetToBeginning();

                target_enjoyGameType_temp = 0.0f;
            }
            else if (enjoyGame == 1)
            {
                tp_enjoyGameType.from = new Vector3(0, enjoyGameType.transform.localPosition.y, target_enjoyGameType_temp);
                tp_enjoyGameType.to = new Vector3(0, enjoyGameType.transform.localPosition.y, -63.3f);
                tp_enjoyGameType.enabled = true;
                tp_enjoyGameType.ResetToBeginning();

                target_enjoyGameType_temp = -3.3f;
            }
            else if (enjoyGame == 2)
            {
                tp_enjoyGameType.from = new Vector3(0, enjoyGameType.transform.localPosition.y, target_enjoyGameType_temp);
                tp_enjoyGameType.to = new Vector3(0, enjoyGameType.transform.localPosition.y, -66.6f);
                tp_enjoyGameType.enabled = true;
                tp_enjoyGameType.ResetToBeginning();

                target_enjoyGameType_temp = -6.6f;
            }
            else
            {
                Debug.LogError("未知庄闲和开奖结果");
            }
            // 播放庄闲和_开奖音效
            AudioList._instance.StartCoroutine(AudioList._instance.PlayPrizeEnjoyGameTypeAudio(enjoyGame, 9f));
        }

        public IEnumerator RotModel_repeat(int animal, int prizeIndex)
        {
            // 等待第一次指针停止(23s),动物变大(1s),撒金币(1s)
            yield return new WaitForSeconds(23f);

            // 获取到开奖结果的位置.
            target_zhizhen = (prizeIndex * 15) - 90;
            if (animal == 0)
            {
                target_dongwu = (prizeIndex * 15) - 0;
            }
            else if (animal == 1)
            {
                target_dongwu = (prizeIndex * 15) - 15;
            }
            else if (animal == 2)
            {
                target_dongwu = (prizeIndex * 15) - 30;
            }
            else if (animal == 3)
            {
                target_dongwu = (prizeIndex * 15) - 45;
            }

            target_zhizhen -= 1440;
            tr_zhizehn.from = zhizhen.transform.localRotation.eulerAngles;
            tr_zhizehn.to = new Vector3(0, target_zhizhen, 0);
            tr_zhizehn.enabled = true;
            tr_zhizehn.ResetToBeginning();

            target_dongwu += 1440;
            tr_dongwu.from = m_gAnimalParent.transform.localRotation.eulerAngles;
            tr_dongwu.to = new Vector3(0, target_dongwu, 0);
            tr_dongwu.enabled = true;
            tr_dongwu.ResetToBeginning();
        }

        /// <summary>
        /// 旋转庄闲和
        /// </summary>
//         public void RotModel_enjoyGameType()
//         {
//             float temp_uvRectY = enjoyGameType.GetComponent<UITexture>().uvRect.y;
//             tp_enjoyGameType.from = new Vector3(0, temp_uvRectY, 0);
//             tp_enjoyGameType.to = new Vector3(0, target_enjoyGameType - temp_uvRectY - 9, 0);
//             tp_enjoyGameType.enabled = true;
//             tp_enjoyGameType.ResetToBeginning();
//         }

        /////////////////////////////////////////开奖动画_场景动画////////////////////////////////////
        /// <summary>
        /// 场景动画:正常模式(动物,相机,闪烁水晶等)
        /// </summary>
        public IEnumerator SimplePrizeAnim(GameObject[] m_gAllAnimal, float m_fAxisY, int prizeColor,int prizeAnimal,int prizeMode)
        {
            GameObject m_gPrizeAnimal = null;
            yield return new WaitForSeconds(23f);
            // 播放音效
            AudioBgCtr._instance.PlayBGM(2);
            AudioList._instance.PlayPrizeAnimalColorAudio(prizeAnimal, prizeColor, prizeMode);
            // 16s后,开奖动物变大
            foreach (GameObject var in m_gAllAnimal)
            {
                // 遍历所有动物,匹配开奖位置的动物(误差5°)
                if (Mathf.Abs(var.transform.rotation.eulerAngles.y - m_fAxisY) <= 5)
                {
                    m_gPrizeAnimal = var;
                }
            }
            if (m_gPrizeAnimal == null)
            {

                Debug.LogError("没有遍历到开奖动物_开奖动画");
            }
            // 开启动物身上的plane
            animalPlane[0] = m_gPrizeAnimal.transform.FindChild("Plane").gameObject;
            if (prizeColor == 0)
            {
                animalPlane[0].GetComponent<Renderer>().material.color = Color.red;
            }
            else if (prizeColor == 1)
            {
                animalPlane[0].GetComponent<Renderer>().material.color = Color.green;
            }
            else if (prizeColor == 2)
            {
                animalPlane[0].GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                Debug.LogError("动物身上的面板不能匹配到颜色");
            }
            animalPlane[0].SetActive(true);
            // 变大动画(1s动画)
            TweenScale temp_ts = m_gPrizeAnimal.GetComponent<TweenScale>();
            temp_ts.enabled = true;
            yield return new WaitForSeconds(1f);
            // 关闭pingpong动画,并且强行赋值动物变大.
            temp_ts.enabled = false;
            m_gPrizeAnimal.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            // 动物上移
            m_vAnimalPosTemp = m_gPrizeAnimal.transform.position;
            TweenPosition temp_tp = m_gPrizeAnimal.GetComponent<TweenPosition>();
            temp_tp.from = m_gPrizeAnimal.transform.localPosition;
            temp_tp.to = new Vector3(m_gPrizeAnimal.transform.localPosition.x, 5f, m_gPrizeAnimal.transform.localPosition.z);
            temp_tp.enabled = true;
            temp_tp.ResetToBeginning();

            yield return new WaitForSeconds(1f);
            // 动物位移到中间   
            temp_tp.from = m_gPrizeAnimal.transform.localPosition;
            //temp_tp.to = new Vector3(0.07f, 5f, -0.17f);
            temp_tp.to = new Vector3(0f, 5f, 0f);
            temp_tp.enabled = true;
            temp_tp.ResetToBeginning();
            // 动物旋转
            m_qAnimalRotTemp.eulerAngles = m_gPrizeAnimal.transform.localRotation.eulerAngles;
            TweenRotation temp_tr = m_gPrizeAnimal.GetComponent<TweenRotation>();
            temp_tr.from = m_gPrizeAnimal.transform.localRotation.eulerAngles;
            temp_tr.to = new Vector3(0, 180 - m_gAnimalParent.transform.rotation.eulerAngles.y, 0);
            temp_tr.enabled = true;
            temp_tr.ResetToBeginning();
            // 相机移动
            TweenPosition temp_tpCamera = m_gCamera.GetComponent<TweenPosition>();
//             temp_tpCamera.from = new Vector3(0.3f, 10f, -10.4f);
//             temp_tpCamera.to = new Vector3(0.3f, 7f, -6.5f);
            temp_tpCamera.from = new Vector3(0f, 8.5f, -8.2f);
            temp_tpCamera.to = new Vector3(0f, 7.5f, -5f);
            temp_tpCamera.enabled = true;
            temp_tpCamera.ResetToBeginning();
            // 相机旋转
            TweenRotation temp_trCamera = m_gCamera.GetComponent<TweenRotation>();
//             temp_trCamera.from = new Vector3(40, 0, 0);
//             temp_trCamera.to = new Vector3(10, 0, 0);
            temp_trCamera.from = new Vector3(35, 0, 0);
            temp_trCamera.to = new Vector3(20, 0, 0);
            temp_trCamera.enabled = true;
            temp_trCamera.ResetToBeginning();
            //播放赢动画(场景中的水晶闪烁)
            SceneAnimCtr._instance.AnimalAnim_Win(m_gPrizeAnimal);

            // 1s后
            yield return new WaitForSeconds(1f);
            SceneAnimCtr._instance.m_iPrizeColor = prizeColor;
            SceneAnimCtr._instance.m_iCrystalFlashNum = 14;
            SceneAnimCtr._instance.m_bCrystalChange = true;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 1;
            SceneAnimCtr._instance.timer = 0;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 0;
            SceneAnimCtr._instance.m_iCrystalFlashMode = 1;

            // 播放7s动画,恢复动物位置和大小,恢复相机,恢复动物旋转
            yield return new WaitForSeconds(5f);

            //ResetGame_SceneAnim();
            Quaternion temp_rot = Quaternion.identity;
            SceneAnimCtr._instance.AnimalAnim_StopWin(m_gPrizeAnimal);
            m_gPrizeAnimal.transform.position = m_vAnimalPosTemp;
            m_gPrizeAnimal.transform.localScale = new Vector3(1f, 1f, 1f);
//             m_gCamera.transform.localPosition = new Vector3(0.3f, 10f, -10.4f);
//             temp_rot.eulerAngles = new Vector3(40, 0, 0);
            m_gCamera.transform.localPosition = new Vector3(0f, 8.5f, -8.2f);
            temp_rot.eulerAngles = new Vector3(35, 0, 0);
            m_gCamera.transform.localRotation = temp_rot;
            m_gPrizeAnimal.transform.localRotation = m_qAnimalRotTemp;
            animalPlane[0].SetActive(false);
        }

        /// <summary>
        /// 场景动画:单颜色开奖模式(闪烁水晶)
        /// </summary>
        public IEnumerator SingleColorAnim(int prizeColor)
        {
            yield return new WaitForSeconds(22f);
            // 播放音效
            AudioBgCtr._instance.PlayBGM(4);
            AudioList._instance.PlayPrizeAnimalColorAudio(1000, prizeColor, 1);
            SceneAnimCtr._instance.m_iPrizeColor = prizeColor;
            SceneAnimCtr._instance.m_iCrystalFlashNum = 20;
            SceneAnimCtr._instance.m_bCrystalChange = true;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 1;
            SceneAnimCtr._instance.timer = 0;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 0;
            SceneAnimCtr._instance.m_iCrystalFlashMode = 1;

            yield return new WaitForSeconds(1f);
            // 相机移动
            TweenPosition temp_tpCamera = m_gCamera.GetComponent<TweenPosition>();
//             temp_tpCamera.from = new Vector3(0.3f, 10f, -10.4f);
//             temp_tpCamera.to = new Vector3(0.3f, 7f, -6.5f);
            temp_tpCamera.from = new Vector3(0f, 8.5f, -8.2f);
            temp_tpCamera.to = new Vector3(0f, 7.5f, -5f);
            temp_tpCamera.enabled = true;
            temp_tpCamera.ResetToBeginning();
            // 相机旋转
            TweenRotation temp_trCamera = m_gCamera.GetComponent<TweenRotation>();
//             temp_trCamera.from = new Vector3(40, 0, 0);
//             temp_trCamera.to = new Vector3(10, 0, 0);
            temp_trCamera.from = new Vector3(35, 0, 0);
            temp_trCamera.to = new Vector3(20, 0, 0);
            temp_trCamera.enabled = true;
            temp_trCamera.ResetToBeginning();

            yield return new WaitForSeconds(8f);

            Quaternion temp_rot = Quaternion.identity;
//             m_gCamera.transform.localPosition = new Vector3(0.3f, 10f, -10.4f);
//             temp_rot.eulerAngles = new Vector3(40, 0, 0);
            m_gCamera.transform.localPosition = new Vector3(0f, 8.5f, -8.2f);
            temp_rot.eulerAngles = new Vector3(35, 0, 0);
            m_gCamera.transform.localRotation = temp_rot;
        }

        /// <summary>
        /// 场景动画:单动物开奖模式(闪烁水晶)
        /// </summary>
        public IEnumerator SingleAnimalAnim(int prizeColor, int prizeAnimal)
        {
            yield return new WaitForSeconds(22f);
            // 播放音效
            AudioBgCtr._instance.PlayBGM(3);
            AudioList._instance.PlayPrizeAnimalColorAudio(prizeAnimal, 1000, 2);

            SceneAnimCtr._instance.m_iPrizeColor = prizeColor;
            SceneAnimCtr._instance.m_iCrystalFlashNum = 20;
            SceneAnimCtr._instance.m_bCrystalChange = true;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 2;
            SceneAnimCtr._instance.timer = 0;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 0;
            SceneAnimCtr._instance.m_iCrystalFlashMode = 2;

            yield return new WaitForSeconds(1f);
            // 相机移动
            TweenPosition temp_tpCamera = m_gCamera.GetComponent<TweenPosition>();
//             temp_tpCamera.from = new Vector3(0.3f, 10f, -10.4f);
//             temp_tpCamera.to = new Vector3(0.3f, 7f, -6.5f);
            temp_tpCamera.from = new Vector3(0f, 8.5f, -8.2f);
            temp_tpCamera.to = new Vector3(0f, 7.5f, -5f);
            temp_tpCamera.enabled = true;
            temp_tpCamera.ResetToBeginning();
            // 相机旋转
            TweenRotation temp_trCamera = m_gCamera.GetComponent<TweenRotation>();
//             temp_trCamera.from = new Vector3(40, 0, 0);
//             temp_trCamera.to = new Vector3(10, 0, 0);
            temp_trCamera.from = new Vector3(35, 0, 0);
            temp_trCamera.to = new Vector3(20, 0, 0);
            temp_trCamera.enabled = true;
            temp_trCamera.ResetToBeginning();

            yield return new WaitForSeconds(8f);

            //ResetGame_SceneAnim();
            Quaternion temp_rot = Quaternion.identity;
//             m_gCamera.transform.localPosition = new Vector3(0.3f, 10f, -10.4f);
//             temp_rot.eulerAngles = new Vector3(40, 0, 0);
            m_gCamera.transform.localPosition = new Vector3(0f, 8.5f, -8.2f);
            temp_rot.eulerAngles = new Vector3(35, 0, 0);
            m_gCamera.transform.localRotation = temp_rot;
        }

        /// <summary>
        /// 场景动画:系统彩金开奖模式
        /// </summary>
        public IEnumerator SysPrizeAnim(GameObject[] m_gAllAnimal, float m_fAxisY, int prizeColor, int prizeAnimal, int prizeMode)
        {
            GameObject m_gPrizeAnimal = null;
            yield return new WaitForSeconds(14f);
            // 播放音效
            AudioBgCtr._instance.PlayBGM(2);
            AudioList._instance.PlayPrizeAnimalColorAudio(prizeAnimal, prizeColor, prizeMode);
            // 14s后,开奖动物变大
            foreach (GameObject var in m_gAllAnimal)
            {
                // 遍历所有动物,匹配开奖位置的动物(误差5°)
                if (Mathf.Abs(var.transform.rotation.eulerAngles.y - m_fAxisY) <= 5)
                {
                    m_gPrizeAnimal = var;
                }
            }
            // 开启动物身上的plane
            animalPlane[0] = m_gPrizeAnimal.transform.FindChild("Plane").gameObject;
            if (prizeColor == 0)
            {
                animalPlane[0].GetComponent<Renderer>().material.color = Color.red;
            }
            else if (prizeColor == 1)
            {
                animalPlane[0].GetComponent<Renderer>().material.color = Color.green;
            }
            else if (prizeColor == 2)
            {
                animalPlane[0].GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                Debug.LogError("动物身上的面板不能匹配到颜色");
            }
            animalPlane[0].SetActive(true);
            // 变大动画(1s动画)
            TweenScale temp_ts = m_gPrizeAnimal.GetComponent<TweenScale>();
            temp_ts.enabled = true;
            yield return new WaitForSeconds(1f);
            // 关闭pingpong动画,并且强行赋值动物变大.
            temp_ts.enabled = false;
            m_gPrizeAnimal.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            // 动物上移
            m_vAnimalPosTemp = m_gPrizeAnimal.transform.position;
            TweenPosition temp_tp = m_gPrizeAnimal.GetComponent<TweenPosition>();
            temp_tp.from = m_gPrizeAnimal.transform.localPosition;
            temp_tp.to = new Vector3(m_gPrizeAnimal.transform.localPosition.x, 5f, m_gPrizeAnimal.transform.localPosition.z);
            temp_tp.enabled = true;
            temp_tp.ResetToBeginning();

            yield return new WaitForSeconds(1f);
            // 动物位移到中间   
            temp_tp.from = m_gPrizeAnimal.transform.localPosition;
            temp_tp.to = new Vector3(0.07f, 5f, -0.17f);
            temp_tp.enabled = true;
            temp_tp.ResetToBeginning();
            // 动物旋转
            m_qAnimalRotTemp.eulerAngles = m_gPrizeAnimal.transform.localRotation.eulerAngles;
            TweenRotation temp_tr = m_gPrizeAnimal.GetComponent<TweenRotation>();
            temp_tr.from = m_gPrizeAnimal.transform.localRotation.eulerAngles;
            temp_tr.to = new Vector3(0, 180 - m_gAnimalParent.transform.rotation.eulerAngles.y, 0);
            temp_tr.enabled = true;
            temp_tr.ResetToBeginning();
            // 相机移动
            TweenPosition temp_tpCamera = m_gCamera.GetComponent<TweenPosition>();
//             temp_tpCamera.from = new Vector3(0.3f, 10f, -10.4f);
//             temp_tpCamera.to = new Vector3(0.3f, 7f, -6.5f);
            temp_tpCamera.from = new Vector3(0f, 8.5f, -8.2f);
            temp_tpCamera.to = new Vector3(0f, 7.5f, -5f);
            temp_tpCamera.enabled = true;
            temp_tpCamera.ResetToBeginning();
            // 相机旋转
            TweenRotation temp_trCamera = m_gCamera.GetComponent<TweenRotation>();
//             temp_trCamera.from = new Vector3(40, 0, 0);
//             temp_trCamera.to = new Vector3(10, 0, 0);
            temp_trCamera.from = new Vector3(35, 0, 0);
            temp_trCamera.to = new Vector3(20, 0, 0);
            temp_trCamera.enabled = true;
            temp_trCamera.ResetToBeginning();
            // 播放win动画
            SceneAnimCtr._instance.AnimalAnim_Win(m_gPrizeAnimal);
            // 1s后,场景中的水晶闪烁
            yield return new WaitForSeconds(1f);
            //m_gPrizeAnimal.GetComponentInChildren<Animator>().SetBool("Win", true);
            SceneAnimCtr._instance.AnimalAnim_Win(m_gPrizeAnimal);
            SceneAnimCtr._instance.m_iPrizeColor = prizeColor;
            SceneAnimCtr._instance.m_iCrystalFlashNum = 14;
            SceneAnimCtr._instance.m_bCrystalChange = true;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 2;
            SceneAnimCtr._instance.timer = 0;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 0;
            SceneAnimCtr._instance.m_iCrystalFlashMode = 2;

            // 播放7s动画,恢复动物位置和大小,恢复相机,恢复动物旋转
            yield return new WaitForSeconds(7f);

            // 关闭彩金动画
            CreateGoldCtr._instance.m_bIsOpen = false;
            Quaternion temp_rot = Quaternion.identity;
            SceneAnimCtr._instance.AnimalAnim_StopWin(m_gPrizeAnimal);
            m_gPrizeAnimal.transform.position = m_vAnimalPosTemp;
            m_gPrizeAnimal.transform.localScale = new Vector3(1f, 1f, 1f);
//             m_gCamera.transform.localPosition = new Vector3(0.3f, 10f, -10.4f);
//             temp_rot.eulerAngles = new Vector3(40, 0, 0);
            m_gCamera.transform.localPosition = new Vector3(0f, 8.5f, -8.2f);
            temp_rot.eulerAngles = new Vector3(35, 0, 0);
            m_gCamera.transform.localRotation = temp_rot;
            m_gPrizeAnimal.transform.localRotation = m_qAnimalRotTemp;
            animalPlane[0].SetActive(false);
        }

        /// <summary>
        /// 场景动画:重复开奖(动物,相机,闪烁水晶等)
        /// </summary>
        public IEnumerator RepeatPrizeAnim(GameObject[] m_gAllAnimal, float[] m_fAxisY, int[] prizeColor, int[] prizeAnimal)
        {
            GameObject[] m_gPrizeAnimal = new GameObject[2];
            yield return new WaitForSeconds(22f);
            // 播放音效
            AudioBgCtr._instance.PlayBGM(2);
            AudioList._instance.PlayPrizeAnimalColorAudio(prizeAnimal[0], prizeColor[0], 4);
            // 15s后,开奖动物变大
            foreach (GameObject var in m_gAllAnimal)
            {
                // 遍历所有动物,匹配开奖位置的动物(误差5°)
                if (Mathf.Abs(var.transform.rotation.eulerAngles.y - m_fAxisY[0]) <= 5)
                {
                    m_gPrizeAnimal[0] = var;
                }
            }
            if (m_gPrizeAnimal == null)
            {

                Debug.LogError("没有遍历到开奖动物_开奖动画");
            }
            // 开启动物身上的plane
            animalPlane[0] = m_gPrizeAnimal[0].transform.FindChild("Plane").gameObject;
            if (prizeColor[0] == 0)
            {
                animalPlane[0].GetComponent<Renderer>().material.color = Color.red;
            }
            else if (prizeColor[0] == 1)
            {
                animalPlane[0].GetComponent<Renderer>().material.color = Color.green;
            }
            else if (prizeColor[0] == 2)
            {
                animalPlane[0].GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                Debug.LogError("动物身上的面板不能匹配到颜色");
            }
            animalPlane[0].SetActive(true);
            // 变大动画(1s动画)
            TweenScale temp_ts = m_gPrizeAnimal[0].GetComponent<TweenScale>();
            temp_ts.enabled = true;
            yield return new WaitForSeconds(1f);
            // 关闭pingpong动画,并且强行赋值动物变大.
            temp_ts.enabled = false;
            m_gPrizeAnimal[0].transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            // 开启金币动画
            CreateGoldCtr._instance.m_bIsOpen = true;
            // 闪烁水晶(闪7秒)
            SceneAnimCtr._instance.m_iPrizeColor = prizeColor[0];
            SceneAnimCtr._instance.m_iCrystalFlashNum = 40;
            SceneAnimCtr._instance.m_bCrystalChange = true;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 1;
            SceneAnimCtr._instance.timer = 0;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 0;
            SceneAnimCtr._instance.m_iCrystalFlashMode = 2;

            yield return new WaitForSeconds(22f);
            foreach (GameObject var in m_gAllAnimal)
            {
                // 遍历所有动物,匹配开奖位置的动物(误差5°)
                if (Mathf.Abs(var.transform.rotation.eulerAngles.y - m_fAxisY[1]) <= 5)
                {
                    m_gPrizeAnimal[1] = var;
                }
            }
            if (m_gPrizeAnimal == null)
            {

                Debug.LogError("没有遍历到开奖动物_开奖动画");
            }
            // 开启动物身上的plane
            animalPlane[1] = m_gPrizeAnimal[1].transform.FindChild("Plane").gameObject;
            if (prizeColor[1] == 0)
            {
                animalPlane[1].GetComponent<Renderer>().material.color = Color.red;
            }
            else if (prizeColor[1] == 1)
            {
                animalPlane[1].GetComponent<Renderer>().material.color = Color.green;
            }
            else if (prizeColor[1] == 2)
            {
                animalPlane[1].GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                Debug.LogError("动物身上的面板不能匹配到颜色");
            }
            animalPlane[1].SetActive(true);
            // 变大动画(1s动画)
            temp_ts = m_gPrizeAnimal[1].GetComponent<TweenScale>();
            temp_ts.enabled = true;
            yield return new WaitForSeconds(1f);
            // 关闭pingpong动画,并且强行赋值动物变大.
            temp_ts.enabled = false;
            m_gPrizeAnimal[1].transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            // 闪烁水晶(闪7秒)
            SceneAnimCtr._instance.m_iPrizeColor = prizeColor[1];
            SceneAnimCtr._instance.m_iCrystalFlashNum = 10;
            SceneAnimCtr._instance.m_bCrystalChange = true;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 1;
            SceneAnimCtr._instance.timer = 0;
            SceneAnimCtr._instance.m_iCrystalChangeNum = 0;
            SceneAnimCtr._instance.m_iCrystalFlashMode = 1;

            // 播放7s动画,恢复动物位置和大小,恢复相机,恢复动物旋转
            yield return new WaitForSeconds(7f);
            // 关闭彩金动画
            CreateGoldCtr._instance.m_bIsOpen = false;
            m_gPrizeAnimal[0].transform.localScale = Vector3.one;
            m_gPrizeAnimal[1].transform.localScale = Vector3.one;
            animalPlane[0].SetActive(false);
            animalPlane[1].SetActive(false);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        
        /////////////////////////////////////////特殊开奖_特殊动画////////////////////////////////////
        /// <summary>
        /// 特殊开奖_单颜色(特殊动画)
        /// </summary>
        public IEnumerator SpcialPrize_SingleColor(int prizeIndex)
        {
            SingleSpecialPrize temp_sp = m_gSpecialPrize_AllColor.GetComponent<SingleSpecialPrize>();
            temp_sp.m_iPrizeIndex = prizeIndex;
            temp_sp.m_bIsOpen = true;

            TweenRotation temp_tr = m_gSpecialPrize_AllColor.GetComponent<TweenRotation>();
            temp_tr.from = new Vector3(0, 0, 0);
            temp_tr.to = new Vector3(0, 2520, 0);
            temp_tr.enabled = true;
            temp_tr.ResetToBeginning();

            m_gSpecialEffect_Color.SetActive(true);
            yield return new WaitForSeconds(31f);
            m_gSpecialEffect_Color.SetActive(false);
        }

        /// <summary>
        /// 特殊开奖_单动物(特殊动画)
        /// </summary>
        public IEnumerator SpecialPrize_SingleAnimal(int prizeIndex)
        {
            SingleSpecialPrize temp_sp = m_gSpecialPrize_AllAnimal.GetComponent<SingleSpecialPrize>();
            temp_sp.m_iPrizeIndex = prizeIndex;
            temp_sp.m_bIsOpen = true;

            TweenRotation temp_tr = m_gSpecialPrize_AllAnimal.GetComponent<TweenRotation>();
            temp_tr.from = new Vector3(0, 0, 0);
            temp_tr.to = new Vector3(0, 2520, 0);
            temp_tr.enabled = true;
            temp_tr.ResetToBeginning();

            m_gSpecialEffect_Animal.SetActive(true);
            yield return new WaitForSeconds(31f);
            m_gSpecialEffect_Animal.SetActive(false);
        }       

        /// <summary>
        /// 特殊开奖_系统彩金
        /// </summary>
        public void SpcialPrize_SysPrize()
        {
            CreateGoldCtr._instance.m_bIsOpen = true;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////

    }
}
