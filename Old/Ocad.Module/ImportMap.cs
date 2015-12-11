using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.IO;

namespace Ocad.Module
{
    [Cmdlet(VerbsData.Import, "Map")]
    public class ImportMap : PSCmdlet
    {
        [Parameter(Position=0, Mandatory=true)]
        public String FilePath;

        protected override void ProcessRecord()
        {
            using (FileStream fileSteam = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                using (BufferedStream buffer = new BufferedStream(fileSteam))
                {
                    using (Ocad.IO.Ocad9.Reader reader = new Ocad.IO.Ocad9.Reader(buffer))
                    {
                        WriteObject(reader.ReadContent());
                    }
                }
            }
        }
    }
}
