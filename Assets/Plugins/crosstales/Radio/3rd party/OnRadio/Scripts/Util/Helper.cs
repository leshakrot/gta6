namespace Crosstales.Radio.OnRadio.Util
{
   /// <summary>Helper-class for OnRadio.</summary>
   public static class Helper
   {
      private static readonly string[] genres =
      {
         "",
         "70's",
         "80's",
         "90's",
         "00's",
         "Adult Contemporary",
         "Alternative",
         "Christian",
         "Christmas",
         "ClassicCountry",
         "Classical",
         "Country",
         "Electronic",
         "Electronic/Chill",
         "Dubstep",
         "House",
         "Industrial",
         "Techno",
         "Trance",
         "Hip Hop",
         "Hit Music",
         "Indian",
         "Jazz",
         "Latin Hits",
         "Metal",
         "Oldies",
         "Rap",
         "Reggae",
         "Rock",
         "Roots", //Not enough songs for good results in this genre
         "Soul", //This is R&B
         "Standards",
         "World",
         "Music"
      };

      public static string getGenre(Model.Genre genre)
      {
         return genres[(int)genre];
      }
   }
}
// © 2019-2021 crosstales LLC (https://www.crosstales.com)