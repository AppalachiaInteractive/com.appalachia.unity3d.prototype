#if UNITY_EDITOR || UNITY_STANDALONE

// Unity's Text component doesn't render <b> tag correctly on mobile devices
#define USE_BOLD_COMMAND_SIGNATURES
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperConsole.Commands;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEngine;
#if UNITY_EDITOR && UNITY_2021_1_OR_NEWER
using SystemInfo = UnityEngine.Device.SystemInfo; // To support Device Simulator on Unity 2021.1+
#endif

// Manages the console commands, parses console input and handles execution of commands
// Supported method parameter types: int, float, bool, string, Vector2, Vector3, Vector4

// Helper class to store important information about a command
namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperConsole
{
    [CallStaticConstructorInEditor]
    public static class DeveloperConsole
    {
        public delegate bool ParseFunction(string input, out object output);

        #region Constants and Static Readonly

        private static readonly List<DeveloperConsoleMethodInfo> matchingMethods = new(4);
        private static readonly List<DeveloperConsoleMethodInfo> methods = new();
        [NonSerialized] private static AppaContext _context;

        private static readonly Dictionary<Type, ParseFunction> parseFunctions = new()
        {
            { typeof(string), ParseString },
            { typeof(bool), ParseBool },
            { typeof(int), ParseInt },
            { typeof(uint), ParseUInt },
            { typeof(long), ParseLong },
            { typeof(ulong), ParseULong },
            { typeof(byte), ParseByte },
            { typeof(sbyte), ParseSByte },
            { typeof(short), ParseShort },
            { typeof(ushort), ParseUShort },
            { typeof(char), ParseChar },
            { typeof(float), ParseFloat },
            { typeof(double), ParseDouble },
            { typeof(decimal), ParseDecimal },
            { typeof(Vector2), ParseVector2 },
            { typeof(Vector3), ParseVector3 },
            { typeof(Vector4), ParseVector4 },
            { typeof(Quaternion), ParseQuaternion },
            { typeof(Color), ParseColor },
            { typeof(Color32), ParseColor32 },
            { typeof(Rect), ParseRect },
            { typeof(RectOffset), ParseRectOffset },
            { typeof(Bounds), ParseBounds },
            { typeof(GameObject), ParseGameObject },
#if UNITY_2017_2_OR_NEWER
            { typeof(Vector2Int), ParseVector2Int },
            { typeof(Vector3Int), ParseVector3Int },
            { typeof(RectInt), ParseRectInt },
            { typeof(BoundsInt), ParseBoundsInt },
#endif
        };

        private static readonly CompareInfo caseInsensitiveComparer = new CultureInfo("en-US").CompareInfo;

        // All the commands

        // All the parse functions

        // All the readable names of accepted types
        private static readonly Dictionary<Type, string> typeReadableNames = new()
        {
            { typeof(string), "String" },
            { typeof(bool), "Boolean" },
            { typeof(int), "Integer" },
            { typeof(uint), "Unsigned Integer" },
            { typeof(long), "Long" },
            { typeof(ulong), "Unsigned Long" },
            { typeof(byte), "Byte" },
            { typeof(sbyte), "Short Byte" },
            { typeof(short), "Short" },
            { typeof(ushort), "Unsigned Short" },
            { typeof(char), "Char" },
            { typeof(float), "Float" },
            { typeof(double), "Double" },
            { typeof(decimal), "Decimal" }
        };

        // CompareInfo used for case-insensitive command name comparison

        // Split arguments of an entered command
        private static readonly List<string> commandArguments = new(8);

        // Command parameter delimeter groups
        private static readonly string[] inputDelimiters = { "\"\"", "''", "{}", "()", "[]" };

        #endregion

        static DeveloperConsole()
        {
            using (_PRF_DebugLogConsole.Auto())
            {
                DebugLogManager.InstanceAvailable += i => _debugLogManager = i;

                AddCommand("help", "Prints all commands", LogAllCommands);
                AddCommand<string>("help", "Prints all matching commands", LogAllCommandsWithName);
                AddCommand("sysinfo", "Prints system information", LogSystemInfo);

                var allTypes = ReflectionExtensions.GetAppalachiaTypes_CACHED();

                for (var i = 0; i < allTypes.Length; i++)
                {
                    var type = allTypes[i];

                    var typeMethods = type.GetMethods_CACHE(
                        BindingFlags.Static |
                        BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.DeclaredOnly
                    );

                    foreach (var typeMethod in typeMethods)
                    {
                        if (!typeMethod.HasAttribute<DeveloperConsoleAttribute>())
                        {
                            continue;
                        }

                        var consoleMethod = typeMethod.GetAttribute_CACHE<DeveloperConsoleAttribute>();

                        AddCommand(
                            consoleMethod.Command,
                            consoleMethod.Description,
                            typeMethod,
                            null,
                            consoleMethod.ParameterNames
                        );
                    }
                }
            }
        }

        #region Static Fields and Autoproperties

        private static readonly ProfilerMarker _PRF_DebugLogConsole =
            new ProfilerMarker(_PRF_PFX + nameof(DeveloperConsole));

        private static DebugLogManager _debugLogManager;

        #endregion

        private static AppaContext Context
        {
            get
            {
                using (_PRF_Context.Auto())
                {
                    if (_context == null)
                    {
                        _context = new AppaContext(typeof(DeveloperConsole));
                    }

                    return _context;
                }
            }
        }

        // Add a command that can be related to either a static or an instance method
        public static void AddCommand(string command, string description, Action method)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, null);
            }
        }

        public static void AddCommand<T1>(string command, string description, Action<T1> method)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, null);
            }
        }

        public static void AddCommand<T1>(string command, string description, Func<T1> method)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, null);
            }
        }

        public static void AddCommand<T1, T2>(string command, string description, Action<T1, T2> method)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, null);
            }
        }

        public static void AddCommand<T1, T2>(string command, string description, Func<T1, T2> method)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, null);
            }
        }

        public static void AddCommand<T1, T2, T3>(
            string command,
            string description,
            Action<T1, T2, T3> method)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, null);
            }
        }

        public static void AddCommand<T1, T2, T3>(string command, string description, Func<T1, T2, T3> method)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, null);
            }
        }

        public static void AddCommand<T1, T2, T3, T4>(
            string command,
            string description,
            Action<T1, T2, T3, T4> method)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, null);
            }
        }

        public static void AddCommand<T1, T2, T3, T4>(
            string command,
            string description,
            Func<T1, T2, T3, T4> method)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, null);
            }
        }

        public static void AddCommand<T1, T2, T3, T4, T5>(
            string command,
            string description,
            Func<T1, T2, T3, T4, T5> method)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, null);
            }
        }

        public static void AddCommand(string command, string description, Delegate method)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, null);
            }
        }

        // Add a command with custom parameter names
        public static void AddCommand<T1>(
            string command,
            string description,
            Action<T1> method,
            string parameterName)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(
                    command,
                    description,
                    method.Method,
                    method.Target,
                    new string[1] { parameterName }
                );
            }
        }

        public static void AddCommand<T1, T2>(
            string command,
            string description,
            Action<T1, T2> method,
            string parameterName1,
            string parameterName2)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(
                    command,
                    description,
                    method.Method,
                    method.Target,
                    new string[2] { parameterName1, parameterName2 }
                );
            }
        }

        public static void AddCommand<T1, T2>(
            string command,
            string description,
            Func<T1, T2> method,
            string parameterName)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(
                    command,
                    description,
                    method.Method,
                    method.Target,
                    new string[1] { parameterName }
                );
            }
        }

        public static void AddCommand<T1, T2, T3>(
            string command,
            string description,
            Action<T1, T2, T3> method,
            string parameterName1,
            string parameterName2,
            string parameterName3)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(
                    command,
                    description,
                    method.Method,
                    method.Target,
                    new string[3] { parameterName1, parameterName2, parameterName3 }
                );
            }
        }

        public static void AddCommand<T1, T2, T3>(
            string command,
            string description,
            Func<T1, T2, T3> method,
            string parameterName1,
            string parameterName2)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(
                    command,
                    description,
                    method.Method,
                    method.Target,
                    new string[2] { parameterName1, parameterName2 }
                );
            }
        }

        public static void AddCommand<T1, T2, T3, T4>(
            string command,
            string description,
            Action<T1, T2, T3, T4> method,
            string parameterName1,
            string parameterName2,
            string parameterName3,
            string parameterName4)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(
                    command,
                    description,
                    method.Method,
                    method.Target,
                    new string[4] { parameterName1, parameterName2, parameterName3, parameterName4 }
                );
            }
        }

        public static void AddCommand<T1, T2, T3, T4>(
            string command,
            string description,
            Func<T1, T2, T3, T4> method,
            string parameterName1,
            string parameterName2,
            string parameterName3)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(
                    command,
                    description,
                    method.Method,
                    method.Target,
                    new string[3] { parameterName1, parameterName2, parameterName3 }
                );
            }
        }

        public static void AddCommand<T1, T2, T3, T4, T5>(
            string command,
            string description,
            Func<T1, T2, T3, T4, T5> method,
            string parameterName1,
            string parameterName2,
            string parameterName3,
            string parameterName4)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(
                    command,
                    description,
                    method.Method,
                    method.Target,
                    new string[4] { parameterName1, parameterName2, parameterName3, parameterName4 }
                );
            }
        }

        public static void AddCommand(
            string command,
            string description,
            Delegate method,
            params string[] parameterNames)
        {
            using (_PRF_AddCommand.Auto())
            {
                AddCommand(command, description, method.Method, method.Target, parameterNames);
            }
        }

        // Add a command related with an instance method (i.e. non static method)
        public static void AddCommandInstance(
            string command,
            string description,
            string methodName,
            object instance,
            params string[] parameterNames)
        {
            using (_PRF_AddCommandInstance.Auto())
            {
                if (instance == null)
                {
                    Context.Log.Error("Instance can't be null!");
                    return;
                }

                AddCommand(command, description, methodName, instance.GetType(), instance, parameterNames);
            }
        }

        // Add a command related with a static method (i.e. no instance is required to call the method)
        public static void AddCommandStatic(
            string command,
            string description,
            string methodName,
            Type ownerType,
            params string[] parameterNames)
        {
            using (_PRF_AddCommandStatic.Auto())
            {
                AddCommand(command, description, methodName, ownerType, null, parameterNames);
            }
        }

        // Add a custom Type to the list of recognized command parameter Types
        public static void AddCustomParameterType(
            Type type,
            ParseFunction parseFunction,
            string typeReadableName = null)
        {
            using (_PRF_AddCustomParameterType.Auto())
            {
                if (type == null)
                {
                    Context.Log.Error("Parameter type can't be null!");
                    return;
                }

                if (parseFunction == null)
                {
                    Context.Log.Error("Parameter parseFunction can't be null!");
                    return;
                }

                parseFunctions[type] = parseFunction;

                if (!string.IsNullOrEmpty(typeReadableName))
                {
                    typeReadableNames[type] = typeReadableName;
                }
            }
        }

        // Parse the command and try to execute it
        public static void ExecuteCommand(string command)
        {
            using (_PRF_ExecuteCommand.Auto())
            {
                if (command == null)
                {
                    return;
                }

                command = command.Trim();

                if (command.Length == 0)
                {
                    return;
                }

                // Split the command's arguments
                commandArguments.Clear();
                FetchArgumentsFromCommand(command, commandArguments);

                // Find all matching commands
                matchingMethods.Clear();
                var parameterCountMismatch = false;
                var commandIndex = FindCommandIndex(commandArguments[0]);
                if (commandIndex >= 0)
                {
                    var _command = commandArguments[0];

                    var commandLastIndex = commandIndex;
                    while ((commandIndex > 0) &&
                           (caseInsensitiveComparer.Compare(
                                methods[commandIndex - 1].command,
                                _command,
                                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                            ) ==
                            0))
                    {
                        commandIndex--;
                    }

                    while ((commandLastIndex < (methods.Count - 1)) &&
                           (caseInsensitiveComparer.Compare(
                                methods[commandLastIndex + 1].command,
                                _command,
                                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                            ) ==
                            0))
                    {
                        commandLastIndex++;
                    }

                    while (commandIndex <= commandLastIndex)
                    {
                        if (!methods[commandIndex].IsValid())
                        {
                            methods.RemoveAt(commandIndex);
                            commandLastIndex--;
                        }
                        else
                        {
                            // Check if number of parameters match
                            if (methods[commandIndex].parameterTypes.Length == (commandArguments.Count - 1))
                            {
                                matchingMethods.Add(methods[commandIndex]);
                            }
                            else
                            {
                                parameterCountMismatch = true;
                            }

                            commandIndex++;
                        }
                    }
                }

                if (matchingMethods.Count == 0)
                {
                    var _command = commandArguments[0];
                    FindCommands(_command, !parameterCountMismatch, matchingMethods);

                    if (matchingMethods.Count == 0)
                    {
                        Context.Log.Warn(string.Concat("ERROR: can't find command '", _command, "'"));
                    }
                    else
                    {
                        var commandsLength = _command.Length + 75;
                        for (var i = 0; i < matchingMethods.Count; i++)
                        {
                            commandsLength += matchingMethods[i].signature.Length + 7;
                        }

                        var stringBuilder = new StringBuilder(commandsLength);
                        if (parameterCountMismatch)
                        {
                            stringBuilder.Append("ERROR: '")
                                         .Append(_command)
                                         .Append("' doesn't take ")
                                         .Append(commandArguments.Count - 1)
                                         .Append(" parameter(s). Available command(s):");
                        }
                        else
                        {
                            stringBuilder.Append("ERROR: can't find command '")
                                         .Append(_command)
                                         .Append("'. Did you mean:");
                        }

                        for (var i = 0; i < matchingMethods.Count; i++)
                        {
                            stringBuilder.Append("\n    - ").Append(matchingMethods[i].signature);
                        }

                        Context.Log.Warn(stringBuilder.ToString());

                        // The log that lists method signature(s) for this command should automatically be expanded for better UX
                        if (_debugLogManager)
                        {
                            _debugLogManager.ExpandLatestPendingLog();
                            _debugLogManager.StripStackTraceFromLatestPendingLog();
                        }
                    }

                    return;
                }

                DeveloperConsoleMethodInfo methodToExecute = null;
                var parameters = new object[commandArguments.Count - 1];
                string errorMessage = null;
                for (var i = 0; (i < matchingMethods.Count) && (methodToExecute == null); i++)
                {
                    var methodInfo = matchingMethods[i];

                    // Parse the parameters into objects
                    var success = true;
                    for (var j = 0; (j < methodInfo.parameterTypes.Length) && success; j++)
                    {
                        try
                        {
                            var argument = commandArguments[j + 1];
                            var parameterType = methodInfo.parameterTypes[j];

                            object val;
                            if (ParseArgument(argument, parameterType, out val))
                            {
                                parameters[j] = val;
                            }
                            else
                            {
                                success = false;
                                errorMessage = string.Concat(
                                    "ERROR: couldn't parse ",
                                    argument,
                                    " to ",
                                    GetTypeReadableName(parameterType)
                                );
                            }
                        }
                        catch (Exception e)
                        {
                            success = false;
                            errorMessage = "ERROR: " + e;
                        }
                    }

                    if (success)
                    {
                        methodToExecute = methodInfo;
                    }
                }

                if (methodToExecute == null)
                {
                    Context.Log.Warn(
                        !string.IsNullOrEmpty(errorMessage) ? errorMessage : "ERROR: something went wrong"
                    );
                }
                else
                {
                    // Execute the method associated with the command
                    var result = methodToExecute.method.Invoke(methodToExecute.instance, parameters);
                    if (methodToExecute.method.ReturnType != typeof(void))
                    {
                        // Print the returned value to the console
                        if ((result == null) || result.Equals(null))
                        {
                            Context.Log.Info("Returned: null");
                        }
                        else
                        {
                            Context.Log.Info("Returned: " + result);
                        }
                    }
                }
            }
        }

        public static void FetchArgumentsFromCommand(string command, List<string> commandArguments)
        {
            using (_PRF_FetchArgumentsFromCommand.Auto())
            {
                for (var i = 0; i < command.Length; i++)
                {
                    if (char.IsWhiteSpace(command[i]))
                    {
                        continue;
                    }

                    var delimiterIndex = IndexOfDelimiterGroup(command[i]);
                    if (delimiterIndex >= 0)
                    {
                        var endIndex = IndexOfDelimiterGroupEnd(command, delimiterIndex, i + 1);
                        commandArguments.Add(command.Substring(i + 1, endIndex - i - 1));
                        i = (endIndex < (command.Length - 1)) && (command[endIndex + 1] == ',')
                            ? endIndex + 1
                            : endIndex;
                    }
                    else
                    {
                        var endIndex = IndexOfChar(command, ' ', i + 1);
                        commandArguments.Add(
                            command.Substring(
                                i,
                                command[endIndex - 1] == ',' ? endIndex - 1 - i : endIndex - i
                            )
                        );
                        i = endIndex;
                    }
                }
            }
        }

        public static void FindCommands(
            string commandName,
            bool allowSubstringMatching,
            List<DeveloperConsoleMethodInfo> matchingCommands)
        {
            using (_PRF_FindCommands.Auto())
            {
                if (allowSubstringMatching)
                {
                    for (var i = 0; i < methods.Count; i++)
                    {
                        if (methods[i].IsValid() &&
                            (caseInsensitiveComparer.IndexOf(
                                 methods[i].command,
                                 commandName,
                                 CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                             ) >=
                             0))
                        {
                            matchingCommands.Add(methods[i]);
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < methods.Count; i++)
                    {
                        if (methods[i].IsValid() &&
                            (caseInsensitiveComparer.Compare(
                                 methods[i].command,
                                 commandName,
                                 CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                             ) ==
                             0))
                        {
                            matchingCommands.Add(methods[i]);
                        }
                    }
                }
            }
        }

        // Returns the first command that starts with the entered argument
        public static string GetAutoCompleteCommand(string commandStart)
        {
            using (_PRF_GetAutoCompleteCommand.Auto())
            {
                var commandIndex = FindCommandIndex(commandStart);
                if (commandIndex < 0)
                {
                    commandIndex = ~commandIndex;
                }

                string result = null;
                for (var i = commandIndex;
                     (i >= 0) &&
                     caseInsensitiveComparer.IsPrefix(
                         methods[i].command,
                         commandStart,
                         CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                     );
                     i--)
                {
                    result = methods[i].command;
                }

                if (result == null)
                {
                    for (var i = commandIndex + 1;
                         (i < methods.Count) &&
                         caseInsensitiveComparer.IsPrefix(
                             methods[i].command,
                             commandStart,
                             CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                         );
                         i++)
                    {
                        result = methods[i].command;
                    }
                }

                return result;
            }
        }

        public static string GetTypeReadableName(Type type)
        {
            using (_PRF_GetTypeReadableName.Auto())
            {
                string result;
                if (typeReadableNames.TryGetValue(type, out result))
                {
                    return result;
                }

                if (IsSupportedArrayType(type))
                {
                    var elementType = type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0];

                    if (elementType == null)

                    {
                        return null;
                    }

                    if (typeReadableNames.TryGetValue(elementType, out result))
                    {
                        return result + "[]";
                    }

                    return elementType.Name + "[]";
                }

                return type.Name;
            }
        }

        public static bool IsSupportedArrayType(Type type)
        {
            using (_PRF_IsSupportedArrayType.Auto())
            {
                if (type.IsArray)
                {
                    if (type.GetArrayRank() != 1)
                    {
                        return false;
                    }

                    type = type.GetElementType();
                }
                else if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() != typeof(List<>))
                    {
                        return false;
                    }

                    type = type.GetGenericArguments()[0];
                }
                else
                {
                    return false;
                }

                return parseFunctions.ContainsKey(type) ||
                       typeof(Component).IsAssignableFrom(type) ||
                       type.IsEnum;
            }
        }

        // Logs the list of available commands
        public static void LogAllCommands()
        {
            using (_PRF_LogAllCommands.Auto())
            {
                var length = 25;
                for (var i = 0; i < methods.Count; i++)
                {
                    if (methods[i].IsValid())
                    {
                        length += methods[i].signature.Length + 7;
                    }
                }

                var stringBuilder = new StringBuilder(length);
                stringBuilder.Append("Available commands:");

                for (var i = 0; i < methods.Count; i++)
                {
                    if (methods[i].IsValid())
                    {
                        stringBuilder.Append("\n    - ").Append(methods[i].signature);
                    }
                }

                Context.Log.Info(stringBuilder.ToString());

                // After typing help, the log that lists all the commands should automatically be expanded for better UX
                if (_debugLogManager)
                {
                    _debugLogManager.ExpandLatestPendingLog();
                    _debugLogManager.StripStackTraceFromLatestPendingLog();
                }
            }
        }

        // Logs the list of available commands that are either equal to commandName or contain commandName as substring
        public static void LogAllCommandsWithName(string commandName)
        {
            using (_PRF_LogAllCommandsWithName.Auto())
            {
                matchingMethods.Clear();

                // First, try to find commands that exactly match the commandName. If there are no such commands, try to find
                // commands that contain commandName as substring
                FindCommands(commandName, false, matchingMethods);
                if (matchingMethods.Count == 0)
                {
                    FindCommands(commandName, true, matchingMethods);
                }

                if (matchingMethods.Count == 0)
                {
                    Context.Log.Warn(string.Concat("ERROR: can't find command '", commandName, "'"));
                }
                else
                {
                    var commandsLength = 25;
                    for (var i = 0; i < matchingMethods.Count; i++)
                    {
                        commandsLength += matchingMethods[i].signature.Length + 7;
                    }

                    var stringBuilder = new StringBuilder(commandsLength);
                    stringBuilder.Append("Matching commands:");

                    for (var i = 0; i < matchingMethods.Count; i++)
                    {
                        stringBuilder.Append("\n    - ").Append(matchingMethods[i].signature);
                    }

                    Context.Log.Info(stringBuilder.ToString());

                    if (_debugLogManager)
                    {
                        _debugLogManager.ExpandLatestPendingLog();
                        _debugLogManager.StripStackTraceFromLatestPendingLog();
                    }
                }
            }
        }

        // Logs system information
        public static void LogSystemInfo()
        {
            using (_PRF_LogSystemInfo.Auto())
            {
                var stringBuilder = new StringBuilder(1024);
                stringBuilder.Append("Rig: ")
                             .AppendSysInfoIfPresent(SystemInfo.deviceModel)
                             .AppendSysInfoIfPresent(SystemInfo.processorType)
                             .AppendSysInfoIfPresent(SystemInfo.systemMemorySize, "MB RAM")
                             .Append(SystemInfo.processorCount)
                             .Append(" cores\n");
                stringBuilder.Append("OS: ").Append(SystemInfo.operatingSystem).Append("\n");
                stringBuilder.Append("GPU: ")
                             .Append(SystemInfo.graphicsDeviceName)
                             .Append(" ")
                             .Append(SystemInfo.graphicsMemorySize)
                             .Append("MB ")
                             .Append(SystemInfo.graphicsDeviceVersion)
                             .Append(SystemInfo.graphicsMultiThreaded ? " multi-threaded\n" : "\n");
                stringBuilder.Append("Data Path: ")
                             .Append(ProjectLocations.GetAssetsDirectoryPath())
                             .Append("\n");
                stringBuilder.Append("Persistent Data Path: ")
                             .Append(AppalachiaApplication.PersistentDataPath)
                             .Append("\n");
                stringBuilder.Append("StreamingAssets Path: ")
                             .Append(AppalachiaApplication.StreamingAssetsPath)
                             .Append("\n");
                stringBuilder.Append("Temporary Cache Path: ")
                             .Append(AppalachiaApplication.TemporaryCachePath)
                             .Append("\n");
                stringBuilder.Append("Device ID: ").Append(SystemInfo.deviceUniqueIdentifier).Append("\n");
                stringBuilder.Append("Max Texture Size: ").Append(SystemInfo.maxTextureSize).Append("\n");
#if UNITY_5_6_OR_NEWER
                stringBuilder.Append("Max Cubemap Size: ").Append(SystemInfo.maxCubemapSize).Append("\n");
#endif
                stringBuilder.Append("Accelerometer: ")
                             .Append(SystemInfo.supportsAccelerometer ? "supported\n" : "not supported\n");
                stringBuilder.Append("Gyro: ")
                             .Append(SystemInfo.supportsGyroscope ? "supported\n" : "not supported\n");
                stringBuilder.Append("Location Service: ")
                             .Append(SystemInfo.supportsLocationService ? "supported\n" : "not supported\n");
#if !UNITY_2019_1_OR_NEWER
			stringBuilder.Append( "Image Effects: " ).Append( SystemInfo.supportsImageEffects ? "supported\n" : "not supported\n" );
			stringBuilder.Append( "RenderToCubemap: " ).Append( SystemInfo.supportsRenderToCubemap ? "supported\n" : "not supported\n" );
#endif
                stringBuilder.Append("Compute Shaders: ")
                             .Append(SystemInfo.supportsComputeShaders ? "supported\n" : "not supported\n");
                stringBuilder.Append("Shadows: ")
                             .Append(SystemInfo.supportsShadows ? "supported\n" : "not supported\n");
                stringBuilder.Append("Instancing: ")
                             .Append(SystemInfo.supportsInstancing ? "supported\n" : "not supported\n");
                stringBuilder.Append("Motion Vectors: ")
                             .Append(SystemInfo.supportsMotionVectors ? "supported\n" : "not supported\n");
                stringBuilder.Append("3D Textures: ")
                             .Append(SystemInfo.supports3DTextures ? "supported\n" : "not supported\n");
#if UNITY_5_6_OR_NEWER
                stringBuilder.Append("3D Render Textures: ")
                             .Append(SystemInfo.supports3DRenderTextures ? "supported\n" : "not supported\n");
#endif
                stringBuilder.Append("2D Array Textures: ")
                             .Append(SystemInfo.supports2DArrayTextures ? "supported\n" : "not supported\n");
                stringBuilder.Append("Cubemap Array Textures: ")
                             .Append(SystemInfo.supportsCubemapArrayTextures ? "supported" : "not supported");

                Context.Log.Info(stringBuilder.ToString());

                // After typing sysinfo, the log that lists system information should automatically be expanded for better UX
                if (_debugLogManager)
                {
                    _debugLogManager.ExpandLatestPendingLog();
                    _debugLogManager.StripStackTraceFromLatestPendingLog();
                }
            }
        }

        public static bool ParseArgument(string input, Type argumentType, out object output)
        {
            using (_PRF_ParseArgument.Auto())
            {
                ParseFunction parseFunction;
                if (parseFunctions.TryGetValue(argumentType, out parseFunction))
                {
                    return parseFunction(input, out output);
                }

                if (typeof(Component).IsAssignableFrom(argumentType))
                {
                    return ParseComponent(input, argumentType, out output);
                }

                if (argumentType.IsEnum)
                {
                    return ParseEnum(input, argumentType, out output);
                }

                if (IsSupportedArrayType(argumentType))
                {
                    return ParseArray(input, argumentType, out output);
                }

                output = null;
                return false;
            }
        }

        public static bool ParseArray(string input, Type arrayType, out object output)
        {
            using (_PRF_ParseArray.Auto())
            {
                var valuesToParse = new List<string>(2);
                FetchArgumentsFromCommand(input, valuesToParse);

                var result = (IList)Activator.CreateInstance(arrayType, valuesToParse.Count);
                output = result;

                if (arrayType.IsArray)
                {
                    var elementType = arrayType.GetElementType();
                    for (var i = 0; i < valuesToParse.Count; i++)
                    {
                        object obj;
                        if (!ParseArgument(valuesToParse[i], elementType, out obj))
                        {
                            return false;
                        }

                        result[i] = obj;
                    }
                }
                else
                {
                    var elementType = arrayType.GetGenericArguments()[0];
                    for (var i = 0; i < valuesToParse.Count; i++)
                    {
                        object obj;
                        if (!ParseArgument(valuesToParse[i], elementType, out obj))
                        {
                            return false;
                        }

                        result.Add(obj);
                    }
                }

                return true;
            }
        }

        public static bool ParseBool(string input, out object output)
        {
            using (_PRF_ParseBool.Auto())
            {
                if ((input == "1") || (input.ToLowerInvariant() == "true"))
                {
                    output = true;
                    return true;
                }

                if ((input == "0") || (input.ToLowerInvariant() == "false"))
                {
                    output = false;
                    return true;
                }

                output = false;
                return false;
            }
        }

        public static bool ParseBounds(string input, out object output)
        {
            using (_PRF_ParseBounds.Auto())
            {
                return ParseVector(input, typeof(Bounds), out output);
            }
        }

        public static bool ParseBoundsInt(string input, out object output)
        {
            using (_PRF_ParseBoundsInt.Auto())
            {
                return ParseVector(input, typeof(BoundsInt), out output);
            }
        }

        public static bool ParseByte(string input, out object output)
        {
            using (_PRF_ParseByte.Auto())
            {
                byte value;
                var result = byte.TryParse(input, out value);

                output = value;
                return result;
            }
        }

        public static bool ParseChar(string input, out object output)
        {
            using (_PRF_ParseChar.Auto())
            {
                char value;
                var result = char.TryParse(input, out value);

                output = value;
                return result;
            }
        }

        public static bool ParseColor(string input, out object output)
        {
            using (_PRF_ParseColor.Auto())
            {
                return ParseVector(input, typeof(Color), out output);
            }
        }

        public static bool ParseColor32(string input, out object output)
        {
            using (_PRF_ParseColor32.Auto())
            {
                return ParseVector(input, typeof(Color32), out output);
            }
        }

        public static bool ParseComponent(string input, Type componentType, out object output)
        {
            using (_PRF_ParseComponent.Auto())
            {
                var gameObject = input == "null" ? null : GameObject.Find(input);
                output = gameObject ? gameObject.GetComponent(componentType) : null;
                return true;
            }
        }

        public static bool ParseDecimal(string input, out object output)
        {
            using (_PRF_ParseDecimal.Auto())
            {
                decimal value;
                var result = decimal.TryParse(
                    !input.EndsWith("f", StringComparison.OrdinalIgnoreCase)
                        ? input
                        : input.Substring(0, input.Length - 1),
                    out value
                );

                output = value;
                return result;
            }
        }

        public static bool ParseDouble(string input, out object output)
        {
            using (_PRF_ParseDouble.Auto())
            {
                double value;
                var result = double.TryParse(
                    !input.EndsWith("f", StringComparison.OrdinalIgnoreCase)
                        ? input
                        : input.Substring(0, input.Length - 1),
                    out value
                );

                output = value;
                return result;
            }
        }

        public static bool ParseEnum(string input, Type enumType, out object output)
        {
            using (_PRF_ParseEnum.Auto())
            {
                const int NONE = 0, OR = 1, AND = 2;

                var outputInt = 0;
                var operation = NONE; // 0: nothing, 1: OR with outputInt, 2: AND with outputInt
                for (var i = 0; i < input.Length; i++)
                {
                    string enumStr;
                    var orIndex = input.IndexOf('|',  i);
                    var andIndex = input.IndexOf('&', i);
                    if (orIndex < 0)
                    {
                        enumStr = input.Substring(i, (andIndex < 0 ? input.Length : andIndex) - i).Trim();
                    }
                    else
                    {
                        enumStr = input.Substring(
                                            i,
                                            (andIndex < 0 ? orIndex : Mathf.Min(andIndex, orIndex)) - i
                                        )
                                       .Trim();
                    }

                    int value;
                    if (!int.TryParse(enumStr, out value))
                    {
                        try
                        {
                            // Case-insensitive enum parsing
                            value = Convert.ToInt32(Enum.Parse(enumType, enumStr, true));
                        }
                        catch
                        {
                            output = null;
                            return false;
                        }
                    }

                    if (operation == NONE)
                    {
                        outputInt = value;
                    }
                    else if (operation == OR)
                    {
                        outputInt |= value;
                    }
                    else
                    {
                        outputInt &= value;
                    }

                    if (orIndex >= 0)
                    {
                        if (andIndex > orIndex)
                        {
                            operation = AND;
                            i = andIndex;
                        }
                        else
                        {
                            operation = OR;
                            i = orIndex;
                        }
                    }
                    else if (andIndex >= 0)
                    {
                        operation = AND;
                        i = andIndex;
                    }
                    else
                    {
                        i = input.Length;
                    }
                }

                output = Enum.ToObject(enumType, outputInt);
                return true;
            }
        }

        public static bool ParseFloat(string input, out object output)
        {
            using (_PRF_ParseFloat.Auto())
            {
                float value;
                var result = float.TryParse(
                    !input.EndsWith("f", StringComparison.OrdinalIgnoreCase)
                        ? input
                        : input.Substring(0, input.Length - 1),
                    out value
                );

                output = value;
                return result;
            }
        }

        public static bool ParseGameObject(string input, out object output)
        {
            using (_PRF_ParseGameObject.Auto())
            {
                output = input == "null" ? null : GameObject.Find(input);
                return true;
            }
        }

        public static bool ParseInt(string input, out object output)
        {
            using (_PRF_ParseInt.Auto())
            {
                int value;
                var result = int.TryParse(input, out value);

                output = value;
                return result;
            }
        }

        public static bool ParseLong(string input, out object output)
        {
            using (_PRF_ParseLong.Auto())
            {
                long value;
                var result = long.TryParse(
                    !input.EndsWith("L", StringComparison.OrdinalIgnoreCase)
                        ? input
                        : input.Substring(0, input.Length - 1),
                    out value
                );

                output = value;
                return result;
            }
        }

        public static bool ParseQuaternion(string input, out object output)
        {
            using (_PRF_ParseQuaternion.Auto())
            {
                return ParseVector(input, typeof(Quaternion), out output);
            }
        }

        public static bool ParseRect(string input, out object output)
        {
            using (_PRF_ParseRect.Auto())
            {
                return ParseVector(input, typeof(Rect), out output);
            }
        }

        public static bool ParseRectInt(string input, out object output)
        {
            using (_PRF_ParseRectInt.Auto())
            {
                return ParseVector(input, typeof(RectInt), out output);
            }
        }

        public static bool ParseRectOffset(string input, out object output)
        {
            using (_PRF_ParseRectOffset.Auto())
            {
                return ParseVector(input, typeof(RectOffset), out output);
            }
        }

        public static bool ParseSByte(string input, out object output)
        {
            using (_PRF_ParseSByte.Auto())
            {
                sbyte value;
                var result = sbyte.TryParse(input, out value);

                output = value;
                return result;
            }
        }

        public static bool ParseShort(string input, out object output)
        {
            using (_PRF_ParseShort.Auto())
            {
                short value;
                var result = short.TryParse(input, out value);

                output = value;
                return result;
            }
        }

        public static bool ParseString(string input, out object output)
        {
            using (_PRF_ParseString.Auto())
            {
                output = input;
                return true;
            }
        }

        public static bool ParseUInt(string input, out object output)
        {
            using (_PRF_ParseUInt.Auto())
            {
                uint value;
                var result = uint.TryParse(input, out value);

                output = value;
                return result;
            }
        }

        public static bool ParseULong(string input, out object output)
        {
            using (_PRF_ParseULong.Auto())
            {
                ulong value;
                var result = ulong.TryParse(
                    !input.EndsWith("L", StringComparison.OrdinalIgnoreCase)
                        ? input
                        : input.Substring(0, input.Length - 1),
                    out value
                );

                output = value;
                return result;
            }
        }

        public static bool ParseUShort(string input, out object output)
        {
            using (_PRF_ParseUShort.Auto())
            {
                ushort value;
                var result = ushort.TryParse(input, out value);

                output = value;
                return result;
            }
        }

        public static bool ParseVector2(string input, out object output)
        {
            using (_PRF_ParseVector2.Auto())
            {
                return ParseVector(input, typeof(Vector2), out output);
            }
        }

        public static bool ParseVector2Int(string input, out object output)
        {
            using (_PRF_ParseVector2Int.Auto())
            {
                return ParseVector(input, typeof(Vector2Int), out output);
            }
        }

        public static bool ParseVector3(string input, out object output)
        {
            using (_PRF_ParseVector3.Auto())
            {
                return ParseVector(input, typeof(Vector3), out output);
            }
        }

        public static bool ParseVector3Int(string input, out object output)
        {
            using (_PRF_ParseVector3Int.Auto())
            {
                return ParseVector(input, typeof(Vector3Int), out output);
            }
        }

        public static bool ParseVector4(string input, out object output)
        {
            using (_PRF_ParseVector4.Auto())
            {
                return ParseVector(input, typeof(Vector4), out output);
            }
        }

        // Remove all commands with the matching command name from the console
        public static void RemoveCommand(string command)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                if (!string.IsNullOrEmpty(command))
                {
                    for (var i = methods.Count - 1; i >= 0; i--)
                    {
                        if (caseInsensitiveComparer.Compare(
                                methods[i].command,
                                command,
                                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                            ) ==
                            0)
                        {
                            methods.RemoveAt(i);
                        }
                    }
                }
            }
        }

        // Remove all commands with the matching method from the console
        public static void RemoveCommand(Action method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                RemoveCommand(method.Method);
            }
        }

        public static void RemoveCommand<T1>(Action<T1> method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                RemoveCommand(method.Method);
            }
        }

        public static void RemoveCommand<T1>(Func<T1> method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                RemoveCommand(method.Method);
            }
        }

        public static void RemoveCommand<T1, T2>(Action<T1, T2> method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                RemoveCommand(method.Method);
            }
        }

        public static void RemoveCommand<T1, T2>(Func<T1, T2> method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                RemoveCommand(method.Method);
            }
        }

        public static void RemoveCommand<T1, T2, T3>(Action<T1, T2, T3> method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                RemoveCommand(method.Method);
            }
        }

        public static void RemoveCommand<T1, T2, T3>(Func<T1, T2, T3> method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                RemoveCommand(method.Method);
            }
        }

        public static void RemoveCommand<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                RemoveCommand(method.Method);
            }
        }

        public static void RemoveCommand<T1, T2, T3, T4>(Func<T1, T2, T3, T4> method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                RemoveCommand(method.Method);
            }
        }

        public static void RemoveCommand<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                RemoveCommand(method.Method);
            }
        }

        public static void RemoveCommand(Delegate method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                RemoveCommand(method.Method);
            }
        }

        public static void RemoveCommand(MethodInfo method)
        {
            using (_PRF_RemoveCommand.Auto())
            {
                if (method != null)
                {
                    for (var i = methods.Count - 1; i >= 0; i--)
                    {
                        if (methods[i].method == method)
                        {
                            methods.RemoveAt(i);
                        }
                    }
                }
            }
        }

        // Remove a custom Type from the list of recognized command parameter Types
        public static void RemoveCustomParameterType(Type type)
        {
            using (_PRF_RemoveCustomParameterType.Auto())
            {
                parseFunctions.Remove(type);
                typeReadableNames.Remove(type);
            }
        }

        // Finds all commands that have a matching signature with command
        // - caretIndexIncrements: indices inside "string command" that separate two arguments in the command. This is used to
        //   figure out which argument the caret is standing on
        // - commandName: command's name (first argument)
        internal static void GetCommandSuggestions(
            string command,
            List<DeveloperConsoleMethodInfo> matchingCommands,
            List<int> caretIndexIncrements,
            ref string commandName,
            out int numberOfParameters)
        {
            using (_PRF_GetCommandSuggestions.Auto())
            {
                var commandNameCalculated = false;
                var commandNameFullyTyped = false;
                numberOfParameters = -1;
                for (var i = 0; i < command.Length; i++)
                {
                    if (char.IsWhiteSpace(command[i]))
                    {
                        continue;
                    }

                    var delimiterIndex = IndexOfDelimiterGroup(command[i]);
                    if (delimiterIndex >= 0)
                    {
                        var endIndex = IndexOfDelimiterGroupEnd(command, delimiterIndex, i + 1);
                        if (!commandNameCalculated)
                        {
                            commandNameCalculated = true;
                            commandNameFullyTyped = command.Length > endIndex;

                            var commandNameLength = endIndex - i - 1;
                            if ((commandName == null) ||
                                (commandNameLength == 0) ||
                                (commandName.Length != commandNameLength) ||
                                (caseInsensitiveComparer.IndexOf(
                                     command,
                                     commandName,
                                     i + 1,
                                     commandNameLength,
                                     CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                                 ) !=
                                 (i + 1)))
                            {
                                commandName = command.Substring(i + 1, commandNameLength);
                            }
                        }

                        i = (endIndex < (command.Length - 1)) && (command[endIndex + 1] == ',')
                            ? endIndex + 1
                            : endIndex;
                        caretIndexIncrements.Add(i + 1);
                    }
                    else
                    {
                        var endIndex = IndexOfChar(command, ' ', i + 1);
                        if (!commandNameCalculated)
                        {
                            commandNameCalculated = true;
                            commandNameFullyTyped = command.Length > endIndex;

                            var commandNameLength = command[endIndex - 1] == ','
                                ? endIndex - 1 - i
                                : endIndex - i;
                            if ((commandName == null) ||
                                (commandNameLength == 0) ||
                                (commandName.Length != commandNameLength) ||
                                (caseInsensitiveComparer.IndexOf(
                                     command,
                                     commandName,
                                     i,
                                     commandNameLength,
                                     CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                                 ) !=
                                 i))
                            {
                                commandName = command.Substring(i, commandNameLength);
                            }
                        }

                        i = endIndex;
                        caretIndexIncrements.Add(i);
                    }

                    numberOfParameters++;
                }

                if (!commandNameCalculated)
                {
                    commandName = string.Empty;
                }

                if (!string.IsNullOrEmpty(commandName))
                {
                    var commandIndex = FindCommandIndex(commandName);
                    if (commandIndex < 0)
                    {
                        commandIndex = ~commandIndex;
                    }

                    var commandLastIndex = commandIndex;
                    if (!commandNameFullyTyped)
                    {
                        // Match all commands that start with commandName
                        if ((commandIndex < methods.Count) &&
                            caseInsensitiveComparer.IsPrefix(
                                methods[commandIndex].command,
                                commandName,
                                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                            ))
                        {
                            while ((commandIndex > 0) &&
                                   caseInsensitiveComparer.IsPrefix(
                                       methods[commandIndex - 1].command,
                                       commandName,
                                       CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                                   ))
                            {
                                commandIndex--;
                            }

                            while ((commandLastIndex < (methods.Count - 1)) &&
                                   caseInsensitiveComparer.IsPrefix(
                                       methods[commandLastIndex + 1].command,
                                       commandName,
                                       CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                                   ))
                            {
                                commandLastIndex++;
                            }
                        }
                        else
                        {
                            commandLastIndex = -1;
                        }
                    }
                    else
                    {
                        // Match only the commands that are equal to commandName
                        if ((commandIndex < methods.Count) &&
                            (caseInsensitiveComparer.Compare(
                                 methods[commandIndex].command,
                                 commandName,
                                 CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                             ) ==
                             0))
                        {
                            while ((commandIndex > 0) &&
                                   (caseInsensitiveComparer.Compare(
                                        methods[commandIndex - 1].command,
                                        commandName,
                                        CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                                    ) ==
                                    0))
                            {
                                commandIndex--;
                            }

                            while ((commandLastIndex < (methods.Count - 1)) &&
                                   (caseInsensitiveComparer.Compare(
                                        methods[commandLastIndex + 1].command,
                                        commandName,
                                        CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                                    ) ==
                                    0))
                            {
                                commandLastIndex++;
                            }
                        }
                        else
                        {
                            commandLastIndex = -1;
                        }
                    }

                    for (; commandIndex <= commandLastIndex; commandIndex++)
                    {
                        if (methods[commandIndex].parameterTypes.Length >= numberOfParameters)
                        {
                            matchingCommands.Add(methods[commandIndex]);
                        }
                    }
                }
            }
        }

        // Create a new command and set its properties
        private static void AddCommand(
            string command,
            string description,
            string methodName,
            Type ownerType,
            object instance,
            string[] parameterNames)
        {
            using (_PRF_AddCommand.Auto())
            {
                // Get the method from the class
                var method = ownerType.GetMethod(
                    methodName,
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    (instance != null ? BindingFlags.Instance : BindingFlags.Static)
                );
                if (method == null)
                {
                    Context.Log.Error(methodName + " does not exist in " + ownerType);
                    return;
                }

                AddCommand(command, description, method, instance, parameterNames);
            }
        }

        private static void AddCommand(
            string command,
            string description,
            MethodInfo method,
            object instance,
            string[] parameterNames)
        {
            using (_PRF_AddCommand.Auto())
            {
                if (string.IsNullOrEmpty(command))
                {
                    Context.Log.Error("Command name can't be empty!");
                    return;
                }

                command = command.Trim();
                if (command.IndexOf(' ') >= 0)
                {
                    Context.Log.Error("Command name can't contain whitespace: " + command);
                    return;
                }

                // Fetch the parameters of the class
                var parameters = method.GetParameters();

                // Store the parameter types in an array
                var parameterTypes = new Type[parameters.Length];
                for (var i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i].ParameterType.IsByRef)
                    {
                        Context.Log.Error("Command can't have 'out' or 'ref' parameters");
                        return;
                    }

                    var parameterType = parameters[i].ParameterType;
                    if (parseFunctions.ContainsKey(parameterType) ||
                        typeof(Component).IsAssignableFrom(parameterType) ||
                        parameterType.IsEnum ||
                        IsSupportedArrayType(parameterType))
                    {
                        parameterTypes[i] = parameterType;
                    }
                    else
                    {
                        Context.Log.Error(
                            string.Concat(
                                "Parameter ",
                                parameters[i].Name,
                                "'s Type ",
                                parameterType,
                                " isn't supported"
                            )
                        );
                        return;
                    }
                }

                var commandIndex = FindCommandIndex(command);
                if (commandIndex < 0)
                {
                    commandIndex = ~commandIndex;
                }
                else
                {
                    var commandFirstIndex = commandIndex;
                    var commandLastIndex = commandIndex;

                    while ((commandFirstIndex > 0) &&
                           (caseInsensitiveComparer.Compare(
                                methods[commandFirstIndex - 1].command,
                                command,
                                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                            ) ==
                            0))
                    {
                        commandFirstIndex--;
                    }

                    while ((commandLastIndex < (methods.Count - 1)) &&
                           (caseInsensitiveComparer.Compare(
                                methods[commandLastIndex + 1].command,
                                command,
                                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                            ) ==
                            0))
                    {
                        commandLastIndex++;
                    }

                    commandIndex = commandFirstIndex;
                    for (var i = commandFirstIndex; i <= commandLastIndex; i++)
                    {
                        var parameterCountDiff = methods[i].parameterTypes.Length - parameterTypes.Length;
                        if (parameterCountDiff <= 0)
                        {
                            // We are sorting the commands in 2 steps:
                            // 1: Sorting by their 'command' names which is handled by FindCommandIndex
                            // 2: Sorting by their parameter counts which is handled here (parameterCountDiff <= 0)
                            commandIndex = i + 1;

                            // Check if this command has been registered before and if it is, overwrite that command
                            if (parameterCountDiff == 0)
                            {
                                var j = 0;
                                while ((j < parameterTypes.Length) &&
                                       (parameterTypes[j] == methods[i].parameterTypes[j]))
                                {
                                    j++;
                                }

                                if (j >= parameterTypes.Length)
                                {
                                    commandIndex = i;
                                    commandLastIndex--;
                                    methods.RemoveAt(i--);
                                }
                            }
                        }
                    }
                }

                // Create the command
                var methodSignature = new StringBuilder(256);
                var parameterSignatures = new string[parameterTypes.Length];

#if USE_BOLD_COMMAND_SIGNATURES
                methodSignature.Append("<b>");
#endif
                methodSignature.Append(command);

                if (parameterTypes.Length > 0)
                {
                    methodSignature.Append(" ");

                    for (var i = 0; i < parameterTypes.Length; i++)
                    {
                        var parameterSignatureStartIndex = methodSignature.Length;

                        methodSignature.Append("[")
                                       .Append(GetTypeReadableName(parameterTypes[i]))
                                       .Append(" ")
                                       .Append(
                                            (parameterNames != null) &&
                                            (i < parameterNames.Length) &&
                                            !string.IsNullOrEmpty(parameterNames[i])
                                                ? parameterNames[i]
                                                : parameters[i].Name
                                        )
                                       .Append("]");

                        if (i < (parameterTypes.Length - 1))
                        {
                            methodSignature.Append(" ");
                        }

                        parameterSignatures[i] = methodSignature.ToString(
                            parameterSignatureStartIndex,
                            methodSignature.Length - parameterSignatureStartIndex
                        );
                    }
                }

#if USE_BOLD_COMMAND_SIGNATURES
                methodSignature.Append("</b>");
#endif

                if (!string.IsNullOrEmpty(description))
                {
                    methodSignature.Append(": ").Append(description);
                }

                methods.Insert(
                    commandIndex,
                    new DeveloperConsoleMethodInfo(
                        method,
                        parameterTypes,
                        instance,
                        command,
                        methodSignature.ToString(),
                        parameterSignatures
                    )
                );
            }
        }

        private static StringBuilder AppendSysInfoIfPresent(
            this StringBuilder sb,
            string info,
            string postfix = null)
        {
            using (_PRF_AppendSysInfoIfPresent.Auto())
            {
                if (info != SystemInfo.unsupportedIdentifier)
                {
                    sb.Append(info);

                    if (postfix != null)
                    {
                        sb.Append(postfix);
                    }

                    sb.Append(" ");
                }

                return sb;
            }
        }

        private static StringBuilder AppendSysInfoIfPresent(
            this StringBuilder sb,
            int info,
            string postfix = null)
        {
            using (_PRF_AppendSysInfoIfPresent.Auto())
            {
                if (info > 0)
                {
                    sb.Append(info);

                    if (postfix != null)
                    {
                        sb.Append(postfix);
                    }

                    sb.Append(" ");
                }

                return sb;
            }
        }

        // Find command's index in the list of registered commands using binary search
        private static int FindCommandIndex(string command)
        {
            using (_PRF_FindCommandIndex.Auto())
            {
                var min = 0;
                var max = methods.Count - 1;
                while (min <= max)
                {
                    var mid = (min + max) / 2;
                    var comparison = caseInsensitiveComparer.Compare(
                        command,
                        methods[mid].command,
                        CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace
                    );
                    if (comparison == 0)
                    {
                        return mid;
                    }

                    if (comparison < 0)
                    {
                        max = mid - 1;
                    }
                    else
                    {
                        min = mid + 1;
                    }
                }

                return ~min;
            }
        }

        // Find the index of char in the string, or return the length of string instead of -1
        private static int IndexOfChar(string command, char c, int startIndex)
        {
            using (_PRF_IndexOfChar.Auto())
            {
                var result = command.IndexOf(c, startIndex);
                if (result < 0)
                {
                    result = command.Length;
                }

                return result;
            }
        }

        // Find the index of the delimiter group that 'c' belongs to
        private static int IndexOfDelimiterGroup(char c)
        {
            using (_PRF_IndexOfDelimiterGroup.Auto())
            {
                for (var i = 0; i < inputDelimiters.Length; i++)
                {
                    if (c == inputDelimiters[i][0])
                    {
                        return i;
                    }
                }

                return -1;
            }
        }

        private static int IndexOfDelimiterGroupEnd(string command, int delimiterIndex, int startIndex)
        {
            using (_PRF_IndexOfDelimiterGroupEnd.Auto())
            {
                var startChar = inputDelimiters[delimiterIndex][0];
                var endChar = inputDelimiters[delimiterIndex][1];

                // Check delimiter's depth for array support (e.g. [[1 2] [3 4]] for Vector2 array)
                var depth = 1;

                for (var i = startIndex; i < command.Length; i++)
                {
                    var c = command[i];
                    if ((c == endChar) && (--depth <= 0))
                    {
                        return i;
                    }

                    if (c == startChar)
                    {
                        depth++;
                    }
                }

                return command.Length;
            }
        }

        // Create a vector of specified type (fill the blank slots with 0 or ignore unnecessary slots)
        private static bool ParseVector(string input, Type vectorType, out object output)
        {
            using (_PRF_ParseVector.Auto())
            {
                var tokens = new List<string>(input.Replace(',', ' ').Trim().Split(' '));
                for (var i = tokens.Count - 1; i >= 0; i--)
                {
                    tokens[i] = tokens[i].Trim();
                    if (tokens[i].Length == 0)
                    {
                        tokens.RemoveAt(i);
                    }
                }

                var tokenValues = new float[tokens.Count];
                for (var i = 0; i < tokens.Count; i++)
                {
                    object val;
                    if (!ParseFloat(tokens[i], out val))
                    {
                        if (vectorType == typeof(Vector3))
                        {
                            output = Vector3.zero;
                        }
                        else if (vectorType == typeof(Vector2))
                        {
                            output = Vector2.zero;
                        }
                        else
                        {
                            output = Vector4.zero;
                        }

                        return false;
                    }

                    tokenValues[i] = (float)val;
                }

                if (vectorType == typeof(Vector3))
                {
                    var result = Vector3.zero;

                    for (var i = 0; (i < tokenValues.Length) && (i < 3); i++)
                    {
                        result[i] = tokenValues[i];
                    }

                    output = result;
                }
                else if (vectorType == typeof(Vector2))
                {
                    var result = Vector2.zero;

                    for (var i = 0; (i < tokenValues.Length) && (i < 2); i++)
                    {
                        result[i] = tokenValues[i];
                    }

                    output = result;
                }
                else if (vectorType == typeof(Vector4))
                {
                    var result = Vector4.zero;

                    for (var i = 0; (i < tokenValues.Length) && (i < 4); i++)
                    {
                        result[i] = tokenValues[i];
                    }

                    output = result;
                }
                else if (vectorType == typeof(Quaternion))
                {
                    var result = Quaternion.identity;

                    for (var i = 0; (i < tokenValues.Length) && (i < 4); i++)
                    {
                        result[i] = tokenValues[i];
                    }

                    output = result;
                }
                else if (vectorType == typeof(Color))
                {
                    var result = Color.black;

                    for (var i = 0; (i < tokenValues.Length) && (i < 4); i++)
                    {
                        result[i] = tokenValues[i];
                    }

                    output = result;
                }
                else if (vectorType == typeof(Color32))
                {
                    var result = new Color32(0, 0, 0, 255);

                    if (tokenValues.Length > 0)
                    {
                        result.r = (byte)Mathf.RoundToInt(tokenValues[0]);
                    }

                    if (tokenValues.Length > 1)
                    {
                        result.g = (byte)Mathf.RoundToInt(tokenValues[1]);
                    }

                    if (tokenValues.Length > 2)
                    {
                        result.b = (byte)Mathf.RoundToInt(tokenValues[2]);
                    }

                    if (tokenValues.Length > 3)
                    {
                        result.a = (byte)Mathf.RoundToInt(tokenValues[3]);
                    }

                    output = result;
                }
                else if (vectorType == typeof(Rect))
                {
                    var result = Rect.zero;

                    if (tokenValues.Length > 0)
                    {
                        result.x = tokenValues[0];
                    }

                    if (tokenValues.Length > 1)
                    {
                        result.y = tokenValues[1];
                    }

                    if (tokenValues.Length > 2)
                    {
                        result.width = tokenValues[2];
                    }

                    if (tokenValues.Length > 3)
                    {
                        result.height = tokenValues[3];
                    }

                    output = result;
                }
                else if (vectorType == typeof(RectOffset))
                {
                    var result = new RectOffset();

                    if (tokenValues.Length > 0)
                    {
                        result.left = Mathf.RoundToInt(tokenValues[0]);
                    }

                    if (tokenValues.Length > 1)
                    {
                        result.right = Mathf.RoundToInt(tokenValues[1]);
                    }

                    if (tokenValues.Length > 2)
                    {
                        result.top = Mathf.RoundToInt(tokenValues[2]);
                    }

                    if (tokenValues.Length > 3)
                    {
                        result.bottom = Mathf.RoundToInt(tokenValues[3]);
                    }

                    output = result;
                }
                else if (vectorType == typeof(Bounds))
                {
                    var center = Vector3.zero;
                    for (var i = 0; (i < tokenValues.Length) && (i < 3); i++)
                    {
                        center[i] = tokenValues[i];
                    }

                    var size = Vector3.zero;
                    for (var i = 3; (i < tokenValues.Length) && (i < 6); i++)
                    {
                        size[i - 3] = tokenValues[i];
                    }

                    output = new Bounds(center, size);
                }
#if UNITY_2017_2_OR_NEWER
                else if (vectorType == typeof(Vector3Int))
                {
                    var result = Vector3Int.zero;

                    for (var i = 0; (i < tokenValues.Length) && (i < 3); i++)
                    {
                        result[i] = Mathf.RoundToInt(tokenValues[i]);
                    }

                    output = result;
                }
                else if (vectorType == typeof(Vector2Int))
                {
                    var result = Vector2Int.zero;

                    for (var i = 0; (i < tokenValues.Length) && (i < 2); i++)
                    {
                        result[i] = Mathf.RoundToInt(tokenValues[i]);
                    }

                    output = result;
                }
                else if (vectorType == typeof(RectInt))
                {
                    var result = new RectInt();

                    if (tokenValues.Length > 0)
                    {
                        result.x = Mathf.RoundToInt(tokenValues[0]);
                    }

                    if (tokenValues.Length > 1)
                    {
                        result.y = Mathf.RoundToInt(tokenValues[1]);
                    }

                    if (tokenValues.Length > 2)
                    {
                        result.width = Mathf.RoundToInt(tokenValues[2]);
                    }

                    if (tokenValues.Length > 3)
                    {
                        result.height = Mathf.RoundToInt(tokenValues[3]);
                    }

                    output = result;
                }
                else if (vectorType == typeof(BoundsInt))
                {
                    var center = Vector3Int.zero;
                    for (var i = 0; (i < tokenValues.Length) && (i < 3); i++)
                    {
                        center[i] = Mathf.RoundToInt(tokenValues[i]);
                    }

                    var size = Vector3Int.zero;
                    for (var i = 3; (i < tokenValues.Length) && (i < 6); i++)
                    {
                        size[i - 3] = Mathf.RoundToInt(tokenValues[i]);
                    }

                    output = new BoundsInt(center, size);
                }
#endif
                else
                {
                    output = null;
                    return false;
                }

                return true;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DeveloperConsole) + ".";

        private static readonly ProfilerMarker _PRF_Context = new ProfilerMarker(_PRF_PFX + nameof(Context));

        private static readonly ProfilerMarker _PRF_AddCommandInstance =
            new ProfilerMarker(_PRF_PFX + nameof(AddCommandInstance));

        private static readonly ProfilerMarker _PRF_AddCommandStatic =
            new ProfilerMarker(_PRF_PFX + nameof(AddCommandStatic));

        private static readonly ProfilerMarker _PRF_AddCustomParameterType =
            new ProfilerMarker(_PRF_PFX + nameof(AddCustomParameterType));

        private static readonly ProfilerMarker _PRF_ExecuteCommand =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteCommand));

        private static readonly ProfilerMarker _PRF_FetchArgumentsFromCommand =
            new ProfilerMarker(_PRF_PFX + nameof(FetchArgumentsFromCommand));

        private static readonly ProfilerMarker _PRF_GetAutoCompleteCommand =
            new ProfilerMarker(_PRF_PFX + nameof(GetAutoCompleteCommand));

        private static readonly ProfilerMarker _PRF_GetTypeReadableName =
            new ProfilerMarker(_PRF_PFX + nameof(GetTypeReadableName));

        private static readonly ProfilerMarker _PRF_IsSupportedArrayType =
            new ProfilerMarker(_PRF_PFX + nameof(IsSupportedArrayType));

        private static readonly ProfilerMarker _PRF_LogAllCommands =
            new ProfilerMarker(_PRF_PFX + nameof(LogAllCommands));

        private static readonly ProfilerMarker _PRF_LogAllCommandsWithName =
            new ProfilerMarker(_PRF_PFX + nameof(LogAllCommandsWithName));

        private static readonly ProfilerMarker _PRF_LogSystemInfo =
            new ProfilerMarker(_PRF_PFX + nameof(LogSystemInfo));

        private static readonly ProfilerMarker _PRF_GetCommandSuggestions =
            new ProfilerMarker(_PRF_PFX + nameof(GetCommandSuggestions));

        private static readonly ProfilerMarker _PRF_RemoveCustomParameterType =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveCustomParameterType));

        private static readonly ProfilerMarker _PRF_RemoveCommand =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveCommand));

        private static readonly ProfilerMarker _PRF_AddCommand =
            new ProfilerMarker(_PRF_PFX + nameof(AddCommand));

        private static readonly ProfilerMarker _PRF_AppendSysInfoIfPresent =
            new ProfilerMarker(_PRF_PFX + nameof(AppendSysInfoIfPresent));

        private static readonly ProfilerMarker _PRF_FindCommandIndex =
            new ProfilerMarker(_PRF_PFX + nameof(FindCommandIndex));

        private static readonly ProfilerMarker _PRF_FindCommands =
            new ProfilerMarker(_PRF_PFX + nameof(FindCommands));

        private static readonly ProfilerMarker _PRF_IndexOfDelimiterGroup =
            new ProfilerMarker(_PRF_PFX + nameof(IndexOfDelimiterGroup));

        private static readonly ProfilerMarker _PRF_IndexOfChar =
            new ProfilerMarker(_PRF_PFX + nameof(IndexOfChar));

        private static readonly ProfilerMarker _PRF_IndexOfDelimiterGroupEnd =
            new ProfilerMarker(_PRF_PFX + nameof(IndexOfDelimiterGroupEnd));

        private static readonly ProfilerMarker _PRF_ParseArgument =
            new ProfilerMarker(_PRF_PFX + nameof(ParseArgument));

        private static readonly ProfilerMarker _PRF_ParseArray =
            new ProfilerMarker(_PRF_PFX + nameof(ParseArray));

        private static readonly ProfilerMarker _PRF_ParseBool =
            new ProfilerMarker(_PRF_PFX + nameof(ParseBool));

        private static readonly ProfilerMarker _PRF_ParseBounds =
            new ProfilerMarker(_PRF_PFX + nameof(ParseBounds));

        private static readonly ProfilerMarker _PRF_ParseByte =
            new ProfilerMarker(_PRF_PFX + nameof(ParseByte));

        private static readonly ProfilerMarker _PRF_ParseChar =
            new ProfilerMarker(_PRF_PFX + nameof(ParseChar));

        private static readonly ProfilerMarker _PRF_ParseColor =
            new ProfilerMarker(_PRF_PFX + nameof(ParseColor));

        private static readonly ProfilerMarker _PRF_ParseColor32 =
            new ProfilerMarker(_PRF_PFX + nameof(ParseColor32));

        private static readonly ProfilerMarker _PRF_ParseComponent =
            new ProfilerMarker(_PRF_PFX + nameof(ParseComponent));

        private static readonly ProfilerMarker _PRF_ParseDecimal =
            new ProfilerMarker(_PRF_PFX + nameof(ParseDecimal));

        private static readonly ProfilerMarker _PRF_ParseDouble =
            new ProfilerMarker(_PRF_PFX + nameof(ParseDouble));

        private static readonly ProfilerMarker _PRF_ParseEnum =
            new ProfilerMarker(_PRF_PFX + nameof(ParseEnum));

        private static readonly ProfilerMarker _PRF_ParseFloat =
            new ProfilerMarker(_PRF_PFX + nameof(ParseFloat));

        private static readonly ProfilerMarker _PRF_ParseFunction =
            new ProfilerMarker(_PRF_PFX + nameof(ParseFunction));

        private static readonly ProfilerMarker _PRF_ParseGameObject =
            new ProfilerMarker(_PRF_PFX + nameof(ParseGameObject));

        private static readonly ProfilerMarker _PRF_ParseInt =
            new ProfilerMarker(_PRF_PFX + nameof(ParseInt));

        private static readonly ProfilerMarker _PRF_ParseLong =
            new ProfilerMarker(_PRF_PFX + nameof(ParseLong));

        private static readonly ProfilerMarker _PRF_ParseQuaternion =
            new ProfilerMarker(_PRF_PFX + nameof(ParseQuaternion));

        private static readonly ProfilerMarker _PRF_ParseRect =
            new ProfilerMarker(_PRF_PFX + nameof(ParseRect));

        private static readonly ProfilerMarker _PRF_ParseRectInt =
            new ProfilerMarker(_PRF_PFX + nameof(ParseRectInt));

        private static readonly ProfilerMarker _PRF_ParseRectOffset =
            new ProfilerMarker(_PRF_PFX + nameof(ParseRectOffset));

        private static readonly ProfilerMarker _PRF_ParseSByte =
            new ProfilerMarker(_PRF_PFX + nameof(ParseSByte));

        private static readonly ProfilerMarker _PRF_ParseShort =
            new ProfilerMarker(_PRF_PFX + nameof(ParseShort));

        private static readonly ProfilerMarker _PRF_ParseString =
            new ProfilerMarker(_PRF_PFX + nameof(ParseString));

        private static readonly ProfilerMarker _PRF_ParseUInt =
            new ProfilerMarker(_PRF_PFX + nameof(ParseUInt));

        private static readonly ProfilerMarker _PRF_ParseULong =
            new ProfilerMarker(_PRF_PFX + nameof(ParseULong));

        private static readonly ProfilerMarker _PRF_ParseUShort =
            new ProfilerMarker(_PRF_PFX + nameof(ParseUShort));

        private static readonly ProfilerMarker _PRF_ParseVector =
            new ProfilerMarker(_PRF_PFX + nameof(ParseVector));

        private static readonly ProfilerMarker _PRF_ParseVector2 =
            new ProfilerMarker(_PRF_PFX + nameof(ParseVector2));

        private static readonly ProfilerMarker _PRF_ParseVector3 =
            new ProfilerMarker(_PRF_PFX + nameof(ParseVector3));

        private static readonly ProfilerMarker _PRF_ParseVector4 =
            new ProfilerMarker(_PRF_PFX + nameof(ParseVector4));

        private static readonly ProfilerMarker _PRF_ParseVector2Int =
            new ProfilerMarker(_PRF_PFX + nameof(ParseVector2Int));

        private static readonly ProfilerMarker _PRF_ParseVector3Int =
            new ProfilerMarker(_PRF_PFX + nameof(ParseVector3Int));

        private static readonly ProfilerMarker _PRF_ParseBoundsInt =
            new ProfilerMarker(_PRF_PFX + nameof(ParseBoundsInt));

        #endregion
    }
}
