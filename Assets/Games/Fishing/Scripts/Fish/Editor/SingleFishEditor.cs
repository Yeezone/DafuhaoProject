using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

namespace com.QH.QPGame.Fishing
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(SingleFish))]
	public class SingleFishEditor : Editor {
		
		SingleFish _target;
		
		void OnEnable () 
		{
			_target = target as SingleFish;
		}
		
		public override void OnInspectorGUI () 
		{
			GUILayout.Space(10f);

			_target.localScale 			= EditorGUILayout.Vector3Field("local Scale", _target.localScale);
			_target.multi	 			= EditorGUILayout.IntField("multi", _target.multi);
			_target.shakeCam	 		= EditorGUILayout.Toggle("shake Cam", _target.shakeCam);
			_target.hurtColor 			= EditorGUILayout.ColorField("Hurt Color", _target.hurtColor);
			_target.hurtTimeLength		= EditorGUILayout.FloatField("Hurt Time Length", _target.hurtTimeLength);
			_target.coinType	 		= EditorGUILayout.IntField("coin Type", _target.coinType);
			_target.coinNum		 		= EditorGUILayout.IntField("coin Num", _target.coinNum);
			_target.deadAudio 			= (AudioClip)EditorGUILayout.ObjectField("dead Audio", _target.deadAudio, typeof(AudioClip), false);
			_target.deadProbability	 	= EditorGUILayout.IntSlider("dead Probability", _target.deadProbability, 0, 100);
			_target.deadOrderInLayer	= EditorGUILayout.IntField("dead Order In Layer", _target.deadOrderInLayer);
            _target.fishAnimFPS         = EditorGUILayout.IntField("fishAnimFPS", _target.fishAnimFPS);

			switch(_target.fishType)
			{
			case BaseFish.FishType.normal:
				_target.deadTimeLength		= EditorGUILayout.FloatField("dead Time Length", _target.deadTimeLength);
				break;
			case BaseFish.FishType.quanping:
				_target.deadTimeLength		= EditorGUILayout.FloatField("dead Time Length", _target.deadTimeLength);
				break;
			case BaseFish.FishType.likui:
				_target.deadTimeLength		= EditorGUILayout.FloatField("dead Time Length", _target.deadTimeLength);
				break;
			}

			_target.lockFishOrder		= EditorGUILayout.IntField("lock Fish Order", _target.lockFishOrder);
			_target.showHighScoreWhenFishDead	 		= EditorGUILayout.Toggle("show HighScore When Fish Dead", _target.showHighScoreWhenFishDead);
			_target.highScorePrefab		= (Transform)EditorGUILayout.ObjectField("high Score Prefab", _target.highScorePrefab, typeof(Transform), false);

			_target.canNotPlayUntilRecycle	= EditorGUILayout.Toggle("can Not Play Until Recycle", _target.canNotPlayUntilRecycle);
			
			GUILayout.Space(10f);
			NGUIEditorTools.BeginContents();
			_target.fishType = (BaseFish.FishType)EditorGUILayout.EnumPopup("fish Type", _target.fishType);
			switch(_target.fishType)
			{
			case BaseFish.FishType.normal:
			{
				_target.beAbsorbed	 		= EditorGUILayout.Toggle("be Absorbed", _target.beAbsorbed);
				break;
			}
			case BaseFish.FishType.ywdj:
			{
				_target.beAbsorbed	 		= EditorGUILayout.Toggle("be Absorbed", _target.beAbsorbed);
				_target.ywdjKillFishPool	= EditorGUILayout.IntField("ywdj Kill Fish Pool", _target.ywdjKillFishPool);
				_target.ywdjLine			= (Transform)EditorGUILayout.ObjectField("ywdj Line", _target.ywdjLine, typeof(Transform), false);
				break;
			}
			case BaseFish.FishType.quanping:
			{
				_target.move2CenterTimeLength	= EditorGUILayout.FloatField("move 2 Center Time Length", _target.move2CenterTimeLength);
				_target.killNorml	 			= EditorGUILayout.Toggle("kill Norml", _target.killNorml);
				_target.killYWDJ	 			= EditorGUILayout.Toggle("kill ywdj", _target.killYWDJ);
				_target.killQuanPing	 		= EditorGUILayout.Toggle("kill Quan Ping", _target.killQuanPing);
				_target.killBlackHall	 		= EditorGUILayout.Toggle("kill Black Hall", _target.killBlackHall);
				_target.killLikui				= EditorGUILayout.Toggle("kill Li Kui", _target.killLikui);
				break;
			}
			case BaseFish.FishType.blackHall:
			{
				_target.blackHallLine			= (Transform)EditorGUILayout.ObjectField("black Hall Line", _target.blackHallLine, typeof(Transform), false);
				_target.move2CenterTimeLength	= EditorGUILayout.FloatField("move 2 Center Time Length", _target.move2CenterTimeLength);
				_target.blackHallAbsorbMulti	= EditorGUILayout.IntField("black Hall Absorb Multi", _target.blackHallAbsorbMulti);

				_target.blackHallNumPrefab		= (Transform)EditorGUILayout.ObjectField("black Hall Num Prefab", _target.blackHallNumPrefab, typeof(Transform), false);
				_target.blackHallHeight	= EditorGUILayout.FloatField("black Hall Height", _target.blackHallHeight);

				_target.blackHallScale	= EditorGUILayout.Vector3Field("black Hall Scale", _target.blackHallScale);
				break;
			}
			case BaseFish.FishType.likui:
			{
				_target.LiKuiAddBeilv = EditorGUILayout.IntField("Once add beilv", _target.LiKuiAddBeilv);
				_target.LiKuibeilvHigh = EditorGUILayout.IntField("the max beilv", _target.LiKuibeilvHigh);
				_target.LiKuiBeilvTime = EditorGUILayout.FloatField("add beilv time", _target.LiKuiBeilvTime);
				_target.LikuiNumPrefab = (Transform)EditorGUILayout.ObjectField("likui Num Prefab", _target.LikuiNumPrefab, typeof(Transform), false);

				_target.LikuiHeight = EditorGUILayout.FloatField( "Likui Height",_target.LikuiHeight);
				_target.LikuiNumScale	= EditorGUILayout.Vector3Field("Likui Scale", _target.LikuiNumScale);
				break;
			}

			}
			NGUIEditorTools.EndContents();
			GUILayout.Space(5f);


			// _target.deadEffList	= (List<EffParam>)EditorGUILayout.ObjectField("deadEffList", _target.deadEffList, typeof(List<EffParam>), false);
			// _target.test	= (EffParam)EditorGUILayout.ObjectField("EffParam", _target.test, typeof(EffParam), false);


			// if (NGUIEditorTools.DrawHeader("deadEffList", "deadEffList", false, true))
			if(NGUIEditorTools.DrawHeader("Effect"))
			{
				NGUIEditorTools.BeginContents();
				{
					GUILayout.BeginVertical();
				
					// EditorGUILayout.PropertyField(serializedObject.FindProperty("mouseDragThreshold"), new GUIContent("Mouse Drag"), GUILayout.Width(120f));
					serializedObject.Update();
					// EditorGUIUtility.LookLikeInspector();
					EditorGUIUtility.LookLikeControls();
					ListIterator3("deadEffList");
					serializedObject.ApplyModifiedProperties();
			
					GUILayout.EndVertical();
				}
				NGUIEditorTools.EndContents();
			}
		}

		/*
		private bool visible = true;
		public void ListIterator(string _listName, ref bool visible)
		{
			SerializedProperty listIterator = serializedObject.FindProperty(_listName);
			visible = EditorGUILayout.Foldout(visible, listIterator.name);
			if(visible)
			{
				EditorGUI.indentLevel ++;
				for(int i=0; i<listIterator.arraySize; i++)
				{
					SerializedProperty elementPorperty = listIterator.GetArrayElementAtIndex(i);
					Rect drawZone = GUILayoutUtility.GetRect(0f, 16f);
					bool showChildren = EditorGUI.PropertyField(drawZone, elementPorperty);
				}
				EditorGUI.indentLevel--;
			}
		}


		public void ListIterator2(string _listName)
		{
			SerializedProperty listIterator = serializedObject.FindProperty(_listName);
			Rect drawZone = GUILayoutUtility.GetRect(0f,16f);
			bool showChildren = EditorGUI.PropertyField(drawZone, listIterator);
			listIterator.NextVisible(showChildren);

			drawZone = GUILayoutUtility.GetRect(0f, 16f);
			showChildren = EditorGUI.PropertyField(drawZone, listIterator);
			bool tobecontinued = listIterator.NextVisible(showChildren);

			int listElement = 0;
			while(tobecontinued)
			{
				drawZone = GUILayoutUtility.GetRect(0f, 16f);
				showChildren = EditorGUI.PropertyField(drawZone, listIterator);
				tobecontinued = listIterator.NextVisible(showChildren);
				listElement ++ ;
			}
		}
		*/

		public void ListIterator3(string _listName)
		{
			SerializedProperty listIterator = serializedObject.FindProperty(_listName);

			while(true)
			{
				Rect drawZone = GUILayoutUtility.GetRect(0f, 16);
			 	if(listIterator.isArray)
				{
					drawZone = new Rect(drawZone.x+10f, drawZone.y, drawZone.width, drawZone.height);
				}
				else 
				{
					drawZone = new Rect(drawZone.x+20f, drawZone.y, drawZone.width, drawZone.height);
				}

	//			if(listIterator.name.Equals("deadEffList"))
	//			{
	//
	//			}

			// 	if(listIterator.hasVisibleChildren)

				bool showChildren = EditorGUI.PropertyField(drawZone, listIterator);
				if(!listIterator.NextVisible(showChildren))
				{
					break;
				}
			}
		}
	}
}
