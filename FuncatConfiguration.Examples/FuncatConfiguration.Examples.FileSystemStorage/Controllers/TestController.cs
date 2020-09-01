using FuncatConfiguration.Examples.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace FuncatConfiguration.Examples.FileSystemStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly AnotherServiceConnectionSettings _anotherServiceConnectionSettings;
        private readonly SomeServiceConnectionSettings _someServiceConnectionSettings;

        public TestController(SomeServiceConnectionSettings someServiceConnectionSettings, AnotherServiceConnectionSettings anotherServiceConnectionSettings)
        {
            _someServiceConnectionSettings = someServiceConnectionSettings;
            _anotherServiceConnectionSettings = anotherServiceConnectionSettings;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"SomeServiceConnectionSettings: uri: {_someServiceConnectionSettings.Uri}, login: {_someServiceConnectionSettings.BasicAuthLogin}, password: {_someServiceConnectionSettings.BasicAuthPassword}\r\n" +
                $"AnotherServiceConnectionSettings: uri: {_anotherServiceConnectionSettings.Uri}, login: {_anotherServiceConnectionSettings.BasicAuthLogin}, password: {_anotherServiceConnectionSettings.BasicAuthPassword}";
        }
    }
}