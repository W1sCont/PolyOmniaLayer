namespace PolyOmniaLayer.Models.Page
{
    public class SortViewModel
    {
        public SortState CategorySort { get; private set; }     
        public SortState NameSort { get; private set; }
        public SortState Current { get; private set; }

        public SortViewModel(SortState sortOrder)
        {
            CategorySort = sortOrder == SortState.CategoryAsc ? SortState.CategoryDesc : SortState.CategoryAsc;
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            Current = sortOrder;
        }
    }
}
