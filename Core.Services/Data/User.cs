﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Data
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Session> Sessions { get; set; }
    }
}
