using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


namespace WindowsFormsApp1
{
    public partial class FormXML : Form
    {
        public FormXML()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FooEx fooEx = new FooEx
            {
                Name = "Zhang",
                Age = 200,
                Description = "Test",
                MyBar = new Bar { User = "PrimaryBar", Money = 123.4567890123m },
                barArray = new Bar[] {
                    new Bar { User = "bar1", Money =  1.4567890123m},
                    new Bar { User = "bar2", Money  = 2.4567890123m},
                    new Bar { User = "bar3", Money  = 3.4567890123m},
                    new Bar { User = "bar4", Money  = 4.4567890123m},
                }
            };

            XmlSerializer serializer =
                new XmlSerializer(typeof(FooEx));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, fooEx);
            Console.WriteLine(writer.ToString());
        }
    }
}
