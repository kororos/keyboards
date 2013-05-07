using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNParser
{
    public class LKParser
    {
        //public FileStream inFile { get; set; }
        //public FileStream outFile { get; set; }

        private String inFileName;
        private String outFileName;
        private String inPath;
        private String outPath;
        private char[] inBuffer;

        private Dictionary<String, Int16> lettersDic;
        private Dictionary<String, Int16> combosDic;

        public LKParser(String inputFile, String outputFile)
        {
            inFileName = inputFile;
            outFileName = outputFile;
            lettersDic = new Dictionary<String, Int16>(StringComparer.OrdinalIgnoreCase);
            combosDic = new Dictionary<String, Int16>(StringComparer.OrdinalIgnoreCase);
        }

        public String getInPath()
        {
            return inPath;
        }

        //TODO: Currently not used
        private String getPath(FileStream fileStream)
        {
            return fileStream.Name;
        }

        public String getOutPath()
        {
            return outPath;
        }

        public async void readFileAsync()
        {
            FileStream inFS = null;
            try
            {
                inFS = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                using(StreamReader txtReader = new StreamReader(inFS))
                {
                    inBuffer = new char[txtReader.BaseStream.Length];
                    await txtReader.ReadAsync(inBuffer,0,(int)inBuffer.Length);
                }
                foreach(char c in inBuffer)
                {
                    if (lettersDic.ContainsKey(c.ToString()))
                        lettersDic[c.ToString()]++;
                    else
                        lettersDic[c.ToString()] = 1;
                }
            }
            finally
            {
                if (inFS != null)
                    inFS.Dispose();
            }
            //using(new FileStream(inFileName, FileMode.Open, FileAccess.Read),
              //                                 new FileStream(outFilename, FileMode.Create, FileAccess.Write)
        }
    }
}
