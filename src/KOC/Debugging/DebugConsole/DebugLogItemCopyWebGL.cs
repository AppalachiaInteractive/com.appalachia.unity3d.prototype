﻿#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IngameDebugConsole
{
	public sealed class DebugLogItemCopyWebGL : AppalachiaBehaviour<DebugLogItemCopyWebGL>, IPointerDownHandler, IPointerUpHandler
	{
		[DllImport( "__Internal" )]
		private static extern void IngameDebugConsoleStartCopy( string textToCopy );
		[DllImport( "__Internal" )]
		private static extern void IngameDebugConsoleCancelCopy();

		private DebugLogItem logItem;

		public void Initialize( DebugLogItem logItem )
		{
			this.logItem = logItem;
		}

		public void OnPointerDown( PointerEventData eventData )
		{
			string log = logItem.Entry.ToString();
			if( !string.IsNullOrEmpty( log ) )
				IngameDebugConsoleStartCopy( log );
		}

		public void OnPointerUp( PointerEventData eventData )
		{
			if( eventData.dragging )
				IngameDebugConsoleCancelCopy();
		}
	}
}
#endif