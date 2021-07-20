using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static HamburgerEx.MenuItemData;

namespace HamburgerEx
{
    class MyListView : ListView
    {
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public SubTitle SubTitle { get; set; }

        public MyListView()
        {
            BackColor = Color.FromArgb(146, 21, 15);
            BorderStyle = BorderStyle.None;
            MultiSelect = false;
            Size = new Size(553, 320);
            UseCompatibleStateImageBehavior = false;
            Scrollable = true;
        }

        /// <summary>
        /// 자동 높이 지정
        /// </summary>
        /// <param name="divide">나누는 수</param>
        public void AutoHeight(int divide)
        {
            int count = Items.Count;
            if (count > 0)
            {
                if (divide > 0)
                {
                    int rowCount = count / divide + ((count % divide > 0) ? 1 : 0);
                    Size = new Size(Width, (GetItemRect(0).Height + 2) * rowCount);
                }
                else
                {
                    Size = new Size(Width, GetItemRect(0).Height);
                }
            }
        }

        /// <summary>
        /// 리스트뷰 아이템 사이의 간격 조정
        /// </summary>
        /// <param name="leftPadding">왼쪽 간격</param>
        /// <param name="topPadding">위쪽 간격</param>
        public void SetIconSpacing(short leftPadding, short topPadding)
        {
            const uint LVM_FIRST = 0x1000;
            const uint LVM_SETICONSPACING = LVM_FIRST + 53;
            SendMessage(Handle, LVM_SETICONSPACING, IntPtr.Zero, (IntPtr)MakeLong(leftPadding, topPadding));
        }

        /// <summary>
        /// long을 만듬
        /// </summary>
        /// <param name="lowPart"></param>
        /// <param name="highPart"></param>
        /// <returns></returns>
        private int MakeLong(short lowPart, short highPart)
        {
            return (int)(((ushort)lowPart) | (uint)(highPart << 16));
        }
    }
}
