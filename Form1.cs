using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyncKey
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //获取Resources中到all.json文件
            //转换成字符串
            string all = Encoding.UTF8.GetString(Properties.Resources.all);
            //解析all.json文件为Json
            Json json = Newtonsoft.Json.JsonConvert.DeserializeObject<Json>(all);
            //设置dataGridView1的数据源为json的data
            dataGridView1.DataSource = json.data;
        }
        public class Json
        {
            public string info { get; set; }
            public List<Key> data { get; set; }
            public Json()
            {
                data = new List<Key>();
            }
        }
        public class Key
        {
            public string key { get; set; }
            public string cat { get; set; }
            public string title { get; set; }
            public string des { get; set; }
        }
    }
}
