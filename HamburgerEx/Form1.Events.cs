using System;
using System.Threading;
using System.Windows.Forms;
using static HamburgerEx.MenuItemData;

namespace HamburgerEx
{
    public partial class BurgerKing
    {
        /// <summary>
        /// 폼 로드됐을 때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            InitFlowLayoutPanelLeft();
            StackOnRightFlowLayoutPanel(NowMainTitle);
            InitMenu();
            InitBurgerDataPanel();
        }

        /// <summary>
        /// 폼이 닫힐 때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BurgerKing_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MakingThread != null) { MakingThread.Abort(); }
        }

        /// <summary>
        /// 리스트뷰에서 선택된 인덱스가 바뀌었을 때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView.SelectedItems.Count > 0)
            {
                int key = listView.SelectedItems[0].ImageIndex;
                MenuItem menuItem = MenuItemData.listMenuItem[key];

                /// 버튼 이미지 처리부분
                if (NowSubTitle != null && NowSubTitle != menuItem.SubTitle)
                {
                    ChangeButtonPicture(menuItem.MainTitle, NowSubTitle);
                }
                if (NowSubTitle != menuItem.SubTitle)
                {
                    ChangeButtonPicture(menuItem.MainTitle, menuItem.SubTitle);
                }
                ///

                listView.SelectedItems.Clear();
                ShowMenuItemData(menuItem);
            }
        }

        private void MyButton_MouseDown(object sender, MouseEventArgs e)
        {
            MyButton button = sender as MyButton;
            if (button == null) return;

            if (!IsShowingMenuItemDataPanel)
            {
                if (button.TitleStyle == TitleStyle.MainTitle && button.MainTitle == NowMainTitle
                    || button.TitleStyle == TitleStyle.SubTitle && button.SubTitle == NowSubTitle)
                {
                    return;
                }
            }

            MainTitle? pastMainTitle = NowMainTitle;
            SubTitle? pastSubTitle = NowSubTitle;

            if (button.TitleStyle == TitleStyle.MainTitle)
            {
                /// 버튼 이미지 처리부분
                if (pastMainTitle != button.MainTitle)
                {
                    ChangeButtonPicture(pastMainTitle, null);
                }
                if (pastSubTitle != null)
                {
                    ChangeButtonPicture(pastMainTitle, pastSubTitle);
                }
                ///

                StackOnRightFlowLayoutPanel(button.MainTitle);
                flowLayoutPanelRight.BringToFront(out IsShowingMenuItemDataPanel, out NowSubTitle);
            }
            else
            {
                /// 버튼 이미지 처리부분
                // 과거 MainTitle과 현재 MainTitle이 다를 때 || 과거 SubTitle과 현재 SubTitle이 다를 때
                if (pastMainTitle != button.MainTitle || pastSubTitle != button.SubTitle)
                {
                    if (pastMainTitle != button.MainTitle)
                    {
                        ChangeButtonPicture(button.MainTitle, null);
                        ChangeButtonPicture(pastMainTitle, null);
                    }
                    if (pastSubTitle != null)
                    {
                        ChangeButtonPicture(pastMainTitle, pastSubTitle);
                    }
                }
                ///

                int key = GetFirstItemIndex(button.MainTitle, button.SubTitle);
                if (key != -1)
                {
                    ShowMenuItemData(MenuItemData.listMenuItem[key]);
                }
            }
            
            NowMainTitle = button.MainTitle;
        }

        private void MyButton_MouseEnter(object sender, EventArgs e)
        {
            MyButton button = sender as MyButton;
            if (button == null) return;

            if (button.Name.Contains("Default"))
            {
                ChangeButtonPicture(button);
            }
        }

        private void MyButton_MouseLeave(object sender, EventArgs e)
        {
            MyButton button = sender as MyButton;
            if (button == null) return;

            if (button.TitleStyle == TitleStyle.MainTitle && button.MainTitle != NowMainTitle
                || button.TitleStyle == TitleStyle.SubTitle && button.SubTitle != NowSubTitle
                || button.TitleStyle == TitleStyle.SubTitle && button.MainTitle != NowMainTitle && button.SubTitle == NowSubTitle)
            {
                ChangeButtonPicture(button);
            }
        }

        private void FlowLayoutPanelRight_MouseEnter(object sender, EventArgs e)
        {
            (sender as FlowLayoutPanel).Focus();
        }

        /// <summary>
        /// 만드는 스레드
        /// </summary>
        Thread MakingThread;

        /// <summary>
        /// 만들기 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeButton_Click(object sender, EventArgs e)
        {
            if (IsRunningMakingThread == true)
            {
                return;
            }

            Button button = sender as Button;
            button.Enabled = false;

            MenuItemDataPanel.Controls.Add(MakingProgressBar);
            MenuItem menuItem = GetMenuItemByName(NameKRLabel.Text);

            // 메이킹 스레드 시작
            MakingThread = new Thread(new ParameterizedThreadStart(MakingThreadMethod));
            MakingThread.Start(new object[] { menuItem, button });
        }
    }
}
