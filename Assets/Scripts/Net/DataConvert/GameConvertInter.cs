using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using com.QH.QPGame.Utility;

namespace com.QH.QPGame.Services.Utility
{
    public partial class GameConvert
    {
        private static Dictionary<FieldInfo, int> mFieldBuffer = new Dictionary<FieldInfo, int>();

        public static int GetSizeConstOfMarshal(FieldInfo fieldInfo)
        {
            if (mFieldBuffer.ContainsKey(fieldInfo))
            {
                return mFieldBuffer[fieldInfo];
            }
            else
            {
                int size = 0;
                object[] objs = fieldInfo.GetCustomAttributes(typeof(MarshalAsAttribute), false);
                if (objs.Length > 0)
                {
                    size = (objs[0] as MarshalAsAttribute).SizeConst;
                }

                mFieldBuffer[fieldInfo] = size;
                return size;
            }
        }


        public static T ByteToStruct<T>(byte[] bytebuffer, int len)
        {
            ByteBuffer buffer = ByteBufferPool.PopPacket();
            buffer.PushByteArray(bytebuffer, 0, len);
            //ByteBuffer buffer = ByteBufferPool.PopPacket(bytebuffer, len);
            Type t = typeof (T);
            object tObj = Activator.CreateInstance(t);
   
            FieldInfo[] fields = t.GetFields();

          // Debug.LogWarning("class:" + t.Name);

            foreach (FieldInfo fieldInfo in fields)
            {
                //buffer.Position = mPointer;
                if (fieldInfo.FieldType.IsArray)
                {
                    int cCount = GetSizeConstOfMarshal(fieldInfo);
                    if (fieldInfo.FieldType.GetElementType() == typeof(string))
                    {
                        //buffer.PopByteArray(cCount);

                        byte[] sData = buffer.PopByteArray(cCount);

                        string dataS = StringHelper.ByteArray2DefaultString(sData);

                        fieldInfo.SetValue(tObj, dataS);

                    }
                    else if (fieldInfo.FieldType.GetElementType() == typeof(byte))
                    {
                        fieldInfo.SetValue(tObj, buffer.PopByteArray(cCount));

                    }
                    else if (fieldInfo.FieldType.GetElementType().IsPrimitive)
                    {
                        int cSize = Marshal.SizeOf(fieldInfo.FieldType.GetElementType());

                        Array arr = Array.CreateInstance(fieldInfo.FieldType.GetElementType(), cCount);

                        for (int i = 0; i < cCount; i++)
                        {
                            ByteBuffer tempB = ByteBufferPool.PopPacket(buffer.PopByteArray(cSize));
                            arr.SetValue(BytesToValue(tempB, fieldInfo.FieldType.GetElementType()), i);
                            ByteBufferPool.DropPacket(tempB);
                        }
                        fieldInfo.SetValue(tObj, arr);
                        //mPointer += cCount * cSize;
                        //throw new Exception("ByteToStruct UNKnow Type To Byte Aray Value  FieldType.IsArray  IsPrimitive"+fieldInfo.FieldType);
                    }
                    else
                    {
                        Type type = fieldInfo.FieldType.GetElementType();
                        int cSize = Marshal.SizeOf(type);
                        Array arr = Array.CreateInstance(fieldInfo.FieldType.GetElementType(), cCount);

                        for (int i = 0; i < cCount; i++)
                        {
                            var buf = buffer.PopByteArray(cSize);
                            var param = new object[] { buf, cSize };

							MethodInfo methodDefine = null;
							var methods = typeof (GameConvert).GetMethods();
							for(int j=0; j<methods.Length; j++)
							{
								if(methods[j].Name == "ByteToStruct" && 
								   methods[j].IsGenericMethodDefinition &&
								   methods[j].GetParameters().Length == 2)
								{
									methodDefine = methods[j];
									break;
								}
							}

                            var method = methodDefine.MakeGenericMethod(type);
                            object val = method.Invoke(null, param);
                            arr.SetValue(val, i);
                        }
                        fieldInfo.SetValue(tObj, arr);
                    }
                }
                else
                {
                    int cCount = GetSizeConstOfMarshal(fieldInfo);

                    if (fieldInfo.FieldType == typeof(string))
                    {
                        System.Text.Encoding encoding = (typeof(T).IsUnicodeClass) ? System.Text.Encoding.Unicode : System.Text.Encoding.GetEncoding("GB2312");

                        int totalCnt = cCount;
                        if (typeof(T).IsUnicodeClass)
                        {
                            totalCnt = cCount * encoding.GetByteCount(new char[1]);
                        }

						if(totalCnt > buffer.RemainLength)
						{
							totalCnt = buffer.RemainLength;
						}

                        byte[] sData = buffer.PopByteArray(totalCnt);
						string dataS = encoding.GetString(sData);

                        if (dataS.Length > 0)
                        {
                            dataS = dataS.Substring(0, dataS.IndexOf((char)0));
                            fieldInfo.SetValue(tObj, dataS);
                        }
                    }
                    else if (fieldInfo.FieldType == typeof(byte))
                    {
                        fieldInfo.SetValue(tObj, buffer.PopByte());
                    }
                    else if (fieldInfo.FieldType.IsPrimitive)
                    {
                        fieldInfo.SetValue(tObj, BytesToValue(buffer, fieldInfo.FieldType));
                    }
                    else
                    {
                        int classSize = Marshal.SizeOf(fieldInfo.FieldType);
                        byte[] sData = buffer.PopByteArray(classSize);

                        var param = new object[] {sData, classSize};

						MethodInfo methodDefine = null;
						var methods = typeof (GameConvert).GetMethods();
						for(int j=0; j<methods.Length; j++)
						{
							if(methods[j].Name == "ByteToStruct" && 
							   methods[j].IsGenericMethodDefinition &&
							   methods[j].GetParameters().Length == 2)
							{
								methodDefine = methods[j];
								break;
							}
						}

                        var method = methodDefine.MakeGenericMethod(fieldInfo.FieldType);
                        object val = method.Invoke(null, param);
                        fieldInfo.SetValue(tObj, val);

                        //cByteToStruct<T>(sData, classSize);
                        //throw new Exception("ByteToStruct UNKnow Type To Byte Aray To Value  " + fieldInfo.FieldType);
                    }
                }

            }

            ByteBufferPool.DropPacket(buffer);
            return (T)tObj;
        }

		public static T ByteToStruct<T>(byte[] bytebuffer)
        {
			return ByteToStruct<T>(bytebuffer, bytebuffer.Length);
        }

        private static object BytesToValue(ByteBuffer bytebuffer, Type valueType)
        {
            if (valueType == typeof (UInt16))
            {
                return BitConverter.ToUInt16(bytebuffer.PopByteArray(sizeof(UInt16)), 0) as object;
            }
            else if (valueType == typeof(UInt32))
            {
                return BitConverter.ToUInt32(bytebuffer.PopByteArray(sizeof(UInt32)), 0) as object;
            }
            else if (valueType == typeof(UInt64))
            {
                return BitConverter.ToUInt64(bytebuffer.PopByteArray(sizeof(UInt64)), 0) as object;
            }
            else if (valueType == typeof(Int16))
            {
                return BitConverter.ToInt16(bytebuffer.PopByteArray(sizeof(Int16)), 0) as object;
            }
            else if (valueType == typeof(Int32))
            {
                return BitConverter.ToInt32(bytebuffer.PopByteArray(sizeof(Int32)), 0) as object;
            }
            else if (valueType == typeof(Int64))
            {
                return BitConverter.ToInt64(bytebuffer.PopByteArray(sizeof(Int64)), 0) as object;
            }
            else if (valueType == typeof(Single))
            {
                return BitConverter.ToSingle(bytebuffer.PopByteArray(sizeof(Single)), 0) as object;
            }
            else if (valueType == typeof(Double))
            {
                return BitConverter.ToDouble(bytebuffer.PopByteArray(sizeof(Double)), 0) as object;
            }
            return null;
        }

        private static byte[] ValueToBytes(object value, Type type)
        {
            if (type == typeof(UInt16))
            {
                return BitConverter.GetBytes((UInt16)value);
            }
            else if (type == typeof(UInt32))
            {
                return BitConverter.GetBytes((UInt32)value);
            }
            else if (type == typeof(UInt64))
            {
                return BitConverter.GetBytes((UInt64)value);
            }
            else if (type == typeof(Int16))
            {
                return BitConverter.GetBytes((Int16)value);
            }
            else if (type == typeof(Int32))
            {
                return BitConverter.GetBytes((Int32)value);
            }
            else if (type == typeof(Int64))
            {
                return BitConverter.GetBytes((Int64)value);
            }
            else if (type == typeof(Single))
            {
                return BitConverter.GetBytes((Single)value);
            }
            else if (type == typeof(Double))
            {
                return BitConverter.GetBytes((Double)value);
            }
            return null;
        }

		public static byte[] StructToByteArray<T>(T structObject)
        {
            ByteBuffer buffer = ByteBufferPool.PopPacket();
            foreach (FieldInfo fieldInfo in structObject.GetType().GetFields())
            {

                //Debug.Log("cStructToByteArray    FieldInfo:"+fieldInfo.FieldType);
                if (fieldInfo.FieldType.IsArray)
                {
                    int cCount = GetSizeConstOfMarshal(fieldInfo);
                    object obj = fieldInfo.GetValue(structObject);
                    if (fieldInfo.FieldType.GetElementType() == typeof(string))
                    {
                        throw new Exception("StructToByteArray UNKnow Type To Byte Aray Value  " + fieldInfo.FieldType);
                    }
                    else if (fieldInfo.FieldType.GetElementType() == typeof(byte))
                    {
                        byte[] value = (byte[])obj;
                        buffer.PutByteArray(value);
                    }
                    else if (fieldInfo.FieldType.GetElementType() == typeof(UInt32))
                    {
                        UInt32[] value = (UInt32[])obj;
                        buffer.PutIntArray(value);
                    }
                    else
                    {
                        throw new Exception("StructToByteArray UNKnow Type To Byte Aray Value  " + fieldInfo.FieldType);
                    }
                }
                else
                {
                    int cCount = GetSizeConstOfMarshal(fieldInfo);
                    object obj = fieldInfo.GetValue(structObject);
                    if (fieldInfo.FieldType == typeof(string))
                    {

                        System.Text.Encoding encoding = (typeof(T).IsUnicodeClass) ? System.Text.Encoding.Unicode : System.Text.Encoding.Default;

                        string strValue = (string)obj;
						if(string.IsNullOrEmpty(strValue))
						{
							strValue = "";
						}

                        //						byte[] strValueArray = StringHelper.String2UTF8ByteArray(strValue);
                        //						int len= cCount - strValueArray.Length;

                        byte[] strValueArray = encoding.GetBytes(strValue);
                        int len = cCount - strValueArray.Length;
                        if (typeof(T).IsUnicodeClass)
                        {
                            len = cCount - strValue.Length;
                        }


                        buffer.PutByteArray(strValueArray);
                        if (len > 0)
                        {
                            char[] temp = new char[len];
                            byte[] tempArray = encoding.GetBytes(temp);
                            buffer.PutByteArray(tempArray);
                        }
                        //throw new Exception("StructToByteArray UNKnow Type To Byte Aray Value  "+fieldInfo.FieldType);
                    }
                    else if (fieldInfo.FieldType == typeof(byte))
                    {
                        byte value = (byte)obj;
                        buffer.PutByte(value);
                    }
                    else if (fieldInfo.FieldType.IsPrimitive)
                    {
                        byte[] array = ValueToBytes(obj, fieldInfo.FieldType);
                        buffer.PutByteArray(array);
                    }
                    else if (fieldInfo.FieldType.IsEnum)
                    {
                        Int32 value = (Int32)obj;
                        byte[] array = ValueToBytes(value, typeof(Int32));
                        buffer.PutByteArray(array);
                    }
                    else if (fieldInfo.FieldType.IsValueType)
                    {
						byte[] valueArray = StructToByteArray(obj);
                        buffer.PutByteArray(valueArray);
                    }
                    else
                    {
                        throw new Exception("StructToByteArray UNKnow Type To Byte Aray Value  " + fieldInfo.FieldType);
                    }

                }
            }
            buffer.Position = 0;
            var bytes = buffer.PopByteArray();
            ByteBufferPool.DropPacket(buffer);
            return bytes;
        }

    }


}
