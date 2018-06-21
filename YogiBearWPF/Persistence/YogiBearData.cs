using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace YogiBearWPF.Persistence
{
    //Fájl betöltés típusa
    public class YogiBearData
    {
        //Pálya fájlból történő beolvasáa, majd továbbadása
        public List<List<Int32>> LoadFromFile()
        {
            try
            {
                //Pálya kiválasztása
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                //Project mappát kínáljuk fel alapértelmezettként
                openFileDialog1.InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Resources\\Map";
                openFileDialog1.Filter = "(*.map)|*.map";

                string selectedmap = ""; //kiválasztott pálya

                if (openFileDialog1.ShowDialog() == true)
                    selectedmap = openFileDialog1.FileName;
                else
                    return null;

                using (StreamReader reader = new StreamReader(selectedmap))
                {
                    String line = reader.ReadLine(); // Soronként olvasunk
                    Int32 size = Int32.Parse(line); // Fájl első paramétere a pályaméret
                    List<List<Int32>> map = new List<List<Int32>>(size); //pálya tárolására alkalmas konténer

                    for (Int32 i = 0; i < size; i++)
                    {
                        line = reader.ReadLine();
                        String[] numbers = line.Split(' ');
                        List<Int32> tmp = new List<Int32>(size);

                        for (Int32 j = 0; j < size; j++)
                            tmp.Add(Int32.Parse(numbers[j]));

                        map.Add(tmp);
                    }

                    return map;
                }
            }
            catch
            {
                throw new YogiBearDataException();
            }

        }

        public List<List<Int32>> LoadFirstLevel(String path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                String line = reader.ReadLine(); // Soronként olvasunk
                Int32 size = Int32.Parse(line); // Fájl első paramétere a pályaméret
                List<List<Int32>> map = new List<List<Int32>>(size); //pálya tárolására alkalmas konténer

                for (Int32 i = 0; i < size; i++)
                {
                    line = reader.ReadLine();
                    String[] numbers = line.Split(' ');
                    List<Int32> tmp = new List<Int32>(size);

                    for (Int32 j = 0; j < size; j++)
                        tmp.Add(Int32.Parse(numbers[j]));

                    map.Add(tmp);
                }

                return map;
            }
        }
    }
}
