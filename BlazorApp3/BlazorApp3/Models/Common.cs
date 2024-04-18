namespace BlazorApp3.Modules.Common
{
    public struct Common<T,U>
    {
        public T first;
        public U second;
    }

    public static class StringHelper
    { 
        public static bool FirstContains(string str, string value)
        {
            if (str.Length < value.Length) return false;
            for(int i = 0;i<value.Length;i++)
            {
                if (str[i] != value[i]) return false;
            }
            return true;
        }
    }

}
