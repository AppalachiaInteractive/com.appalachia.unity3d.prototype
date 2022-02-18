using System;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperConsole.Commands
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class DeveloperConsoleMethodAttribute : Attribute
    {
        public DeveloperConsoleMethodAttribute(
            string command,
            string description,
            params string[] parameterNames)
        {
            m_command = command;
            m_description = description;
            m_parameterNames = parameterNames;
        }

        #region Fields and Autoproperties

        private readonly string m_command;
        private readonly string m_description;
        private readonly string[] m_parameterNames;

        #endregion

        public string Command => m_command;
        public string Description => m_description;
        public string[] ParameterNames => m_parameterNames;
    }
}
