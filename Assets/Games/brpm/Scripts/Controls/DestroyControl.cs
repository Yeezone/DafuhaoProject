using UnityEngine;
using System.Collections;


namespace com.QH.QPGame.BRPM
{
	public class DestroyControl : MonoBehaviour {

		//操作时间
		public float destroyTime = 0;

        //是否销毁
        public bool isDestroy = true;

		//计时
		public float time = 0;

		void Start()
		{
			time = 0;
		}

		void Update()
		{
			time += Time.deltaTime;
			if(time >= destroyTime)
			{
                if (isDestroy == true)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    gameObject.SetActive(false);
                }
			}
		}

	}
}

