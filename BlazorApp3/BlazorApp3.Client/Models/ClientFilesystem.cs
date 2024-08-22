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

        public ClientFilesystem(string name, ClientFilesystem? parent = null, bool isfolder = true)
        {
            Name = name;
            Parent = parent;
            IsFolder = isfolder;
        }

        public new ClientFilesystem? Get(string key)
        {
            if (key.IndexOf('\\') != -1)
            {
                ClientFilesystem? res = null;
                string[] split = key.Split('\\');
                for (int i = 1; i < split.Length; i++)
                {
                    if (this[split[i]] != null)
                        res = this[split[i]];
                    return res;
                }
            }
            return this;
        }
        public void Add(RoutesModel route)
        {
            List<RoutesModel> names = new List<RoutesModel>();
            string[] split = route.Route.Split("\\");
            int i;
            ClientFilesystem it = this;
            for (i = 1; i < split.Length; i++)
            {
                if(!it.ContainsKey(split[i]))
                    it.Add(split[i],new ClientFilesystem(split[i], it, i != split.Length-1));
                it = it[split[i]];
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
            string Path = "host\\"+Name;
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
