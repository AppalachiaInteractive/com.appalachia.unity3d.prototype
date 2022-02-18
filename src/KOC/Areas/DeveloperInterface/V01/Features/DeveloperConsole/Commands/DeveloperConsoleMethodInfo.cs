using System;
using System.Reflection;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperConsole.Commands
{
    [NonSerializable]
    public class DeveloperConsoleMethodInfo : AppalachiaSimpleBase
    {
        public DeveloperConsoleMethodInfo(
            MethodInfo method,
            Type[] parameterTypes,
            object instance,
            string command,
            string signature,
            string[] parameters)
        {
            this.method = method;
            this.parameterTypes = parameterTypes;
            this.instance = instance;
            this.command = command;
            this.signature = signature;
            this.parameters = parameters;
        }

        #region Fields and Autoproperties

        public readonly MethodInfo method;
        public readonly object instance;
        public readonly string command;
        public readonly string signature;
        public readonly string[] parameters;
        public readonly Type[] parameterTypes;

        #endregion

        public bool IsValid()
        {
            if (!method.IsStatic && ((instance == null) || instance.Equals(null)))
            {
                return false;
            }

            return true;
        }
    }
}
