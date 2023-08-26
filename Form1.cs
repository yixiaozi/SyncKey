using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SyncKey.Form1;

namespace SyncKey
{
    public partial class Form1 : Form
    {
        public List<Key> data = new List<Key>();
        public Form1()
        {
            InitializeComponent();
            button1_Click(null,null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data.Clear();
            //获取Resources中到all.json文件
            //转换成字符串
            string all = Encoding.UTF8.GetString(Properties.Resources.all);
            //解析all.json文件为Json
            Json json = Newtonsoft.Json.JsonConvert.DeserializeObject<Json>(all);
            data.AddRange(json.data);
            //将E:\Develop\SyncKey\微力同步神KEY，通用同步key\data文件夹下载的文件解析成Jsoninfo
            foreach(string path in Directory.GetFiles("E:\\Develop\\SyncKey\\微力同步神KEY，通用同步key\\data"))
            {
                JsonInfo jsonInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonInfo>(File.ReadAllText(path));
                data.AddRange(jsonInfo.data);
            }

            //将"E:\Develop\SyncKey\微力同步神KEY，通用同步key\home.json"解析成List<Jsoninfo>
            List<JsonInfo> jsonInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JsonInfo>>(File.ReadAllText("E:\\Develop\\SyncKey\\微力同步神KEY，通用同步key\\home.json"));
            foreach(JsonInfo jsonInfo in jsonInfos)
            {
                data.AddRange(jsonInfo.data);
            }
            //显示data到dataGridView1中
            //根据key去重
            data=data.ToArray().Distinct(new KeyComparer()).ToList();
            dataGridView1.DataSource = data.Distinct().OrderBy(m=>m.title).ToList();
            //第一列宽度200
            dataGridView1.Columns[0].Width = 200;   
            dataGridView1.Columns[1].Width = 400;   
            dataGridView1.Columns[2].Width = dataGridView1.Width - 600 - 88- dataGridView1.FirstDisplayedScrollingColumnHiddenWidth;

        }

        class KeyComparer : IEqualityComparer<Key>
        {
            public bool Equals(Key x, Key y)
            {
                return x.key == y.key;
            }

            public int GetHashCode(Key obj)
            {
                return obj.key.GetHashCode();
            }
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
        public class JsonInfo
        {
            public info info { get; set; }
            public List<Key> data { get; set; }
            public JsonInfo()
            {
                data = new List<Key>();
            }
        }
        public class info
        {
            //"code":"hydy","name":"华语电影","cat_num":24
            public string code { get; set; }
            public string name { get; set; }
            public int cat_num { get; set; }
        }
        public class Key
        {
            public string key { get; set; }
            //public string cat { get; set; }
            public string title { get; set; }
            public string des { get; set; }
            //public string tok { get; set; }
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //搜索
            dataGridView1.DataSource = data.Where(m => m.title.Contains(textBox1.Text)|| m.des.Contains(textBox1.Text)).ToList();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //获取当前行
            Key key = dataGridView1.Rows[e.RowIndex].DataBoundItem as Key;
            textBox2.Text = key.key;
            richTextBox1.Text = key.title;
            richTextBox2.Text = key.des;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox2.Text);
        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox1.Text);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2_Click(null, null);
            }
        }
    }
}
