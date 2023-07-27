using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Library1
{
    public class SerializeAndDeserialise
    {
        public static Mess Serialize(object anySerializableObject)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new CustomBinder();
            using (var memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, anySerializableObject);
                return new Mess { Data = memoryStream.ToArray() };
            }
        }
        public static object Deserialize(Mess message)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new CustomBinder();
            using (var memoryStream = new MemoryStream(message.Data))
            {
                return formatter.Deserialize(memoryStream);
            }
        }
    }    
}
