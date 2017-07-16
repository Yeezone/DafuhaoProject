using UnityEngine;
using System.Collections;


namespace com.QH.QPGame.GDNN
{
	public class Destr : MonoBehaviour
	{

		public float time = 4.0f;

		void Update ()
		{

			time -= Time.deltaTime;

			if (time < 0.0f) {
				Destroy (this.gameObject);
			}
		}
	}
}