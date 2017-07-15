using UnityEngine;

namespace com.QH.QPGame.DDZ
{
    [AddComponentMenu("Custom/Controls/LableEx")]

    public class UILabelEx : MonoBehaviour
    {
        public GameObject Target = null;
        //--------------------------------------------------------------------------
        private int _number = 0;
        private int _shownumber = 0;
        private int _basescore = 0;
        private bool _bChange = false;

        private void Start()
        {

        }


        private void FixedUpdate()
        {

            if (_bChange)
            {
                if (_number >= 0)
                {

                    int step = (int) (_basescore/10);
                    _shownumber += step;

                    if (_shownumber >= _number)
                    {
                        _bChange = false;
                        _shownumber = _number;
                        Target.GetComponent<UILabel>().text = _shownumber.ToString();
                    }
                    else
                    {
                        Target.GetComponent<UILabel>().text = _shownumber.ToString();
                    }
                }
                else
                {
                    int step = (int) (_basescore/10);
                    _shownumber += step;


                    if (_shownumber <= _number)
                    {
                        _bChange = false;
                        _shownumber = _number;
                        Target.GetComponent<UILabel>().text = _shownumber.ToString();
                    }
                    else
                    {
                        Target.GetComponent<UILabel>().text = _shownumber.ToString();
                    }
                }
            }
        }

        public void SetChangeNumber(int num, int basescore)
        {
            _number = num;
            _basescore = basescore;
            _shownumber = 0;
            _bChange = true;
            Target.GetComponent<UILabel>().text = "0";
        }
    }
}