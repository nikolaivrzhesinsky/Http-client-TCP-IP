using System.Text;

namespace Engine.PipeHttp;

public class HtmlForm
{
    private Dictionary<string, string> FormTags = new Dictionary<string, string>();
    public HtmlForm(string _action, string _method, string _enctype = "application/x-www-form-urlencoded")
    {
        action = _action;
        method = _method;
        enctype = _enctype;
    }
    public string action { get; private set; }
    public string method { get; private set; }
    public string enctype { get; private set; }
    public void AddTag (string name, string value)
    {
        FormTags.Add(name, value);
    }

    public string ToEncType()
    {
        string body = "";
        if (enctype == "application/x-www-form-urlencoded")
        {
            StringBuilder sb = new StringBuilder();
            foreach(var tag in FormTags)
            {
                sb.Append(tag.Key + "=" + tag.Value + "&");
            }
            sb = sb.Remove(sb.Length - 1, 1);
        }
        else if (enctype == "application/json")
        {
            var x = FormTags.Select(d =>
                string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value)));
            body =  "{" + string.Join(",", x) + "}";
        }
        return body;
    }
}