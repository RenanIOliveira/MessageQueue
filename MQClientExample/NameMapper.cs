using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQClientExample
{
    internal class NameMapper
    {

        string path;
        public NameMapper(string dirPath = "C:\\Users\\renan\\OneDrive\\Documents\\Names")
        {
            path = dirPath;

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public void AddName(string name, string id)
        {
            name = name + ".txt";
            var path = Path.Combine(this.path, name);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            //File.WriteAllText(path, id);
            var stream = File.Open(path,FileMode.CreateNew);
            using var writer = new StreamWriter(stream);

            writer.WriteLine(id);
            writer.Flush();
            writer.Close();
        }

        public string? getIdFromName(string? name)
        {
            name = name + ".txt";

            if (name == null) return null;

            string[] fileEntries = Directory.GetFiles(this.path);

           
            if (fileEntries.Select(f => Path.GetFileName(f)).Contains(name))
            {
                if (!File.Exists(Path.Combine(this.path, name))) return null;

                var lines = File.ReadAllLines(Path.Combine(this.path,name));
                if (lines.Count() == 0) return null;
                
                return lines[0];
            }

            return null;
        }
    }
}
