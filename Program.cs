using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ToDo_Server
{
    internal class Program
    {
        static List<TODO> todos = new List<TODO>()
        {

            new TODO(1,"do homework"),
            new TODO(2,"do exercise"),
            new TODO(3,"read a book")
        };


        static void Main(string[] args)
        {
            HttpListener toDoServer = new HttpListener();

            toDoServer.Prefixes.Add("http://localhost:9090/");

            toDoServer.Prefixes.Add("http://localhost:9090/add/");

            toDoServer.Prefixes.Add("http://localhost:9090/update/");

            toDoServer.Prefixes.Add("http://localhost:9090/delete/");



            toDoServer.Start();
            Console.WriteLine("server started");
            while (true)
            {
                var clientContext = toDoServer.GetContext();

                if (clientContext.Request.HttpMethod == "GET")
                {
                    Console.WriteLine("Get request received");
                    GetRequest(clientContext);
                }

                else if (clientContext.Request.HttpMethod == "POST")
                {
                    Console.WriteLine("Post request received");
                    using (var stream = new StreamReader(clientContext.Request.InputStream))
                    {

                        var plainText = stream.ReadToEnd();

                        var newToDo = JsonConvert.DeserializeObject<TODO>(plainText);

                        todos.Add(newToDo);
                        clientContext.Request.InputStream.Close();
                    }
                    clientContext.Response.StatusCode = 200;
                }

                else if (clientContext.Request.HttpMethod == "PUT")
                {
                    Console.WriteLine("Post request received");
                    using (var stream = new StreamReader(clientContext.Request.InputStream))
                    {

                        var plainText = stream.ReadToEnd();

                        var newToDo = JsonConvert.DeserializeObject<TODO>(plainText);
                        todos.FirstOrDefault(t => t.No == newToDo.No).Name = newToDo.Name;
                        clientContext.Request.InputStream.Close();
                    }
                    clientContext.Response.StatusCode = 200;


                }

                else if (clientContext.Request.HttpMethod == "DELETE")
                {
                    int.TryParse(clientContext.Request.QueryString.Get("No"), out int NoToBeDeleted);

                     var mustBeDeleted=  todos.FirstOrDefault(t => t.No == NoToBeDeleted);
                    todos.Remove(mustBeDeleted);
                }
            }
        }
        static void GetRequest(HttpListenerContext context)
        {
            var jsonString = JsonConvert.SerializeObject(todos);

            using (var writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.WriteLine(jsonString);

            }
            var length = context.Response.OutputStream.Flush;
            context.Response.StatusCode = 200;
        }
    }
}