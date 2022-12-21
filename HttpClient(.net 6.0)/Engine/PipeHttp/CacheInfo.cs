namespace Engine.PipeHttp;

using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

public enum CacheType
{
    NO_STORE,
    NO_CACHE,
    MUST_REVALIDATE,
    MUST_REVALIDATE_CONDITIONAL
}

public class CacheInfo
{
    private string expires;
    public DateTime expiresDT;
    private bool isExpires = false;
    public string ETag;
    public bool isETag = false;
    public string lastModified;
    private DateTime lastModifiedDT;
    public bool isLastModified = false;
    private bool isNoStore = false;
    private bool isNoCache = false;
    private bool isMustRevalidate = false;
    public int maxAge = 0;
    public string subtype;
    public string pathFile;
    public DateTime responseDT;
    public CacheType cacheType;
    public static Dictionary<string, CacheInfo> CacheTable = new Dictionary<string, CacheInfo>();
    public void SetCacheType()
    {
        if (isNoStore)
        {
            this.cacheType = CacheType.NO_STORE;
            return;
        }

        if (isMustRevalidate && (maxAge > 0 || isExpires))
        {
            if (isLastModified || isETag)
            {
                this.cacheType = CacheType.MUST_REVALIDATE_CONDITIONAL;
                return;
            }
            this.cacheType = CacheType.MUST_REVALIDATE;
            return;
        }
        
        if (isLastModified || isETag)
        {
            this.cacheType = CacheType.NO_CACHE;
            return;
        }
        
        this.cacheType = CacheType.NO_STORE;
    }

    public bool Validate()
    {
        if (maxAge > 0)
        {
            FileManager.Log(maxAge.ToString());
            return (DateTime.UtcNow.AddSeconds(-maxAge) <= responseDT);
        }

        return false;
    }
    
    public void SetExpires(string fieldValue)
    {
        expires = fieldValue.Trim();
        //Console.WriteLine("exp:" + expires);
        FileManager.Log("exp:" + expires+"\n");
        if (DateTime.TryParseExact(expires,
                "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat,
                DateTimeStyles.AdjustToUniversal, out expiresDT))
        {
            this.isExpires = true;
        }
        Console.WriteLine(isExpires);
        FileManager.Log(isExpires.ToString()+"\n");
    }
    
    public void SetLastModified(string fieldValue)
    {
        lastModified = fieldValue.Trim();
        //Console.WriteLine("lm:" + lastModified);
        FileManager.Log("lm:" + lastModified+"\n");
        if (DateTime.TryParseExact(lastModified,
                "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat,
                DateTimeStyles.AdjustToUniversal, out lastModifiedDT))
        {
            this.isLastModified = true;
        }
    }

    public void SetCacheControl(string fieldValue)
    {
        var controls = fieldValue.Split(",");
        for (int i = 0; i < controls.Length; i++)
        {
            controls[i] = controls[i].Trim();
            if (controls[i].Contains("max-age"))
            {
                this.maxAge = Convert.ToInt32(controls[i].Substring(controls[i].IndexOf("=") + 1));
                //Console.WriteLine(this.maxAge);
                FileManager.Log("maxAge"+this.maxAge+"\n");
            }
        }
        if (controls.Contains("no-store"))
        {
            this.isNoStore = true;
            return;
        }

        if (controls.Contains("no-cache"))
        {
            this.isNoCache = true;
            return;
        }

        if (controls.Contains("must-revalidate"))
        {
            this.isMustRevalidate = true;
        }
    }

    public void SetETag(string fieldValue)
    {
        if (Regex.IsMatch(fieldValue.Trim(), @"""\S+"""))
        {
            this.isETag = true;
            this.ETag = fieldValue.Trim().Trim('"');
        }
        FileManager.Log("et:" + this.ETag+"\n");
        FileManager.Log(this.isETag.ToString()+"\n");
        
    }

    public void SetCache(string fieldName, string fieldValue)
    {
        if (fieldName == "Expires")
        {
            SetExpires(fieldValue);
            return;
        }
        if (fieldName == "Last-Modified")
        {
            SetLastModified(fieldValue);
            return;
        }

        if (fieldName == "Cache-Control")
        {
            SetCacheControl(fieldValue);
            return;
        }
        
        if (fieldName == "ETag")
        {
            SetETag(fieldValue);
            return;
        }
    }
    
    public CacheInfo(string fieldName, string fieldValue)
    {
        SetCache(fieldName, fieldValue);
    }
}