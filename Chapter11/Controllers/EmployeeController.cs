using Chapter11.Models;
using Chapter11.Repository;
using Microsoft.AspNetCore.Mvc;
using static CSharp.Functional.Functional;
using CSharp.Functional.Extensions;
using Newtonsoft.Json.Linq;

namespace Chapter11.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private readonly ICachingRepository<Employee> _repo;

        private Func<string, Uri> createUri = (uri) => new Uri(uri);


        public EmployeeController(ICachingRepository<Employee> repo)
        {
            _repo = repo;
        }

        Try<JObject> Parse(string s) => () => JObject.Parse(s);
        Try<Uri> CreateUri(string uri) => () => new Uri(uri);

        Try<Uri> ExtractUri_Function(string json) =>
            Parse(json)
            .Bind(jobj => CreateUri((string)jobj["uri"]));

        Try<Uri> ExtractUri_Linq(string json) =>
            from jobj in Parse(json)
            let uriAsString = (string)jobj["uri"]
            from uri in CreateUri(uriAsString)
            select uri;

        public Uri GetUri([FromBody] string obj)
        {
            //var result = Parse(obj)
            //     .Bind(jobj => CreateUri((string)jobj["uri"]))
            //     .Run()
            //     .Match(ex => null, v => v);
            //return result;

            var result = from jobj in Parse(obj)
                         let s = (string)jobj["uri"]
                         from uri in CreateUri(s)
                         select uri;
            return result;
        }

        [HttpPost, Route("api/Employee")]
        public Employee GetEmployee([FromBody] int id)
        {

            createUri.Apply("www.github.com").AsTry().Run();
            var emp = _repo.Lookup(id).Match(() => null, e => e);
            return emp;
        }
    }
}
