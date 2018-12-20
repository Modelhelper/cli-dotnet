using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using ModelHelper.Core.Help;
using ModelHelper.Extensions;
using Enumerable = System.Linq.Enumerable;

namespace ModelHelper.Core.CommandLine
{
    public static class ArgumentParser
    {
        public static Dictionary<string, string> Parse<TCommand>(this TCommand command, List<string> arguments)  where TCommand : class
        {
            var result = new ParseResult();
            var argumentDictionary = arguments.AsArgumentDictionary();

            var commandType = command.GetType();
            var props = commandType.GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                var att = (OptionAttribute) Attribute.GetCustomAttribute(prop, typeof(OptionAttribute));

                if (att != null)
                {
                    var key = att.Key; 
                    var exists = argumentDictionary.ContainsKey(att.Key) ||
                                 (att.Aliases != null && att.Aliases.Any(a => argumentDictionary.ContainsKey(a)));
                    var argumentValue = "";


                    if (argumentDictionary.ContainsKey(key))
                    {
                        argumentValue = argumentDictionary[key];
                    } else if (att.Aliases != null && att.Aliases.Any(a => argumentDictionary.ContainsKey(a)))
                    {
                        key = att.Aliases.FirstOrDefault(a => argumentDictionary.ContainsKey(a));
                        argumentValue = argumentDictionary[key];
                    }
                        
                    if (prop.PropertyType == typeof(bool))
                    {
                        //prop.SetValue(prop, arg);
                        prop.SetValue(command, exists, null);
                    }
                    else if (prop.PropertyType == typeof(string))
                    {
                        if (exists)
                        {
                            prop.SetValue(command, argumentDictionary[key], null);
                        }
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        if (exists)
                        {
                            prop.SetValue(command, Convert.ToInt32(argumentDictionary[key]), null);
                        }
                    }
                    else if (prop.PropertyType == typeof(IEnumerable<string>) || prop.PropertyType == typeof(List<string>))
                    {

                        var list = exists ? argumentDictionary[key].Split(',').ToList() : new List<string>();
                        prop.SetValue(command, list, null);
                    }
                    else
                    {
                        
                    }

                    if (!string.IsNullOrEmpty(att.ParameterProperty))
                    {
                        var paramProperty = commandType.GetProperty(att.ParameterProperty);
                        if (paramProperty != null)
                        {
                            
                            if (paramProperty.PropertyType == typeof(int))
                            {
                                paramProperty.SetValue(command, Convert.ToInt32(argumentValue));
                            }
                            else if (paramProperty.PropertyType == typeof(bool))
                            {
                                paramProperty.SetValue(command, Convert.ToBoolean(argumentValue));
                            }
                            else 
                            {
                                paramProperty.SetValue(command, Convert.ToString(argumentValue));
                            }
                            //paramProperty.SetValue(command, argumentValue);
                        }
                    }

                    if (att.ParameterIsRequired)
                    {
                        
                    }
                    //switch (prop.PropertyType)
                    //{
                    //    case typeof(int):
                    //}
                    
                    
                }                
            }
           
            return argumentDictionary;
        }
    }

    public class ParseResult
    {

    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class OptionAttribute : Attribute
    {        
        public string Key { get; set; }
        public string[] Aliases { get; set; }

        public bool IsRequired { get; set; }
        public bool ParameterIsRequired { get; set; }

        public string ParameterProperty { get; set; }

        public object DefaultValue { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ParameterForAttribute : Attribute
    {
        public string PropertyName { get; set; }

    }
}