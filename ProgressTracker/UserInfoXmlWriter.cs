namespace ProgressTracker
{
    using System.Xml;
    using System.Xml.Serialization;
    using ProgressTracker.ViewModels;

    /// <summary>
    /// Provides method for UserInfo object serialization to xml format.
    /// </summary>
    public static class UserInfoXmlWriter
    {
        /// <summary>
        /// Wrutes UserInfo object to file in xml format.
        /// </summary>
        /// <param name="fileName">Name of destination file.</param>
        /// <param name="userInfo">User information to export.</param>
        public static void Write(string fileName, UserInfo userInfo)
        {
            if (userInfo is null)
            {
                return;
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            using XmlWriter writer = XmlWriter.Create(fileName, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("UserInfo");

            WriteElement("Name", userInfo.Name);
            WriteElement("AverageSteps", userInfo.AverageSteps.ToString());
            WriteElement("BestSteps", userInfo.BestSteps.ToString());
            WriteElement("WorstSteps", userInfo.WorstSteps.ToString());

            foreach (var info in userInfo.DailyInfo)
            {
                writer.WriteStartElement("DayInfo");
                WriteElement("day", info.Key.ToString());
                WriteElement("rank", info.Value.rank.ToString());
                WriteElement("status", info.Value.status.ToString());
                WriteElement("steps", info.Value.steps.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            void WriteElement(string name, string value)
            {
                writer.WriteStartElement(name);
                writer.WriteString(value);
                writer.WriteEndElement();
            }
        }
    }
}
