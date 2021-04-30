using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.IO.Compression;

namespace IronMountains2._2
{
    class Program
    {
        //Finall application for the IronMountain Test
        #region Fields
        static string Zip = "zip";
        #endregion
        #region Methods
        //Here we check if we have the image.zip file in the same folder as the executable
        //if we have it we check for the zip folder and delete it
        //unzip the images.zip file and reading the contents of Path.meta
        static void Main(string[] args)
        {
            if (File.Exists("images.zip"))
            {
                if (Directory.Exists(Zip))
                    Directory.Delete(Zip, true);
                ZipFile.ExtractToDirectory("images.zip", Directory.GetCurrentDirectory() + "\\" + Zip);
                if (File.Exists(Zip + "\\Paths.meta"))
                {
                    string[] lines = File.ReadAllLines(Zip + "\\Paths.meta");
                    SaveData(lines);
                }
                else
                {
                    Console.WriteLine("Incorrect zip file");
                    Directory.Delete(Zip, true);
                }
            }
            else
            {
                Console.WriteLine("Please put the zip in the same folder as the exe");
            }

        }
        //in this method we are accessing the database and for each element in the stirng array trying to insert it using the stored procedure 'new_procedure'
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
                foreach (var line in data)
                {
                    try
                    {            
                        //checking if the file exist in the correct Path
                        if(File.Exists(Directory.GetCurrentDirectory()+"\\"+Zip+"\\"+ line.Split(separator)[2].Split("\\")[0] + "\\" + line.Split(separator)[2].Split("\\")[1]))
                        {
                            //the sql command
                            string query = "CALL new_procedure(" + line.Split(separator)[0].Substring(5) + ",\'" + line.Split(separator)[2].Split("\\")[0]+"\\\\"+ line.Split(separator)[2].Split("\\")[1] + "\')";
                            Console.WriteLine(line.Split(separator)[0].Substring(4) + "," + line.Split(separator)[2]);
                            //executing the command
                            var cmd = new MySqlCommand(query, dbCon.Connection);
                            var reader = cmd.ExecuteReader();
                            reader.Close();
                        }
                        else
                        {
                            Console.WriteLine("The File doesnt exist in the folder");
                        }
                        //closing the reader
                    }
                    catch(Exception)
                    {
                        //we get Exception if Image already exist in dbb
                    }
                  
                }
                dbCon.Close();
            }
            else
            {
                
            }
        }
        //Here we are trying to identify the separator but this code would stop working if we change Date format in the other app or we get to the year 3000:))))))-so yeah not much time left:))
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
        #endregion
    }
}
