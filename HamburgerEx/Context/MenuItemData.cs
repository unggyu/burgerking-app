using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace HamburgerEx
{
    class MenuItemData
    {
        public static string ImageRootPath = $@"{Directory.GetParent(Environment.CurrentDirectory).Parent.FullName}\images";
        public static string ImageExtension = "jpg";

        public enum TitleStyle
        {
            MainTitle,
            SubTitle
        }

        public enum MainTitle
        {
            BkSpecial,
            SetMenu,
            Burger,
            Side,
            Drink,
            Breakfast
        }

        public enum SubTitle
        {
            One, SpecialOffer = 0, Whopper = 0,
            Two, DoubleKing = 1, Garlic = 1,
            Three, SnackKing = 2, BeefChicken = 2
        }

        public enum MenuStyle
        {
            Burger,
            Side,
            Drink,
            Set
        }

        public MenuItemData()
        {
            InitSetMenuItems();
        }

        private void InitSetMenuItems()
        {
            var burgers = listMenuItem.Where(menuItem => menuItem.MenuStyle == MenuStyle.Burger);
            Side side = listMenuItem.SingleOrDefault(menuItem => menuItem.NameKR.Equals("프렌치프라이") && menuItem.MenuStyle == MenuStyle.Side) as Side;
            Drink drink = listMenuItem.SingleOrDefault(menuItem => menuItem.NameKR.Equals("코카-콜라") && menuItem.MenuStyle == MenuStyle.Drink) as Drink;

            listMenuItem.Where(menuItem => menuItem.MenuStyle == MenuStyle.Set).ForEach(menuItem =>
            {
                string burgerName = menuItem.NameKR.Replace("세트", string.Empty);
                SetMenuItem setMenuItem = menuItem as SetMenuItem;
                setMenuItem.Burger = burgers.SingleOrDefault(burgerMenuItem => burgerMenuItem.NameKR.Equals(burgerName)) as Burger;
                setMenuItem.Side = side;
                setMenuItem.Drink = drink;

                double nutrient_temp;
                double nutrientPercentage_temp;

                string calories = (Double.Parse(setMenuItem.Burger.Nutrient.Calories) + Double.Parse(setMenuItem.Side.Nutrient.Calories) + Double.Parse(setMenuItem.Drink.Nutrient.Calories)).ToString();
                string servingSize = (Double.Parse(setMenuItem.Burger.Nutrient.ServingSize) + Double.Parse(setMenuItem.Side.Nutrient.ServingSize) + Double.Parse(setMenuItem.Drink.Nutrient.ServingSize)).ToString();
                nutrient_temp = ParseNutrientValue(setMenuItem.Burger.Nutrient.Protein) + ParseNutrientValue(setMenuItem.Side.Nutrient.Protein) + ParseNutrientValue(setMenuItem.Drink.Nutrient.Protein);
                nutrientPercentage_temp = ParsePercentageValue(setMenuItem.Burger.Nutrient.Protein) + ParsePercentageValue(setMenuItem.Side.Nutrient.Protein) + ParsePercentageValue(setMenuItem.Drink.Nutrient.Protein);
                string protein = $"{nutrient_temp}({nutrientPercentage_temp})";
                nutrient_temp = ParseNutrientValue(setMenuItem.Burger.Nutrient.Sodium) + ParseNutrientValue(setMenuItem.Side.Nutrient.Sodium) + ParseNutrientValue(setMenuItem.Drink.Nutrient.Sodium);
                nutrientPercentage_temp = ParsePercentageValue(setMenuItem.Burger.Nutrient.Sodium) + ParsePercentageValue(setMenuItem.Side.Nutrient.Sodium) + ParsePercentageValue(setMenuItem.Drink.Nutrient.Sodium);
                string sodium = $"{nutrient_temp}({nutrientPercentage_temp})";
                string sugars = (Double.Parse(setMenuItem.Burger.Nutrient.Sugars) + Double.Parse(setMenuItem.Side.Nutrient.Sugars) + Double.Parse(setMenuItem.Drink.Nutrient.Sugars)).ToString();
                nutrient_temp = ParseNutrientValue(setMenuItem.Burger.Nutrient.SaturatedFat) + ParseNutrientValue(setMenuItem.Side.Nutrient.SaturatedFat) + ParseNutrientValue(setMenuItem.Drink.Nutrient.SaturatedFat);
                nutrientPercentage_temp = ParsePercentageValue(setMenuItem.Burger.Nutrient.SaturatedFat) + ParsePercentageValue(setMenuItem.Side.Nutrient.SaturatedFat) + ParsePercentageValue(setMenuItem.Drink.Nutrient.SaturatedFat);
                string saturatedFat = $"{nutrient_temp}({nutrientPercentage_temp})";

                setMenuItem.Nutrient = new Nutrient() { Calories = calories, ServingSize = servingSize, Protein = protein, Sodium = sodium, Sugars = sugars, SaturatedFat = saturatedFat };

                setMenuItem.Price = setMenuItem.Burger.Price + setMenuItem.Side.Price + setMenuItem.Drink.Price;
            });
        }

        public List<MenuItem> listMenuItem = new List<MenuItem>()
        {
            new SetMenuItem()
            {
                NameKR = "통새우와퍼주니어세트",
                NameEN = "TONG SHRIMP WHOPPER JR®",
                Description = "직화 방식으로 구운 100% 순쇠고기 패티에 갈릭페퍼 통새우와 스파이시토마토소스가 더해진 프리미엄 버거",
                Price = 6600,
                Nutrient = new Nutrient() { Calories = "816", ServingSize = "640", Protein = "25(43)", Sodium = "863(43)", Sugars = "44.4", SaturatedFat = "10(66)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Set,
            },
            new SetMenuItem()
            {
                NameKR = "통새우스테이크버거세트",
                NameEN = "TONG SHRIMP STEAK BURGER",
                Description = "직화 방식으로 구운 프리미엄 스테이크 패티에 갈릭페퍼 통새우와 고소한 치즈, 스파이시토마토소스가 더해진 프리미엄 버거",
                Price = 9600,
                Nutrient = new Nutrient() { Calories = "1274", ServingSize = "826", Protein = "41(75)", Sodium = "1178(89)", Sugars = "44.4", SaturatedFat = "22(147)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "통새우와퍼세트",
                NameEN = "TONG SHRIMP WHOPPER",
                Description = "직화 방식으로 구운 100% 순쇠고기 패티에 갈릭페퍼 통새우와 스파이시토마토소스가 더해진 프리미엄 버거",
                Price = 8500,
                Nutrient = new Nutrient() { Calories = "1176", ServingSize = "806", Protein = "38(69)", Sodium = "1325(66)", Sugars = "44.4", SaturatedFat = "18(120)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "와퍼세트",
                NameEN = "WHOPPER",
                Description = "불에 직접 구운 순 쇠고기 패티에 싱싱한 야채가 한가득~ 버거킹의 대표 메뉴!",
                Price = 7700,
                Nutrient = new Nutrient() { Calories = "1054", ServingSize = "756", Protein = "33(60)", Sodium = "1150(58)", Sugars = "49.9", SaturatedFat = "16(107)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "불고기와퍼세트",
                NameEN = "BULGOGI WHOPPER",
                Description = "불에 직접 구운 순 쇠고기 패티가 들어간 와퍼에 달콤한 불고기 소스까지!",
                Price = 7700,
                Nutrient = new Nutrient() { Calories = "1117", ServingSize = "756", Protein = "32(58)", Sodium = "1253(63)", Sugars = "45.1", SaturatedFat = "17(113)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "치즈와퍼세트",
                NameEN = "CHEESE WHOPPER",
                Description = "불에 직접 구운 순 쇠고기 패티가 들어간 와퍼에 고소한 치즈까지!",
                Price = 8300,
                Nutrient = new Nutrient() { Calories = "1151", ServingSize = "781", Protein = "37(67)", Sodium = "1629(81)", Sugars = "46.5", SaturatedFat = "19(127)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "와퍼주니어세트",
                NameEN = "WHOPPER JR",
                Description = "불에 직접 구운 순 쇠고기 패티가 들어간 와퍼의 주니어 버전~ 작지만 꽉 찼다!",
                Price = 6000,
                Nutrient = new Nutrient() { Calories = "834", ServingSize = "636", Protein = "21(38)", Sodium = "911(46)", Sugars = "44", SaturatedFat = "1(73)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "불고기와퍼주니어세트",
                NameEN = "BULGOGI WHOPPER JR",
                Description = "불에 직접 구운 순 쇠고기 패티가 들어간 와퍼주니어에 달콤한 불고기 소스까지!",
                Price = 6000,
                Nutrient = new Nutrient() { Calories = "815", ServingSize = "636", Protein = "22(40)", Sodium = "864(43)", Sugars = "43", SaturatedFat = "11(73)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "콰트로치즈와퍼주니어세트",
                NameEN = "QUATTRO CHEESE WHOPPER JR",
                Description = "진짜 불맛을 즐겨라, 4가지 고품격 치즈와 불에 직접 구운 와퍼 패티의 만남!",
                Price = 6600,
                Nutrient = new Nutrient() { Calories = "881", ServingSize = "651", Protein = "25(45)", Sodium = "972(49)", Sugars = "42.5", SaturatedFat = "13(87)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "치즈와퍼주니어세트",
                NameEN = "CHEESE WHOPPER JR",
                Description = "불에 직접 구운 순 쇠고기 패티가 들어간 와퍼주니어에 고소한 치즈 추가!",
                Price = 6300,
                Nutrient = new Nutrient() { Calories = "873", ServingSize = "648", Protein = "23(42)", Sodium = "1112(56)", Sugars = "44.2", SaturatedFat = "13(87)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "갈릭스테이크버거세트",
                NameEN = "GARLIC STEAK BURGER",
                Description = "두툼한 스테이크 패티, 향긋한 갈릭, 달콤한 볶음 양파의 맛있는 조화!",
                Price = 8800,
                Nutrient = new Nutrient() { Calories = "1072", ServingSize = "733", Protein = "40(73)", Sodium = "1483(74)", Sugars = "48.3", SaturatedFat = "18(120)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.Garlic,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "BLT뉴올리언스치킨버거세트",
                NameEN = "BLT NEW ORLEANS CHICKEN BURGER",
                Description = "두툼한 치킨 통가슴살에 베이컨과 치즈,양상추,토마토,잠발라야 시즈닝을 더한 새로운 매콤한 치킨버거",
                Price = 6700,
                Nutrient = new Nutrient() { Calories = "1295", ServingSize = "797", Protein = "38(68)", Sodium = "1821(91)", Sugars = "48.5", SaturatedFat = "16(106)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "뉴올리언스치킨버거세트",
                NameEN = "NEW ORLEANS CHICKEN BURGER",
                Description = "두툼한 치킨 통가슴살에 잠발라야 시즈닝을 더한 새로운 매콤한 치킨버거",
                Price = 5500,
                Nutrient = new Nutrient() { Calories = "1233", ServingSize = "760", Protein = "34(55)", Sodium = "1748(70)", Sugars = "42.2", SaturatedFat = "13(67)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "BLT롱치킨버거세트",
                NameEN = "BLT LONG CHICKEN BURGER",
                Description = "바삭한 베이컨과 신선한 야채 그리고 담백한 롱~ 치킨패티의 완벽한 조화!",
                Price = 6900,
                Nutrient = new Nutrient() { Calories = "1077", ServingSize = "747", Protein = "32(58)", Sodium = "1558(78)", Sugars = "42.4", SaturatedFat = "9(60)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "롱킹세트",
                NameEN = "LONG KING MEAL",
                Description = "100% 순쇠고기 패티가 두 장! 달콤 상큼한 스위트 사우전드 소스의 완벽한 조화",
                Price = 8000,
                Nutrient = new Nutrient() { Calories = "980", ServingSize = "708", Protein = "28(51)", Sodium = "1324(66)", Sugars = "48.6", SaturatedFat = "12(80)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "오리지널롱치킨버거세트",
                NameEN = "ORIGINAL LONG CHICKEN  SANDWICH",
                Description = "담백한 치킨 패티에 부드러운 마요네즈 소스와 싱싱한 야채가 듬뿍~ ",
                Price = 6400,
                Nutrient = new Nutrient() { Calories = "1106", ServingSize = "686", Protein = "30(55)", Sodium = "999(50)", Sugars = "42.1", SaturatedFat = "14(91)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "치즈버거세트",
                NameEN = "CHEESE BURGER",
                Description = "불에 구운 쇠고기 패티와 사르르 치즈까지, 작지만 알차다! ",
                Price = 5000,
                Nutrient = new Nutrient() { Calories = "801", ServingSize = "611", Protein = "23(42)", Sodium = "1093(55)", Sugars = "45.3", SaturatedFat = "12(80)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Set
            },
            new SetMenuItem()
            {
                NameKR = "불고기버거세트",
                NameEN = "BULGOGI BURGER",
                Description = "달콤한 불고기소스를 더한 실속 만점의 버거. 크기는 깜찍, 맛은 어메이징! ",
                Price = 5100,
                Nutrient = new Nutrient() { Calories = "806", ServingSize = "618", Protein = "21(38)", Sodium = "773(39)", Sugars = "49.3", SaturatedFat = "11(73)" },
                MainTitle = MainTitle.SetMenu,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Set
            },
            new Burger()
            {
                NameKR = "통새우와퍼주니어",
                NameEN = "TONG SHRIMP WHOPPER JR",
                Description = "직화 방식으로 구운 100% 순쇠고기 패티에 갈릭페퍼 통새우와 스파이시토마토소스가 더해진 프리미엄 버거",
                Price = 4600,
                Nutrient = new Nutrient() { Calories = "381", ServingSize = "162", Protein = "21(37)", Sodium = "522(26)", Sugars = "5", SaturatedFat = "7(49)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "통새우스테이크버거",
                NameEN = "TONG SHRIMP STEAK BURGER",
                Description = "직화 방식으로 구운 프리미엄 스테이크 패티에 갈릭페퍼 통새우와 고소한 치즈, 스파이시토마토소스가 더해진 프리미엄 버거",
                Price = 7600,
                Nutrient = new Nutrient() { Calories = "839", ServingSize = "348", Protein = "37(66)", Sodium = "1437(72)", Sugars = "5", SaturatedFat = "19(129)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "통새우와퍼",
                NameEN = "TONG SHRIMP WHOPPER",
                Description = "직화 방식으로 구운 100% 순쇠고기 패티에 갈릭페퍼 통새우와 스파이시토마토소스가 더해진 프리미엄 버거",
                Price = 6500,
                Nutrient = new Nutrient() { Calories = "741", ServingSize = "328", Protein = "34(63)", Sodium = "984(49)", Sugars = "5", SaturatedFat = "15(99)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "와퍼",
                NameEN = "WHOPPER",
                Description = "불에 직접 구운 순 쇠고기 패티에 싱싱한 야채가 한가득~ 버거킹의 대표 메뉴!",
                Price = 5600,
                Nutrient = new Nutrient() { Calories = "619", ServingSize = "278", Protein = "29(53)", Sodium = "809(40)", Sugars = "10.5", SaturatedFat = "13(84)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "불고기와퍼",
                NameEN = "BULGOGI WHOPPER",
                Description = "불에 직접 구운 순 쇠고기 패티가 들어간 와퍼에 달콤한 불고기 소스까지!",
                Price = 5600,
                Nutrient = new Nutrient() { Calories = "682", ServingSize = "278", Protein = "28(52)", Sodium = "912(46)", Sugars = "5.7", SaturatedFat = "14(96)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "콰트로치즈와퍼",
                NameEN = "QUATTRO CHEESE WHOPPER",
                Description = "진짜 불맛을 즐겨라, 4가지 고품격 치즈와 불에 직접 구운 와퍼 패티의 만남!",
                Price = 6500,
                Nutrient = new Nutrient() { Calories = "769", ServingSize = "309", Protein = "40(73)", Sodium = "1051(53)", Sugars = "6.2", SaturatedFat = "20(131)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "치즈와퍼",
                NameEN = "CHEESE WHOPPER",
                Description = "불에 직접 구운 순 쇠고기 패티가 들어간 와퍼에 고소한 치즈까지!",
                Price = 6200,
                Nutrient = new Nutrient() { Calories = "716", ServingSize = "303", Protein = "33(59)", Sodium = "1288(64)", Sugars = "7.1", SaturatedFat = "16(108)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "와퍼주니어",
                NameEN = "WHOPPER JR",
                Description = "불에 직접 구운 순 쇠고기 패티가 들어간 와퍼의 주니어 버전~ 작지만 꽉 찼다!",
                Price = 4000,
                Nutrient = new Nutrient() { Calories = "399", ServingSize = "158", Protein = "17(32)", Sodium = "570(29)", Sugars = "4.6", SaturatedFat = "8(51)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "불고기와퍼주니어",
                NameEN = "BULGOGI WHOPPER JR",
                Description = "불에 직접 구운 순 쇠고기 패티가 들어간 와퍼주니어에 달콤한 불고기 소스까지!",
                Price = 4000,
                Nutrient = new Nutrient() { Calories = "380", ServingSize = "158", Protein = "18(33)", Sodium = "523(26)", Sugars = "3.6", SaturatedFat = "8(51)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "콰트로치즈와퍼주니어",
                NameEN = "QUATTRO CHEESE WHOPPER JR",
                Description = "진짜 불맛을 즐겨라, 4가지 고품격 치즈와 불에 직접 구운 와퍼 패티의 만남!",
                Price = 4600,
                Nutrient = new Nutrient() { Calories = "446", ServingSize = "173", Protein = "21(39)", Sodium = "631(32)", Sugars = "3.1", SaturatedFat = "10(64)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "치즈와퍼주니어",
                NameEN = "CHEESE WHOPPER JR",
                Description = "불에 직접 구운 순 쇠고기 패티가 들어간 와퍼주니어에 고소한 치즈 추가!",
                Price = 4300,
                Nutrient = new Nutrient() { Calories = "438", ServingSize = "170", Protein = "19(35)", Sodium = "771(39)", Sugars = "4.8", SaturatedFat = "10(65)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Whopper,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "갈릭스테이크버거",
                NameEN = "GARLIC STEAK BURGER",
                Description = "두툼한 스테이크 패티, 향긋한 갈릭, 달콤한 볶음 양파의 맛있는 조화!",
                Price = 6700,
                Nutrient = new Nutrient() { Calories = "637", ServingSize = "295", Protein = "36(66)", Sodium = "1142(57)", Sugars = "8.9", SaturatedFat = "15(101)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.Garlic,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "BLT뉴올리언스치킨버거",
                NameEN = "BLT NEW ORLEANS CHICKEN BURGER",
                Description = "두툼한 치킨 통가슴살에 베이컨과 치즈,양상추,토마토,잠발라야 시즈닝을 더한 새로운 매콤한 치킨버거",
                Price = 5700,
                Nutrient = new Nutrient() { Calories = "860", ServingSize = "319", Protein = "34(62)", Sodium = "1480(74)", Sugars = "9.1", SaturatedFat = "13(85)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "뉴올리언스치킨버거",
                NameEN = "NEW ORLEANS CHICKEN BURGER",
                Description = "두툼한 치킨 통가슴살에 잠발라야 시즈닝을 더한 새로운 매콤한 치킨버거",
                Price = 4500,
                Nutrient = new Nutrient() { Calories = "798", ServingSize = "282", Protein = "30(55)", Sodium = "1407(70)", Sugars = "2.8", SaturatedFat = "10(67)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "2000치킨버거",
                NameEN = "2000 CHICKEN BURGER",
                Description = "신선한 양상추와 바삭한 치킨패티의 만남!",
                Price = 2000,
                Nutrient = new Nutrient() { Calories = "461", ServingSize = "160", Protein = "12(22)", Sodium = "810(41)", Sugars = "2", SaturatedFat = "7(47)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "2000치즈버거",
                NameEN = "2000 CHEESE BURGER",
                Description = "달콤 상큼한 스위트사우전 소스, 신선한토마토와 고소한 치즈의 조화!",
                Price = 2000,
                Nutrient = new Nutrient() { Calories = "432", ServingSize = "172", Protein = "17(30)", Sodium = "640(32)", Sugars = "8.4", SaturatedFat = "8(53)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "BLT롱치킨버거",
                NameEN = "BLT LONG CHICKEN BURGER",
                Description = "바삭한 베이컨과 신선한 야채 그리고 담백한 롱~ 치킨패티의 완벽한 조화!",
                Price = 4900,
                Nutrient = new Nutrient() { Calories = "642", ServingSize = "269", Protein = "28(52)", Sodium = "1217(61)", Sugars = "3", SaturatedFat = "6(43)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "롱킹",
                NameEN = "LONG KING",
                Description = "100% 순쇠고기 패티가 두 장! 달콤 상큼한 스위트 사우전드 소스의 완벽한 조화",
                Price = 5900,
                Nutrient = new Nutrient() { Calories = "545", ServingSize = "230", Protein = "24(44)", Sodium = "983(49)", Sugars = "9.2", SaturatedFat = "6(61)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "오리지널롱치킨버거",
                NameEN = "ORIGINAL LONG CHICKEN BURGER",
                Description = "담백한 치킨 패티에 부드러운 마요네즈 소스와 싱싱한 야채가 듬뿍~ ",
                Price = 4400,
                Nutrient = new Nutrient() { Calories = "571", ServingSize = "210", Protein = "25(45)", Sodium = "1100(55)", Sugars = "5.7", SaturatedFat = "5.5(37)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "치즈버거",
                NameEN = "CHEESE BURGER",
                Description = "불에 구운 쇠고기 패티와 사르르 치즈까지, 작지만 알차다! ",
                Price = 3000,
                Nutrient = new Nutrient() { Calories = "366", ServingSize = "133", Protein = "19(35)", Sodium = "752(38)", Sugars = "5.9", SaturatedFat = "9(57)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "불고기버거",
                NameEN = "BULGOGI BURGER",
                Description = "달콤한 불고기소스를 더한 실속 만점의 버거. 크기는 깜찍, 맛은 어메이징! ",
                Price = 3000,
                Nutrient = new Nutrient() { Calories = "371", ServingSize = "140", Protein = "17(31)", Sodium = "432(22)", Sugars = "9.9", SaturatedFat = "8(51)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Burger
            },
            new Burger()
            {
                NameKR = "햄버거",
                NameEN = "HAMBURGER",
                Description = "불에 구운 소고기 패티가 쏙~ 실속 있게 즐긴다! ",
                Price = 2700,
                Nutrient = new Nutrient() { Calories = "306", ServingSize = "121", Protein = "17(31)", Sodium = "513(26)", Sugars = "6.3", SaturatedFat = "6(40)" },
                MainTitle = MainTitle.Burger,
                SubTitle = SubTitle.BeefChicken,
                MenuStyle = MenuStyle.Burger
            },
            new Side()
            {
                NameKR = "텐더킹 2조각",
                NameEN = "TENDER KING",
                Description = "부드러운 닭 안심살에 잠발라야 시즈닝의 매콤함이 더해지다!",
                Price = 2000,
                Nutrient = new Nutrient() { Calories = "211", ServingSize = "80", Protein = "15(27)", Sodium = "542(27)", Sugars = "0.8", SaturatedFat = "2(13)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "텐더킹 4조각",
                NameEN = "TENDER KING",
                Description = "부드러운 닭 안심살에 잠발라야 시즈닝의 매콤함이 더해지다!",
                Price = 3800,
                Nutrient = new Nutrient() { Calories = "422", ServingSize = "160", Protein = "30(54)", Sodium = "1083(54)", Sugars = "1.7", SaturatedFat = "4(25)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "텐더킹 6조각",
                NameEN = "TENDER KING",
                Description = "부드러운 닭 안심살에 잠발라야 시즈닝의 매콤함이 더해지다!",
                Price = 5600,
                Nutrient = new Nutrient() { Calories = "634", ServingSize = "240", Protein = "45(81)", Sodium = "1625(81)", Sugars = "2.5", SaturatedFat = "6(38)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "치킨프라이",
                NameEN = "CHICKEN FRIES",
                Description = "어니언과 갈릭의 향긋한 풍미, 달콤 바삭한 치킨프라이",
                Price = 2000,
                Nutrient = new Nutrient() { Calories = "232", ServingSize = "84", Protein = "16(28)", Sodium = "357(18)", Sugars = "0.1", SaturatedFat = "4(24)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "치즈프라이",
                NameEN = "CHEESE FRIES",
                Description = "바삭한 프렌치프라이에 고소한 치즈가 듬뿍!",
                Price = 2500,
                Nutrient = new Nutrient() { Calories = "440", ServingSize = "139", Protein = "12(21)", Sodium = "447(22)", Sugars = "0.8", SaturatedFat = "8(51)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "프렌치프라이",
                NameEN = "FRENCH FRIES",
                Description = "세계 최고의 감자만 엄선해서 버거킹만의 비법으로 바삭하게!",
                Price = 1600,
                Nutrient = new Nutrient() { Calories = "285", ServingSize = "102", Protein = "4(6)", Sodium = "326(16)", Sugars = "0.3", SaturatedFat = "3(21)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "어니언링",
                NameEN = "ONION RING",
                Description = "오직 버거킹에서만 즐길 수 있다! 어니언의 맛있는 변신! ",
                Price = 2000,
                Nutrient = new Nutrient() { Calories = "332", ServingSize = "91", Protein = "5(8)", Sodium = "476(24)", Sugars = "2.5", SaturatedFat = "3(23)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "너겟킹 4조각",
                NameEN = "NUGGET KING",
                Description = "바삭~ 촉촉~ 한입에 쏙 부드러운 너겟킹!",
                Price = 2000,
                Nutrient = new Nutrient() { Calories = "139", ServingSize = "78", Protein = "6(10)", Sodium = "289(14)", Sugars = "0.4", SaturatedFat = "2(14)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "너겟킹 6조각",
                NameEN = "NUGGET KING",
                Description = "바삭~ 촉촉~ 한입에 쏙 부드러운 너겟킹!",
                Price = 3000,
                Nutrient = new Nutrient() { Calories = "210", ServingSize = "117", Protein = "8(15)", Sodium = "434(22)", Sugars = "0.6", SaturatedFat = "3(20)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "너겟킹 10조각",
                NameEN = "NUGGET KING",
                Description = "바삭~ 촉촉~ 한입에 쏙 부드러운 너겟킹!",
                Price = 5000,
                Nutrient = new Nutrient() { Calories = "347", ServingSize = "195", Protein = "14(25)", Sodium = "723(25)", Sugars = "1", SaturatedFat = "5(34)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "컵아이스크림",
                NameEN = "CUP ICECREAM",
                Description = "부드러운 아이스크림이 한 컵 가득",
                Price = 600,
                Nutrient = new Nutrient() { Calories = "154", ServingSize = "110", Protein = "2(4)", Sodium = "50(2)", Sugars = "16.9", SaturatedFat = "7(44)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "바닐라선데",
                NameEN = "VANILLA SUNDAE",
                Description = "향긋한 바닐라 향 때문에 더 맛있다! ",
                Price = 1500,
                Nutrient = new Nutrient() { Calories = "210", ServingSize = "150", Protein = "3(5)", Sodium = "68(3)", Sugars = "23", SaturatedFat = "9(60)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "딸기선데",
                NameEN = "STRAWBERRY SUNDAE",
                Description = "딸기맛의 상큼함이 살아있어요! ",
                Price = 1500,
                Nutrient = new Nutrient() { Calories = "261", ServingSize = "163", Protein = "3(5)", Sodium = "70(3)", Sugars = "35.3", SaturatedFat = "11(76)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "초코선데",
                NameEN = "CHOCOLATE SUNDAE",
                Description = "달콤하고 진한 초코의 맛! ",
                Price = 1500,
                Nutrient = new Nutrient() { Calories = "264", ServingSize = "175", Protein = "2(4)", Sodium = "81(4)", Sugars = "37.8", SaturatedFat = "7(46)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Side()
            {
                NameKR = "콘샐러드",
                NameEN = "CORN SALAD",
                Description = "달콤한 옥수수와 싱싱한 야채의 어울림",
                Price = 1600,
                Nutrient = new Nutrient() { Calories = "189", ServingSize = "105", Protein = "2(4)", Sodium = "226(11)", Sugars = "6.3", SaturatedFat = "1(9)" },
                MainTitle = MainTitle.Side,
                MenuStyle = MenuStyle.Side
            },
            new Drink()
            {
                NameKR = "아메리카노",
                NameEN = "AMERICANO (ICE/HOT)",
                Description = "자연을 담은 버거킹 RA인증커피",
                Price = 1500,
                Nutrient = new DrinkNutrient() { Calories = "6", ServingSize = "227", Protein = "0(0)", Sodium = "0(0)", Sugars = "0", SaturatedFat = "0(0)", Caffeine = "85.3" },
                MainTitle = MainTitle.Drink,
                MenuStyle = MenuStyle.Drink
            },
            new Drink()
            {
                NameKR = "코카-콜라",
                NameEN = "COCA-COLA",
                Description = "코카-콜라로 더 짜릿하게!",
                Price = 1700,
                Nutrient = new DrinkNutrient() { Calories = "150", ServingSize = "376", Protein = "0(0)", Sodium = "15(1)", Sugars = "39.1", SaturatedFat = "0(0)", Caffeine = "0" },
                MainTitle = MainTitle.Drink,
                MenuStyle = MenuStyle.Drink
            },
            new Drink()
            {
                NameKR = "코카-콜라 제로",
                NameEN = "COCA-COLA ZERO",
                Description = "100% 짜릿함, 칼로리는 제로! ",
                Price = 1700,
                Nutrient = new DrinkNutrient() { Calories = "0", ServingSize = "376", Protein = "0(0)", Sodium = "30(2)", Sugars = "0", SaturatedFat = "0(0)", Caffeine = "0" },
                MainTitle = MainTitle.Drink,
                MenuStyle = MenuStyle.Drink
            },
            new Drink()
            {
                NameKR = "환타오렌지",
                NameEN = "FANTA ORANGE",
                Description = "톡 쏘는 상큼함!",
                Price = 1700,
                Nutrient = new DrinkNutrient() { Calories = "188", ServingSize = "376", Protein = "0(0)", Sodium = "15(1)", Sugars = "51.1", SaturatedFat = "0(0)", Caffeine = "0" },
                MainTitle = MainTitle.Drink,
                MenuStyle = MenuStyle.Drink
            },
            new Drink()
            {
                NameKR = "스프라이트",
                NameEN = "SPRITE",
                Description = "나를 깨우는 상쾌함! ",
                Price = 1700,
                Nutrient = new DrinkNutrient() { Calories = "165", ServingSize = "376", Protein = "0(0)", Sodium = "23(1)", Sugars = "42.1", SaturatedFat = "0(0)", Caffeine = "0" },
                MainTitle = MainTitle.Drink,
                MenuStyle = MenuStyle.Drink
            },
            new Drink()
            {
                NameKR = "핫초코",
                NameEN = "HOT CHOCOLATE",
                Description = "달콤한 초코, 따뜻하게 즐기세요~",
                Price = 2000,
                Nutrient = new DrinkNutrient() { Calories = "210", ServingSize = "227", Protein = "2(5)", Sodium = "86(4)", Sugars = "23.0", SaturatedFat = "4(25)", Caffeine = "0" },
                MainTitle = MainTitle.Drink,
                MenuStyle = MenuStyle.Drink
            },
            new Drink()
            {
                NameKR = "미닛메이드",
                NameEN = "MINUITE MADE",
                Description = "오렌지의 신선함이 가득~  ",
                Price = 2500,
                Nutrient = new DrinkNutrient() { Calories = "172", ServingSize = "350", Protein = "0(0)", Sodium = "11(1)", Sugars = "36", SaturatedFat = "4(25)", Caffeine = "0" },
                MainTitle = MainTitle.Drink,
                MenuStyle = MenuStyle.Drink
            },
            new Drink()
            {
                NameKR = "생수(순수)",
                NameEN = "MINERAL WATER",
                Description = "깨끗하고 순수한 물",
                Price = 1200,
                Nutrient = new DrinkNutrient() { Calories = "0", ServingSize = "0", Protein = "0(0)", Sodium = "0(0)", Sugars = "0", SaturatedFat = "0(0)", Caffeine = "0" },
                MainTitle = MainTitle.Drink,
                MenuStyle = MenuStyle.Drink
            },
        };

        /// <summary>
        /// '('의 전방탐색, 
        /// 영양소 값
        /// </summary>
        /// <param name="nutrientText"></param>
        /// <returns></returns>
        private double ParseNutrientValue(string nutrientText)
        {
            Match match = Regex.Match(nutrientText, @".+(?=\()");
            return Double.Parse(match.Value);
        }

        /// <summary>
        /// '('와 ')'사이의 값, 
        /// 1일 기준치 퍼센트 값
        /// </summary>
        /// <param name="nutrientText"></param>
        /// <returns></returns>
        private double ParsePercentageValue(string nutrientText)
        {
            MatchCollection matches = Regex.Matches(nutrientText, @"[^()]+");
            return Double.Parse(matches[1].Value);
        }
    }
}
