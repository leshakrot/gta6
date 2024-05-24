using UnityEngine;

namespace Crosstales.Radio.OnRadio.Demo
{
	/// <summary>Query for the Reco2 service.</summary>
	[HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_on_radio_1_1_demo_1_1_query_reco2.html")]

	public class QueryReco2 : MonoBehaviour
	{
		/// <summary>'Reco2Service' from the scene.</summary>
		[Tooltip("'Reco2Service' from the scene.")]
		public OnRadio.Service.Reco2Service Service;

		public void SetArtist(string artist)
		{
			Service.Artist = artist;
		}
	}
}
