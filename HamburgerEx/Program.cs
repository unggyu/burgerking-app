using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static HamburgerEx.MenuItemData;

namespace HamburgerEx
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BurgerKing());
        }

        #region ExtensionMethods

        public static Tuple<int, int> CoordinatesOf<T>(this T[,] matrix, T value, bool IsEquals)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x,y].Equals(value) && IsEquals)
                    {
                        return Tuple.Create(x, y);
                    }
                    else if ( matrix[x,y].ToString().Contains(value.ToString()) && !IsEquals)
                    {
                        return Tuple.Create(x, y);
                    }
                }
            }

            return Tuple.Create(-1, -1);
        }

        /// <summary>
        /// TableLayoutPanel의 Style자동 추가
        /// </summary>
        /// <param name="table"></param>
        public static void TableLayoutPanelAutoStyle(this TableLayoutPanel table)
        {
            for (int column = 0; column < table.ColumnCount; column++)
            {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / table.ColumnCount));
            }

            for (int row = 0; row < table.RowCount; row++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / table.RowCount));
            }
        }

        public static void ForEach<T>(this IEnumerable<T> data, Action<T> action)
        {
            foreach (T d in data)
            {
                action(d);
            }
        }

        public static IEnumerable<string[]> ToIEnumerable(this string[,] table)
        {
            return table

            // Convert to IEnumerable<string>
            .OfType<string>()

            // Create anonymous type where Index1 and Index2
            // reflect the indices of the 2-dim. array
            .Select((_string, _index) => new {
                Index1 = (_index / table.GetLength(1)),
                Index2 = (_index % table.GetLength(1)), // ← I added this only for completeness
                Value = _string
            })

            // Group by Index1, which generates IEnmurable<string> for all Index1 values
            .GroupBy(v => v.Index1)

            // Convert all Groups of anonymous type to String-Arrays
            .Select(group => group.Select(v => v.Value).ToArray());
        }

        public static void BringToFront(this FlowLayoutPanel flowLayoutPanel, out bool isShowingMenuItemDataPanel, out SubTitle? nowSubTitle)
        {
            flowLayoutPanel.BringToFront();
            isShowingMenuItemDataPanel = false;
            nowSubTitle = null;
        }

        #endregion
    }
}
