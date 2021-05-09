using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LPLManager.FileManagers
{
    public static class FileManager
    {
        public static List<string> GetPathWithParticularExtension(string ext)
        {
            try
            {
                return Directory.Exists("playlists") ? Directory.GetFiles("playlists", "*." + ext).ToList() : new List<string>();
            }
            catch (Exception e)
            {
                throw new Exception("Error: ", e);
            }
        }

        public static List<string> GetImageForDatabase(string database)
        {
            try
            {
                return Directory.GetFiles(@"thumbnails\" + database + @"\Named_Boxarts\").ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Error: ", e);
            }
        }

        public static void CreateFolderIfNotExist(string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
            catch (Exception e)
            {
                throw new Exception("Error: ", e);
            }
        }

        public static void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception e)
            {
                throw new Exception("Error: ", e);
            }
        }

        public static void SaveImage(BitmapImage imageToSave, string path)
        {
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(imageToSave));
                    encoder.Save(fileStream);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error: ", e);
            }
        }

        public static BitmapImage LoadImage(string path)
        {
            try
            {
                BitmapImage myRetVal = null;
                BitmapImage img = new BitmapImage();
                //img.BeginInit();
                //img.UriSource = new Uri(path, UriKind.Absolute);
                //img.CacheOption = BitmapCacheOption.OnLoad;
                //img.EndInit();
                ////return new BitmapImage(new Uri(path));
                ///
                using (FileStream stream = File.OpenRead(path))
                { 
                    img.BeginInit();
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.StreamSource = stream;
                    img.EndInit();
                }
                myRetVal = img;
                return myRetVal;
            }
            catch (Exception e)
            {
                throw new Exception("Error: ", e);
            }

            /*
                BitmapImage myRetVal = null;
                if (imgPath != null)
                {
                    BitmapImage img = new BitmapImage();
                    using (FileStream stream = File.OpenRead(imgPath))
                    {
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.StreamSource = stream;
                        img.EndInit();
                    }
                    myRetVal = image;
                }
                return myRetVal;
            */
        }
    }
}
