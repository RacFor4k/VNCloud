
namespace GeneralModules
{
    public class RoutesModel
    {
        public int Id { get; set; }

        public string Route { get; set; }

        public string ParentID { get; set; }

        public bool IsFolder { get; set; }
    }
}
