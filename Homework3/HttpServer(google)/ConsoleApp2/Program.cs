using System.Net;
using System.Text;

HttpListener server = new HttpListener();
// установка адресов прослушки
server.Prefixes.Add("http://127.0.0.1:2323/");
server.Start(); // начинаем прослушивать входящие подключения

Console.WriteLine("Сервер запущен");
// получаем контекст
var context = await server.GetContextAsync();
 
var response = context.Response;
// отправляемый в ответ код htmlвозвращает
var file = File.ReadAllText("static/index.html");
byte[] buffer = Encoding.UTF8.GetBytes(file);
// получаем поток ответа и пишем в него ответ
response.ContentLength64 = buffer.Length;
using Stream output = response.OutputStream;
// отправляем данные
await output.WriteAsync(buffer);
await output.FlushAsync();
 
Console.WriteLine("Запрос обработан");
 
server.Stop();
Console.WriteLine("Сервер прекратил работу");