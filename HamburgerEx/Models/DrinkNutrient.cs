using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamburgerEx
{
    class DrinkNutrient : Nutrient
    {
        public static new string[] columns = { "열량(Kcal)", "중량(g/ml)", "단백질(g)", "나트륨(mg)", "당류(g)", "포화지방(g)", "카페인(mg)" };

        public string Caffeine; // 카페인
    }
}