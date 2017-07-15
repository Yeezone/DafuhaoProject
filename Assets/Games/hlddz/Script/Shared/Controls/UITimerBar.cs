using UnityEngine;

namespace com.QH.QPGame.DDZ
{
    [AddComponentMenu("Custom/Controls/TimerBar")]
    public class UITimerBar : MonoBehaviour
    {
        private uint _startTime = 0;
        private uint _oldtime = 0;
        private uint _time = 0;
        private bool _enable = false;
        public GameObject ProgBar = null;
        public GameObject target = null;
        public string EndCallBack = "OnSpeakTimerEnd";

        public float volume = 1f;
        public float pitch = 1f;
        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (_enable == false) return;

            if (_time > 0)
            {
                uint nNow = (uint) System.Environment.TickCount;
                if ((nNow - _startTime) > 50)
                {

                    _time = _time - 50;
                    _startTime = (uint) System.Environment.TickCount;
                    //
                    Show(_time);

                }
            }
            else
            {
                KillTimer();
            }
        }

        public void SetTimer(uint time)
        {
            if (time > 0)
            {
                _startTime = (uint) System.Environment.TickCount;
                _time = time;
                _oldtime = time;
                _enable = true;
                Show(time);
                gameObject.SetActive(true);
            }
            else
            {
                _enable = false;
                _time = 0;
                _oldtime = 0;
                gameObject.SetActive(false);
            }
        }

        public void KillTimer()
        {
            if (target != null && !string.IsNullOrEmpty(EndCallBack))
            {
                target.SendMessage(EndCallBack, gameObject, SendMessageOptions.DontRequireReceiver);
            }
            _enable = false;
            _time = 0;
            gameObject.SetActive(false);

        }

        private void Show(uint t)
        {
            if (ProgBar != null)
            {
                float v = (float) _time/(float) _oldtime;
                ProgBar.GetComponent<UISlider>().sliderValue = v;

            }

        }

        public int GetTimedelay()
        {
            return (int) (_oldtime - _time);
        }

    }

}