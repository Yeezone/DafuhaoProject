using System.Collections;

namespace com.QH.QPGame.Lobby.Surfaces
{
    public class SurfaceMain : Surface
    {
        void Awake()
        {
            StartCoroutine(CheckRes());
        }

        private IEnumerator CheckRes()
        {
            GameApp.GetInstance();
            GameApp.ResMgr.Initialize();
            while (!GameApp.ResMgr.ResReady)
            {
                yield return null;
            }

            GameApp.GetInstance().Initialize();
        }

    }
}

