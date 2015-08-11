using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;

namespace SME.SMECollect
{
    public class SMECallStack
    {
        string m_method;
        string m_file;
        int m_line;

        #region Constructors
        public SMECallStack(string method, string file, int line)
        {
            m_method = method;
            m_file = file;
            m_line = line;
        }
        public SMECallStack(StackFrame stackframe)
        {
            MethodBase method = stackframe.GetMethod();
            Type type = method.DeclaringType;
            m_method = type.ToString();
            m_method += "." + method.Name + "(";
            ParameterInfo[] parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                m_method += parameters[i].ParameterType.Name + " ";
                m_method += parameters[i].Name;
                if (i < parameters.Length - 1)
                    m_method += ",";
            }
            m_method += ")";
            m_file = stackframe.GetFileName();
            m_line = stackframe.GetFileLineNumber();
        }
        public SMECallStack(XElement xelement)
        {
            LoadFromXElement(xelement);
        }
        #endregion

        #region Functions
        public XElement ToXElement()
        {
            XElement xmldoc = new XElement("Stack",
                                    new XElement("Method", m_method),
                                    new XElement("File", m_file),
                                    new XElement("Line", m_line.ToString()
                                  ));
            return xmldoc;
        }
        public override string ToString()
        {
            string temp = "CallStack";
            temp += ":Method:" + m_method;
            temp += ":FileName:" + m_file;
            temp += ":Line:" + m_line;
            return temp;
        }

        public void LoadFromXElement(XElement xelement)
        {
            if (xelement.Name.ToString().Equals("Stack"))
            {
                XElement el = (XElement)xelement.FirstNode;
                m_method = el.Value;
                el = (XElement)el.NextNode;
                m_file = el.Value;
                el = (XElement)el.NextNode;
                m_line = int.Parse(el.Value);
            }
            else
                throw new Exception("This XElement is not CallStack XElement");
        }
        #endregion

        #region Static Functions
        public static List<SMECallStack> ParseFromException(Exception exception)
        {
            List<SMECallStack> callstacklist = new List<SMECallStack>();
            string exceptionstack = exception.StackTrace;

            if (exceptionstack == null)
                return null;
            // stacktrace string split seperators
            string[] location_seperator = new string[] { " 위치: " };
            string[] file_seperater = new string[] { " 파일 ", ":줄 " };
            string[] locationarray = null;
            string[] filearray = null;
            // parse
            locationarray = exceptionstack.Split(location_seperator, StringSplitOptions.None);
            for (int i = 1; i < locationarray.Length; i++)
            {
                filearray = locationarray[i].Split(file_seperater, StringSplitOptions.None);
                //if(filearray.Length == 3)
                //    callstacklist.Add(new SMECallStack(filearray[0],
                //                                   filearray[1],
                //                                   int.Parse(filearray[2])));
                if (filearray.Length == 1)
                    callstacklist.Add(new SMECallStack(filearray[0], "library method", 0));
                else if (filearray.Length == 2)
                    callstacklist.Add(new SMECallStack(filearray[0],
                                                       filearray[1],
                                                       0));
                else if (filearray.Length == 3)
                    callstacklist.Add(new SMECallStack(filearray[0],
                                                       filearray[1],
                                                       int.Parse(filearray[2])));
            }
            return callstacklist;
        }
        #endregion
    }
}
