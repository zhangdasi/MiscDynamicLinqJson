using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.ModelCar;
using EntityFramework.DynamicLinq;
using System.Linq.Dynamic.Core;
using System.Text.Json;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tenant = _dbContext.Tenant.Where(t => t.TenantID == 1).FirstOrDefault();
            if (tenant == null)
                throw new Exception("Error");

            _t = tenant;
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        ZhangDasiDBEntities _dbContext = new ZhangDasiDBEntities();
        Tenant _t;
        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _t.Name = "OPQRST";
            _dbContext.SaveChanges();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (ZhangDasiDBEntities dbCntxt = new ZhangDasiDBEntities())
            {
                //dbCntxt.Configuration.ProxyCreationEnabled = false;
                var tenant = dbCntxt.Tenant.Include("CarTenant.Car")
                    .Where(t => t.TenantID == 2)
                    .Select("tt => new { tt.Name, tt.CarTenant.Select(t => new { t.CarID }) as CT }").ToDynamicList();

                /*.Select(
                 tt => new { tt.Name, ct = tt.CarTenant.Select(t => new { t.CarID }) }).FirstOrDefault();
                 */

                foreach (var t in tenant) { 
                    Console.WriteLine(t.Name);
                    foreach (var c in t.CT)
                        Console.WriteLine(c.CarID);
                }

                string jsonString;
                jsonString = JsonSerializer.Serialize(tenant);

                Console.WriteLine(jsonString);

            }
        }
    }
}
