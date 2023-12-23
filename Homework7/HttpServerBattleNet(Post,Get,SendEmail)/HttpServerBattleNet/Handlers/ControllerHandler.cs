using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using HttpServerBattleNet.asda;
using HttpServerBattleNet.Attribuets;
using HttpServerBattleNet.Model;
using Newtonsoft.Json;

namespace HttpServerBattleNet.Handler;

public class ControllerHandler : Handler
{
    public override void HandleRequest(HttpListenerContext context)
    {
        try
        {
            var request = context.Request;
            using var response = context.Response;

            var strParams = request.Url!
                .Segments
                .Skip(1)
                .Select(s => s.Replace("/", ""))
                .ToArray();

            if (strParams!.Length < 2)
                Console.WriteLine("the number of lines in the query string is less than two!");
        
            var controllerName = strParams[0];
            var methodName = strParams[1];
            var id = strParams.Length >= 3 ? strParams[2] : null;
            var assembly = Assembly.GetExecutingAssembly();

            var controller = assembly.GetTypes()
                .Where(t => Attribute.IsDefined(t, typeof(ControllerAttribute)))
                .FirstOrDefault(c =>
                    ((ControllerAttribute)Attribute.GetCustomAttribute(c, typeof(ControllerAttribute))!)
                    .ControllerName.Equals(controllerName, StringComparison.OrdinalIgnoreCase));

            var method = (controller?.GetMethods())
                .FirstOrDefault(x => x.GetCustomAttributes(true)
                    .Any(attr => attr.GetType().Name.Equals($"{request.HttpMethod}Attribute",
                                     StringComparison.OrdinalIgnoreCase) &&
                                 ((HttpMethodAttribute)attr).ActionName.Equals(methodName,
                                     StringComparison.OrdinalIgnoreCase)));

            var queryParams = method?.GetParameters()
                .Select((p, i) =>
                {
                    if (p.ParameterType == typeof(int) && i == 0)
                    {
                        return Convert.ChangeType(id, p.ParameterType);
                    }
                    return Convert.ChangeType(strParams[i], p.ParameterType);
                })
                .ToArray();

            if (request is { HttpMethod: "POST", HasEntityBody: true })
            {
                var encoding = request.ContentEncoding;
                var reader = new StreamReader(request.InputStream, encoding);

                var parsedData = HttpUtility.ParseQueryString(reader.ReadToEnd());
                var email = parsedData["email"];
                var password = parsedData["password"];
                
                var resultFromMethod = method?.Invoke(
                    Activator.CreateInstance(controller),
                    new object[] { email ?? string.Empty, password ?? string.Empty } );
                
                ProcessResult(resultFromMethod, response);   
            }
            else
            { 
                var resultFromMethod = method?.Invoke(Activator.CreateInstance(controller), queryParams);
                ProcessResult(resultFromMethod, response);   
            }
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void ProcessResult<T>(T result, HttpListenerResponse response)
    {
        switch (result)
        {
            case string resultOfString:
            {
                response.ContentType = DictionaryExtensions._dictOfExtenshions[".html"];
                var buffer = Encoding.UTF8.GetBytes(resultOfString);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                break;
            }
            case T[] arrayOfObjects:
            {
                response.ContentType = DictionaryExtensions._dictOfExtenshions["json"];
                var json = JsonConvert.SerializeObject(arrayOfObjects, Formatting.Indented);
                var buffer = Encoding.UTF8.GetBytes(json);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                break;
            }
            case T obj:
            {
                response.ContentType = DictionaryExtensions._dictOfExtenshions["json"];
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                var buffer = Encoding.UTF8.GetBytes(json);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                break;
            }
        }
    }
}

