using System.Collections;
using Engine.PipeHttp;

namespace Engine;

public class FileManager
{
    private static List<string> Caches = new List<string>();

    public static void DeleteCaches()
    {
        foreach (var cache in Caches)
        {
            FileInfo file = new FileInfo(cache);
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
    public static String GetErorMessage(String status)
    {
        String message=$"<!DOCTYPE html> <html lang=\"ru\"><head><meta charset=\"utf-8\"><h1>Error</h1></head><h2>{status}</h2><body></body></html>";
        return message;
    }
    
    public static void Log(string message) {
        File.AppendAllText("log.txt", message);
    }

    public static void DeleteResponseFile(String filePath, String url, bool finish = false)
    {
        FileInfo file = new FileInfo(filePath);
        try
        {
            if (file.Exists)
            {
                if (finish == true)
                {
                    file.Delete();
                    return;
                }
                if (CacheInfo.CacheTable.ContainsKey(url))
                {
                    var cacheResponse = CacheInfo.CacheTable[url];
                    if (cacheResponse.cacheType != CacheType.NO_STORE)
                    {
                        if (!Caches.Contains(filePath))
                        {
                            Caches.Add(filePath);
                        }

                        return;
                    }
                    
                    file.Delete();

                    return; 
                } 
            }
        }
        catch (Exception e)
        {
            //Caches.Add(filePath);
        }
    }

}