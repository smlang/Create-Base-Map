using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.IO;

namespace Ocad.Module
{
    [Cmdlet(VerbsData.Export, "Map")]
    public class ExportMap : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public String FilePath;

        [Parameter(Position = 1, Mandatory = true)]
        public Ocad.Model.Map Map;

        protected override void ProcessRecord()
        {
            Map.FileName.Value = FilePath;
            using (FileStream fileSteam = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                using (BufferedStream buffer = new BufferedStream(fileSteam))
                {
                    using (Ocad.IO.Ocad9.Writer writer = new Ocad.IO.Ocad9.Writer(buffer))
                    {
                        writer.Write(Map);
                        writer.Flush();
                    }
                }
            }
        }
    }
}
