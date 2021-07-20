namespace HamburgerEx
{
    class Nutrient
    {
        public static string[] columns = { "열량(Kcal)", "중량(g/ml)", "단백질(g)", "나트륨(mg)", "당류(g)", "포화지방(g)" };

        public string Calories { get; set; } // 칼로리 (kcal)
        public string ServingSize { get; set; } // 중량
        public string Protein { get; set; } // 단백질
        public string Sodium { get; set; } // 나트륨
        public string Sugars { get; set; } // 당류
        public string SaturatedFat { get; set; } // 포화지방
    }
}
