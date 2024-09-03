using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FirstApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        public static List<String> list = new List<string>();
        private readonly IDataProtector _dataProtector;
        private readonly IConfiguration _configuration;

        public SampleController(IDataProtectionProvider dataProtector, IConfiguration configuration)
        {
            _configuration = configuration;
            var kvURL = _configuration["KeyVaultURL"];
            var tenantId = _configuration["TenantId"];
            var clientId = _configuration["ClientId"];
            var clientSecret = _configuration["ClientSecret"];
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var keyClient = new KeyClient(new Uri(kvURL), clientSecretCredential);
            var key = keyClient.GetKey("EncryptKey");
            string encryptKey = key.Value.ToString();
            _dataProtector = dataProtector.CreateProtector(encryptKey);
        }

        [HttpGet]
        public List<string> Get()
        {
            
            return list;
        } 
        [HttpPost]
        public string Post(string input)
        {
            string encripttxt = this._dataProtector.Protect(input);
            string decryptTxt = this._dataProtector.Unprotect(encripttxt);
            list.Add(encripttxt + "->" + decryptTxt);
            return encripttxt;
        }

        [HttpDelete]
        public void Delete() { 
            list.Clear();
        }

        [HttpPut]
        public void Put(string input)
        {
            list.Add(input);
            change();
        }

        private void change()
        {
            
        }
    }
}
