using UnityEngine;

namespace com.QH.QPGame.DDZ
{
    internal enum EffectType
    {
        NULL = 0,
        BOMB = 1,
        ROCKET = 2,
        PLANE = 3,
        BEI = 4
    }

    public class UIEffect : MonoBehaviour
    {
        private static UIEffect instance = null;
        //
        private GameObject o_panel = null;
        private GameObject o_effect_bomb = null;
        private GameObject o_effect_plane = null;
        private GameObject o_effect_rocket = null;
        private GameObject o_effect_bei = null;
        private static bool _bShow = false;
        private static EffectType _nType = EffectType.NULL;
        private static int _nTick = 0;

        private void Start()
        {

        }

        private void FixedUpdate()
        {
            if (_bShow)
            {
                if (_nType == EffectType.BOMB)
                {
                    if (System.Environment.TickCount - _nTick > 1500)
                    {
                        ShowBomb(false);
                    }
                }
                else if (_nType == EffectType.PLANE)
                {
                    if (System.Environment.TickCount - _nTick > 1500)
                    {

                        ShowPlane(false);

                    }
                }
                else if (_nType == EffectType.ROCKET)
                {
                    if (System.Environment.TickCount - _nTick > 1500)
                    {
                        ShowRocket(false);
                    }
                }
                else if (_nType == EffectType.BEI)
                {
                    if (System.Environment.TickCount - _nTick > 1500)
                    {
                        ShowBei(false, 0);

                    }
                }
            }
        }

        //private void Update()
        //{

        //}

        private void Awake()
        {
            o_panel = GameObject.Find("scene_effect");
            o_effect_bomb = GameObject.Find("scene_effect/sp_effect_bomb");
            o_effect_plane = GameObject.Find("scene_effect/sp_effect_plane");
            o_effect_rocket = GameObject.Find("scene_effect/sp_effect_rocket");
            o_effect_bei = GameObject.Find("scene_effect/sp_effect_bei");

            if (instance == null)
            {
                instance = this;
            }

        }

        private void OnDestroy()
        {
            instance = null;
        }

        //
        public void ShowBomb(bool bshow)
        {
            if (bshow)
            {
                o_panel.SetActive(true);
                _nTick = System.Environment.TickCount;
                _nType = EffectType.BOMB;
                o_effect_plane.SetActive(false);
                o_effect_rocket.SetActive(false);
                o_effect_bei.SetActive(false);
                ;
                o_effect_bomb.SetActive(true);
            }
            else
            {
                o_panel.SetActive(false);
                _nTick = 0;
                _nType = EffectType.NULL;

            }
            _bShow = bshow;

        }

        public void ShowPlane(bool bshow)
        {
            if (bshow)
            {
                o_panel.SetActive(true);
                _nTick = System.Environment.TickCount;
                _nType = EffectType.PLANE;
                o_effect_bomb.SetActive(false);
                o_effect_rocket.SetActive(false);
                o_effect_bei.SetActive(false);
                o_effect_plane.SetActive(true);
            }
            else
            {
                o_panel.SetActive(false);
                _nTick = 0;
                _nType = EffectType.NULL;

            }
            _bShow = bshow;
        }

        public void ShowRocket(bool bshow)
        {
            if (bshow)
            {
                o_panel.SetActive(true);
                _nTick = System.Environment.TickCount;
                _nType = EffectType.ROCKET;
                o_effect_bomb.SetActive(false);
                o_effect_plane.SetActive(false);
                o_effect_bei.SetActive(false);
                o_effect_rocket.SetActive(true);

            }
            else
            {
                o_panel.SetActive(false);
                _nTick = 0;
                _nType = EffectType.NULL;

            }
            _bShow = bshow;
        }

        public void ShowBei(bool bshow, int nBei)
        {
            if (bshow)
            {
                o_panel.SetActive(true);
                _nTick = System.Environment.TickCount;
                _nType = EffectType.BEI;
                o_effect_bomb.SetActive(false);
                o_effect_plane.SetActive(false);
                o_effect_rocket.SetActive(false);
                o_effect_bei.SetActive(true);
                o_effect_bei.GetComponent<UISprite>().spriteName = "x" + nBei.ToString();

            }
            else
            {
                o_panel.SetActive(false);
                _nTick = 0;
                _nType = EffectType.NULL;
            }
            _bShow = bshow;
        }

        public void ClearAllEffect()
        {
            o_panel.SetActive(false);

            _nTick = 0;
            //_nQty  = 0;
            _nType = EffectType.NULL;

        }

        //
        public static UIEffect Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("UIEffect").AddComponent<UIEffect>();
                }
                return instance;
            }
        }

    }
}