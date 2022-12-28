//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Reflection.Emit;
//using System.Text;

//namespace MDRX.DataAccessCore.Server.Common
//{
//    public static class DynamicMethodHelper
//    {
//        private static readonly Hashtable creatorCache = Hashtable.Synchronized(new Hashtable());
//        public static readonly Hashtable getterCache = Hashtable.Synchronized(new Hashtable());
//        public static readonly Hashtable setterCache = Hashtable.Synchronized(new Hashtable());
//        private static readonly Type coType = typeof(CreateObject);

//        public static CreateObject CreateObjectFactory<T>() where T : class
//        {
//            return CreateObjectFactory(typeof(T));
//        }

//        public static CreateObject CreateObjectFactory(Type t)
//        {
//            CreateObject createObject = creatorCache[t] as CreateObject;
//            if (createObject == null)
//            {
//                lock (creatorCache.SyncRoot)
//                {
//                    createObject = creatorCache[t] as CreateObject;
//                    if (createObject != null)
//                        return createObject;
//                    DynamicMethod dynamicMethod = new DynamicMethod("DM$OBJ_FACTORY_" + t.Name, typeof(object), null, t);
//                    ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
//                    ilGenerator.Emit(OpCodes.Newobj, t.GetConstructor(Type.EmptyTypes));
//                    ilGenerator.Emit(OpCodes.Ret);
//                    createObject = (CreateObject)dynamicMethod.CreateDelegate(coType);
//                    creatorCache.Add(t, createObject);
//                }
//            }
//            return createObject;
//        }

//        public static PropertyGetHandler GetPropertyGetter(PropertyInfo propInfo)
//        {
//            PropertyGetHandler propertyGetHandler = getterCache[propInfo.DeclaringType.Name + "." + propInfo.Name] as PropertyGetHandler;
//            if (propertyGetHandler == null)
//            {
//                lock (getterCache.SyncRoot)
//                {
//                    propertyGetHandler = getterCache[propInfo.DeclaringType.Name + "." + propInfo.Name] as PropertyGetHandler;
//                    if (propertyGetHandler != null)
//                        return propertyGetHandler;
//                    DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[1]
//                    {
//            typeof (object)
//                    }, propInfo.DeclaringType.Module);
//                    ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
//                    ilGenerator.Emit(OpCodes.Ldarg_0);
//                    ilGenerator.EmitCall(OpCodes.Callvirt, propInfo.GetGetMethod(), null);
//                    EmitBoxIfNeeded(ilGenerator, propInfo.PropertyType);
//                    ilGenerator.Emit(OpCodes.Ret);
//                    propertyGetHandler = (PropertyGetHandler)dynamicMethod.CreateDelegate(typeof(PropertyGetHandler));
//                    getterCache.Add(propInfo.DeclaringType.Name + "." + propInfo.Name, propertyGetHandler);
//                }
//            }
//            return propertyGetHandler;
//        }

//        public static PropertySetHandler GetPropertySetter(PropertyInfo propInfo)
//        {
//            PropertySetHandler propertySetHandler = setterCache[propInfo.DeclaringType.Name + "." + propInfo.Name] as PropertySetHandler;
//            if (propertySetHandler == null)
//            {
//                lock (setterCache.SyncRoot)
//                {
//                    propertySetHandler = setterCache[propInfo.DeclaringType.Name + "." + propInfo.Name] as PropertySetHandler;
//                    if (propertySetHandler != null)
//                        return propertySetHandler;
//                    DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, null, new Type[2]
//                    {
//            typeof (object),
//            typeof (object)
//                    }, propInfo.DeclaringType.Module, true);
//                    ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
//                    ilGenerator.Emit(OpCodes.Ldarg_0);
//                    ilGenerator.Emit(OpCodes.Ldarg_1);
//                    EmitCastToReference(ilGenerator, propInfo.PropertyType);
//                    ilGenerator.EmitCall(OpCodes.Callvirt, propInfo.GetSetMethod(true), null);
//                    ilGenerator.Emit(OpCodes.Ret);
//                    propertySetHandler = (PropertySetHandler)dynamicMethod.CreateDelegate(typeof(PropertySetHandler));
//                    setterCache.Add(propInfo.DeclaringType.Name + "." + propInfo.Name, propertySetHandler);
//                }
//            }
//            return propertySetHandler;
//        }

//        private static void EmitCastToReference(ILGenerator ilGenerator, Type type)
//        {
//            if (type.IsValueType)
//                ilGenerator.Emit(OpCodes.Unbox_Any, type);
//            else
//                ilGenerator.Emit(OpCodes.Castclass, type);
//        }

//        private static void EmitBoxIfNeeded(ILGenerator ilGenerator, Type type)
//        {
//            if (!type.IsValueType)
//                return;
//            ilGenerator.Emit(OpCodes.Box, type);
//        }

//        private static void EmitFastInt(ILGenerator ilGenerator, int value)
//        {
//            switch (value)
//            {
//                case -1:
//                    ilGenerator.Emit(OpCodes.Ldc_I4_M1);
//                    break;
//                case 0:
//                    ilGenerator.Emit(OpCodes.Ldc_I4_0);
//                    break;
//                case 1:
//                    ilGenerator.Emit(OpCodes.Ldc_I4_1);
//                    break;
//                case 2:
//                    ilGenerator.Emit(OpCodes.Ldc_I4_2);
//                    break;
//                case 3:
//                    ilGenerator.Emit(OpCodes.Ldc_I4_3);
//                    break;
//                case 4:
//                    ilGenerator.Emit(OpCodes.Ldc_I4_4);
//                    break;
//                case 5:
//                    ilGenerator.Emit(OpCodes.Ldc_I4_5);
//                    break;
//                case 6:
//                    ilGenerator.Emit(OpCodes.Ldc_I4_6);
//                    break;
//                case 7:
//                    ilGenerator.Emit(OpCodes.Ldc_I4_7);
//                    break;
//                case 8:
//                    ilGenerator.Emit(OpCodes.Ldc_I4_8);
//                    break;
//                default:
//                    if (value > -129 && value < 128)
//                    {
//                        ilGenerator.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
//                        break;
//                    }
//                    ilGenerator.Emit(OpCodes.Ldc_I4, value);
//                    break;
//            }
//        }

//        public delegate object CreateObject();

//        public delegate object PropertyGetHandler(object target);

//        public delegate void PropertySetHandler(object target, object parameter);
//    }
//}
