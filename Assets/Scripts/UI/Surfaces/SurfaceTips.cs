using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace com.QH.QPGame.Lobby.Surfaces
{
	public class SurfaceTips : Surface 
    {
        //TODO 暂留，日后清理
        public UILabel	Text_lbl;
        public UISprite	Bg_spr;

        private Action callback;

		public void	Show(string _message, float time, Action cb)
		{
		    callback = cb;

			Text_lbl.text = _message;
		    Text_lbl.gameObject.SetActive(true);
			Bg_spr.gameObject.SetActive(true);

		    if (time > 0.0f)
		    {
                StartCoroutine(DelayClose(time));
            }
		}

	    public bool IsShown()
	    {
	        return Text_lbl.gameObject.activeSelf;
	    }

	    public override void Hide()
	    {
            Text_lbl.gameObject.SetActive(false);
            Bg_spr.gameObject.SetActive(false);
	    }

        IEnumerator DelayClose(float _time)
        {
            string text = Text_lbl.text;
            while (_time > 0f)
            {
                yield return new WaitForFixedUpdate();
                _time -= Time.deltaTime;

                if (_time < 30.0f)
                {
                    Text_lbl.text = text + string.Format(" ({0})", (int)_time);
                }
            }

            Hide();

            if (callback != null)
            {
                callback();
            }
        }

	}
	
}

