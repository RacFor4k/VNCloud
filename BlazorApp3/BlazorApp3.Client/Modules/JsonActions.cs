using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Nodes;

namespace BlazorApp3.Client.Modules
{
    public static class JsonActions
    {

        private static byte[] ObjToByte(object obj)
        {
            if (obj == null)
                return null;
#pragma warning disable SYSLIB0011 // Тип или член устарел
            BinaryFormatter bf = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Тип или член устарел
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static JsonObject SerializeToJson(string key, object[] values)
        {
            var jsonObj = new JsonObject();
            var jsonArray = new JsonArray();
                foreach (var obj in values)
                {
                    // Предполагаем, что ObjToByte - это ваш метод для конвертации объекта в байты
                    JsonNode jsonNode = JsonNode.Parse(ObjToByte(obj));
                    jsonArray.Add(jsonNode);
                }
                jsonObj.Add(key, jsonArray);
            return jsonObj;
        }

        // Функция для сериализации в строку
        public static string SerializeToString(string key, object[] values)
        {
            var jsonObj = SerializeToJson(key, values);
            return jsonObj.ToJsonString();
        }
    }
}
