using System.Collections;
using Engine.PipeHttp;

namespace Engine;

public class FileManager
{

    public static String GetErorMessage(String status)
    {
        String message=$"<!DOCTYPE html> <html lang=\"ru\"><head><meta charset=\"utf-8\"><h1>Error</h1></head><h2>{status}</h2><body></body></html>";
        return message;
    }
    
    public static void Log(string message) {
        File.AppendAllText("log.txt", message);
    }

    public static void DeleteResponseFile(String filePath)
    {
        FileInfo file = new FileInfo(filePath);
        
        if (file.Exists)
        {
            file.Delete();
        }
    }

}