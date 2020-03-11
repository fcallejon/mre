using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Function
{
    public class RandomOrgResponse<T>
    {
        [JsonProperty("result")]
        public Result<T> Result { get; set; }
    }

    public class Result<T>
    {
        [JsonProperty("random")]
        public Random<T> Random { get; set; }
    }

    public class Random<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }

}
