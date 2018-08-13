using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteNodeManager : MonoBehaviour {
	static List<RouteNodeManager> routeNodeList = new List<RouteNodeManager>();
	public static List<RouteNodeManager> RouteNodeList {
		get {
			return routeNodeList;
		}
	}

	void Awake () {
		routeNodeList.Add(this);
	}
}
