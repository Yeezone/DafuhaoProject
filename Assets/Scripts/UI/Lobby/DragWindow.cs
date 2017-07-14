using UnityEngine;
using System;
using com.QH.QPGame.Lobby;

namespace com.QH.QPGame.Utility
{
    public class DragWindow : MonoBehaviour
    {
        private Vector3 delta;
        private Ray ray;
        private RaycastHit rayHit;
        private Vector3 MovePosition;
        private bool OnMouse = false;
        private bool DBClick = false;
        private TimeSpan mLastClick = TimeSpan.Zero;

        public Camera cCamera;

		void Awake()
		{
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray = cCamera.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out rayHit))
				{
					if (rayHit.collider.name == this.name)
					{
                        TimeSpan time = DateTime.Now.TimeOfDay;
                        if (time.TotalMilliseconds - mLastClick.TotalMilliseconds < 200)
                        {
                            //Win32Api.GetInstance().SwitchMaxWindow();
                        }
                        else
                        {
                            delta = Input.mousePosition;
                            OnMouse = true; 
                        }

                        mLastClick = time;
					}
				}
            }

            if (Input.GetMouseButtonUp(0))
            { 
				OnMouse = false; 
			}

            if (OnMouse && !Win32Api.GetInstance().IsMaxWindow())
            {
				Win32Api.GetInstance().MoveWindow((int)delta.x, (int)delta.y);
            }
        }
    }
}