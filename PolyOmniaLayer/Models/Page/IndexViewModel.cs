using PolyOmnia.BLL.DTO;

namespace PolyOmniaLayer.Models.Page
{
    public class IndexViewModel(
        HomeViewModel homeViewModel,
        PageViewModel pageViewModel,
        FilterViewModel filterViewModel,
        SortViewModel sortViewModel)
    {
        public HomeViewModel home { get; } = homeViewModel;
        public PageViewModel PageViewModel { get; } = pageViewModel;
        public FilterViewModel FilterViewModel { get; } = filterViewModel;
        public SortViewModel SortViewModel { get; } = sortViewModel;
    }
}
