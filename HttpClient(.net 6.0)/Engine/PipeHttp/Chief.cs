using HttpClient_.net_6._0_.PipeHttp;

namespace Engine.PipeHttp;

public class Chief
{
    public static String type;
    public static String pathFile;

    public static async Task Ainigilyator(String uriWF)
    {
        
        if (uriWF == null)
        return;
        Connect.CloseConn();

            
        Response response = new Response();
        string absUri = new Uri(uriWF).AbsoluteUri;
        
        type = "";
        pathFile = "";
        
        
       
        if (CacheInfo.CacheTable.ContainsKey(absUri))
        {
            var cacheResponse = CacheInfo.CacheTable[absUri];
            if (cacheResponse.cacheType == CacheType.MUST_REVALIDATE ||
                cacheResponse.cacheType == CacheType.MUST_REVALIDATE_CONDITIONAL)
            {
                if (cacheResponse.Validate()) // свежий контент
                {
                    type = cacheResponse.subtype;
                    pathFile = cacheResponse.pathFile;
                    FileManager.Log("CACHED");
                    return;
                }
                // если несвежий, удалить из CacheTable
            }

            if (cacheResponse.cacheType == CacheType.NO_STORE)
            {
                CacheInfo.CacheTable.Remove(absUri);
            }
        }

        Request.GetRequestFromUri(uriWF);
        FileManager.Log("Request complete\n");

        await Connect.CreateConn();
        FileManager.Log("Connect complete\n");
        bool conditional = CacheInfo.CacheTable.ContainsKey(absUri) &&
                           (CacheInfo.CacheTable[absUri].cacheType == CacheType.NO_CACHE ||
                            CacheInfo.CacheTable[absUri].cacheType == CacheType.MUST_REVALIDATE_CONDITIONAL);
        if (conditional)
        {
            FileManager.Log("\nOLD\n");
        }
        
        
        await Request.RequestHttp(conditional, Response.authenticate);
        
        FileManager.Log("Request complete\n");
        
        Response response = new Response();
            
        if (Request.requestUri.Scheme == "http")
        {
            await response.ResponseHttp(Connect.tcpClient); // тут было статик
        }
        else
        {
            if (Connect.sslStream != null) 
                new ResponseHttps().ResponseHttp(Connect.sslStream);
        }
            
        type = response.GetSubType() ;
        pathFile = response.GetPath();
        FileManager.Log("\n--------------------------------\n");
    }
}