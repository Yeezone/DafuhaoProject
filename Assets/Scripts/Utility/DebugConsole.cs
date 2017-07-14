using UnityEngine;
using com.QH.QPGame.Lobby;

namespace com.QH.QPGame.Utility
{
	/// <summary>
	/// 负责产生管理调试信息,提供更详细的查错信息
	/// @Author: guofeng
	/// </summary>
	public class DebugConsole : UnitySingleton<DebugConsole>
	{
		private bool enable = true;

	    public void Show()
	    {
            Debug.developerConsoleVisible = true;
	    }

	    private void Update ()
		{
			if (Input.GetKey (KeyCode.LeftControl)) {
				if (Debug.developerConsoleVisible && Input.GetKey (KeyCode.Delete)) {
					Debug.ClearDeveloperConsole ();
				}

				if (Input.GetKey (KeyCode.O)) {
					enable = !enable;
					Debug.developerConsoleVisible = enable;
				}
			}

			if (!enable) {
				Debug.ClearDeveloperConsole ();
			}
		}

		void OnGUI ()
		{
            if (GameApp.Options != null && GameApp.GameData != null)
		    {
                if (GUILayout.Button("(Press To GC)  " +
                   " Version:" + GameApp.GameData.Version +
                   " Server:" + GameApp.GameData.Host + ":" + GameApp.GameData.Port +
                   " HeapUesd:" + Profiler.usedHeapSize / 1024 + "kb" +
                   " MonoUsed:" + Profiler.GetMonoUsedSize() / 1024 + "kb")
               )
                {
                    Caching.CleanCache();
                    System.GC.Collect();
                    Resources.UnloadUnusedAssets();
                }
		    }
		}
	}
}
