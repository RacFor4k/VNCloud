using BlazorApp3.Models;
using DataLibrary;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Security.Policy;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;




namespace BlazorApp3.Modules
{
    public class SQLquery
    {
        //List<AccountModel> accounts;
        static IDataAccess _data = new DataAccess();
        static string filePath = "appsettings.json";
        static string json = File.ReadAllText(filePath);
        static JObject jObj = JObject.Parse(json);
        static string _config = (string)jObj["ConnectionStrings"]["default"];


        public static async Task<List<AccountModel>> SearchAllData()
        {
            string sql = "select * from accounts;";
            var temp = _data.LoadData<AccountModel, dynamic>(sql, new { }, _config);
            if (temp.Result.Count == 0)
                throw (new Exception("nothing found"));
            return temp.Result;
        }
        /// <summary>
        /// Возвращает данные аккаунта по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static async Task<List<AccountModel>> SearchData(byte[] login) {
            string sql = "select * from accounts where login=@login;";
            var temp = _data.LoadData<AccountModel, dynamic>(sql, new { }, _config);
            if (temp.Result.Count == 0)
                throw (new Exception("nothing found"));
            return temp.Result;
        }
        /// <summary>
        /// Возвращает пути по ID владельца
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static async Task<List<RoutesModel>> SearchData(int parentId)
        {
            string sql = "select * from routes where parentId=@parentId;";
            var temp = _data.LoadData<RoutesModel, dynamic>(sql, new { parentId }, _config);
            if (temp.Result.Count == 0)
                throw (new Exception("nothing found"));
            return temp.Result;
        }
        /// <summary>
        /// Добавляет аккаунт в базу данных(возвращает строку с ошибкой, если не получилось)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static async Task<string> CreateData(byte[] login, string email)
        {
            try
            {
                string sql = "insert into accounts (login, email) values (@login, @email);";
                await _data.SaveData(sql, new { login, email }, _config);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
        /// <summary>
        /// Добавляет ссылку в базу данных(возвращает строку с ошибкой, если не получилось)
        /// </summary>
        /// <param name="idParent"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> CreateData(int idParent, string url, bool isFolder = false)
        {
            try
            {
                string sql = "insert into routes (idParent, url, isFolder) values (@idParent, @url, @isFolder);";
                await _data.SaveData(sql, new { idParent, url, isFolder}, _config);
                return null;

            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
        /// <summary>
        /// Удаление данных из таблицы по id (nametable = accounts для аккаунтов,nametable = routes для путей)(возвращает строку с ошибкой, если не получилось)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nametable"></param>
        /// <returns></returns>
        public static async Task<string> DeleteData(int id, string nametable)
        {
            try
            {
                string sql = $"delete from {nametable} where id = @id;";
                await _data.SaveData(sql, new { id }, _config);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    } 
}
