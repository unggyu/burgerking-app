using System.Windows.Forms;
using static HamburgerEx.MenuItemData;

namespace HamburgerEx
{
    class MyButton : PictureBox
    {
        public static string Folder = "buttons";
        public static string Extension = "png";

        public string ImageName
        {
            set
            {
                Name = value;
                ImageLocation = $@"{ImageRootPath}\{Folder}\{Name}.{Extension}";
                SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }
        public TitleStyle TitleStyle { get; set; }
        public MainTitle? MainTitle { get; set; }
        public SubTitle? SubTitle { get; set; }
    }
}
