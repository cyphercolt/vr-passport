﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Coral : MonoBehaviour
{
	public Vector3 minScale, maxScale;
	public Color[] possibleColors;
	public int rotatedCopyCount = 0;
	public bool blendColors=true;
	public bool guaranteeOneMaxColor = true;
	private static bool hasGivenColliderWarning=false;
	private static bool hasGivenColorWarning=false;

	void Start ()
	{
		if (IsMaster()) {
			SetScale();
			SetColor (GetRandomColor ());
			CreateRotatedCopies ();

			if (!hasGivenColliderWarning && GetComponent<Collider> () != null) {
				hasGivenColliderWarning = true;
				Debug.LogWarning ("Corals should not have a collider. This wrecks performance, and possibly coral generation code. " + gameObject.name);
			}
		}
	}

	private bool IsMaster(){
		return transform.parent==null || transform.parent.gameObject.GetComponent<Coral> () == null;
	}

	private void SetScale(){
		if (minScale.sqrMagnitude == 0)
			minScale = new Vector3 (1, 1, 1);
		if (maxScale.sqrMagnitude == 0)
			maxScale = new Vector3 (1, 1, 1);

		transform.localScale = Vector3.Lerp (minScale, maxScale, Random.Range (0f, 1f));
	}

	private void CreateRotatedCopies(){
		float degreesPerStep = 180 / (rotatedCopyCount + 1);
		for(int i=0;i<rotatedCopyCount;i++){
			GameObject copy = Instantiate(gameObject,transform.position,transform.rotation) as GameObject;
			Coral coralOfCopy=copy.GetComponent<Coral>();

			float degrees=degreesPerStep*(i+1);

			copy.name+=" at "+degrees+" degrees";
			copy.transform.parent=gameObject.transform;
			copy.transform.RotateAround(copy.transform.position,copy.transform.up,degreesPerStep);
		}
	}

	private Color GetRandomColor ()
	{
		for (int i=0; i<possibleColors.Length; i++)
			possibleColors [i].a = 1f;

		if (possibleColors.Length == 0) {
			if (hasGivenColorWarning){
			Debug.LogWarning ("Coral "+gameObject+" has no possible colors... using bright defaults.");
				hasGivenColorWarning=true;
			}
			SetDefaultPossibleColors();
		}
		if (possibleColors.Length == 1 || !blendColors)
			return possibleColors [Random.Range (0, possibleColors.Length)];

		int a = Random.Range (0, possibleColors.Length);
		int b = a;
		while (b==a)
			b = Random.Range (0, possibleColors.Length);

		Color newColor=Color.Lerp (possibleColors [a], possibleColors [b], Random.Range (0f, 1f));

		if (guaranteeOneMaxColor && Mathf.Max (newColor.r,newColor.g,newColor.b)<1f) {
			if(newColor.r>0)
				newColor.r=1f;
			else if(newColor.g>0)
				newColor.g=1f;
			else
				newColor.b=1f;
		}

		return newColor;
	}

	private void SetDefaultPossibleColors(){
		possibleColors = new Color[]{new Color (1f, 0.2f, 0),
			new Color (0, 1f, 0.2f),
			new Color (0, 0.2f, 1f),
			new Color (1f, 0, 1f),
			new Color (0, 1f, 1f),
			new Color (1f, 1f, 0),
			new Color (1f, 1f, 1f)};
	}

	private void SetColor (Color newColor)
	{
		foreach (GameObject child in ParentChildFunctions.GetAllChildren(gameObject,true)) {
			MeshRenderer mr = child.GetComponent<MeshRenderer> ();
			if (mr != null) {
				foreach (Material m in mr.materials)
					m.color = newColor;
			}
		}
	}
}
