using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string json = @"
                { 
                    ""Name"" : ""<b>Foo Bar</b>"", 
                    ""Description"" : ""<p>Bada Boom Bada Bing</p>"", 
                    ""MyBar"":{""User"":""Zhang"", ""Money"":12345678901234567890.123456789}
                }";

            var f = JsonConvert.DeserializeObject<Foo>(json, new JsonSerializerSettings
            {
                ContractResolver = CustomResolver.Instance
            }) ;
            Console.WriteLine(f.MyBar.Money);
        }
    }

    public class CustomResolver : DefaultContractResolver
    {
        public new static readonly CustomResolver Instance = new CustomResolver();

        protected override JsonContract CreateContract(Type objectType)
        {
            Console.WriteLine("@CreateContract, Type: " +objectType.Name);

            JsonContract contract = base.CreateContract(objectType);

            /*
            // this will only be called once and then cached
            if (objectType == typeof(DateTime) || objectType == typeof(DateTimeOffset))
            {
                contract.Converter = new JavaScriptDateTimeConverter();
            }
            */

            return contract;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);

            // Find all string properties that do not have an [AllowHtml] attribute applied
            // and attach an HtmlEncodingValueProvider instance to them
            /*
            foreach (JsonProperty prop in props.Where(p => p.PropertyType == typeof(string)))
            {
                Console.WriteLine(prop.UnderlyingName);
                PropertyInfo pi = type.GetProperty(prop.UnderlyingName);

                if (pi != null && pi.GetCustomAttribute(typeof(AllowHtmlAttribute), true) == null)
                {
                    prop.ValueProvider = new HtmlEncodingValueProvider(pi);
                }
            }*/

            foreach(var prop in props)
            {
                Console.WriteLine("@Create prop: " + prop.UnderlyingName);
                PropertyInfo pi = type.GetProperty(prop.UnderlyingName);
                prop.ValueProvider = new HtmlEncodingValueProvider(pi);                 
            }

            return props;
        }

        protected class HtmlEncodingValueProvider : IValueProvider
        {
            PropertyInfo targetProperty;

            public HtmlEncodingValueProvider(PropertyInfo targetProperty)
            {
                Console.WriteLine("@ValueProvider Construct: " + targetProperty.Name);
                this.targetProperty = targetProperty;
            }

            // SetValue gets called by Json.Net during deserialization.
            // The value parameter has the original value read from the JSON;
            // target is the object on which to set the value.
            public void SetValue(object target, object value)
            {
                Console.WriteLine("Set Value: " + this.targetProperty.Name +" " + this.targetProperty);
                
                //var encoded = HttpUtility.HtmlEncode((string)value); //, useNamedEntities: true
                if (this.targetProperty.Name != "Money" && this.targetProperty.Name != "MyBar")
                    targetProperty.SetValue(target, ">>" + (string)value);
                else
                    targetProperty.SetValue(target, value);
            }

            // GetValue is called by Json.Net during serialization.
            // The target parameter has the object from which to read the string;
            // the return value is the string that gets written to the JSON
            public object GetValue(object target)
            {
                // if you need special handling for serialization, add it here
                return targetProperty.GetValue(target);
            }
        }
    }

    public class Foo
    {
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public int Age { get; set; }
        public Bar MyBar { get; set; }
    }

    public class FooEx : Foo
    {
        public Bar[] barArray { get; set; }
    }

    public class Bar
    {
        public string User { get; set; }
        public decimal Money { get; set; }
    }


}
