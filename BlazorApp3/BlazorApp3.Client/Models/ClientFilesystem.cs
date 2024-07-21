using BlazorApp3.Models;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace BlazorApp3.Client.Models
{
    public class ClientFilesystem : Dictionary<string, ClientFilesystem?>
    {

        public ClientFilesystem? Parent;
       // public List<ClientFilesystem?> Child = new List<ClientFilesystem?>();
        public string Name;

        public bool IsFolder;

        public ClientFilesystem(string name, bool isfolder = true)
        {
           Name = name;
        }

        public new ClientFilesystem? Get(string key)
        {
            if (key.IndexOf('\\') != -1)
            {
                ClientFilesystem? res = null;
                string[] split = key.Split('\\');
                for (int i = 0; i < split.Length; i++)
                {
                    if (this[split[i]] != null)
                        res = this[split[i]];
                    return res;
                }
            }
            return this[key];
        }
        public void Add(RoutesModel route)
        {
            List<RoutesModel> names = new List<RoutesModel>();
            string[] split = route.Route.Split("\\");
            int i;
            for (i = 0; i < split.Length; i++)
            {   
                names.Add(new RoutesModel
                {
                    Route = split[i],
                    IsFolder = i < split.Length - 1 || route.IsFolder,
                });
            }
            var Head = GetHead();
            ClientFilesystem? HeadChild = Head;
            ClientFilesystem Temp = this;
            ClientFilesystem PastTemp = Temp;
            i = 0;
            do
            {
                HeadChild = Head[names[i].Route];
            } while (HeadChild != null);
            for (; i < names.Count; i++)
            {
                PastTemp = Temp;
                Temp.Add(names[i].Route, new ClientFilesystem(names[i].Route, names[i].IsFolder));
                Temp = Temp[names[i].Route];
                Temp.Parent = PastTemp;
            }
        }

        public ClientFilesystem GetHead()
        {
            ClientFilesystem? Head = Parent;
            if (Head == null)
            {
                return this;
            }
            do
            {
                if (Head.Parent == null)
                {
                    return Head;
                }
                Head = Head.Parent;
            } while (Head != null);
            return this;
        }

        public List<ClientFilesystem?> PathToHead()
        {
            List<ClientFilesystem?> Path = [this];
            ClientFilesystem? Head = Parent;
            if (Head == null)
            {
                return Path;
            }
            do
            {
                if (Head.Parent == null)
                {
                    return Path;
                }
                Path.Add(Head.Parent);
            } while (Head != null);
            return Path;
        }
        public string PathToHeadStr()
        {
            string Path = Name;
            ClientFilesystem? Head = Parent;
            if (Head == null)
            {
                return Path;
            }
            do
            {
                if (Head.Parent == null)
                {
                    return Path;
                }
                Path = Head.Parent.Name+"\\"+Path;
            } while (Head != null);
            return Path;
        }

    }
}
