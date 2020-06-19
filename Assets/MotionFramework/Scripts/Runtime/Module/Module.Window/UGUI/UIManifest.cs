﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFramework.Window
{
	[DisallowMultipleComponent]
	public class UIManifest : MonoBehaviour
	{
		public List<string> CacheAtlasTags = new List<string>();
		public List<string> ElementPath = new List<string>();
		public List<Transform> ElementTrans = new List<Transform>();
		
		private readonly Dictionary<string, Transform> _runtimeDic = new Dictionary<string, Transform>(200);


		private void Awake()
		{
			if (Application.isPlaying)
				ConvertListToDic();
		}

		/// <summary>
		/// 游戏运行时把List内容存在字典里方便查询
		/// </summary>
		private void ConvertListToDic()
		{
			_runtimeDic.Clear();

			if (ElementPath.Count == 0)
				throw new Exception($"Fatal error : {this.gameObject.name} elementPath list is empty.");
			if (ElementTrans.Count == 0)
				throw new Exception($"Fatal error : {this.gameObject.name} elementTrans list is empty.");
			if (ElementPath.Count != ElementTrans.Count)
				throw new Exception($"Fatal error : {this.gameObject.name} elementTrans list and elementPath list must has same count.");

			for (int i = 0; i < ElementPath.Count; i++)
			{
				string path = ElementPath[i];
				Transform trans = ElementTrans[i];
				_runtimeDic.Add(path, trans);
			}
		}

		/// <summary>
		/// 根据全路径获取UI元素
		/// </summary>
		public Transform GetUIElement(string path)
		{
			if (string.IsNullOrEmpty(path))
				return null;

			if (_runtimeDic.TryGetValue(path, out Transform result) == false)
			{
				MotionLog.Warning($"Not found ui element : {path}");
			}
			return result;
		}

		/// <summary>
		/// 根据全路径获取UI组件
		/// </summary>
		public Component GetUIComponent(string path, string typeName)
		{
			Transform element = GetUIElement(path);
			if (element == null)
				return null;

			Component component = element.GetComponent(typeName);
			if (component == null)
				MotionLog.Warning($"Not found ui component : {path}, {typeName}");
			return component;
		}

		/// <summary>
		/// 根据全路径获取UI组件
		/// </summary>
		public T GetUIComponent<T>(string path) where T : UnityEngine.Component
		{
			Transform element = GetUIElement(path);
			if (element == null)
				return null;

			Component component = element.GetComponent<T>();
			if (component == null)
				MotionLog.Warning($"Not found ui component : {path}, {typeof(T)}");
			return component as T;
		}
	}
}