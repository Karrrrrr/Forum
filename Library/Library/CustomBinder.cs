using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Library1
{
    class CustomBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Assembly currentasm = Assembly.GetExecutingAssembly();
            return Type.GetType($"{currentasm.GetName().Name}.{typeName.Split('.')[1]}");
        }
    }
}
