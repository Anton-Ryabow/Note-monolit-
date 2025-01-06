using Microsoft.Extensions.Primitives;

namespace Note
{
    public static class Service
    {
        public static bool AreAllStrings(string[] strings)
        {
            foreach (string str in strings)
            {
                if (String.IsNullOrEmpty(str)) return false;
            }
            return true;
        }
    }

    public static class StringExtension
    {
        public static string GetHashPass(this string pass)
        {
            int code = 0;

            char[] chars = pass.ToCharArray();

            foreach (char c in chars)
            {
                code += c.GetHashCode();
            }

            return code.ToString();
        }

        public static string GetHashPass(this StringValues pass)
        {
            int code = 0;

            char[] chars = pass.ToString().ToCharArray();

            foreach (char c in chars)
            {
                code += c.GetHashCode();
            }

            return code.ToString();
        }
    }
}
