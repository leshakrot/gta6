using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.Radio.Demo
{
	/// <summary>A complex object for all parameters of a gui-prefab used in GUIRadioplayer.</summary>
	public class ComplexObject
	{
		public GUIRadioStatic Script;
		public Transform ObjectTransform;
		public RectTransform ObjectRectTransform;
		public Image ObjectImage;

		public ComplexObject(GUIRadioStatic script, Transform objectTransform, RectTransform objectRectTransform, Image objectImage)
		{
			Script = script;
			ObjectTransform = objectTransform;
			ObjectRectTransform = objectRectTransform;
			ObjectImage = objectImage;
		}
	}
}
// © 2020-2021 crosstales LLC (https://www.crosstales.com)