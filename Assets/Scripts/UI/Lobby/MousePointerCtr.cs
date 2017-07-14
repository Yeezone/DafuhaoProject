using UnityEngine;
using System.Collections;

public class MousePointerCtr : MonoBehaviour {

    public static MousePointerCtr instance;

	public bool MouseEnable = true;

    //鼠标显示的状态图片
    public Texture2D[] cursor_pick;
    //显示大小
    private Vector2 hotspot = Vector2.one;
    //显示模式
    private CursorMode mode = CursorMode.Auto;

    private Texture texture;
    public int kuan;
    public int gao;

	void Start () {
		if(MouseEnable)
		{
			instance = this;
			texture = cursor_pick[1];
			Cursor.visible = false;
		}
	}


    void Update()
    {
       
    }

	void OnDestroy()
	{
		instance = null;
	}

    /// <summary>
    /// 退出捕鱼,启用此方法,恢复鼠标样式
    /// </summary>
    void OnDisable()
    {
        if (this.enabled != true)
        {
            texture = null;
            Cursor.visible = true;
        }
    }

    public void SetNpcTalk()
    {
    }

    public void SetMouse_over()
    {
        texture = cursor_pick[0];
        kuan = 76;
        gao = 86;
        Cursor.visible = false;
    }

    public void SetMouse_out()
    {
        texture = cursor_pick[1];
        kuan = 64;
        gao = 64;
        Cursor.visible = false;
    }

    public void SetMouse_exit()
    {
        texture = cursor_pick[1];
        kuan = 64;
        gao = 64;
        Cursor.visible = false;
    }
	public void Lobby_SetMouse_exit()
	{
		texture = null;
		Cursor.visible = true;
	}

    void OnGUI()
    {
		if(!Cursor.visible)
		{
			//获取鼠标位置
			Vector3 mousepos = Input.mousePosition;
			
			//绘制光标图片
			GUI.DrawTexture(new Rect(mousepos.x - kuan / 2, Screen.height - mousepos.y - gao / 2, kuan, gao), texture);
		}
       
    }

}
