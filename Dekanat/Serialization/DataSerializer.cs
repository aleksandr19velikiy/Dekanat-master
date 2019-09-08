using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.Serialization
{
    class DataSerializer
    {
        public static void Serialize(string fileName, object data)
        {
            var formatter = new BinaryFormatter();
            var fileStream = new FileStream(fileName, FileMode.Create);
            formatter.Serialize(fileStream, data);
            fileStream.Close();
        }

        public static object Deserialize(string fileName)
        {
            var formatter = new BinaryFormatter();
            var fileStream = new FileStream(fileName, FileMode.Open);
            var deserializedItem = formatter.Deserialize(fileStream);
            fileStream.Close();
            return deserializedItem;
        }
    }
}
