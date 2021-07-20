using HamburgerEx.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static HamburgerEx.MenuItemData;

namespace HamburgerEx
{
    public partial class BurgerKing : Form
    {
        private const int LeftPadding = 64 + 69;
        private const int TopPadding = 100 + 50;

        /// <summary>
        /// 몽고 클라이언트
        /// </summary>
        private MongoClient MongoClient = new MongoClient(connectionString: "mongodb://localhost");

        /// <summary>
        /// 주문내역 테이블
        /// </summary>
        private IMongoCollection<Order> OrderHistory;

        /// <summary>
        /// Button에 상속된 커스텀 버튼들
        /// 순서대로 flowLayoutPanel에 쌓일 것들
        /// </summary>
        private List<MyButton> MyButtons = new List<MyButton>
        {
            new MyButton { ImageName = "bkSpecialDefault", TitleStyle = TitleStyle.MainTitle, MainTitle = MainTitle.BkSpecial, Enabled = false },
            new MyButton { ImageName = "soDefault", TitleStyle = TitleStyle.SubTitle, MainTitle = MainTitle.BkSpecial, SubTitle = SubTitle.SpecialOffer, Enabled = false },
            new MyButton { ImageName = "dkDefault", TitleStyle = TitleStyle.SubTitle, MainTitle = MainTitle.BkSpecial, SubTitle = SubTitle.DoubleKing, Enabled = false },
            new MyButton { ImageName = "skDefault", TitleStyle = TitleStyle.SubTitle, MainTitle = MainTitle.BkSpecial, SubTitle = SubTitle.SnackKing, Enabled = false },
            new MyButton { ImageName = "setMenuDefault", TitleStyle = TitleStyle.MainTitle, MainTitle = MainTitle.SetMenu },
            new MyButton { ImageName = "whopperDefault", TitleStyle = TitleStyle.SubTitle, MainTitle = MainTitle.SetMenu, SubTitle = SubTitle.Whopper },
            new MyButton { ImageName = "gnsDefault", TitleStyle = TitleStyle.SubTitle, MainTitle = MainTitle.SetMenu, SubTitle = SubTitle.Garlic },
            new MyButton { ImageName = "bncDefault", TitleStyle = TitleStyle.SubTitle, MainTitle = MainTitle.SetMenu, SubTitle = SubTitle.BeefChicken },
            new MyButton { ImageName = "burgerHover", TitleStyle = TitleStyle.MainTitle, MainTitle = MainTitle.Burger },
            new MyButton { ImageName = "whopperDefault", TitleStyle = TitleStyle.SubTitle, MainTitle = MainTitle.Burger, SubTitle = SubTitle.Whopper },
            new MyButton { ImageName = "gnsDefault", TitleStyle = TitleStyle.SubTitle, MainTitle = MainTitle.Burger, SubTitle = SubTitle.Garlic },
            new MyButton { ImageName = "bncDefault", TitleStyle = TitleStyle.SubTitle, MainTitle = MainTitle.Burger, SubTitle = SubTitle.BeefChicken },
            new MyButton { ImageName = "sideDefault", TitleStyle = TitleStyle.MainTitle, MainTitle = MainTitle.Side },
            new MyButton { ImageName = "drinkDefault", TitleStyle = TitleStyle.MainTitle, MainTitle = MainTitle.Drink },
            new MyButton { ImageName = "breakfastDefault", TitleStyle = TitleStyle.MainTitle, MainTitle = MainTitle.Breakfast, Enabled = false },
        };

        /// <summary>
        /// 메뉴데이터들
        /// MainTitle를 넣으면 MenuData가 나온다
        /// </summary>
        private Dictionary<MainTitle?, MenuData> MenuDatas = new Dictionary<MainTitle?, MenuData>()
        {
            {
                MainTitle.BkSpecial, new MenuData()
                {
                    MyListViews = new List<MyListView>
                    {
                        new MyListView() { SubTitle = SubTitle.SpecialOffer },
                        new MyListView() { SubTitle = SubTitle.DoubleKing },
                        new MyListView() { SubTitle = SubTitle.SnackKing }
                    },
                    Titles = new List<string>
                    {
                        "main_title_bkspecial",
                        "sub_title_SpecialOffer",
                        "sub_title_Hero",
                        "sub_title_Wow"
                    }
                }
            },
            {
                MainTitle.SetMenu, new MenuData()
                {
                    MyListViews = new List<MyListView>
                    {
                        new MyListView() { SubTitle = SubTitle.Whopper },
                        new MyListView() { SubTitle = SubTitle.Garlic },
                        new MyListView() { SubTitle = SubTitle.BeefChicken }
                    },
                    Titles = new List<string>
                    {
                        "main_title_burger2",
                        "sub_title_Whopper",
                        "sub_title_Garlic",
                        "sub_title_BeefChicken"
                    }
                }
            },
            {
                MainTitle.Burger, new MenuData()
                {
                    MyListViews = new List<MyListView>
                    {
                        new MyListView() { SubTitle = SubTitle.Whopper },
                        new MyListView() { SubTitle = SubTitle.Garlic },
                        new MyListView() { SubTitle = SubTitle.BeefChicken }
                    },
                    Titles = new List<string>
                    {
                        "main_title_burger1",
                        "sub_title_Whopper",
                        "sub_title_Garlic",
                        "sub_title_BeefChicken"
                    }
                }
            },
            {
                MainTitle.Side, new MenuData()
                {
                    MyListViews = new List<MyListView>
                    {
                        new MyListView()
                    },
                    Titles = new List<string>
                    {
                        "main_title_side"
                    }
                }
            },
            {
                MainTitle.Drink, new MenuData()
                {
                    MyListViews = new List<MyListView>
                    {
                        new MyListView()
                    },
                    Titles = new List<string>
                    {
                        "main_title_drink"
                    }
                }
            },
            {
                MainTitle.Breakfast, new MenuData()
                {
                    MyListViews = new List<MyListView>
                    {
                        new MyListView()
                    },
                    Titles = new List<string>
                    {
                        "main_title_breakfast"
                    }
                }
            }
        };

        /// <summary>
        /// 메뉴아이템 데이터
        /// </summary>
        private MenuItemData MenuItemData = new MenuItemData();

        /// <summary>
        /// 메뉴아이템 small이미지들 리스트
        /// </summary>
        private ImageList ImageList = new ImageList()
        {
            ColorDepth = ColorDepth.Depth32Bit,
            ImageSize = new Size(122, 145) // 정상크기 : 122, 145
        };

        /// <summary>
        /// 메뉴아이템 상세정보 패널
        /// </summary>
        private Panel MenuItemDataPanel = new Panel()
        {
            BackColor = Color.FromArgb(146, 21, 15),
            Dock = DockStyle.Fill
        };

        /// <summary>
        /// 현재 메인타이틀
        /// </summary>
        private MainTitle? NowMainTitle = MainTitle.Burger;

        /// <summary>
        /// 현재 서브타이틀
        /// </summary>
        private SubTitle? NowSubTitle = null;

        /// <summary>
        /// 현재 메뉴아이템데이터패널이 보여지고 있는가
        /// </summary>
        private bool IsShowingMenuItemDataPanel = false;

        /// <summary>
        /// 현재 주문스레드가 작동중인가
        /// </summary>
        private bool IsRunningMakingThread = false;

        #region BurgerDataPanel 자식컨트롤들

        private Label NameKRLabel = new Label()
        {
            AutoSize = true,
            Location = new Point(10, 10),
            ForeColor = Color.White,
            Font = new Font("맑은 고딕", 25F, FontStyle.Bold)
        };
        private Label NameENLabel = new Label()
        {
            AutoSize = true,
            Location = new Point(10, 50),
            ForeColor = Color.White,
            Font = new Font("맑은 고딕", 20F, FontStyle.Regular)
        };
        private Label DescriptionLabel = new Label()
        {
            AutoSize = true,
            Location = new Point(16, 100),
            ForeColor = Color.White,
            Font = new Font("맑은 고딕", 10F, FontStyle.Regular)
        };
        private Label IsSetLabel = new Label()
        {
            AutoSize = true,
            Location = new Point(15, 150),
            ForeColor = Color.White,
            Font = new Font("맑은 고딕", 12F, FontStyle.Regular)
        };
        private Label PriceLabel = new Label()
        {
            AutoSize = true,
            Location = new Point(70, 135),
            ForeColor = Color.White,
            Font = new Font("맑은 고딕", 22F, FontStyle.Bold)
        };
        private PictureBox BigPictureBox = new PictureBox()
        {
            Location = new Point(0, 185),
            SizeMode = PictureBoxSizeMode.AutoSize
        };
        private Button MakeButton = new Button()
        {
            Text = "만들기",
            Location = new Point(1100, 500),
            Size = new Size(100, 50),
            UseVisualStyleBackColor = true
        };
        private ProgressBar MakingProgressBar = new ProgressBar()
        {
            Dock = DockStyle.Bottom,
            Size = new Size(0, 10),
            Minimum = 0,
            Maximum = 6,
            Step = 1
        };
        private MyListView MenuSlider = new MyListView()
        {
            Dock = DockStyle.Bottom,
            Size = new Size(0, 183),
            Alignment = ListViewAlignment.Left,
            BackColor = Color.FromArgb(146, 21, 15),
            BorderStyle = BorderStyle.None
        };
        #endregion

        public BurgerKing()
        {
            Controls.Add(MenuItemDataPanel);
            InitializeComponent();
            flowLayoutPanelRight.BringToFront();
            InitBurgerKingDB();
        }

        private void InitBurgerKingDB()
        {
            OrderHistory = MongoClient.GetDatabase(name: "BurgerKing").GetCollection<Order>(name: "OrderHistory");
        }

        #region InitControls

        private void InitMenu()
        {
            int index = 0;
            foreach (KeyValuePair<MainTitle?, MenuData> menuData in MenuDatas)
            {
                int count = 0;
                foreach (MyListView myListView in menuData.Value.MyListViews)
                {
                    myListView.LargeImageList = ImageList;

                    foreach (MenuItem menuItem in MenuItemData.listMenuItem)
                    {
                        if (menuData.Key != menuItem.MainTitle) continue;
                        
                        if ((SubTitle)count == menuItem.SubTitle)
                        {
                            string smallImagePath = MakeImagePath("menuItems", menuItem.NameKR + "small", ImageExtension);
                            ImageList.Images.Add(Image.FromFile(smallImagePath));
                            ListViewItem lvItem = new ListViewItem() { ImageIndex = index++ };
                            myListView.Items.Add(lvItem);
                        }
                    }

                    myListView.SelectedIndexChanged += ListView_SelectedIndexChanged;
                    myListView.AutoHeight(4);
                    ++count;
                }
            }
        }

        private void InitFlowLayoutPanelLeft()
        {
            foreach (MyButton myBtn in MyButtons)
            {
                myBtn.MouseDown += MyButton_MouseDown;
                myBtn.MouseEnter += MyButton_MouseEnter;
                myBtn.MouseLeave += MyButton_MouseLeave;
                flowLayoutPanelLeft.Controls.Add(myBtn);
            }
        }

        private void StackOnRightFlowLayoutPanel(MainTitle? subMenu)
        {
            string folder = "titles";
            MenuData menuData = MenuDatas[subMenu];

            if (flowLayoutPanelRight.Controls.Count > 0)
            {
                flowLayoutPanelRight.Controls.Clear();
            }

            for (int i = 0; i < menuData.MyListViews.Count(); i++)
            {
                MyListView myListView = menuData.MyListViews[i];
                if (i == 0) // MainTitle
                {
                    flowLayoutPanelRight.Controls.Add(MakePictureBox(folder, menuData.Titles[i], ImageExtension));
                }

                if (menuData.Titles.Count > i + 1) // SubTitle
                {
                    flowLayoutPanelRight.Controls.Add(MakePictureBox(folder, menuData.Titles[i + 1], ImageExtension));
                }
                flowLayoutPanelRight.Controls.Add(menuData.MyListViews[i]); // Listview
                menuData.MyListViews[i].SetIconSpacing(LeftPadding, TopPadding);
            }
        }

        private void InitBurgerDataPanel()
        {
            MenuItemDataPanel.Controls.Add(NameKRLabel);
            MenuItemDataPanel.Controls.Add(NameENLabel);
            MenuItemDataPanel.Controls.Add(DescriptionLabel);
            MenuItemDataPanel.Controls.Add(IsSetLabel);
            MenuItemDataPanel.Controls.Add(PriceLabel);
            MenuItemDataPanel.Controls.Add(BigPictureBox);
            MenuItemDataPanel.Controls.Add(MakeButton);
            MakeButton.Click += MakeButton_Click;
            MenuItemDataPanel.Controls.Add(MenuSlider);
            MenuSlider.SetIconSpacing(LeftPadding - 10, TopPadding);
            MenuSlider.SelectedIndexChanged += ListView_SelectedIndexChanged;
        }

        #endregion

        #region MakeControls

        private PictureBox MakePictureBox(string folder, string imageName, string extension)
        {
            return new PictureBox()
            {
                Name = imageName,
                ImageLocation = MakeImagePath(folder, imageName, extension),
                SizeMode = PictureBoxSizeMode.AutoSize
            };
        }

        /// <summary>
        /// NutrientPanel을 스타일에 따라 만들어 반환
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private TableLayoutPanel MakeNutrientPanel(MenuStyle style)
        {
            TableLayoutPanel panel = new TableLayoutPanel()
            {
                Location = new Point(170, 145),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset,
                RowCount = 2,
                Name = "NutrientPanel"
            };

            int i = 0;
            if (MenuStyle.Drink != style)
            {
                panel.Size = new Size(550, 50);
                panel.ColumnCount = 6;
                foreach (string column in Nutrient.columns)
                {
                    panel.Controls.Add(MakeMyLabel(column), i++, 0);
                }
            }
            else
            {
                panel.Size = new Size(600, 50);
                panel.ColumnCount = 7;
                foreach (string column in DrinkNutrient.columns)
                {
                    panel.Controls.Add(MakeMyLabel(column), i++, 0);
                }
            }
            return panel;
        }

        private Label MakeMyLabel(string text)
        {
            return new Label()
            {
                Text = text,
                ForeColor = Color.White,
                Font = new Font("맑은 고딕", 10F, FontStyle.Regular),
                Anchor = AnchorStyles.None,
                AutoSize = true
            };
        }
        #endregion

        /// <summary>
        /// 메뉴아이템의 데이터를 보여준다
        /// </summary>
        /// <param name="menuItem"></param>
        private void ShowMenuItemData(MenuItem menuItem)
        {
            Control nutrientPanel = MenuItemDataPanel.Controls["NutrientPanel"];
            if (nutrientPanel != null)
            {
                nutrientPanel.Controls.Clear();
                MenuItemDataPanel.Controls.Remove(nutrientPanel);
            }

            AddMenuItemData(menuItem);

            TableLayoutPanel newNutrientPanel = MakeNutrientPanel(menuItem.MenuStyle);
            AddDataToNutrientTable(newNutrientPanel, menuItem);
            MenuItemDataPanel.Controls.Add(newNutrientPanel);

            if (!IsShowingMenuItemDataPanel || NowMainTitle != menuItem.MainTitle)
            {
                ShowMenuSlider(menuItem.MainTitle);
                MenuItemDataPanel.BringToFront();
            }

            newNutrientPanel.BringToFront();
            NowSubTitle = menuItem.SubTitle;
            IsShowingMenuItemDataPanel = true;
        }

        private void AddMenuItemData(MenuItem menuItem)
        {
            NameKRLabel.Text = menuItem.NameKR;
            NameENLabel.Text = menuItem.NameEN;
            DescriptionLabel.Text = menuItem.Description;
            IsSetLabel.Text = ((menuItem.MenuStyle == MenuStyle.Set) ? "세트" : "단품") + " ₩";
            PriceLabel.Text = menuItem.Price.ToString("#,###");
            if (menuItem.MenuStyle == MenuStyle.Set && menuItem is SetMenuItem setMenuItem)
            {
                BigPictureBox.Image = CombineThreeImages(setMenuItem.Burger.NameKR, setMenuItem.Side.NameKR, setMenuItem.Drink.NameKR, "menuItems", "big", ImageExtension);
            }
            else
            {
                BigPictureBox.ImageLocation = MakeImagePath("menuItems", menuItem.NameKR + "big", ImageExtension);
            }
        }

        /// <summary>
        /// 3개의 이미지 합성
        /// </summary>
        /// <param name="menuItemName1">아마도 버거</param>
        /// <param name="menuItemName2">아마도 감튀</param>
        /// <param name="menuItemName3">아마도 콜라</param>
        /// <param name="file">파일이름</param>
        /// <param name="size">크기</param>
        /// <param name="extension">확장자</param>
        /// <returns></returns>
        private Bitmap CombineThreeImages(string menuItemName1, string menuItemName2, string menuItemName3, string file, string size, string extension)
        {
            Bitmap bmap = new Bitmap(1100, 400);

            using (Graphics g = Graphics.FromImage(bmap))
            {
                Bitmap img1 = new Bitmap(MakeImagePath(file, menuItemName1 + size, extension));
                img1.MakeTransparent(Color.FromArgb(146, 21, 15));
                Bitmap img2 = new Bitmap(MakeImagePath(file, menuItemName2 + size, extension));
                img2.MakeTransparent(Color.FromArgb(146, 21, 15));
                Bitmap img3 = new Bitmap(MakeImagePath(file, menuItemName3 + size, extension));
                img3.MakeTransparent(Color.FromArgb(146, 21, 15));
                g.DrawImage(img1, 0, 0);
                g.DrawImage(img2, 300, 0);
                g.DrawImage(img3, 600, 0);
            }
            return bmap;
        }

        /// <summary>
        /// 3개의 이미지 합성
        /// </summary>
        /// <param name="menuItemName1">아마도 버거</param>
        /// <param name="menuItemName2">아마도 감튀</param>
        /// <param name="menuItemName3">아마도 콜라</param>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <param name="file3"></param>
        /// <param name="size"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        private Bitmap CombineThreeImages(string menuItemName1, string menuItemName2, string menuItemName3, string file1, string file2, string file3, string size, string extension)
        {
            Bitmap bmap = new Bitmap(1100, 400);

            using (Graphics g = Graphics.FromImage(bmap))
            {
                Bitmap img1 = new Bitmap(MakeImagePath(file1, menuItemName1 + size, extension));
                img1.MakeTransparent(Color.FromArgb(146, 21, 15));
                Bitmap img2 = new Bitmap(MakeImagePath(file2, menuItemName2 + size, extension));
                img2.MakeTransparent(Color.FromArgb(146, 21, 15));
                Bitmap img3 = new Bitmap(MakeImagePath(file3, menuItemName3 + size, extension));
                img3.MakeTransparent(Color.FromArgb(146, 21, 15));
                g.DrawImage(img1, 0, 0);
                g.DrawImage(img2, 300, 0);
                g.DrawImage(img3, 600, 0);
            }
            return bmap;
        }

        private void AddDataToNutrientTable(TableLayoutPanel table, MenuItem menuItem)
        {
            table.Controls.Add(MakeMyLabel(menuItem.Nutrient.Calories), 0, 1);
            table.Controls.Add(MakeMyLabel(menuItem.Nutrient.ServingSize), 1, 1);
            table.Controls.Add(MakeMyLabel(menuItem.Nutrient.Protein), 2, 1);
            table.Controls.Add(MakeMyLabel(menuItem.Nutrient.Sodium), 3, 1);
            table.Controls.Add(MakeMyLabel(menuItem.Nutrient.Sugars), 4, 1);
            table.Controls.Add(MakeMyLabel(menuItem.Nutrient.SaturatedFat), 5, 1);
            if (MenuStyle.Drink == menuItem.MenuStyle)
            {
                if (menuItem.Nutrient is DrinkNutrient drinkNutrient)
                {
                    table.Controls.Add(MakeMyLabel(drinkNutrient.Caffeine), 6, 1);
                }
            }
            table.TableLayoutPanelAutoStyle();
        }

        private void ShowMenuSlider(MainTitle subMenu)
        {
            if (MenuSlider.Items.Count > 0)
            {
                MenuSlider.Items.Clear();
            }

            MenuSlider.LargeImageList = ImageList;

            int index = 0;
            foreach (MenuItem menuItem in MenuItemData.listMenuItem)
            {
                if (subMenu != menuItem.MainTitle)
                {
                    index++;
                    continue;
                }

                ListViewItem lvItem = new ListViewItem() { ImageIndex = index++ };
                MenuSlider.Items.Add(lvItem);
            }
        }

        /// <summary>
        /// 이미지의 상태가 Hover일 때는 Default의 이미지로 바꾸고 Default일 때는 Hover의 이미지로 바꾼다
        /// </summary>
        /// <param name="button"></param>
        private void ChangeButtonPicture(MyButton button)
        {
            if (button == null) return;

            string name = button.Name;
            button.Name = button.Name.Replace((name.Contains("Hover")) ? "Hover" : "Default", (name.Contains("Hover") ? "Default" : "Hover"));
            button.ImageLocation = MakeImagePath("buttons", button.Name, MyButton.Extension);
        }

        /// <summary>
        /// 이미지의 상태가 Hover일 때는 Default의 이미지로 바꾸고 Default일 때는 Hover의 이미지로 바꾼다
        /// </summary>
        /// <param name="subMenu"></param>
        /// <param name="subTitle"></param>
        private void ChangeButtonPicture(MainTitle? subMenu, SubTitle? subTitle)
        {
            var data = MyButtons.Where(btn => btn.MainTitle == subMenu && btn.SubTitle == subTitle);
            
            if (data.Count() > 0)
            {
                ChangeButtonPicture(data.SingleOrDefault());
            }
        }

        /// <summary>
        /// 이미지 경로를 만듬
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageName"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        private string MakeImagePath(string file, string imageName, string extension)
        {
            return $@"{ImageRootPath}\{file}\{imageName}.{extension}";
        }

        /// <summary>
        /// MainTitle과 SubTitle에 의거한 첫 번째 아이템의 ImageIndex 반환
        /// </summary>
        /// <param name="mainTitle"></param>
        /// <param name="subTitle"></param>
        /// <returns></returns>
        private int GetFirstItemIndex(MainTitle? mainTitle, SubTitle? subTitle)
        {
            var items = MenuDatas[mainTitle].MyListViews[(int)subTitle].Items;
            return (items.Count > 0) ? items[0].ImageIndex : -1;
        }

        /// <summary>
        /// 이름으로 메뉴아이템 가져오기
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private MenuItem GetMenuItemByName(string name)
        {
            return MenuItemData.listMenuItem.SingleOrDefault(x => x.NameKR.Equals(name));
        }

        private Order MakeOrder(MenuItem menuItem)
        {
            return new Order()
            {
                MenuItemNameKR = menuItem.NameKR,
                MenuItemNameEN = menuItem.NameEN,
                Price = menuItem.Price,
                OrderTime = DateTime.Now
            };
        }

        /// <summary>
        /// 만드는 스레드 메서드
        /// </summary>
        /// <param name="param"></param>
        private void MakingThreadMethod(object param)
        {
            IsRunningMakingThread = true;

            object[] args = (object[])param;
            MenuItem menuItem = args[0] as MenuItem;
            Button button = args[1] as Button;

            for (int i = MakingProgressBar.Minimum + 1; i <= MakingProgressBar.Maximum; i++)
            {
                // 만드는 과정 출력
                MakingProgressBar.Invoke(new ShowMakingProcessMethodInvoker(ShowMakingProcess), new object[] { menuItem, i });
                Thread.Sleep(1000);
                MakingProgressBar.Invoke(new MethodInvoker(() => MakingProgressBar.PerformStep()));
            }

            // 완성된 모습 출력
            MakingProgressBar.Invoke(new ShowMadeImageInvoker(ShowMadeImage), new object[] { menuItem });

            MessageBox.Show(menuItem.NameKR + "이(가) 완성되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // DB에 저장
            try
            {
                OrderHistory.InsertOne(document: MakeOrder(menuItem));
            }
            catch (Exception)
            {
                MessageBox.Show("DB에 주문내역을 저장하지 못하였습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            IsRunningMakingThread = false;
            button.Enabled = true;

            MakingThread.Abort();
        }

        private delegate void ShowMakingProcessMethodInvoker(MenuItem menuItem, int i);
        /// <summary>
        /// 만드는 과정 이미지 출력
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="index"></param>
        private void ShowMakingProcess(MenuItem menuItem, int index)
        {
            string extension = "jpg";
            switch (menuItem.MenuStyle)
            {
                case MenuStyle.Burger:
                    BigPictureBox.ImageLocation = MakeImagePath("hamburgerProcess", index.ToString(), extension);
                    break;
                case MenuStyle.Side:
                    BigPictureBox.ImageLocation = MakeImagePath("sideProcess", index.ToString(), extension);
                    break;
                case MenuStyle.Drink:
                    BigPictureBox.ImageLocation = MakeImagePath("drinkProcess", index.ToString(), extension);
                    break;
                case MenuStyle.Set:
                    BigPictureBox.Image = CombineThreeImages(index.ToString(), index.ToString(), index.ToString(), "hamburgerProcess", "sideProcess", "drinkProcess", string.Empty, extension);
                    break;
            }
        }

        private delegate void ShowMadeImageInvoker(MenuItem menuItem);
        /// <summary>
        /// 만든 이미지 출력
        /// </summary>
        /// <param name="menuItem"></param>
        private void ShowMadeImage(MenuItem menuItem)
        {
            switch (menuItem.MenuStyle)
            {
                case MenuStyle.Burger:
                case MenuStyle.Side:
                case MenuStyle.Drink:
                    BigPictureBox.ImageLocation = MakeImagePath("menuItems", menuItem.NameKR + "big", ImageExtension);
                    break;
                case MenuStyle.Set:
                    SetMenuItem setMenuItem = menuItem as SetMenuItem;
                    BigPictureBox.Image = CombineThreeImages(setMenuItem.Burger.NameKR, setMenuItem.Side.NameKR, setMenuItem.Drink.NameKR, "menuItems", "menuItems", "menuItems", "big", ImageExtension);
                    break;
            }
            MenuItemDataPanel.Controls.Remove(MakingProgressBar);
            MakingProgressBar.Value = 0;
        }
    }
}
