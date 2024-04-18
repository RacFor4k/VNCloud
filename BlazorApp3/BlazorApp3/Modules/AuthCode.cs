using BlazorApp3.Modules.Common;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Crypto.Paddings;
using System.ComponentModel;

namespace BlazorApp3.Models
{
    
    public static class AuthCode
    {
        private static int comp(Code f, Code s)
        {
            if (f.deathTime == s.deathTime)
                return 0;
            if (f.deathTime > s.deathTime)
                return 1; ;
            return -1;
        }


        private struct Code
        {   
            public byte[] login;
            public string code;
            public int deathTime;
        }

        private static List<Code> codes = new List<Code>(); 

        private static async Task DeleteExpired()
        {
            for(int i = codes.Count-1; i >=0; i --)
            {
                if (codes[i].deathTime < Common.GetAbsoluteTime().TotalSeconds)
                    codes.RemoveAt(i);
                else break;
            }
        }

        public static async Task Erase(string Code)
        {
            await DeleteExpired();
            codes.Remove(codes.Find(x => x.code == Code));
        }

        public static async Task<bool> IsExsist(string Code, byte[] login)
        {
            await DeleteExpired();
            if (codes.Find(x => x.code == Code && x.login == login).login != login)return true;
            return false;
        }
        public static async Task<string> GenerateCode(int seed, byte[] login, int liveTime = 300)
        {
            string result = "";
            Random rand = new Random(seed);
            for(int i = 0;i<6;i++)
            {
                if (rand.Next(0, 1) == 0)
                    result += rand.Next('A', 'Z');
                else result += rand.Next('0', '9');
            }
            Code code = new Code()
            {
                login = login,
                code = result,
                deathTime = (int)Common.GetAbsoluteTime().TotalSeconds + liveTime
            };
            await DeleteExpired();
            codes.Insert(Common<Code>.BinarySearch(codes, code, comp),code);
            return result;
        }
    }
}
