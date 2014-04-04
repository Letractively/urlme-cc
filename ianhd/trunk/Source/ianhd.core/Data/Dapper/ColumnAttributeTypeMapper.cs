using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ianhd.core.Data
{
    public class ColumnAttributeTypeMapper<T> : FallbackTypeMapper
    {
        public ColumnAttributeTypeMapper()
            : base(new Dapper.ITypeMap[]
                {
                    new CustomPropertyTypeMap(
                       typeof(T),
                       (type, columnName) =>
                           type.GetProperties().FirstOrDefault(prop =>
                               prop.GetCustomAttributes(false)
                                   .OfType<ColumnMappingAttribute>()
                                   .Any(attr => attr.Name == columnName)
                               )
                       ),
                    new DefaultTypeMap(typeof(T))
                })
        {

        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnMappingAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public class FallbackTypeMapper : Dapper.ITypeMap
    {
        private readonly IEnumerable<Dapper.ITypeMap> _mappers;

        public FallbackTypeMapper(IEnumerable<Dapper.ITypeMap> mappers)
        {
            _mappers = mappers;
        }


        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    ConstructorInfo result = mapper.FindConstructor(names, types);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public Dapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetConstructorParameter(constructor, columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public Dapper.IMemberMap GetMember(string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetMember(columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }
    }
}
