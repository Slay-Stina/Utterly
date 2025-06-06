using System.Text.Json;
using Utterly.Areas.Identity.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Utterly.Tools;

public static class Extensions
{
    public static StringContent ToContent(this UtterlyPost post)
    {
        return new StringContent(JsonSerializer.Serialize(post), System.Text.Encoding.UTF8, Application.Json);
    }
}
