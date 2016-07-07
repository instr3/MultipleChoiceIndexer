using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.International.Converters.PinYinConverter;

namespace MultipleChoiceIndexer
{
    class Program
    {
        const string Matcher = @"^\s*(?<id>(\d)+)(?<dot>(\.|、|·|．|(\s*)))\s*(?<text>[^\n]+)$";
        static void Main(string[] args)
        {
             List<string> myl = new List<string>();
             Dictionary<string, int> myd = new Dictionary<string, int>();
             StreamWriter sw1 = new StreamWriter("out1.txt");
             StreamWriter sw2 = new StreamWriter("out2.txt");
             StreamReader sr = new StreamReader("in.txt");
            string str;
            while(!sr.EndOfStream)
            //for (int i = 1; i <= 100;++i )
            {
                str = sr.ReadLine();
                if (str == "") continue;
                Match match = Regex.Match(str, Matcher);
                if (match.Success)
                {
                    //Console.WriteLine(match.Groups["text"]);
                    string text = match.Groups["text"].ToString();
                    string pystr = "";
                    foreach (char c in text)
                    {
                        if (ChineseChar.IsValidChar(c))
                        {
                            ChineseChar cc = new ChineseChar(c);
                            pystr += cc.Pinyins[0] + c;
                        }
                    }
                    myl.Add(text);
                    if(!myd.ContainsKey(pystr))
                        myd.Add(pystr, myl.Count - 1);
                    else
                    {
                        myl[myd[pystr]] += "+" + (myl.Count - 1);
                    }
                    sw1.WriteLine("[" + (myl.Count - 1) + "]" + text);
                    //Console.WriteLine(pystr);
                }
                else
                {
                    sw1.WriteLine(str);
                }
            }
            var dicSort = from objDic in myd orderby objDic.Key ascending select objDic;
            char last = '0';
            foreach (var i in dicSort)
            {
                if(i.Key[0]!=last)
                {
                    last=i.Key[0];
                    sw2.WriteLine();
                    sw2.WriteLine(">>>>>>>>>>>>>>>>>> " + last + " <<<<<<<<<<<<<<<<<<");
                }
                sw2.WriteLine("[" + i.Value.ToString("d4") + "]" + myl[i.Value]);
            }
            sw1.Close();
            sw2.Close();
            Console.WriteLine("Done");
            /*
            Console.WriteLine(
                Microsoft.International.Converters.PinYinConverter.ChineseChar.IsValidChar('我')
            );
            Console.WriteLine(
                ChineseChar.IsValidChar('我')
            );*/
            Console.ReadKey();
        }
    }
}
