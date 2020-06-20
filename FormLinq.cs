using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Linq.Dynamic.Core;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;

using WindowsFormsApp1.ModelCar;

using System.Linq.Expressions;

namespace WindowsFormsApp1
{
    public partial class FormLinq : Form
    {
        public FormLinq()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bar[] ba = new Bar[] { new Bar { User = "A", Money = 1000 }, 
                new Bar { User = "B", Money = 2000}, 
                new Bar { User = "C", Money = 3000} };

            /*
            var l = ba.AsQueryable<Bar>().Where("User==@0", "A");
            */
            var converter = new CriteriaToExpressionConverterForObjects();
            var l = ba.AsQueryable<Bar>().AppendWhere(converter, CriteriaOperator.Parse("[Money]>1500")).Cast<Bar>();

            foreach (var b in l)
                Console.WriteLine(b.User + " " + b.Money);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (ZhangDasiDBEntities dbCntxt = new ZhangDasiDBEntities())
            {
                //dbCntxt.Configuration.ProxyCreationEnabled = false;
                var tenant = dbCntxt.CarTenant.Include("Car").Include("Tenant");
                var converter = new CriteriaToEFExpressionConverter();
                var l = tenant.AppendWhere(converter, CriteriaOperator.Parse("Contains([Car.Vendor],'E')")).Cast<CarTenant>();

                foreach (var c in l)
                    Console.WriteLine(c.CarID + " " + c.Car.Vendor + " " + c.Car.Price+ " " + c.Tenant.Name);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bar[] ba = new Bar[] { new Bar { User = "A", Money = 1000 },
                new Bar { User = "B", Money = 2000},
                new Bar { User = "C", Money = 3000} };

            /*
            var l = ba.AsQueryable<Bar>().Where("User==@0", "A");
            */

            // filter: Money>1500
            var filter = CriteriaOperator.Parse("[Money]>1500");
            
            // get expression's body: bar.Money>1500
            var converter = new CriteriaToExpressionConverterForObjects();
            ParameterExpression pe = Expression.Parameter(typeof(Bar), "bar");
            var expPredicateBody =  converter.Convert(pe, filter);

            // get lambda expression: bar => bar.Money>1500 
            LambdaExpression lambda =  Expression.Lambda<Func<Bar, bool>>(expPredicateBody, new ParameterExpression[] { pe });
            Console.WriteLine(lambda.Compile().DynamicInvoke(ba[1]));
        }
    }
}
