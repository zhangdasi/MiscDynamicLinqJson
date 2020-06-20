using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            Employee employee = new Employee
            {
                FirstName = "James",
                LastName = "Newton-King",
                Roles = new List<string>
                {
                    "Admin"
                },
                MyBar = new Bar { User = "Zhang", Money= 1234567m}
            };

            string json = JsonConvert.SerializeObject(employee, Formatting.Indented, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new KeysJsonConverter(typeof(Employee)) },
                //NullValueHandling = NullValueHandling.Ignore
            });

            Console.WriteLine(json);
            */

            string json = @"
                { 
                    ""FirstName"" : ""Ba"", 
                    ""LastName"" : ""Da"", 
                    ""Money"":1000,
                    ""MyBar"":{""User"":""Zhang"", ""Money"":12345678901234567890.123456789}
                }";


            var emp = JsonConvert.DeserializeObject<Employee>(json
            , new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new KeysJsonConverter(typeof(Employee)) },
                FloatParseHandling = FloatParseHandling.Decimal,
                NullValueHandling = NullValueHandling.Ignore
            }); 

            
        }
    }

    public class KeysJsonConverter : JsonConverter
    {
        private readonly Type[] _types;

        public KeysJsonConverter(params Type[] types)
        {
            _types = types;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                //IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                //o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

                o.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
            JToken token = JToken.Load(reader);
            return token.ToObject<Employee>();
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }
    }

    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Decimal Money { get; set; }
        //public IList<string> Roles { get; set; }
        public Bar MyBar { get; set; }
    }

}
