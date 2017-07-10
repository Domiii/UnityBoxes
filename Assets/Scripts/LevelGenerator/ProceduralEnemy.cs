using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralEnemy : MonoBehaviour {

	[Range(1, 100)]
	public float strength = 10;

	public bool IsGenerated {
		get {
			return GetComponent<Unit>() != null;
		}
	}

	void Start () {
		if (!IsGenerated) {
			Generate ();
		}
	}

	public void Generate() {
		if (IsGenerated) {
			Clear ();
		}

		GenShape ();
		GenUnit ();
		GenMaterial ();
	}

	/// <summary>
	/// Make sure, gameObject rests on the y = 0 plane
	/// </summary>
	public void FixYPosition() {
		var bounds = GetComponent<Collider> ().bounds;
		transform.position -= Vector3.up * bounds.min.y;
	}

	#region Shape
	static readonly PrimitiveType[] AllShapes = new []{
		PrimitiveType.Capsule,
		PrimitiveType.Cube,
		PrimitiveType.Cylinder,
		PrimitiveType.Sphere
	};
	static Vector3 halfSize = new Vector3 (1,0.5f,1);

	void GenShape() {
		var shapeType = AllShapes[Random.Range(0, AllShapes.Length)];
		var size = Mathf.Sqrt(strength);

		PrimitiveHelper.DecoratePrimitive (gameObject, shapeType);

		// set size
		transform.localScale = Vector3.one * size;

		var bounds = GetComponent<Collider> ().bounds;

		// some primitives have twice the height of others... fix that!
		if (bounds.extents.y/size > 0.51f) {
			transform.localScale = Vector3.Scale(transform.localScale, halfSize);
		}
	}
	#endregion

	#region Material
	// source: http://answers.unity3d.com/questions/914923/standard-shader-emission-control-via-script.html
	private const string EmissiveValue = "_EmissionScaleUI";
	private const string EmissiveColour = "_EmissionColor";
	private const string EmissiveColour2 = "_EmissionColorUI";
	void GenMaterial() {
		var mat = new Material(Shader.Find("Standard"));

		// setup color
		var col = Color.HSVToRGB (Random.value, 0.5f, 0.5f);
		mat.color = col;

		// setup emission
		mat.SetFloat(EmissiveValue, 0.5f);
		var emisColor = Color.HSVToRGB (Random.value, 0.5f, 0.5f);
		mat.SetColor(EmissiveColour, emisColor);
		mat.SetColor(EmissiveColour2, emisColor);
		mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
		mat.EnableKeyword("_EMISSION");

		// done!
		GetComponent<MeshRenderer> ().material = mat;
	}
	#endregion

	void GenUnit() {
		var unit = gameObject.AddComponent <Unit>();
	}

	public void Clear() {
		var comps = gameObject.GetComponents<Component> ();
		for (var i = comps.Length-1; i >= 0; --i)
		{
			var comp = comps [i];
			if (!(comp is Transform) && comp != this)
			{
				DestroyImmediate(comp);
			}
		}
	}
}
