using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace VoteBot.Config
{
    class Config
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonIgnore]
        public const string ConfigFileName = "Config.json";

        public static Config Get()
        {
            Config cfg;
            string path = @$"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{ConfigFileName}";
            if (!File.Exists(path))
            {
                cfg = new()
                {
                    Token = ""
                };
                File.WriteAllText(path, JsonConvert.SerializeObject(cfg, Formatting.Indented));
            }
            else
            {
                cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigFileName));
            }
            return cfg;
        }
    }
}
