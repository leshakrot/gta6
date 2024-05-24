namespace Crosstales.Radio.OnRadio.Model
{
   /// <summary>Extended RadioStation.</summary>
   public class RadioStationExt : Crosstales.Radio.Model.RadioStation
   {
      public string StationId;

      public RadioStationExt(string name, string stationId)
      {
         Name = string.IsNullOrEmpty(name) ? UNKNOWN_STATION : name;
         StationId = stationId.Trim();
      }

      public override bool Equals(object obj)
      {
         if (obj == null || GetType() != obj.GetType())
            return false;

         RadioStationExt rs = (RadioStationExt)obj;

         return base.Equals(obj) && StationId == rs.StationId;
      }

      public override int GetHashCode()
      {
         return base.GetHashCode() + (StationId == null ? 0 : StationId.GetHashCode());
      }
   }
}