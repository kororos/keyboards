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

        private Dictionary<String, UInt16> lettersDic;
        private Dictionary<String, UInt16> combosDic;

        public LKParser(String inputFile, String outputFile)
        {
            inFileName = inputFile;
            outFileName = outputFile;
            inPath = Path.GetDirectoryName(inFileName);
            outPath = Path.GetDirectoryName(outFileName);
            lettersDic = new Dictionary<String, UInt16>(StringComparer.OrdinalIgnoreCase);
            combosDic = new Dictionary<String, UInt16>(StringComparer.OrdinalIgnoreCase);
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

        public async Task<Dictionary<string, UInt16>> readFileAsync()
        {
            FileStream inFS = null;
            try
            {
                inFS = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                using (StreamReader txtReader = new StreamReader(inFS))
                {
                    inBuffer = new char[txtReader.BaseStream.Length];
                    await txtReader.ReadAsync(inBuffer, 0, (int)inBuffer.Length);
                }
                foreach (char c in inBuffer)
                {
                    /*if (Char.IsControl(c) || Char.IsHighSurrogate(c) || Char.IsLowSurrogate(c) ||
                        Char.IsSeparator(c) || Char.IsSurrogate(c))
                    {
                        //do nothig here
                    }
                    else
                    {
                        if (lettersDic.ContainsKey(c.ToString()))
                            lettersDic[c.ToString()]++;
                        else
                            lettersDic[c.ToString()] = 1;
                    }*/

                    switch (c)
                    {
                        case '\n':
                        case '\t':
                        case '\f':
                        case '\r':
                        case '\v':
                        case ' ':
                            break;
                        default:

                            if (lettersDic.ContainsKey(c.ToString()))
                                lettersDic[c.ToString()]++;
                            else
                                lettersDic[c.ToString()] = 1;
                            break;
                    }
                }
            }
            finally
            {
                if (inFS != null)
                    inFS.Dispose();
            }

            string outputString = "";
            foreach (string s in lettersDic.Keys)
            {
                outputString += "Key: " + s + "\tTimes: " + lettersDic[s] + "\n";
            }

            //using(new FileStream(inFileName, FileMode.Open, FileAccess.Read),
            //                                 new FileStream(outFilename, FileMode.Create, FileAccess.Write)
            return lettersDic;
        }

        public Dictionary<string, UInt16> getLettersDic()
        {
            return lettersDic;
        }

        async public void writeFileAsync()
        {
            string outputString = "";

            List<KeyValuePair<string, UInt16>> sorted = (from kv in lettersDic orderby kv.Value select kv).ToList();

            foreach (string s in lettersDic.Keys)
            {
                outputString += "Key: " + s + "\tTimes: " + lettersDic[s] + "\n";
            }

            FileStream outFS = null;
            try
            {
                outFS = new FileStream(outFileName, FileMode.Create, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(outFS))
                {
                    await sw.WriteAsync(outputString);
                    sw.Flush();
                    sw.Close();
                }
            }
            finally
            {
                if (outFS != null)
                {
                    outFS.Dispose();
                }
            }
            //
        }
    }
}
