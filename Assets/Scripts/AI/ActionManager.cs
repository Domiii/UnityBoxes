//using UnityEngine;
//using System.Collections;
//
//public class ActionManager : MonoBehaviour {
//	#region Action Management
//	public Actions.Action LastAction {
//		get;
//		private set;
//	}
//
//	public void Dispatch<A>(A action) 
//		where A : Actions.Action
//	{
//		var lastActionName = LastAction != null ? LastAction.Name : null;
//		action.PreviousActionName = lastActionName;
//
//		// always first cancel current action
//		//DoCancelCurrentAction ();
//
//		if (action.HasActionChanged) {
//			SendMessage ("Stop" + lastActionName, LastAction, SendMessageOptions.DontRequireReceiver);
//			SendMessage ("Start" + action.Name, action, SendMessageOptions.DontRequireReceiver);
//		}
//
//		SendMessage (action.Name, action, SendMessageOptions.DontRequireReceiver);
//
//		LastAction = action;
//	}
//
//	public void StopCurrentAction() {
//		Dispatch (Actions.Idle.Default);
//	}
//	#endregion
//}