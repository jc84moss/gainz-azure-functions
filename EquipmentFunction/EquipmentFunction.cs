using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Core.Services.Interfaces;
using Core.Services.Data;
using Core.Services.Services;
using System.Collections.Generic;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace EquipmentFunction
{
    public static class GetAllEquipment
    {
        [FunctionName("GetAllEquipment")]
        public static async Task<IEnumerable<Equipment>> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Get")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function geting all equipment.");

            IDocumentDbRepository<Equipment> Repository = new DocumentDbRepository<Equipment>();
            var collectionId = Environment.GetEnvironmentVariable("EquipmentCollectionId");
            return await Repository.GetItemsAsync(collectionId);
        }
    }

    public static class GetSessionTypes
    {
        [FunctionName("GetSessionTypes")]
        public static async Task<IEnumerable<SessionType>> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetSessionTypes")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function geting all equipment.");

            IDocumentDbRepository<SessionType> Repository = new DocumentDbRepository<SessionType>();
            var collectionId = Environment.GetEnvironmentVariable("SessionTypeCollectionId");
            return await Repository.GetItemsAsync(collectionId);
        }
    }

    public static class CreateOrUpdateEquipment
    {
        [FunctionName("CreateOrUpdateEquipment")]
        public static async Task<bool> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = "CreateOrUpdate")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# trigger function to create an equipment record into cosmos.");
            try
            {
                IDocumentDbRepository<Equipment> Repository = new DocumentDbRepository<Equipment>();
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var Equipment = JsonConvert.DeserializeObject<Equipment>(requestBody);
                var collectionId = Environment.GetEnvironmentVariable("EquipmentCollectionId");
                if (req.Method == "POST")
                {
                    Equipment.Id = null;
                    await Repository.CreateItemAsync(Equipment, collectionId);
                }
                else
                {
                    await Repository.UpdateItemAsync(Equipment.Id, Equipment, collectionId);
                }
                return true;
            }
            catch
            {
                log.LogError("Error occured while creating a record in Cosmos Db");
                return false;
            }
        }
    }

    //TODO: Move to seperate user function
    public static class GetUser
    {
        [FunctionName("GetUser")]
        public static async Task<User> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetUser")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function geting users.");

            IDocumentDbRepository<User> Repository = new DocumentDbRepository<User>();
            var collectionId = Environment.GetEnvironmentVariable("UserCollectionId");
            return (await Repository.GetItemsAsync(collectionId)).First();
        }
    }

    //TODO: Move to seperate user function
    public static class CreateOrUpdateEnvironmentSettings
    {
        //TODO: dependent on userId
        [FunctionName("CreateOrUpdateEnvironmentSettings")]
        public static async Task<bool> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = "CreateOrUpdateEnvironmentSettings")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function creating users.");
            try
            {
                //**TODO: FIX TEMP Workaround
                IDocumentDbRepository<User> Repository = new DocumentDbRepository<User>();
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var user = JsonConvert.DeserializeObject<User>(requestBody);
                var collectionId = Environment.GetEnvironmentVariable("UserCollectionId");
                if (req.Method == "POST")
                {
                    user.Id = null;
                    await Repository.CreateItemAsync(user, collectionId);
                }
                else
                {
                    await Repository.UpdateItemAsync(user.Id, user, collectionId);
                }
                return true;
            }
            catch
            {
                log.LogError("Error occured while creating a record in Cosmos Db");
                return false;
            }
        }
    }
}