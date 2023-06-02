using UnityEngine;

namespace Assets.Scripts.UI
{
    public class NumberSeparator
    {
        public static string SplitNumber(int number)
        {
            int step = 1000;

            if (number < step)
                return number.ToString();

            int exponent = (int) (Mathf.Log(number) / Mathf.Log(step));
            double truncatedNumber = number / Mathf.Pow(step, exponent);
            string suffix;

            switch (exponent)
            {
                case 1:
                    suffix = "K";
                    break;
                case 2:
                    suffix = "M";
                    break;
                case 3:
                    suffix = "B";
                    break;
                case 4:
                    suffix = "T";
                    break;
                default:
                    suffix = "E";
                    break;
            }

            return string.Format("{0:N1}{1}", truncatedNumber, suffix);
        }
    }
}