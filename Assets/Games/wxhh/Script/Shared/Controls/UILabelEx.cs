using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.WXHH
{

    [AddComponentMenu("Custom/Controls/LableEx")]

    public class UILabelEx : MonoBehaviour
    {
        public GameObject Target = null;
        //--------------------------------------------------------------------------
        int _number = 0;
        int _shownumber = 0;
        int _basescore = 0;
        bool _bChange = false;
        void Start()
        {

        }


        void FixedUpdate()
        {

            if (_bChange)
            {
                if (_number >= 0)
                {

                    int step = (int)(_basescore / 40);
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
                    int step = (int)(_basescore / 40);
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

        public void SetChangeNumber(int num)
        {
            _number = num;
            _basescore = num;
            _shownumber = 0;
            _bChange = true;
            Target.GetComponent<UILabel>().text = "0";
        }
    }
}