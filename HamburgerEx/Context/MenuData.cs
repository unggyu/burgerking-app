using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamburgerEx
{
    class MenuData
    {
        /// <summary>
        /// 꺼내 먹어요
        /// </summary>
        public List<MyListView> MyListViews { get; set; }

        /// <summary>
        /// Rule : 처음건 SubMenu 이미지 이름 그 다음부터는 SubTitle 이미지 이름들
        /// </summary>
        public List<string> Titles { get; set; }
    }
}
