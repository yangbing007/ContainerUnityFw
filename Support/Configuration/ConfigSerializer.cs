﻿

using System;
using System.Configuration;
using System.IO;
using SysConfiguration = System.Configuration.Configuration;

namespace Microsoft.Practices.Unity.GuardSupport.Configuration
{
    public class ConfigSerializer
    {
        private readonly string filename;
        private readonly string configFileDir;

        public ConfigSerializer(string filename)
        {
            this.filename = filename;
            this.configFileDir = AppDomain.CurrentDomain.BaseDirectory;
        }

        public void Save(string sectionName, ConfigurationSection section)
        {
            DeleteFileIfExists();

            var filemap = new ExeConfigurationFileMap()
                              {
                                  ExeConfigFilename = filename
                              };
            SysConfiguration configuration = ConfigurationManager.OpenMappedExeConfiguration(filemap,
                                                                                             ConfigurationUserLevel.None);

            if (configuration.GetSection(sectionName) != null)
            {
                configuration.Sections.Remove(sectionName);
            }
            configuration.Sections.Add(sectionName, section);
            configuration.Save();
        }

        public SysConfiguration Load()
        {
            var filemap = new ExeConfigurationFileMap { ExeConfigFilename = filename };
            return ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
        }

        private void DeleteFileIfExists()
        {
            string fullName = Path.Combine(this.configFileDir, filename);
            File.Delete(fullName);
        }
    }
}
