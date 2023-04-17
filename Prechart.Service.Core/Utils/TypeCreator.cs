using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Prechart.Service.Core.Utils;

    public static class TypeCreator
    {
        private static readonly Dictionary<Type, Type> Types = new Dictionary<Type, Type>();
        private static readonly object Locker = new object();
        private static ModuleBuilder moduleBuilder;

        public static Type CreateFromInterfaceType(Type destType)
        {
            if (!Types.ContainsKey(destType))
            {
                lock (Locker)
                {
                    if (!Types.ContainsKey(destType))
                    {
                        if (moduleBuilder == null)
                        {
                            var assemblyName = Guid.NewGuid().ToString();

                            var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
                            moduleBuilder = assembly.DefineDynamicModule("Module");
                        }

                        var type = moduleBuilder.DefineType("event_" + destType.Name.ToLowerInvariant(), TypeAttributes.Public, typeof(object));
                        var fieldsList = new List<string>();

                        type.AddInterfaceImplementation(destType);

                        foreach (var v in destType.GetProperties())
                        {
                            fieldsList.Add(v.Name);

                            var field = type.DefineField("_" + v.Name.ToLowerInvariant(), v.PropertyType, FieldAttributes.Private);
                            var property = type.DefineProperty(v.Name, PropertyAttributes.None, v.PropertyType, Array.Empty<Type>());
                            var getter = type.DefineMethod("get_" + v.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual, v.PropertyType, Array.Empty<Type>());
                            var setter = type.DefineMethod("set_" + v.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual, null, new Type[] { v.PropertyType });

                            var getGenerator = getter.GetILGenerator();
                            var setGenerator = setter.GetILGenerator();

                            getGenerator.Emit(OpCodes.Ldarg_0);
                            getGenerator.Emit(OpCodes.Ldfld, field);
                            getGenerator.Emit(OpCodes.Ret);

                            setGenerator.Emit(OpCodes.Ldarg_0);
                            setGenerator.Emit(OpCodes.Ldarg_1);
                            setGenerator.Emit(OpCodes.Stfld, field);
                            setGenerator.Emit(OpCodes.Ret);

                            property.SetGetMethod(getter);
                            property.SetSetMethod(setter);

                            var vGetMethod = v.GetGetMethod();
                            if (vGetMethod != null)
                            {
                                type.DefineMethodOverride(getter, vGetMethod);
                            }

                            var vSetMethod = v.GetSetMethod();
                            if (vSetMethod != null)
                            {
                                type.DefineMethodOverride(setter, vSetMethod);
                            }
                        }

                        Types.Add(destType, type.CreateType());
                    }
                }
            }

            return Types[destType];
        }
    }
