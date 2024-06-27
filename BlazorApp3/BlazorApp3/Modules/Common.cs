namespace BlazorApp3.Modules.Common
{
    static class Vars
    {
        public static string host = "C:\\Profiles";

    }
}

namespace BlazorApp3.Modules.Common
{
    public static class Common<T>
    {
        public delegate int Comparator(T x, T y);
        public static int BinarySearch(List<T> list, T item, Comparator comp, bool isInsert = true)
        {
            int left = 0;
            int right = list.Count - 1;
            int mid = left + (right - left) / 2;
            while (left <= right)
            {
                mid = left + (right - left) / 2;
                T midItem = list[mid];
                int result = comp(item, midItem);

                if (result == 0)
                {
                    return mid; // Элемент найден
                }
                else if (result < 0)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }
            if(isInsert)
            return mid; // Элемент не найден
            return -1;
        }
    }

    public static class Common
    {
        public static TimeSpan GetAbsoluteTime()
        {
            return DateTime.MinValue.Subtract(DateTime.UtcNow);
        }
    }
}
    public static class StringHelper
    {
        public static bool FirstContains(string str, string value)
        {
            if (str.Length < value.Length) return false;
            for (int i = 0; i < value.Length; i++)
            {
                if (str[i] != value[i]) return false;
            }
            return true;
        }
    }

