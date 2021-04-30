using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.IO.Compression;

namespace IronMountains2._2
{
    class Program
    {
        static string zip = "zip";
        static void Main(string[] args)
        {
            if (File.Exists("images.zip"))
            {
                if(Directory.Exists(zip))
                    Directory.Delete(zip, true);
                ZipFile.ExtractToDirectory("images.zip", Directory.GetCurrentDirectory() + "\\" + zip);
                if(File.Exists(zip+"\\Paths.meta"))
                {
                    string[] lines = File.ReadAllLines(zip + "\\Paths.meta");
                    SaveData(lines);
                }
                else
                {
                    Console.WriteLine("Incorrect zip file");
                    Directory.Delete(zip, true);
                }
            }
            else
            {
                Console.WriteLine("Please put the zip in the same folder as the exe");
            }

        }
        static public void SaveData(string[] data)
        {
            var dbCon = DBConnection.Instance();
            dbCon.Server = "localhost";
            dbCon.DatabaseName = "zipschema";
            dbCon.UserName = "root";
            dbCon.Password = "Qweasdzxc123Halo02";
            if (dbCon.IsConnect())
            {
                string separator = GetSeparator(data[0]);
                //suppose col0 and col1 are defined as VARCHAR in the DB
                foreach (var line in data)
                {
                    try
                    {                       
                        string query = "CALL new_procedure(" + line.Split(separator)[0].Substring(5) + ",\"" + line.Split(separator)[2] + "\")";
                        Console.WriteLine(line.Split(separator)[0].Substring(4) + "," + line.Split(separator)[2]);
                        var cmd = new MySqlCommand(query, dbCon.Connection);
                        var reader = cmd.ExecuteReader();
                        reader.Close();
                    }
                    catch(Exception ex)
                    {

                    }
                  
                }
                dbCon.Close();
            }
            else
            {
                
            }
        }
        static string GetSeparator(string line)
        {
            string separator = "";
            string str = line.Substring(10);
            foreach(var el in str.ToCharArray())
            {
                if (el == '2')
                    break;
                else
                    separator += el.ToString();

            }
            return separator;
        }
    }
}
