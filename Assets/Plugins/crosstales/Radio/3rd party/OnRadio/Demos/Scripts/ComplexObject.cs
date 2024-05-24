using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.Radio.OnRadio.Demo
{
	/// <summary>A complex object for all parameters of a gui-prefab used in GUIOnRadio.</summary>
	public class ComplexObject
	{
		public BaseGUIStatic Script;
		public Transform ObjectTransform;
		public RectTransform ObjectRectTransform;
		public Image ObjectImage;

		public ComplexObject(BaseGUIStatic script, Transform objectTransform, RectTransform objectRectTransform, Image objectImage)
		{
			Script = script;
			ObjectTransform = objectTransform;
			ObjectRectTransform = objectRectTransform;
			ObjectImage = objectImage;
		}
	}
}
// © 2020-2021 crosstales LLC (https://www.crosstales.com)