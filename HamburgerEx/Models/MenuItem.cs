using static HamburgerEx.MenuItemData;
using static HamburgerEx.Nutrient;

namespace HamburgerEx
{
    class MenuItem
    {
        public string NameKR { get; set; }
        public string NameEN { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Nutrient Nutrient { get; set; }
        public MainTitle MainTitle { get; set; }
        public SubTitle SubTitle { get; set; }
        public MenuStyle MenuStyle { get; set; }
    }
}
