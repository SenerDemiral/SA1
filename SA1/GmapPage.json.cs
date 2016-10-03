using Starcounter;

namespace SA1 {
    [GmapPage_json]
    partial class GmapPage : Page
    {
        public void RefreshData()
        {
            Sener = Db.SQL<Gmap>("SELECT i FROM Gmap i");
        }

    }

    [GmapPage_json.Sener]
    partial class Gmapelement : Json, IBound<Gmap>
    {
        protected override void OnData()
        {
            base.OnData();
        }
    }
}
