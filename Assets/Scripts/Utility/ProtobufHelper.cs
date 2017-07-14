using ProtoBuf;
using System.IO;

namespace com.QH.QPGame.Utility
{

    public class ProtobufHelper
    {
        public static byte[] Serialize(IExtensible msg)
        {
            byte[] result;
            using (MemoryStream stream = new MemoryStream())
            {

                Serializer.Serialize(stream, msg);

                result = stream.ToArray();
            }

            return result;

        }


        public static IExtensible Deserialize<IExtensible>(byte[] msg)
        {
            IExtensible result;
            using (MemoryStream stream = new MemoryStream(msg))
            {

                result = Serializer.Deserialize<IExtensible>(stream);
            }

            return result;

        }





    }
}

