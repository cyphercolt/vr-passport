﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//attach this script, once, to any gameobject and there will be a debug canvas with an FPS counter, updated every second
public class FPSInfo : MonoBehaviour
{
	public bool showOnScreen = true;
	public bool showInConsole = true;
	private GameObject canvas;
	private Text text;
	private float interval = 1;
	private float timer = 0;
	private int frameCount;

	void Start ()
	{
		if (showOnScreen) {
			GameObject prefab = Resources.Load ("test/Debug Canvas") as GameObject;
			canvas = Instantiate (prefab) as GameObject;
			text = canvas.transform.GetChild (0).gameObject.GetComponent<Text> ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		frameCount++;
		timer += Time.deltaTime;
		if (timer > interval) {
			float fps = Mathf.Round ((float)frameCount / interval);
			timer = 0;
			frameCount = 0;

			if (showOnScreen)
				text.text = fps + " FPS";
			if (showInConsole)
				Debug.Log (fps + " FPS");
		}
	}
}
