using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
            if (!File.Exists(ConfigFileName))
            {
                cfg = new()
                {
                    Token = "ENTER ME PLEASE"
                };
                File.WriteAllText(ConfigFileName, JsonConvert.SerializeObject(cfg, Formatting.Indented));
            }
            else
            {
                cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigFileName));
            }
            return cfg;
        }
    }
}
