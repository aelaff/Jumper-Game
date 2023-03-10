using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Router : MonoBehaviour {
	//containing the sengelton one shared instance for the firebase database 
	private static DatabaseReference baseRef = FirebaseDatabase.DefaultInstance.RootReference;

	public static DatabaseReference Players() {
		return baseRef.Child ("players");
	}
	public static DatabaseReference Grounds()
	{
		return baseRef.Child("grounds");
	}
	public static DatabaseReference PlayerWithUID(string uid) {
		return baseRef.Child ("players").Child (uid);
	}
	public static DatabaseReference GroundWithUID(string gid)
	{
		return baseRef.Child("grounds").Child(gid);
	}
}
